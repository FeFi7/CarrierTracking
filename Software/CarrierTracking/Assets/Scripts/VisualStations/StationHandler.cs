using System;
using System.Collections;
using UnityEngine;

public class StationHandler : MonoBehaviour
{
    public Camera EditCam;
    public Camera MainCam; //= Camera.main

    public GameObject DefaultBackgroundPlane;
    public GameObject StationsParent;
    public GameObject DefaultParent;

    static ArrayList stations;
    static int currentViewedStation = -1;

    void Start()
    {
        stations = new ArrayList();
    }

    long last = -1L;

    void Update()
    {
        bool ctrl = Input.GetKey(KeyCode.LeftControl)
         || Input.GetKey(KeyCode.RightControl);

        long current = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (current - last > 128) //Cooldown
        {
            last = current;
            
            if(Input.GetKey(KeyCode.N) && ctrl) //Check if control + n is pressed
            {
                Station station = createStation();

                stations.Add(station);

                ViewStation(station);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                currentViewedStation++;
                currentViewedStation %= stations.Count;

                ViewStation((Station)stations[currentViewedStation]);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                currentViewedStation -= 1;
                if (currentViewedStation < 0)
                    currentViewedStation = stations.Count-1;

                ViewStation((Station)stations[currentViewedStation]);
            }
        }
    }

    public void ViewStation(Station station)
    {
        //station
        // MainCam.transform.Rotate(new Vector3(0.0f, 0.0f, 0.05f));

        MainCam.transform.position = new Vector3(station.getCenter().x, station.getCenter().y + 69.0f, station.getCenter().z);
        EditCam.transform.position = new Vector3(station.getCenter().x, station.getCenter().y + 69.0f, station.getCenter().z + 30.0f);
        currentViewedStation = station.getID()-1;

    }

    
    public static Station getViewedStation()
    {
        return (Station) stations[currentViewedStation];    //placeholder, weil sonst VS meckert! -- wegmachen
        //TODO: machen!...
    }

    public Station createStation()
    {
        Station NewStation = new Station(stations.Count + 1);

        GameObject CopyedDefaultParent = GameObject.Instantiate(DefaultParent, NewStation.getCenter(), Quaternion.identity);
        CopyedDefaultParent.transform.SetParent(StationsParent.transform, true);
        CopyedDefaultParent.name = "Station" + NewStation.getID();
        NewStation.DefineParent(CopyedDefaultParent);

        GameObject CopyedBackgroundPlane = GameObject.Instantiate(DefaultBackgroundPlane, NewStation.getCenter(), Quaternion.identity);
        CopyedBackgroundPlane.transform.SetParent(NewStation.getParent().transform, true);
        CopyedBackgroundPlane.name = "Background Plane";
        NewStation.setBackgroundPlane(CopyedBackgroundPlane);

        return NewStation;
    }
}
