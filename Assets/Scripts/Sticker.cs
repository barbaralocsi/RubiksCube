using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticker : MonoBehaviour
{
    [SerializeField] private StickerColor stickerColor;

    public StickerColor StickerColor
    {
        get { return stickerColor; }
        set { stickerColor = value; }
    }

    public Vector3 GetNormalVector()
    {
        return transform.up;
    }

    public Axis GetNormalAxis()
    {
        return AxisExtensions.Vector3ToAxis(GetNormalVector(), transform.parent.parent.transform).Key;
    }

    public Vector3 GetCubeCoordinates()
    {
        return transform.parent.localPosition;
    }
}
