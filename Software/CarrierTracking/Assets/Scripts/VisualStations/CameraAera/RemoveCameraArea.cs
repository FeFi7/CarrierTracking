using UnityEngine;

public class RemoveCameraArea : MonoBehaviour
{

    public Camera EditCam;

    void OnMouseOver()
    {
        if (EditCam.enabled) //Wenne iene
        {
            if (Input.GetMouseButtonDown(2))
            {
                Station station = StationHandler.GetSelectedStation();
                string id = transform.gameObject.name;
                if (station.IsCameraAreaIDRegistered(id))
                {
                    CameraArea area = station.GetAreaByID(id);
                    station.RemoveCameraArea(area.GetID());
                    Destroy(transform.gameObject);
                }
            }
        }

    }
}
