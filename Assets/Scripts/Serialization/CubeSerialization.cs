using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CubeSerialization : MonoBehaviour
{
    [SerializeField] GameObject cubeMain;

    List<Sticker> stickers;

    XMLFileManager fileManager;

    // Full path to the file
    string fullPath;

    // Start is called before the first frame update
    void Start()
    {
        stickers = FindObjectsOfType<Sticker>().ToList();
        fileManager = new XMLFileManager();
        fullPath = Path.Combine(Application.persistentDataPath, "cubeData.xml");
        LoadCube();
    }

    public void SaveCube()
    {
        List<StickerData> serializableStickers = new List<StickerData>();
        foreach (var sticker in stickers)
        {
            Axis axis = sticker.GetNormalAxis();
            StickerData serializableSticker = new StickerData(axis, sticker.GetCubeCoordinates(), sticker.StickerColor);
            serializableStickers.Add(serializableSticker);
        }
        fileManager.Save(fullPath, serializableStickers);
    }

    public void LoadCube()
    {
        if (!fileManager.Exists(fullPath))
        {
            return;
        }

        List<StickerData> serializableStickers = fileManager.Load<List<StickerData>>(fullPath).ToList();
        foreach (var serializableSticker in serializableStickers)
        {
            Vector3 position = serializableSticker.GetPosition();
            Axis axis = serializableSticker.GetAxis();

            Sticker sticker = stickers.First(x => x.GetCubeCoordinates() == position && x.GetNormalAxis() == axis);

            sticker.StickerColor = serializableSticker.color;
        }

        RepaintAll();
    }

    private void RepaintAll()
    {
        ColorMaterialMap colorMaterialMap = FindObjectOfType<ColorMaterialMap>();
        foreach (Sticker sticker in FindObjectsOfType<Sticker>())
        {
            sticker.GetComponent<Renderer>().material = colorMaterialMap.colorMaterialPairs.First(x => x.StickerColor == sticker.StickerColor).Material;
        }
    }
}
