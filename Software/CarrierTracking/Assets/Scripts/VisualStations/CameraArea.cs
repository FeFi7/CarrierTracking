using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea
{
    private readonly GameObject gObject;
    private readonly string id;

    public CameraArea(GameObject gObject)
    {
        this.gObject = gObject;
        id = StationHandler.GenerateRandomID();
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
}
