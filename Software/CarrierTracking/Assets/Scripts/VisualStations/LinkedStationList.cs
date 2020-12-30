using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedStationList
{
    private List<Station> Stations;
    private int Number = 1;

    private Station Start = null;
    private Station End = null;
    private Station Selected = null;

    public LinkedStationList()
    {
        Stations = new List<Station>();
    }

    //registers a station in the doubly linked list
    public void Add(Station Station)
    {
        if (Stations.Count < 1)
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
        Stations.Add(Station);
        Number++;
    }

    public List<Station> GetAllStation()
    {
        return Stations;
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
        Station Bridge = Selected;

        Bridge.GetPreviousStation().LinkNextStation(Bridge.GetNextStation());
        Bridge.GetNextStation().LinkPreviousStation(Bridge.GetPreviousStation());

        Stations.Remove(Bridge);
        SelectPrevious();
    }

    public int GetSize()
    {
        return Stations.Count;
    }

    public int GetNextStationNumber()
    {
        return Number;
    }

    public List<GameObject> GetAllAreas()
    {
        List<GameObject> areas = new List<GameObject>();
        Station Next = Start;
        for (int i = 0; i < Stations.Count; i++)
        {
            foreach (GameObject area in Next.GetAreas())
            {
                areas.Add(area);
            }
            Next = Selected.GetNextStation();
        }
        return areas;
    }
}
