using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public List<int> CarrierIDs = new List<int>();
    public List<int> StationIDs = new List<int>();
    public List<Carrier> Carriers = new List<Carrier>();
    public List<DStation> Stations = new List<DStation>();

    public GameData()
    {
        this.CarrierIDs = GameManager.Instance.CarrierIDs;
        this.StationIDs = GameManager.Instance.StationIDs;
        this.Carriers = GameManager.Instance.Carriers;
        this.Stations = GameManager.Instance.Stations;
        

    }

}
