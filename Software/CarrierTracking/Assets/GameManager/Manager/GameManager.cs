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
    public List<Station> LinkedStationsList = new List<Station>();
    public string PathToPictures;
    public int CycleTime;
    


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
    public DStation GetStationByID(string stationid)
    {
        foreach(DStation element in Stations)
        {
            if(stationid.Equals(element.StationID))
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
    public string GetStationID(string stationname)
    {
        
        foreach (DStation element in Stations)
        {
            if (element.name == stationname)
            {
                return element.StationID;
            }
        }
        return "not found";
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
        
        loadSettings();
        //load();
        
        // für Sachen vor Start function

    }

    //@Moritz
    public int generateCarrier(string Name)
    {
        Carrier carrier = new Carrier(Name);
        Carriers.Add(carrier);
        save();
        return carrier.id;
    }

    public int generateCarrier(string Name, string info)
    {
        Carrier carrier = new Carrier(Name,info);        
        Carriers.Add(carrier);
        save();
        return carrier.id;
    }





    public void LoadStationsFromList() {

        Stations.Clear();
        foreach (Station s in StationHandler.GetStationList().GetAllStation())
        {
            DStation station = new DStation(s);
            Stations.Add(station);
        }
    
    }


    public void modifyCarrier(int id, string name) 
    {
        Carrier c = GameManager.Instance.Carriers[id - 1];
        c.name = name;
        save();
   
   
    }

    public void modifyCarrier(int id, string name, string info)
    {
        Carrier c = GameManager.Instance.Carriers[id - 1];
        c.name = name;
        c.info = info;
        save();


    }

    //@Moritz

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
        foreach (Carrier c in Carriers)
        {
            if (c.id == id)
            {
                Carriers.Remove(c);
                break;
            }
        }
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
        s.loadStation();
      
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

            //BinaryFormatter b = new BinaryFormatter();
            //s.Position = 0;
            //return (YourObjectType)b.Deserialize(s);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            GameSettings data = (GameSettings) formatter.Deserialize(stream);
            GameManager.Instance.PathToPictures = data.PathToPictures;
            GameManager.Instance.CycleTime = data.CycleTime;
            stream.Close();
            return true;
        }
        else

            return false;
    }

}