using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Carrier
{
    public string name;
    public int id;    
    public string info;

    public float[] position;

    public Carrier(string name)
    {
        this.name = name;        
        this.id = GameManager.Instance.generateCarrierID();

    }

    public Carrier(string name, string info)
    {
        this.name = name;        
        this.id = GameManager.Instance.generateCarrierID();
        this.info = info;

    }

    public Carrier(string name,int cid)
    {
        this.name = name;        
        this.id = cid;
    }

    public Carrier(string name, int cid,string info)
    {
        this.name = name;
        this.id = cid;
        this.info = info;

    }
}
