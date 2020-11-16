using System;
using System.Collections;
using UnityEngine;

public class Station
{
    Vector3 location;

    GameObject BackgroundPlane;
    ArrayList areas;
    int id;

    public Station(int id)
    {
        location = new Vector3(id * 256.0f, 0.0f, 0.0f); // SET TO Sinnvoll (abhäning von der ID)
        areas = new ArrayList();
        this.id = id;
    }

    //TODO: GetLocation (for Camera Positioning), GetWalls, GetBackgroundPane
}
