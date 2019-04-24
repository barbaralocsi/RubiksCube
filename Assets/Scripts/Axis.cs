using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Axis
{
    Unknown,
    X,
    Y,
    Z
}

public static class AxisExtensions
{
    public static Vector3 ToVector3(this Axis axis, Transform transform)
    {
        switch (axis)
        {
            case Axis.X:
                return transform.right;
            case Axis.Y:
                return transform.up;
            case Axis.Z:
                return transform.forward;
            default:
                throw new System.Exception($"Unknown axis: {axis}");
        }
    }

    /// <summary>
    /// Decides which Axis of the given transform is nearest to the given vector
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="transform"></param>
    /// <returns>A KeyValuePair with Axis as Key and a number as value. The sign of this value decides the angle of the vectors. (obtuse/acute) </returns>
    public static KeyValuePair<Axis, float> Vector3ToAxis(Vector3 vector, Transform transform)
    {
        Dictionary<Axis, float> axisDotPair = new Dictionary<Axis, float>();

        // With the dot product we project the vector on the 3 axises to check which way it goes more
        axisDotPair.Add(Axis.X, Vector3.Dot(vector, transform.right));
        axisDotPair.Add(Axis.Y, Vector3.Dot(vector, transform.up));
        axisDotPair.Add(Axis.Z, Vector3.Dot(vector, transform.forward));

        // The biggest dot product result it hte nearest axis
        var max = axisDotPair.Max(x => Mathf.Abs(x.Value));

        return axisDotPair.First(x => (Mathf.Abs(x.Value)).Equals(max));
    }

    public static bool VectorEqualsValueAtAxis(Axis axis, Vector3 vector, float value)
    {
        return Mathf.Approximately(GetValueAtAxis(axis, vector), value);
    }

    public static float GetValueAtAxis(Axis axis, Vector3 vector)
    {
        switch (axis)
        {
            case Axis.X:
                return vector.x;
            case Axis.Y:
                return vector.y;
            case Axis.Z:
                return vector.z;
            default:
                throw new System.Exception($"Unknown axis: {axis}");
        }
    }

    public static Vector2 GetValuesExceptAxis(Axis axis, Vector3 vector)
    {
        switch (axis)
        {
            case Axis.X:
                return new Vector2(vector.y, vector.z);
            case Axis.Y:
                return new Vector2(vector.x, vector.z);
            case Axis.Z:
                return new Vector2(vector.x, vector.y);
            default:
                throw new System.Exception($"Unknown axis: {axis}");
        }
    }


    public static Vector3 Get3DPosition(Axis axis,float value, Vector2 vector2)
    {
        switch (axis)
        {
            case Axis.X:
                return new Vector3(value, vector2.x, vector2.y);
            case Axis.Y:
                return new Vector3(vector2.x, value,  vector2.y);
            case Axis.Z:
                return new Vector3(vector2.x, vector2.y, value);
            default:
                throw new System.Exception($"Unknown axis: {axis}");
        }
    }



}
