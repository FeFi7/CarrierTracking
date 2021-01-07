using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStationInfo : MonoBehaviour
{
    public GameObject StationButtonPrefab;

    public int stationID;
    public string stationName;

    //public void TaskOnClick()
    //{
    //    StationController.Instance.OpenInfo(transform.parent.gameObject.GetComponent<PrefabStationInfo>().stationID);
    //}

    public void ShowStation()
    {
        Station station = StationHandler.GetStationList().GetStationByID(stationID.ToString());
        StationHandler.ViewSpecialStation(station);
    }
}
