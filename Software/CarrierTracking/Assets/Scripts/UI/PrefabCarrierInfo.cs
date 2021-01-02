using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabCarrierInfo : MonoBehaviour
{
    public GameObject CarrierButtonPrefab;

    public int CarrierID;
    public string CarrierName;

    public void TaskOnClick()
    {
        CarrierController.Instance.OpenInfo(CarrierID);
    }
}
