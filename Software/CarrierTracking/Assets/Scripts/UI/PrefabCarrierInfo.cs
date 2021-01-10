using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabCarrierInfo : MonoBehaviour
{
    //Set by inspector
    public GameObject CarrierButtonPrefab;

    public int CarrierID;
    public string CarrierName;

    //Öffnet das Update/Info Panel des ausgewählten Carriers 
    public void TaskOnClick()
    {
        CarrierController.Instance.OpenInfo(CarrierID);

        Station station = StationHandler.GetStationList().GetStationByID(CarrierID.ToString());
        StationHandler.ViewSpecialStation(station);
    }

}
