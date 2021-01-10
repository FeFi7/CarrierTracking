using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Drawing;
using Models;
using System;

[System.Serializable]
public class CarrierHandler : MonoBehaviour
{
    private static CarrierHandler instance;

    public GameObject sampleCarrier;
    public GameObject AreasParent;
    public float cycleTime = 5.0f;
    private Dictionary<string, List<GameObject>> carrierList = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, List<QrCode>> qrCodesDict = new Dictionary<string, List<QrCode>>();
    private Dictionary<string, int> carrierPicsX = new Dictionary<string, int>();
    private Dictionary<string, int> carrierPicsY = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        RestartInvoke();
    }

    // Update is called once per frame
    void Update()
    {

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

    //restart cycle manually
    void RestartInvoke()
    {
        InvokeRepeating("paintCarrierInSelectedStation", 5.0f, cycleTime);
    }

    void paintCarrierInSelectedStation()
    {
        Debug.Log("Station wird befüllt");

        string _stationId;
        if (StationHandler.GetSelectedStation() != null)
        {
            Debug.Log("Station_id: " + StationHandler.GetSelectedStation().GetID());
            _stationId = StationHandler.GetSelectedStation().GetID();
        }
        else
            return;
        
        List<QrCode> _qrCodes;
        float maxX;
        float maxY;

        // Hole qrCodes aus Dictionary
        if (qrCodesDict.ContainsKey(_stationId))
            _qrCodes = qrCodesDict[_stationId];
        else
            return; // Noch keine QrCodes erkannt

        Debug.Log(_qrCodes.Count + " für diese station gefunden");

        // Hole X und Y Werte von QrCode-Bild aus Dictionary
        if (carrierPicsX.ContainsKey(_stationId))
            maxX = carrierPicsX[_stationId];
        else
        {
            Debug.Log("Noch keine Pixelwerte(X) für Bilder erkannt");
            return;
        }
        if (carrierPicsY.ContainsKey(_stationId))
            maxY = carrierPicsY[_stationId];
        else
        {
            Debug.Log("Noch keine Pixelwerte(Y) für Bilder erkannt");
            return;
        }

        float[] percentX = new float[_qrCodes.Count];
        float[] percentY = new float[_qrCodes.Count];
        float[] rotation = new float[_qrCodes.Count];

        for (int i = 0; i < _qrCodes.Count; i++)
        {
            percentX[i] = 100.0f / maxX * _qrCodes[i].X;
            percentY[i] = 100.0f / maxY * _qrCodes[i].Y;
            rotation[i] = _qrCodes[i].Degree;
        }

        // Falls zu der Station schon Carrier angelegt wurden, dann alte Carrier löschen, ansonsten neue Liste anlegen
        if (carrierList.ContainsKey(_stationId))
        {
            foreach (GameObject carrier in carrierList[_stationId])
            {
                Destroy(carrier);
            }

            carrierList[_stationId].Clear();
        }
        else
        {
            carrierList.Add(_stationId, new List<GameObject>());
        }

        // platziere Kästen für jeden gefunden QrCode
        foreach (GameObject area in StationHandler.GetSelectedStation().GetAreaObjects())
        {
            for (int i = 0; i < _qrCodes.Count; i++)
            {
                //GameObject sampleCarrierClone = Instantiate(sampleCarrier);
                carrierList[_stationId].Add(Instantiate(sampleCarrier));

                //Falls Carrier angelegt, setze Carriername als Mouseover-Text, ansonsten die ID
                int carrierId;
                if(Int32.TryParse(_qrCodes[i].Text,out carrierId))
                {
                    if (GameManager.Instance.GetCarrierByID(carrierId) != null)
                    {
                        carrierList[_stationId][carrierList[_stationId].Count - 1].GetComponent<MouseOverBehaviour>().MouseOverText = GameManager.Instance.GetCarrierByID(carrierId).name;
                    }
                    else
                        carrierList[_stationId][carrierList[_stationId].Count - 1].GetComponent<MouseOverBehaviour>().MouseOverText = _qrCodes[i].Text+"(nicht angelegt)";
                }
                else
                    carrierList[_stationId][carrierList[_stationId].Count - 1].GetComponent<MouseOverBehaviour>().MouseOverText = _qrCodes[i].Text + "(nicht angelegt)";

                PositionRelativeTo(carrierList[_stationId][carrierList[_stationId].Count - 1], area, percentX[i], percentY[i], rotation[i]);
            }
        }
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

    public List<QrCode> getQrCodesForStation(string stationId)
    {
        if (qrCodesDict.ContainsKey(stationId))
            return qrCodesDict[stationId];
        else
            return new List<QrCode>();
    }

    public void setQrCodesForStation(List<QrCode> qrCodes, string stationId)
    {
        if (qrCodesDict.ContainsKey(stationId))
            qrCodesDict[stationId] = qrCodes;
        else
            qrCodesDict.Add(stationId, qrCodes);
    }

    public void setCarrierPicPixelSizes(int x, int y, string stationId)
    {
        if (carrierPicsX.ContainsKey(stationId))
            carrierPicsX[stationId] = x;
        else
            carrierPicsX.Add(stationId, x);

        if (carrierPicsY.ContainsKey(stationId))
            carrierPicsY[stationId] = y;
        else
            carrierPicsY.Add(stationId, y);
    }
}
