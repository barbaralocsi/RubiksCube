using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Used to save data about a sticker
/// </summary>
[System.Serializable]
public class StickerData
{
    public StickerColor color;
    // Knowing the side and 2 coordinates of the sticker on that side of the cube is enough to restore the cube
    public CubeSide cubeSide;
    public float x;
    public float y;

    public StickerData(Axis axis, Vector3 position, StickerColor color)
    {
        this.color = color;
        cubeSide = GetCubeSide(axis, AxisExtensions.GetValueAtAxis(axis, position));

        Vector2 vector2 = AxisExtensions.GetValuesExceptAxis(axis, position);

        this.x = vector2.x;
        this.y = vector2.y;
    }

    // Needed for serialization
    public StickerData()
    {

    }

    public Vector3 GetPosition()
    {
        AxisSignPair axisSignPair = GetAxisSignPair(cubeSide);
        return AxisExtensions.Get3DPosition(axisSignPair.axis, axisSignPair.sign, new Vector2(x, y));
    }

    public Axis GetAxis()
    {
        return GetAxisSignPair(cubeSide).axis;
    }

    public class AxisSignPair
    {
        public Axis axis;
        public float sign;

        public AxisSignPair()
        {

        }

        public AxisSignPair(Axis axis, float sign)
        {
            this.axis = axis;
            this.sign = sign;
        }
    }

    private static Dictionary<CubeSide, AxisSignPair> CubeSideAxisPairs = new Dictionary<CubeSide, AxisSignPair>()
    {
        { CubeSide.Right, new AxisSignPair(){axis = Axis.X, sign = 1 } },
        { CubeSide.Left, new AxisSignPair(){axis = Axis.X, sign = -1 } },
        { CubeSide.Top, new AxisSignPair(){axis = Axis.Y, sign = 1 } },
        { CubeSide.Bottom, new AxisSignPair(){axis = Axis.Y, sign = -1 } },
        { CubeSide.Front, new AxisSignPair(){axis = Axis.Z, sign = 1 } },
        { CubeSide.Back, new AxisSignPair(){axis = Axis.Z, sign = -1 } }
    };

    public static AxisSignPair GetAxisSignPair(CubeSide cubeSide)
    {
        return CubeSideAxisPairs[cubeSide];
    }

    public static CubeSide GetCubeSide(Axis axis, float value)
    {
        return CubeSideAxisPairs.First(x => x.Value.axis == axis && x.Value.sign == Mathf.Sign(value)).Key;
    }

}
