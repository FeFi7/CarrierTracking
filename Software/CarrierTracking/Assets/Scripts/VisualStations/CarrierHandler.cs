using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Drawing;
using Models;

[System.Serializable]
public class CarrierHandler : MonoBehaviour
{
    private static CarrierHandler instance;

    public GameObject sampleCarrier;
    public GameObject AreasParent;
    public float cycleTime = 5.0f;
    public string ImgPath = "Assets//CameraPics//";
    //private List <GameObject> CarrierList = new List <GameObject>();
    private Dictionary<string, List<GameObject>> CarrierList = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, List<QrCode>> qrCodesDict = new Dictionary<string, List<QrCode>>();


    static bool settingChanged = false;
    
    DirectoryInfo dInfo = new DirectoryInfo(@"Assets//CameraPics//");

    // Start is called before the first frame update
    void Start()
    {
        if (!Directory.Exists(ImgPath))
        {
            Directory.CreateDirectory(ImgPath);
        }
        RestartInvoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (settingChanged) 
        {
            RestartInvoke();
            settingChanged = false;
        }
    }

    public static CarrierHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CarrierHandler>();
            }
            return instance;
        }
    }

    //restart cycle after time was changed in settings
    public void ChangeCycleTime(int seconds)
    {
        cycleTime = (float)seconds;
        RestartInvoke();
    }

    //restarty cycle after path was changed in settings
    public void ChangePathFolder (string Path)
    {
        ImgPath = Path;
        RestartInvoke();
    }

    //restart cycle manually
    void RestartInvoke()
    {
        CancelInvoke();
        InvokeRepeating("checkForPic", 5, cycleTime);
    }

    void checkForPic()
    {
        var whitelist = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", 
                                ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF"};

        string[] files = Directory.GetFiles(ImgPath);

        string qr = "";

        //check all files in ImgPath
        foreach (string s in files)
        {
            FileInfo fi = null;
            try
            {
                fi = new FileInfo(s);
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e.Message);
                continue;
            }

            //if file extension in whitelist, get get qrCodes
            for (int i = 0; i <= whitelist.Length - 1; i++)
            {
                
                if (whitelist[i].Contains(fi.Extension))
                {
                    qr = ImgPath + fi.Name;

                    //get width and height of pic for relative calculation in calcQR
                    using (var fileStream = new FileStream(qr, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var image = Image.FromStream(fileStream, false, false))
                        {
                            var imgX = image.Width;
                            var imgY = image.Height;

                            fileStream.Close();
                            

                            //calc position and place GameObject relative to the image
                            calcQR(qr, imgX, imgY);
                        }
                    }
                }
            }
        }
    }

    void calcQR(string qrImage, float imgX, float imgY)
    {
        string _stationId = StationHandler.GetSelectedStation().GetID();
        var _qrCodes = QrCodeRecognition.getCodesFromPic(qrImage);
        
        Debug.Log(_qrCodes.Count);

        // Falls QrCodes vorher schon gespeichert -> überschreiben, ansonsten hinzufügen
        if (qrCodesDict.ContainsKey(_stationId))
            qrCodesDict[_stationId] = _qrCodes;
        else
            qrCodesDict.Add(_stationId, _qrCodes);

        Debug.Log("QrQodesDict: " + qrCodesDict[_stationId].Count);

        //float maxX = 831.0f;
        //float maxY = 605.0f;

        float maxX = imgX;
        float maxY = imgY;
        //float rotation = 0.0f;

        //float percentX = 1, percentY = 1;

        float[] percentX = new float[_qrCodes.Count];
        float[] percentY = new float[_qrCodes.Count];
        float[] rotation = new float[_qrCodes.Count];

        for (int i = 0; i < _qrCodes.Count; i++)
        {
            percentX[i] = 100.0f / maxX * _qrCodes[i].X;
            percentY[i] = 100.0f / maxY * _qrCodes[i].Y;
            rotation[i] = _qrCodes[i].Degree;

        }

        //percentX = 23;
        //percentY = 24;

        /*foreach (Transform child in AreasParent.transform)
        {
            GameObject area = child.gameObject;
            PositionRelativeTo(sampleCarrier, area, percentX, percentY, rotation);
        }
        */

        // Falls zu der Station schon Carrier angelegt wurden, dann alte Carrier löschen, ansonsten neue Liste anlegen
        if (CarrierList.ContainsKey(_stationId))
        {
            foreach (GameObject carrier in CarrierList[_stationId])
            {
                Destroy(carrier);
            }

            CarrierList[_stationId].Clear();
        }
        else
        {
            CarrierList.Add(_stationId, new List<GameObject>());
        }
        
        Debug.Log("Anzahl Areas: " + StationHandler.GetAllAreas().Count);

        foreach (GameObject area in StationHandler.GetAllAreas())
        {
            for (int i = 0; i < _qrCodes.Count; i++)
            {
                //GameObject sampleCarrierClone = Instantiate(sampleCarrier);
                CarrierList[_stationId].Add(Instantiate(sampleCarrier));
                Debug.Log("In der Liste vorhanden: " + CarrierList[_stationId].Count);
                PositionRelativeTo(CarrierList[_stationId][CarrierList[_stationId].Count -1 ], area, percentX[i], percentY[i], rotation[i]);
            }
        }

        //after getting position and rotation of carrier, delete everything from folder
        DeletePic(dInfo);
    }

    public void PositionRelativeTo(GameObject carrier, GameObject area, float percentX, float percentZ, float rotation)
    {
        float offsetX = area.transform.localScale.x / 100.0f * percentX;
        float offsetZ = area.transform.localScale.z / 100.0f * percentZ;

        float obj_rotation = rotation + 90;

        offsetX -= (area.transform.localScale.x / 2);
        offsetZ -= (area.transform.localScale.z / 2);

        if (rotation == 180)
        {
            offsetX = -offsetX;
            offsetZ = -offsetZ;
        }
        else if (rotation > 0)
        {
            if (rotation == 90)
            {
                float chache = offsetX;
                offsetX = offsetZ;
                offsetZ = -chache;
            }
            else if (rotation == 270)
            {
                float chache = offsetX;
                offsetX = -offsetZ;
                offsetZ = chache;
            }
        }

        


        //carrier position
        carrier.transform.position = area.transform.position;
        carrier.transform.position = new Vector3(carrier.transform.position.x + offsetX,
         area.transform.position.y + (carrier.transform.localScale.y / 2), carrier.transform.position.z - offsetZ);

        //carrier rotation
        carrier.transform.rotation = Quaternion.Euler(0, obj_rotation, 0);

        carrier.transform.parent = area.transform;
    }

    //delete all files from directory
    public void DeletePic(DirectoryInfo directory)
    {
        foreach (FileInfo file in directory.GetFiles())
            file.Delete();
    }

    public List<QrCode> getQrCodesForStation(string stationId)
    {
        if (qrCodesDict.ContainsKey(stationId))
            return qrCodesDict[stationId];
        else
            return new List<QrCode>();
    }
}
