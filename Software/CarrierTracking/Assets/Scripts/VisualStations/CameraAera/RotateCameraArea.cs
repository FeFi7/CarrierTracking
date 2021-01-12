using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraArea : MonoBehaviour
{
    public Camera EditCam;

    void OnMouseOver()
    {
        if (EditCam.enabled)
        {
            if (Input.GetMouseButtonDown(1)) {
                //transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
            }
            
        }

    }
}
