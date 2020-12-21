using System;
using UnityEngine;

public class StationHandler : MonoBehaviour
{
    public Camera EditCam;
    public Camera MainCam; //= Camera.main

    ArrayList stations;

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

        if (current - last > 100)
        {
            last = current;
            if(Input.GetKey(KeyCode.N) && ctrl)
            {
                //TODO: Create new Station
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                //TODO: siwtch viewed station to the left
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                //TODO: siwtch viewed station to the right
            }
        }
    }

    public void setViewedStation(Station station)
    {
        //TODO: machen!...
    }

    public Station getViewedStation()
    {
        //TODO: machen!...
    }

    public void createStation()
    {
        //TODO: machen!... (ggf Parameter)
    }

    public void loadStation(Station station)
    {
       //TODO: nicht sciher ob schon fertig
       stations.Add(station);
    }
}
