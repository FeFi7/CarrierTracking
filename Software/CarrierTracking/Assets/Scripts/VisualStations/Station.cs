using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station
{
    private Vector3 Location;
    private GameObject Parent; //gameobject that is parent to the other gameobjects of the station
    private GameObject BackgroundPlane;
    private Station NextStation = null;
    private Station PreviousStation = null;
    private readonly int Number;

    /*
     * 
     * also the station acts as a node of the linkedlist
     */
    public Station(int Number)
    {
        this.Number = Number;

        Location = new Vector3(Number * 256.0f, 0.0f, 0.0f); // SET TO Sinnvoll (abhäning von der ID)

        init();
    }

    private void init()
    {
        //Copies the default gameobject with its children
        GameObject CopyedDefaultParent = GameObject.Instantiate(StationHandler.GetDefaultStationParent(), Location, Quaternion.identity);
        CopyedDefaultParent.transform.SetParent(StationHandler.GetStationsParent().transform, true);
        CopyedDefaultParent.name = "Station" + Number;
        Parent = CopyedDefaultParent;

        //Copies the default background plane
        GameObject CopyedBackgroundPlane = GameObject.Instantiate(StationHandler.GetDefaultBackgroundPlane(), Location, Quaternion.identity);
        CopyedBackgroundPlane.transform.SetParent(Parent.transform, true);
        CopyedBackgroundPlane.name = "Background Plane";
        BackgroundPlane = CopyedBackgroundPlane; 
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

    public Station GetPreviousStation()
    {
        return PreviousStation;
    }

    public void LinkPreviousStation(Station Previous)
    {
        PreviousStation = Previous;
    }

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

    public List<GameObject> GetAllGameObjects()
    {
        return GetAllChildren(Parent);
    }

    public List<GameObject> GetAreas()
    {
        return GetAllChildren(Parent.transform.Find("Areas").gameObject);
    }

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

    public Vector3 GetCenterLocation()
    {
        return Location;
    }

    public int GetNumber()
    {
        return Number;
    }

    private List<GameObject> GetAllChildren(GameObject Root)
    {
        List<GameObject> result = new List<GameObject>();
        if (Root.transform.childCount > 0)
        {
            foreach (Transform VARIABLE in Root.transform)
            {
                Recursive(result, VARIABLE.gameObject);
            }
        }
        return result;
    }

    private void Recursive(List<GameObject> Rist, GameObject Root)
    {
        Rist.Add(Root);
        if (Root.transform.childCount > 0)
        {
            foreach (Transform VARIABLE in Root.transform)
            {
                Recursive(Rist, VARIABLE.gameObject);
            }
        }
    }
}
