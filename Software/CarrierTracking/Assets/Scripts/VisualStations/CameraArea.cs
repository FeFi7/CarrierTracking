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
        //var filePath = Application.dataPath + "/Resources/UV_1.png";
        //guiText.text = filePath;
        if (System.IO.File.Exists(filePath))
        {
            var bytes = System.IO.File.ReadAllBytes(filePath);
            var tex = new Texture2D(1, 1);
            
            tex.LoadImage(bytes);

            defaultX = tex.width;
            defaultZ = tex.height;

            gObject.transform.localScale = new Vector3(defaultX, 0.5f, defaultZ) * 0.05f;
            gObject.GetComponent<Renderer>().material.mainTexture = tex;

        }
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
