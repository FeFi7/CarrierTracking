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

        if (MainCam != null)
        {
            if (MainCam.enabled)
            {
                MainCam.transform.rotation = Quaternion.Euler(90, 0, 180);
            }

        }
    }

    void getInput()
    {
        if (Input.GetKey(KeyCode.LeftControl)
        || Input.GetKey(KeyCode.RightControl))
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
                EditCam.enabled = true;
                MainCam.enabled = false;
            }
        }
    }
}
