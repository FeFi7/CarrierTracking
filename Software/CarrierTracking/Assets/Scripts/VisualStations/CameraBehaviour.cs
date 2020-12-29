using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Camera EditCam;
    public Camera MainCam; //= Camera.main

    // Start is called before the first frame update
    void Start()
    {
        EditCam.enabled = false;
        MainCam.enabled = true;
    }

    void Update()
    {
        getInput();

        if(MainCam != null) { 
            if (MainCam.enabled)
            {
                //if (Input.GetKey(KeyCode.LeftArrow))
                //    MainCam.transform.Rotate(new Vector3(0.0f, 0.0f, -0.00f));

                //if (Input.GetKey(KeyCode.RightArrow))
                //    MainCam.transform.Rotate(new Vector3(0.0f, 0.0f, 0.00f));

                //Kp in welcher Klasse die Kamera gedreht wird, aber die Zeile wirkt halt dem entgegen
                MainCam.transform.rotation = Quaternion.Euler(90, 0, 180);
            } else
            {
                //if (Input.GetKey(KeyCode.LeftArrow))
                //    EditCam.transform.RotateAround(StationHandler.getViewedStation().getCenterLocation(), new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime);

                //if (Input.GetKey(KeyCode.RightArrow))
                //    EditCam.transform.RotateAround(StationHandler.getViewedStation().getCenterLocation(), new Vector3(0.0f, 1.0f, 0.0f), -20 * Time.deltaTime);

            }
        }
    }

    void getInput()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            EditCam.enabled = false;
            MainCam.enabled = true;

            if (MainCam.enabled)
                MainCam.transform.rotation = Quaternion.Euler(90, 0, 180);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //rotate = false;
            EditCam.enabled = true;
            MainCam.enabled = false;
        }
    }
}
