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

    //Set for public static access
    public static GameObject DBP = null;
    public static GameObject SP = null;
    public static GameObject DSP = null;

    public static LinkedStationList Stations = new LinkedStationList();

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

        if (current - last > 64) //Cooldown
        {
            last = current;
            
            if(Input.GetKey(KeyCode.N) && ctrl) //Check if control + n is pressed
            {
               CreateStation();
            }

            if (Input.GetKey(KeyCode.R) && ctrl) //Check if control + n is pressed
            {
                DeleteSelectedStation();
            }

            //if (MainCam != null)
            //{
                //if (MainCam.enabled)
                //{
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        ViewNextStation();
                    }
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        ViewPreviousStation();
                    }
                //}
            //}
            
        }
    }

    public void DeleteSelectedStation()
    {
        Station SelectedParent = Stations.GetSelected();
        SelectedParent.GetParent().name = "deleted...";
        List<GameObject> ToDestroy = SelectedParent.GetAllGameObjects();

        Stations.DeleteSelected();

        foreach (GameObject gameObject in ToDestroy)
        {
            Destroy(gameObject);
        }
        Destroy(SelectedParent.GetParent());
        PositionCamToSelectedStation();
    }

    public void ViewNextStation()
    {
        Stations.SelectNext();
        PositionCamToSelectedStation();
    }

    public void ViewPreviousStation()
    {
        Stations.SelectPrevious();
        PositionCamToSelectedStation();
    }

    public void PositionCamToSelectedStation()
    {
        Station Selected = Stations.GetSelected();
        MainCam.transform.position = new Vector3(Selected.GetCenterLocation().x, Selected.GetCenterLocation().y + 64.0f, Selected.GetCenterLocation().z);
        EditCam.transform.position = new Vector3(Selected.GetCenterLocation().x, Selected.GetCenterLocation().y + 64.0f, Selected.GetCenterLocation().z + 30.0f);
    }

    public static Station GetSelectedStation()
    {
        return Stations.GetSelected();
    }

    public Station CreateStation()
    {
        Station NewStation = new Station(Stations.GetNextStationNumber());
        Stations.Add(NewStation);
        Stations.SelectNewest();
        PositionCamToSelectedStation();
        return NewStation;
    }

    public Station LoadStation(List<GameObject> walls, List<GameObject> areas, GameObject background)
    {
        Station LoadedStation = new Station(Stations.GetNextStationNumber());

        LoadedStation.SetBackgroundPlane(background);

        GameObject WallsParent = LoadedStation.GetWallsParent();
        foreach (GameObject wall in walls)
        {
            wall.transform.SetParent(WallsParent.transform, false);
            wall.name = "Wall" + WallsParent.transform.childCount;
            wall.AddComponent<RemoveObject>(); //

            wall.transform.position = new Vector3(LoadedStation.GetCenterLocation().x + wall.transform.position.x, LoadedStation.GetCenterLocation().y + wall.transform.position.y, LoadedStation.GetCenterLocation().z + wall.transform.position.z);
        }

        GameObject AreasParent = LoadedStation.GetAreasParent();
        foreach (GameObject area in areas)
        {
            area.transform.SetParent(AreasParent.transform, false);
            area.name = "Area" + AreasParent.transform.childCount;
            area.AddComponent<RemoveObject>();

            area.transform.position = new Vector3(LoadedStation.GetCenterLocation().x + area.transform.position.x, LoadedStation.GetCenterLocation().y + area.transform.position.y, LoadedStation.GetCenterLocation().z + area.transform.position.z);
        }

        Stations.Add(LoadedStation);
        Stations.SelectNewest();
        PositionCamToSelectedStation();

        return LoadedStation;
    }

    public static GameObject GetDefaultBackgroundPlane()
    {
        return DBP;
    }

    public static GameObject GetStationsParent()
    {
        return SP;
    }

    public static GameObject GetDefaultStationParent()
    {
        return DSP;
    }

    public static List<GameObject> GetAllAreas()
    {
        return Stations.GetAllAreas();
    }
}
