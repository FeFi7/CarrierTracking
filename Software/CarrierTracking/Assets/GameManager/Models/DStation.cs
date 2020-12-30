using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DStation
{
    public string name;
    public int StationID;

    public DStation(string name)
    {
        this.name = name;
        this.StationID = GameManager.Instance.generateStationID();


    }

}
