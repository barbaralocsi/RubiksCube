using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Cube : MonoBehaviour
{
    [SerializeField] float sideRotationSpeed = 6f;
    [SerializeField] float cubeTurnSpeed = 0.3f;

    [SerializeField] int scrambleStepCount = 20;
    [SerializeField] float scrambleSpeedMultiplier = 1.5f;


    private int previousTurnId = -1;

    List<GameObject> cubes;
    List<Sticker> stickers;
    
    // The number of cube elements currently rotated
    public int currentlyRotating { get; private set; }


    HintManager hintManager;

    void Start()
    {
        cubes = new List<GameObject>();
        foreach (Transform childTransform in transform)
        {
            cubes.Add(childTransform.gameObject);
        }

        stickers = FindObjectsOfType<Sticker>().ToList();

        hintManager = FindObjectOfType<HintManager>();
    }


    private void CheckSolved()
    {
        Dictionary<Color, Vector3> keyValues = new Dictionary<Color, Vector3>();

        foreach (var sticker in stickers)
        {
            Color color = sticker.gameObject.GetComponent<Renderer>().material.color;
            Vector3 direction = sticker.transform.up;

            if (keyValues.ContainsKey(color))
            {
                if (Mathf.Abs(Vector3.Dot(keyValues[color], direction)-1) > 0.1f)
                {
                    // The cube is not solved
                    return;
                }
            }
            else
            {
                keyValues.Add(color, direction);
            }
        }
        // The cube is solved
        Debug.Log("SUCCESS!! CUBE SOLVED!!");
        FindObjectOfType<Timer>().StopTimer();
    }

    /// <summary>
    /// Turns the whole cube based on the turnVector
    /// </summary>
    /// <param name="turnVector"></param>
    public void TurnCube(Vector2 turnVector)
    {
        transform.Rotate(Camera.main.transform.up, Vector3.Dot(turnVector, Camera.main.transform.right) * cubeTurnSpeed, Space.World);
        transform.Rotate(Camera.main.transform.right, Vector3.Dot(turnVector, Camera.main.transform.up) * cubeTurnSpeed, Space.World);
    }

    /// <summary>
    /// Rotates the side defined by axis and positionValue around axis with rotationValue ammount.
    /// </summary>
    /// <param name="axis">Rotating around this</param>
    /// <param name="positionValue">Axis way coordinate of the side we want to rotate</param>
    /// <param name="rotationValue">Ammount of rotation</param>
    public void RotateSide(Axis axis, float positionValue, float rotationValue)
    {
        // Hiding the hint before starting rotation
        hintManager.HideHint();

        // Only start a new rotation if there is no rotation now
        if (currentlyRotating == 0)
        {
            Vector3 rotationVector = axis.ToVector3(transform);

            foreach (var cube in cubes)
            {
                var cubePosition = cube.gameObject.transform.localPosition; // using local position 

                if (AxisExtensions.VectorEqualsValueAtAxis(axis, cubePosition, positionValue))
                {
                    StartCoroutine(ContinousRotation(cube.gameObject, axis, rotationValue));
                }
            }
        }
        else
        {
            Debug.Log("Rotation in progress I won't start new rotation");
        }

    }

    IEnumerator ContinousRotation(GameObject cube, Axis axis, float originalRotationValue)
    {
        currentlyRotating++;
        var absRotationValue = Mathf.Abs(originalRotationValue);
        var sign = Mathf.Sign(originalRotationValue);

        var rotationStep = absRotationValue * Time.deltaTime * sideRotationSpeed;

        while (absRotationValue > Mathf.Epsilon)
        {
            rotationStep = Mathf.Min(rotationStep, absRotationValue);

            Vector3 rotationVector = axis.ToVector3(transform);

            cube.transform.RotateAround(rotationVector, rotationVector, sign * rotationStep); 
            absRotationValue = absRotationValue - rotationStep;
            yield return null;
        }
        cube.transform.localPosition = new Vector3(Mathf.Round(cube.transform.localPosition.x), Mathf.Round(cube.transform.localPosition.y), Mathf.Round(cube.transform.localPosition.z));
        currentlyRotating--;

        if (currentlyRotating == 0)
        {
            // After the rotation is complete(no more small cubes are rotating) we check if the cube is solved. 
            CheckSolved();
        }
    }

    // For scrambling
    class RotationInfo
    {
        public Axis Axis { get; set; }
        public float PositionValue { get; set; }

        public RotationInfo(Axis Axis, float PositionValue)
        {
            this.Axis = Axis;
            this.PositionValue = PositionValue;
        }
    }


    // Scrambling uses these to scramble the cube
    private RotationInfo[] presetRotations = new RotationInfo[6]
    {
        new RotationInfo(Axis.X,1),
        new RotationInfo(Axis.X,-1 ),
        new RotationInfo(Axis.Y,1),
        new RotationInfo(Axis.Y,-1),
        new RotationInfo(Axis.Z,1 ),
        new RotationInfo(Axis.Z,-1 ),
    };


    /// <summary>
    /// Rotates a random side of the cube
    /// </summary>
    public void RandomRotateSide()
    {
        // Dont turn the same side multiple times after each other
        int turnId = Random.Range(0, 6);

        turnId = (turnId == previousTurnId) ? turnId+1 : turnId;
        turnId = (turnId == 6) ? 0 : turnId;

        previousTurnId = turnId;

        RotationInfo rotationInfo = presetRotations[turnId];

        float rotationValue = Mathf.Sign(Random.Range(-1f, 1f)) * 90;

        RotateSide(rotationInfo.Axis, rotationInfo.PositionValue, rotationValue);
    }


    /// <summary>
    /// Scrambles the cube with scrambleStepCount number of rotations
    /// The scramble is speed is scrambleSpeedMultiplier times the normal rotation speed (sideRotationSpeed)
    /// </summary>
    public void ScrambleCube()
    {
        if (currentlyRotating == 0)
        {
            StartCoroutine("RandomScrambleCorutine");
        }
    }

    IEnumerator RandomScrambleCorutine()
    {
        float originalSideRotationSpeed = sideRotationSpeed;
        sideRotationSpeed = sideRotationSpeed * scrambleSpeedMultiplier;
        for (int i = 0; i < scrambleStepCount; i++)
        {
            yield return new WaitUntil(() => currentlyRotating == 0);

            if (currentlyRotating == 0)
            {
                RandomRotateSide();
            }
        }
        sideRotationSpeed = originalSideRotationSpeed;
    }
}