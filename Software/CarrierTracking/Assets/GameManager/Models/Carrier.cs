using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Carrier
{
    public string name;
    public int id, StationID;

    public float[] position;

    public Carrier(string name, int StationID)
    {
        this.name = name;
        this.StationID = StationID;
        this.id = GameManager.Instance.generateCarrierID();


    }

}
