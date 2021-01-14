using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Camera editCam;
    public Camera mainCam;
    private string viewedStation;

    // Start is called before the first frame update
    void Start()
    {
        editCam.enabled = false;
        mainCam.enabled = true;
        viewedStation = "";
    }

    void Update()
    {
        //GetInput();

        if (mainCam != null && editCam != null)
        {
            if (StationHandler.GetStationList().GetSize() <= 0 && !viewedStation.Equals("null"))
            {
                viewedStation = "null";

                mainCam.transform.position = new Vector3(-55, -30, -84);
                editCam.transform.position = new Vector3(-55, 64, -30);

                mainCam.enabled = true;
            }
            else if (StationHandler.GetStationList().GetSize() > 0 && StationHandler.GetSelectedStation() != null && !viewedStation.Equals(StationHandler.GetSelectedStation().GetID()))
            {
                Station selectedStation = StationHandler.GetSelectedStation();
                viewedStation = selectedStation.GetID();

                mainCam.transform.position = new Vector3(selectedStation.GetCenterLocation().x, selectedStation.GetCenterLocation().y + 300.0f, selectedStation.GetCenterLocation().z);
                editCam.transform.position = new Vector3(selectedStation.GetCenterLocation().x, selectedStation.GetCenterLocation().y + 64.0f, selectedStation.GetCenterLocation().z - 30.0f);
            }
        }
    }

    /*
    void GetInput()
    {
        //if (Input.GetKey(KeyCode.LeftControl)
        //|| Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                editCam.enabled = false;
                mainCam.enabled = true;

                if (mainCam.enabled)
                    mainCam.transform.rotation = Quaternion.Euler(90, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                editCam.enabled = true;
                mainCam.enabled = false;
            }
        }
    }
    */
}
