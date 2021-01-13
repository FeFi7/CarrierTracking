using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StationHandler : MonoBehaviour
{
    public Camera editCam;
    public Camera mainCam;

    //Set by inspector
    public GameObject defaultBackgroundPlane;
    public GameObject stationsParent;   //all gameobjects belonging to a station are subordinate to this object
    public GameObject defaultStationParent; //for better organisation each individual station has its own parent, this is the tamplate

    //Set for public static access
    public static GameObject DBP = null;
    public static GameObject SP = null;
    public static GameObject DSP = null;

    public static Camera EC;
    public static Camera MC;

    public static LinkedStationList stations = new LinkedStationList();

    //TODO:
    /* - png als background laden
     * - 
     */

    void Start()
    {
        DBP = defaultBackgroundPlane;
        SP = stationsParent;
        DSP = defaultStationParent;
        EC = editCam;
        MC = mainCam;
    }

    long last = -1L;

    /*
     * only as an example, since all important methods in this class are static, they can be integrated into the ui relatively easily.
     */
    void Update()
    {
        long current = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        
        //check if control is pressed


        if (current - last > 64) //Cooldown
        {
            if (Input.GetKey(KeyCode.LeftControl)
                || Input.GetKey(KeyCode.RightControl)) //check if an control/strg key is pressed
            {
                last = current;
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

    /*
     *Deletes all gameobjects belonging to the selected station and all registrations in the backend of the StationHandler. 
     */
    public static void DeleteSelectedStation()
    {
        Station selectedParent = stations.GetSelected();
        selectedParent.GetParent().name = "deleted...";
        List<GameObject> toDestroy = selectedParent.GetAllGameObjects();

        stations.DeleteSelected();

        foreach (GameObject gameObject in toDestroy)
        {
            Destroy(gameObject);
        }
        Destroy(selectedParent.GetParent());
    }

    //the cameras jump to the next linked station
    public static void ViewNextStation()
    {
        stations.SelectNext();
    }

    //the cameras jump to the previous linked station
    public static void ViewPreviousStation()
    {
        stations.SelectPrevious();
    }

    //the cameras jump to given station
    public static void ViewSpecialStation(Station station)
    {
        if(station == null)
        {
            Debug.Log("Special Station to view is null...");
            return;
        }

        if(!stations.GetSelected().GetID().Equals(station.GetID()))
        {
            stations.Select(station);
        }
    }

    //Returns the currently selected station object 
    public static Station GetSelectedStation()
    {
        return stations.GetSelected();
    }

    //creates a new station 
    public static Station CreateStation()
    {
        Station NewStation = new Station(stations.GetNextStationNumber());
        
        stations.Add(NewStation);
        stations.SelectNewest();
        
        return NewStation;
    }

    //loads an already known station
    public static Station LoadStation(string name, string id, string info, List<GameObject> walls, List<GameObject> areas, GameObject background)
    {
        Station LoadedStation = new Station(stations.GetNextStationNumber(), name, id, info);

        background.transform.position = new Vector3(LoadedStation.GetCenterLocation().x + background.transform.position.x, LoadedStation.GetCenterLocation().y + background.transform.position.y, LoadedStation.GetCenterLocation().z + background.transform.position.z);
        background.transform.SetParent(LoadedStation.GetParent().transform, false);
        LoadedStation.SetBackgroundPlane(background);

        GameObject WallsParent = LoadedStation.GetWallsParent();
        foreach (GameObject wall in walls)
        {
            wall.transform.SetParent(WallsParent.transform, false);
            wall.name = "Wall" + WallsParent.transform.childCount;

            //positions the gameobject at the new station positioned
            wall.transform.position = new Vector3(LoadedStation.GetCenterLocation().x + wall.transform.position.x, LoadedStation.GetCenterLocation().y + wall.transform.position.y, LoadedStation.GetCenterLocation().z + wall.transform.position.z);
        }

        GameObject AreasParent = LoadedStation.GetAreasParent();
        foreach (GameObject area in areas)
        {
            area.transform.SetParent(AreasParent.transform, false);

            //positions the gameobject at the new station positioned
            area.transform.position = new Vector3(LoadedStation.GetCenterLocation().x + area.transform.position.x, LoadedStation.GetCenterLocation().y + area.transform.position.y, LoadedStation.GetCenterLocation().z + area.transform.position.z);

            //registers the gameobject with the corresponding id in the station. Assumes that the object is named after its ID.
            LoadedStation.RegisterCameraArea(area.name, area);
        }

        //TODO: register CameraAreas in Station

        stations.Add(LoadedStation);
        stations.SelectNewest();

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

    //returns all camera areas gameobjects of all stations
    public static List<GameObject> GetAllAreas()
    {
        return stations.GetAllAreas();
    }

    private static System.Random random = new System.Random();

    //method for generating a random id for the station and the camera areas
    public static string GenerateRandomID()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 10)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    //returns a list with all the names of the different stations
    public List<string> GetAllStationNames()
    {
        return stations.GetAllNames();
    }

    //returns the linked list for even more station search options
    public static LinkedStationList GetStationList()
    {
        return stations;
    }
}
