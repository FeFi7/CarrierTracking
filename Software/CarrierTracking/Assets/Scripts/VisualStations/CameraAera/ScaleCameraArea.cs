using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class ScaleCameraArea : MonoBehaviour
{
    public Camera EditCam;

    void OnMouseOver()
    {
        if (EditCam.enabled)
        {
            float zoomValue = Input.GetAxis("Mouse ScrollWheel");

            if (zoomValue != 0)
            {
                CameraArea area = StationHandler.GetSelectedStation().GetAreaByID(this.name);

                transform.localScale += new Vector3(transform.localScale.x, 0, transform.localScale.z) * zoomValue * 0.2f;

                transform.localScale = Vector3.Max(transform.localScale, new Vector3(area.GetDefaultX() / 50, 0.5f, area.GetDefaultZ() / 50));
                transform.localScale = Vector3.Min(transform.localScale, new Vector3(area.GetDefaultX() * 2, 0.5f, area.GetDefaultZ() * 2));
            }
        }
    }
}

