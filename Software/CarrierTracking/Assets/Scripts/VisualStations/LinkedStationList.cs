using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedStationList
{
    private readonly List<Station> stations;
    private int number = 1;

    private Station start = null;
    private Station end = null;
    private Station selected = null;

    public LinkedStationList()
    {
        stations = new List<Station>();
    }

    //registers a station in the list
    public void Add(Station Station)
    {
        if (stations.Count < 1)
        {
            start = Station;
            end = Station;

            Station.LinkNextStation(Station);
            Station.LinkPreviousStation(Station);
        }
        else
        {
            end.LinkNextStation(Station);
            Station.LinkPreviousStation(end);
            Station.LinkNextStation(start);
            start.LinkPreviousStation(Station);
            end = Station;
        }
        stations.Add(Station);
        number++;
    }

    public List<Station> GetAllStation()
    {
        return stations;
    }

    public void SelectNext()
    {
        selected = selected.GetNextStation(); 
    }

    public void SelectPrevious()
    {
        selected = selected.GetPreviousStation();
    }

    public void SelectNewest()
    {
        selected = end;
    }

    public void Select(Station station)
    {
        selected = station;
    }

    public Station GetSelected()
    {
        return selected;
    }

    public void DeleteSelected()
    {
        Station bridge = selected;

        bridge.GetPreviousStation().LinkNextStation(bridge.GetNextStation());
        bridge.GetNextStation().LinkPreviousStation(bridge.GetPreviousStation());

        stations.Remove(bridge);
        SelectPrevious();

    }

    public int GetSize()
    {
        return stations.Count;
    }

    public int GetNextStationNumber()
    {
        return number;
    }

    public List<GameObject> GetAllAreas()
    {
        List<GameObject> areas = new List<GameObject>();
        Station next = start;
        for (int i = 0; i < stations.Count; i++)
        {
            foreach (GameObject area in next.GetAreaObjects())
            {
                areas.Add(area);
            }
            next = next.GetNextStation();
        }
        return areas;
    }


    public List<string> GetAllNames() { 
        List<string> stationNames = new List<string>();

        Station next = start;

        for (int i = 0; i < stations.Count; i++)
        {
            stationNames.Add(next.GetName());
            next = next.GetNextStation();
        }
        return stationNames;
    }

    public Station GetStationByID(string id)
    {
        Station current = start;
        for (int i = 0; i < stations.Count; i++)
        {
            if (current.GetID().Equals(id))
                return current;
            current = current.GetNextStation();
        }
        return null;
    }

    public Station GetStationByName(string name)
    {
        Station current = start;
        for (int i = 0; i < stations.Count; i++)
        {
            if (current.GetName().Equals(name))
                return current;
            current = current.GetNextStation();
        }
        return null;

    }
}
