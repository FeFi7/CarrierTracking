using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea
{
    private readonly GameObject gObject;
    private readonly string id;
    private float defaultX;
    private float defaultZ;

    public CameraArea(GameObject gObject)
    {
        this.gObject = gObject;
        id = StationHandler.GenerateRandomID();
        defaultX = 1.92f * 32;
        defaultZ = 1.08f * 32;
    }

    public CameraArea(string id, GameObject gObject)
    {
        this.id = id;
        this.gObject = gObject;

    }

    public GameObject GetGameObject()
    {
        return gObject;
    }

    public string GetID()
    {
        return id;
    }

    public void LoadPngAsTexture(string filePath)
    {
        if (System.IO.File.Exists(filePath))
        {
            var bytes = System.IO.File.ReadAllBytes(filePath);
            var tex = new Texture2D(1, 1);


            tex.LoadImage(bytes);

            RotateTexture180Degree(tex);

            defaultX = tex.width;
            defaultZ = tex.height;

            gObject.transform.localScale = new Vector3(defaultX, 0.5f, defaultZ) * 0.05f;
            gObject.GetComponent<Renderer>().material.mainTexture = tex;

        }
    }

    private void RotateTexture180Degree(Texture2D original)
    {
        var originalPixels = original.GetPixels();

        Color[] newPixels = new Color[originalPixels.Length];

        int width = original.width;
        int rows = original.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                newPixels[x + y * width] = originalPixels[(width - x - 1) + (rows - y - 1) * width];
            }
        }

        original.SetPixels(newPixels);
        original.Apply();
    }

    public float GetDefaultX()
    {
        return defaultX;
    }

    public float GetDefaultZ()
    {
        return defaultZ;
    }
}
