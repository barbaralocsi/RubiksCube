using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class MyWindow : EditorWindow
{
    string recordButton = "Repaint cube";

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(MyWindow));
    }

    void OnGUI()
    {
        if (GUILayout.Button(recordButton))
        {
            RepaintAll();
        }
    }

    ColorMaterialMap colorMaterialMap;

    private void RepaintAll()
    {
        colorMaterialMap = Resources.FindObjectsOfTypeAll<ColorMaterialMap>()[0];
        foreach (Sticker sticker in FindObjectsOfType<Sticker>())
        {
            sticker.GetComponent<Renderer>().material = colorMaterialMap.colorMaterialPairs.First(x => x.StickerColor == sticker.StickerColor).Material;
        }
    }
}