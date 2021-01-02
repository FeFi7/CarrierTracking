using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedStationList
{
    private List<Station> stations;
    private int number = 1;

    private Station Start = null;
    private Station End = null;
    private Station Selected = null;

    public LinkedStationList()
    {
        stations = new List<Station>();
    }

    //registers a station in the doubly linked list
    public void Add(Station Station)
    {
        if (stations.Count < 1)
        {
            Start = Station;
            End = Station;

            Station.LinkNextStation(Station);
            Station.LinkPreviousStation(Station);
        }
        else
        {
            End.LinkNextStation(Station);
            Station.LinkPreviousStation(End);
            Station.LinkNextStation(Start);
            Start.LinkPreviousStation(Station);
            End = Station;
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
        Selected = Selected.GetNextStation(); 
    }

    public void SelectPrevious()
    {
        Selected = Selected.GetPreviousStation();
    }

    public void SelectNewest()
    {
        Selected = End;
    }

    public Station GetSelected()
    {
        return Selected;
    }

    public void DeleteSelected()
    {
        Station bridge = Selected;

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
        Station Next = Start;
        for (int i = 0; i < stations.Count; i++)
        {
            foreach (GameObject area in Next.GetAreaObjects())
            {
                areas.Add(area);
            }
            Next = Selected.GetNextStation();
        }
        return areas;
    }


    public List<string> GetAllNames() { 
        List<string> stationNames = new List<string>();

        Station next = Start;

        for (int i = 0; i < stations.Count; i++)
        {
            stationNames.Add(next.GetName());
            next = Selected.GetNextStation();
        }
        return stationNames;
    }

    public Station GetStationByID(string id)
    {
        Station current = Start;
        for (int i = 0; i < stations.Count; i++)
        {
            if (current.GetID().Equals(id))
                return current;
            current = Selected.GetNextStation();
        }
        return null;

    }

    public Station GetStationByName(string name)
    {
        Station current = Start;
        for (int i = 0; i < stations.Count; i++)
        {
            if (current.GetName().Equals(name))
                return current;
            current = Selected.GetNextStation();
        }
        return null;

    }
}
