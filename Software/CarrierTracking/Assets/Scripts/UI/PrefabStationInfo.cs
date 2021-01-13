using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStationInfo : MonoBehaviour
{
    //Set by inspector
    public GameObject stationButtonPrefab;

    public int stationID;
    public string stationName;

    //Öffnet angeklickte Station 
    public void ShowStation()
    {
        Station station = StationHandler.GetStationList().GetStationByID(stationID.ToString());
        StationHandler.ViewSpecialStation(station);
    }
}
