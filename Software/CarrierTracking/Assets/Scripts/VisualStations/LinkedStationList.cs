using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedStationList
{
    Station start = null;
    Station end = null;

    Station current = null;

    int counter = 0;
    int removes = 0;

    public LinkedStationList()
    {

    }

    public void Add(Station station)
    {
        if(counter-removes > 0)
        {
           
            end.setNextStation(station);
            station.setPreviousStation(end);

            start.setPreviousStation(station);
            station.setNextStation(start);

            end = station;
        }
        else
        {
            start = station;
            end = station;

            station.setNextStation(station);
            station.setPreviousStation(station);
        }

        current = station;
        counter++;
    }

    public void RemoveCurrent()
    {

        current.getPreviousStation().setNextStation(current.getNextStation());
        current.getNextStation().setPreviousStation(current.getPreviousStation());

        setCurrent(current.getPreviousStation());

        removes++;
    }

    public void setCurrent(Station station)
    {
        current = station; 
    }

    public Station GetCurrent()
    {
        return current;
    }

    public int getAmount()
    {
        return counter-removes;
    }

    public int getNextID()
    {
        return counter + 1;
    }

    public List<GameObject> GetAllAreas()
    {
        List<GameObject> areas = new List<GameObject>();
        Station next = start;
        for (int i = 0; i < counter; i++)
        {
            foreach (GameObject area in next.getAreas())
            {
                areas.Add(area);
            }
            next = current.getNextStation();
        }
        return areas;
    }
}
