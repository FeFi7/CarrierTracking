using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Carrier
{
    public string name;
    public int id, StationID;
    public string info;

    public float[] position;

    public Carrier(string name, int StationID)
    {
        this.name = name;
        this.StationID = StationID;
        this.id = GameManager.Instance.generateCarrierID();


    }

    public Carrier(string name, int StationID, string info)
    {
        this.name = name;
        this.StationID = StationID;
        this.id = GameManager.Instance.generateCarrierID();
        this.info = info;

    }

    public Carrier(string name,int cid,  int StationID)
    {
        this.name = name;
        this.StationID = StationID;
        this.id = cid;


    }

    public Carrier(string name, int cid, int StationID,string info)
    {
        this.name = name;
        this.StationID = StationID;
        this.id = cid;
        this.info = info;



    }
}
