using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station
{
    Vector3 location;

    GameObject Parent;
    GameObject BackgroundPlane;

    Station NextStation = null;
    Station PreviousStation = null;

    int id;

    public Station(int id)
    {
        this.id = id;

        location = new Vector3(id * 256.0f, 0.0f, 0.0f); // SET TO Sinnvoll (abhäning von der ID)

        init();
    }

    private void init()
    {
        GameObject CopyedDefaultParent = GameObject.Instantiate(StationHandler.getDefaultStationParent(), location, Quaternion.identity);
        CopyedDefaultParent.transform.SetParent(StationHandler.getStationsParent().transform, true);
        CopyedDefaultParent.name = "Station" + id;
        Parent = CopyedDefaultParent;

        GameObject CopyedBackgroundPlane = GameObject.Instantiate(StationHandler.getDefaultBackgroundPlane(), location, Quaternion.identity);
        CopyedBackgroundPlane.transform.SetParent(Parent.transform, true);
        CopyedBackgroundPlane.name = "Background Plane";
        BackgroundPlane = CopyedBackgroundPlane; 
    }

    public Station getNextStation()
    {
        return NextStation;
    }

    public void setNextStation(Station next)
    {
        this.NextStation = next;
    }

    public Station getPreviousStation()
    {
        return PreviousStation;
    }

    public void setPreviousStation(Station previous)
    {
        this.PreviousStation = previous;
    }

    public GameObject getParent()
    {
        return Parent;
    }

    public GameObject GetAreasParent()
    {
        return Parent.transform.Find("Areas").gameObject;
    }

    public GameObject getWallsParent()
    {
        return Parent.transform.Find("Walls").gameObject;
    }

    public List<GameObject> getAllGameObjects()
    {
        return getAllChildren(Parent);
    }

    public List<GameObject> getAreas()
    {
        return getAllChildren(Parent.transform.Find("Areas").gameObject);
    }

    public List<GameObject> getWalls()
    {
        return getAllChildren(Parent.transform.Find("Walls").gameObject);
    }

    public void setBackgroundPlane(GameObject Background)
    {
        this.BackgroundPlane = Background;
        Background.transform.position = location;
    }

    public GameObject getBackgroundPlane()
    {
        return BackgroundPlane;
    }


    public Vector3 getCenterLocation()
    {
        return location;
    }

    public int getID()
    {
        return id;
    }

    private List<GameObject> getAllChildren(GameObject root)
    {
        List<GameObject> result = new List<GameObject>();
        if (root.transform.childCount > 0)
        {
            foreach (Transform VARIABLE in root.transform)
            {
                recursive(result, VARIABLE.gameObject);
            }
        }
        return result;
    }

    private void recursive(List<GameObject> list, GameObject root)
    {
        list.Add(root);
        if (root.transform.childCount > 0)
        {
            foreach (Transform VARIABLE in root.transform)
            {
                recursive(list, VARIABLE.gameObject);
            }
        }
    }
}
