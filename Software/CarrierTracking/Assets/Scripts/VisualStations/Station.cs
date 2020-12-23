using System;
using System.Collections;
using UnityEngine;

public class Station
{


    Vector3 location;

    GameObject Parent;

    GameObject Background;
    ArrayList areas;
    ArrayList walls;

    int id;

    public Station(int id)
    {
        this.id = id;
        location = new Vector3(id * 256.0f, 0.0f, 0.0f); // SET TO Sinnvoll (abhäning von der ID)
        areas = new ArrayList();
        walls = new ArrayList();

        init();
    }

    private void init()
    {

        //copyedArea.transform.position = new Vector3(copyedArea.transform.position.x, copyedArea.transform.position.y, copyedArea.transform.position.z);
    }

    public void DefineParent(GameObject Parent)
    {
        this.Parent = Parent;
    }

    public GameObject getParent()
    {
        return Parent;
    }

    public void setBackgroundPlane(GameObject Background)
    {
        this.Background = Background;
        Background.transform.position = location;
    }

    public GameObject getBackgroundPlane()
    {
        return Background;
    }

    public Vector3 getCenter()
    {
        return location;
    }

    public int getID()
    {
        return id;
    }
}
