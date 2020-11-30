using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadQrCodesButton : MonoBehaviour
{
    int i = 1;
    public Button yourButton;



    void Start()
    {
        //Button btn = yourButton.GetComponent<Button>();
        //btn.onClick.AddListener(onClick);
    }

    public void onClick()
    {
        //var defaultPlane = GameObject.Find("Default Background Plane");
        
        
        //var qrCodes = QrCodeRecognition.getCodesFromPic("Assets/Images/0"+ i +"_2DPlan.png");
        //if (i < 4)
        //    i++;

        //foreach(var qrCode in qrCodes)
        //{
        //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    GameObject plane = GameObject.Find("Default Background Plane");

        //    cube.transform.SetParent(plane.transform, true);


            
        //    cube.transform.position = new Vector3(qrCode.X / plane.transform.localScale.x, 0, qrCode.Y / plane.transform.localScale.z);
            
       
            
        //}
    }
}
