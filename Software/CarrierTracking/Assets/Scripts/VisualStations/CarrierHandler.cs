using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierHandler : MonoBehaviour
{
    public GameObject sampleCarrier;
    public GameObject AreasParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //var qrCodes = QrCodeRecognition.getCodesFromPic("Assets/Images/01_2DPlan.png");

        float maxX = 831.0f;
        float maxY = 605.0f;

        float percentX = 1, percentY = 1;

        //foreach (var qrCode in qrCodes)
        //{
        //    percentX = 100.0f / maxX * qrCode.X;
        //    percentY = 100.0f / maxY * qrCode.Y;
        //}

        percentX = 23;
        percentY = 24;

        foreach (Transform child in AreasParent.transform)
        {
            GameObject area = child.gameObject;
            PositionRelativeTo(sampleCarrier, area, percentX, percentY);
        }
           
    }


    public void PositionRelativeTo(GameObject carrier, GameObject area, float percentX, float percentZ)
    {
        float offsetX = area.transform.localScale.x / 100.0f * percentX;
        float offsetZ = area.transform.localScale.z / 100.0f * percentZ;

        float rotation = area.transform.rotation.eulerAngles.y;

        offsetX -= (area.transform.localScale.x / 2);
        offsetZ -= (area.transform.localScale.z / 2);

        if(rotation == 180)
        {
            offsetX = -offsetX;
            offsetZ = -offsetZ;
        } else if(rotation > 0)
        {
            if(rotation == 90)
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

        carrier.transform.position = area.transform.position;
        carrier.transform.position = new Vector3(carrier.transform.position.x + offsetX,
            area.transform.position.y + (carrier.transform.localScale.y / 2), carrier.transform.position.z - offsetZ);

    }

}
