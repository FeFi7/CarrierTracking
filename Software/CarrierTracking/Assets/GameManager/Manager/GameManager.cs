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
    public string PathToPictures;
    public int CycleTime;
    private Dictionary<int, GameObject> CarrierDict = new Dictionary<int, GameObject>();


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

    //@Moritz
    public DStation GetStationByID(int stationid)
    {
        foreach(DStation element in Stations)
        {
            if(stationid == element.StationID)
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

    //@Moritz
    public string GetCarrierByName(string stationName) 
    {
        foreach (Carrier element in Carriers)
        {
            if (stationName == element.name)
            {
                return element.id.ToString();
            }
        }
        return null;
    }


    void Awake()
    {
<<<<<<< Updated upstream
        load();
        loadSettings();
=======
        loadSettings();
        load();
        // für Sachen vor Start function

>>>>>>> Stashed changes
    }

    //@Moritz
    public int generateCarrier(string Name, int StationID)
    {
        Carrier carrier = new Carrier(Name, StationID);
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = carrier.name + carrier.id.ToString();
        cube.transform.position = new Vector3(0, 0, 0);
        CarrierDict.Add(carrier.id, cube);
        Carriers.Add(carrier);
        save();
        return carrier.id;
    }

    //public void generateCarrier(string Name, int StationID)
    //{
    //    Carrier carrier = new Carrier(Name, StationID);


    //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    cube.transform.position = new Vector3(0, 0, 0);

    //    Carriers.Add(carrier);

    //}
    public void deleteCarrierByID(int id) {
        GameObject carrier = CarrierDict[id];
        Carriers[id - 1] = null;
        Destroy(carrier);
        save();
    }

    

<<<<<<< Updated upstream
    public void loadCarrier(string Name,int cid,  int StationID)
    {
        foreach (Carrier c in Carriers)
        {
            if (c.id == cid)
            {
                Carrier carrier = new Carrier(Name, c.id, StationID);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = carrier.name + carrier.id.ToString();
                cube.transform.position = new Vector3(0, 0, 0);
                CarrierDict.Add(carrier.id, cube);
            }
            else Debug.Log("Carrier not found");
        }
        
       
=======
    public void modifyCarrier(int id, string name) {
        Carrier c = GameManager.Instance.Carriers[id - 1];
        c.name = name;
        save();
    }
    public void loadCarrier(string Name, int StationID)
    {
        Carrier carrier = new Carrier(Name, StationID);

        
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0, 0);
        CarrierDict.Add(carrier.id, cube);


>>>>>>> Stashed changes
    }

    //@Moritz
    public int generateStation(string Name)
    {
        DStation station = new DStation(Name);


        Stations.Add(station);
        save();
        return station.StationID;
    }

    //public void generateStation(string Name)
    //{
    //    DStation station = new DStation(Name);


    //    Stations.Add(station);

    //}


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

    public void deleteCarrierByID(int id)
    {
        GameObject carrier = CarrierDict[id];
        Carriers[id - 1] = null;
        Destroy(carrier);
        save();
    }

    public void modifyCarrierByID(int id, string name)
    {
        Carrier c = GameManager.Instance.Carriers[id - 1];
        c.name = name;
        save();
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
        //Debug.Log(path);
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
                loadCarrier(c.name,c.id, s.StationID);
            }
        }

    }
    public void saveSettings()
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/GameSettings.txt";

        FileStream stream = new FileStream(path, FileMode.Create);
        GameSettings data = new GameSettings();
        formatter.Serialize(stream, data);
        stream.Close();

    }
    public bool loadSettings()
    {
        string path = Application.persistentDataPath + "/GameSettings.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameSettings data = formatter.Deserialize(stream) as GameSettings;
            GameManager.Instance.PathToPictures = data.PathToPictures;
            GameManager.Instance.CycleTime = data.CycleTime;
            stream.Close();
            return true;
        }
        else

            return false;
    }

}