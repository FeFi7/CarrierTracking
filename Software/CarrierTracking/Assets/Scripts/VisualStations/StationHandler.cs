using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationHandler : MonoBehaviour
{
    public Camera EditCam;
    public Camera MainCam;

    //Set by inspector
    public GameObject DefaultBackgroundPlane;
    public GameObject StationsParent;
    public GameObject DefaultStationParent;

    public static GameObject DBP = null;
    public static GameObject SP = null;
    public static GameObject DSP = null;

    public static LinkedStationList stations = new LinkedStationList();

    void Start()
    {
        DBP = DefaultBackgroundPlane;
        SP = StationsParent;
        DSP = DefaultStationParent;
    }

    long last = -1L;

    void Update()
    {
        //check if control is pressed
        bool ctrl = Input.GetKey(KeyCode.LeftControl)
         || Input.GetKey(KeyCode.RightControl);

        long current = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (current - last > 128) //Cooldown
        {
            last = current;
            
            if(Input.GetKey(KeyCode.N) && ctrl) //Check if control + n is pressed
            {
                createStation();
            }

            if (Input.GetKey(KeyCode.R) && ctrl) //Check if control + n is pressed
            {
                RemoveCurrentStation();
            }

            if (MainCam != null)
            {
                if (MainCam.enabled)
                {
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        ViewNextStation();
                    }
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        ViewPreviousStation();
                    }
                }
            }
            
        }
    }

    public void RemoveCurrentStation()
    {

        //Station currentStation = stations.GetCurrent();
        //foreach (GameObject gameObject in currentStation.getAllGameObjects())
        //{
        //    Destroy(gameObject);
        //}

        //Destroy(currentStation.getParent());

        //stations.RemoveCurrent();

        //ViewStation(stations.GetCurrent());

        Station ToRemove = stations.GetCurrent();
        ToRemove.getParent().name = "removed...";
        List<GameObject> ToDestroy = ToRemove.getAllGameObjects();

        stations.RemoveCurrent();

        foreach (GameObject gameObject in ToDestroy)
        {
            Destroy(gameObject);
        }
        
    }

    public void ViewStation(Station station)
    {
        MainCam.transform.position = new Vector3(station.getCenterLocation().x, station.getCenterLocation().y + 64.0f, station.getCenterLocation().z);
        EditCam.transform.position = new Vector3(station.getCenterLocation().x, station.getCenterLocation().y + 64.0f, station.getCenterLocation().z + 30.0f);
        stations.setCurrent(station);
    }

    public void ViewNextStation()
    {
        stations.setCurrent(stations.GetCurrent().getNextStation());
        ViewStation(stations.GetCurrent());
    }

    public void ViewPreviousStation()
    {
        stations.setCurrent(stations.GetCurrent().getPreviousStation());
        ViewStation(stations.GetCurrent());
    }
    
    public static Station getViewedStation()
    {
        return stations.GetCurrent();
    }

    public Station createStation()
    {
        Station NewStation = new Station(stations.getNextID());
        stations.Add(NewStation);
        ViewStation(NewStation);

        return NewStation;
    }

    public Station loadStation(List<GameObject> walls, List<GameObject> areas, GameObject background)
    {
        Station LoadedStation = new Station(stations.getNextID());

        LoadedStation.setBackgroundPlane(background);

        GameObject WallsParent = LoadedStation.getWallsParent();
        foreach (GameObject wall in walls)
        {
            wall.transform.SetParent(WallsParent.transform, false);
            wall.name = "Wall" + WallsParent.transform.childCount;
            wall.AddComponent<RemoveObject>();

            wall.transform.position = new Vector3(LoadedStation.getCenterLocation().x + wall.transform.position.x, LoadedStation.getCenterLocation().y + wall.transform.position.y, LoadedStation.getCenterLocation().z + wall.transform.position.z);
        }

        GameObject AreasParent = LoadedStation.GetAreasParent();
        foreach (GameObject area in areas)
        {
            area.transform.SetParent(AreasParent.transform, false);
            area.name = "Area" + AreasParent.transform.childCount;
            area.AddComponent<RemoveObject>();

            area.transform.position = new Vector3(LoadedStation.getCenterLocation().x + area.transform.position.x, LoadedStation.getCenterLocation().y + area.transform.position.y, LoadedStation.getCenterLocation().z + area.transform.position.z);
        }

        stations.Add(LoadedStation);
        ViewStation(LoadedStation);
        return LoadedStation;
    }

    public static GameObject getDefaultBackgroundPlane()
    {
        return DBP;
    }

    public static GameObject getStationsParent()
    {
        return SP;
    }

    public static GameObject getDefaultStationParent()
    {
        return DSP;
    }

    public static List<GameObject> GetAllAreas()
    {
        return stations.GetAllAreas();
    }
}
