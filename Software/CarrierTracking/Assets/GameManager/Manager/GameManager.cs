using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public List<int> CarrierIDs = new List<int>();
    public List<int> StationIDs = new List<int>();
    public List<Carrier> Carriers = new List<Carrier>();
    public List<DStation> Stations = new List<DStation>();

    //@Moritz
    public Carrier GetCarrierByID (int carrierid)
    {
        foreach(Carrier element in Carriers)
        {
            if(carrierid == element.id)
            {
                return element; 
            }
        }
        return null;
    }

    // public List<SaveObject> SaveObjects { get; private set; }
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }

    }


    //@Moritz 
    public int GetStationID(string stationname)
    {
        int stationID = 0;
        foreach (DStation element in Stations)
        {
            if (element.name == stationname)
            {
                return element.StationID;
            }
        }
        return stationID;
    }


    void Awake()
    {
        // für Sachen vor Start function

    }

    //@Moritz
    public int generateCarrier(string Name, int StationID)
    {
        Carrier carrier = new Carrier(Name, StationID);


        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0, 0);

        Carriers.Add(carrier);

        return carrier.id;
    }

    //public void generateCarrier(string Name, int StationID)
    //{
    //    Carrier carrier = new Carrier(Name, StationID);


    //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    cube.transform.position = new Vector3(0, 0, 0);

    //    Carriers.Add(carrier);

    //}

    public void loadCarrier(string Name, int StationID)
    {
        Carrier carrier = new Carrier(Name, StationID);

        int z = 5 * StationID;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0, z);



    }

    public void generateStation(string Name)
    {
        DStation station = new DStation(Name);


        Stations.Add(station);

    }


    public int generateCarrierID()
    {
        if (CarrierIDs.Count == 0)
        {
            CarrierIDs.Add(1);
            return 1;
        }
        else
        {
            int max = CarrierIDs[CarrierIDs.Count - 1];
            max++;
            CarrierIDs.Add(max);
            return max;
        }

    }

    public int generateStationID()
    {
        if (StationIDs.Count == 0)
        {
            StationIDs.Add(1);
            return 1;
        }
        else
        {
            int max = StationIDs[StationIDs.Count - 1];
            max++;
            instance.StationIDs.Add(max);
            return max;
        }
    }

    public void save()
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath+"/GameData.txt";

        FileStream stream = new FileStream(path, FileMode.Create);
        GameData data = new GameData();
        formatter.Serialize(stream, data);
        stream.Close();



    }

    public bool load()
    {
        string path = Application.persistentDataPath + "/GameData.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            GameManager.Instance.Carriers = data.Carriers;
            GameManager.Instance.Stations = data.Stations;
            GameManager.Instance.CarrierIDs = data.CarrierIDs;
            GameManager.Instance.StationIDs = data.StationIDs;
            stream.Close();
            foreach (DStation s in GameManager.Instance.Stations)
            {
                loadStation(s);
            }
            return true;
        }
        else

            return false;


    }

    public void loadStation(DStation s)
    {
        foreach (Carrier c in GameManager.Instance.Carriers)
        {
            if (c.StationID == s.StationID)
            {
                loadCarrier(c.name, s.StationID);
            }
        }

    }

}
