using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DStation
{
    public string name;
    public string StationID;
    public string info;
    public float[] centerLocation;
    public List<Walls> SaveWalls;
    public List<AreaObjects> SaveAreaObjects;

    public DStation(Station s)
    {
        centerLocation = new float[3];

        this.name = s.GetName();
        this.StationID = s.GetID();
        this.info = s.GetInfo();
        this.centerLocation[0] = s.GetCenterLocation().x;
        this.centerLocation[1] = s.GetCenterLocation().y;
        this.centerLocation[2] = s.GetCenterLocation().z;

        this.SaveWalls = new List<Walls>();
        this.SaveAreaObjects = new List<AreaObjects>();

        List<GameObject> Walls = s.GetWalls();
        List<GameObject> AreaObject = s.GetAreaObjects();
        foreach (GameObject gObj in Walls)
        {
            Walls wall = new Walls(gObj, centerLocation);
            SaveWalls.Add(wall);
        }
        foreach (GameObject gObj in AreaObject)
        {
            AreaObjects aeraobj = new AreaObjects(gObj, centerLocation);
            SaveAreaObjects.Add(aeraobj);

        }

    }

    public void loadStation()
    {
        if (StationHandler.GetStationList().GetStationByID(this.StationID) != null )
        {
            Debug.Log("[loadStation PROBLEM] Station ID bereits existent!...");
            return;
        }

        Vector3 Location = new Vector3(0, 0, 0);
        GameObject CopyedBackgroundPlane = GameObject.Instantiate(StationHandler.GetDefaultBackgroundPlane(), Location, Quaternion.identity);
        CopyedBackgroundPlane.name = "Background Plane";
        List<GameObject> Walls= new List<GameObject>();
        List<GameObject> AreaObject = new List<GameObject>();


        foreach (Walls w in this.SaveWalls)
        {
            GameObject data = w.ReturnToGameObject();
            Walls.Add(data);
        }
        foreach (AreaObjects a in this.SaveAreaObjects)
        {
            GameObject data = a.ReturnToGameObject();
            AreaObject.Add(data);
        }
        
        StationHandler.LoadStation(this.name,this.StationID,this.info,Walls,AreaObject, CopyedBackgroundPlane);

    }





}