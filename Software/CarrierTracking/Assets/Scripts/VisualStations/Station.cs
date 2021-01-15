using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station
{
    private Vector3 Location;
    private GameObject Parent; //gameobject that is parent to the other gameobjects of the station
    private GameObject BackgroundPlane;

    /*
    * the station acts also as a node of the linkedlist
    */
    private Station NextStation = null;
    private Station PreviousStation = null;

    private Dictionary<string, CameraArea> areas;   //Get area GameObject by Area ID

    //private readonly int number;
    private string name;
    private string info;
    private string id;

    public Station(int number)
    {
        //this.number = number;
        this.name = "Station" + number;
        this.id = StationHandler.GenerateRandomID();
        this.info = "-";

        this.areas = new Dictionary<string, CameraArea>();

        Location = new Vector3(number * 256.0f, 0.0f, 0.0f); //the number decides the position of the station

        Init();

    }

    public Station(int number, string name, string id, string info)
    {
        //this.number = number;
        this.name = name;
        this.id = id;
        this.info = info;

        this.areas = new Dictionary<string, CameraArea>();

        Location = new Vector3(number * 256.0f, 0.0f, 0.0f); //the number decides the position of the station

        Init();
    }

    private void Init()
    {
        //Copies the default gameobject with its children
        GameObject CopyedDefaultParent = GameObject.Instantiate(StationHandler.GetDefaultStationParent(), Location, Quaternion.identity);
        CopyedDefaultParent.transform.SetParent(StationHandler.GetStationsParent().transform, true);
        CopyedDefaultParent.name = name;
        Parent = CopyedDefaultParent;

        //Copies the default background plane
        GameObject CopyedBackgroundPlane = GameObject.Instantiate(StationHandler.GetDefaultBackgroundPlane(), Location, Quaternion.identity);
        CopyedBackgroundPlane.transform.SetParent(Parent.transform, true);
        CopyedBackgroundPlane.name = "Background Plane";
        BackgroundPlane = CopyedBackgroundPlane; 
    }

    public string GetID()
    {
        return id;
    }

    public void SetID(string id)
    {
        this.id = id;
    }

    public string GetName()
    {
        return name;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SetInfo(string info)
    {
        this.info = info;
    }

    public string GetInfo()
    {
        return info;
    }

    //returns the staton that is linked next (Linked)
    public Station GetNextStation()
    {
        return NextStation;
    }

    public void LinkNextStation(Station Next)
    {
        this.NextStation = Next;
    }

    //return the previous linked station
    public Station GetPreviousStation()
    {
        return PreviousStation;
    }

    public void LinkPreviousStation(Station Previous)
    {
        PreviousStation = Previous;
    }

    //Returns the gameobject to which all gameobjects of this station are attached.
    public GameObject GetParent()
    {
        return Parent;
    }

    public GameObject GetAreasParent()
    {
        return Parent.transform.Find("Areas").gameObject;
    }

    public GameObject GetWallsParent()
    {
        return Parent.transform.Find("Walls").gameObject;
    }

    //returns all gemobjects that belong to this station
    public List<GameObject> GetAllGameObjects()
    {
        return GetAllChildren(Parent);
    }

    //before the gameobjects are saved, this method should be called to allow easy repositioning on later loads!
    public void GetEverythingReadyToSave()
    {
        foreach (GameObject area in GetAreaObjects())
        {
            GetReadyToSafe(area);
        }
        foreach (GameObject wall in GetWalls())
        {
            GetReadyToSafe(wall);
        }
        GetReadyToSafe(GetBackgroundPlane());
    }

    public void GetReadyToSafe(GameObject gObject)
    {
        gObject.transform.position = new Vector3(gObject.transform.position.x - GetCenterLocation().x, gObject.transform.position.y - GetCenterLocation().y, gObject.transform.position.z - GetCenterLocation().z);
    }

    //registers the CameraArea gameobject with the a random id in the station (returns the area id)
    public string RegisterCameraArea(GameObject gObject)
    {
        string areaID = "area already exits";
        if (areas.Count < 1)
        {
            CameraArea area = new CameraArea(gObject);
            areas.Add(area.GetID(), area);
            gObject.name = area.GetID();
            areaID = area.GetID();

            gObject.transform.SetParent(GetAreasParent().transform, true);
        }
        return areaID;
    }

    public Boolean IsCameraAreaIDRegistered(string id)
    {
        return areas.ContainsKey(id);
    }

    //unregisters the CameraArea with the corresponding id in the station
    public void RemoveCameraArea(string id)
    {
        areas.Remove(id);
    }

    //Returns all gameobjects that are considered a CameraArea in this station.
    public List<GameObject> GetAreaObjects()
    {
        return GetAllChildren(Parent.transform.Find("Areas").gameObject);
    }

    public CameraArea GetCameraArea()
    {
        foreach (GameObject area in GetAreaObjects())
        {
            return GetAreaByID(area.name);
        }
        return null;   
    }

    //Returns the camera area that matches the given id.
    public CameraArea GetAreaByID(string id)
    {
        return areas[id];
    }

    //Returns all gameobjects that are considered a wall in this station.
    public List<GameObject> GetWalls()
    {
        return GetAllChildren(Parent.transform.Find("Walls").gameObject);
    }

    public void SetBackgroundPlane(GameObject Background)
    {
        this.BackgroundPlane = Background;
        Background.transform.position = Location;
    }

    public GameObject GetBackgroundPlane()
    {
        return BackgroundPlane;
    }

    //Returns a vector whose values represent the coordinates of the centre of the station.
    public Vector3 GetCenterLocation()
    {
        return Location;
    }

    private List<GameObject> GetAllChildren(GameObject Root)
    {
        List<GameObject> result = new List<GameObject>();
        if (Root.transform.childCount > 0)
        {
            foreach (Transform VARIABLE in Root.transform)
            {
                RecursiveSearch(result, VARIABLE.gameObject);
            }
        }
        return result;
    }

    private void RecursiveSearch(List<GameObject> List, GameObject Root)
    {
        List.Add(Root);
        if (Root.transform.childCount > 0)
        {
            foreach (Transform VARIABLE in Root.transform)
            {
                RecursiveSearch(List, VARIABLE.gameObject);
            }
        }
    }
}
