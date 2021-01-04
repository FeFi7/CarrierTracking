using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStationInfo : MonoBehaviour
{
    public GameObject StationButtonPrefab;

    public int stationID;
    public string stationName;

    public void TaskOnClick()
    {
        StationController.Instance.OpenInfo(stationID);
    }
}
