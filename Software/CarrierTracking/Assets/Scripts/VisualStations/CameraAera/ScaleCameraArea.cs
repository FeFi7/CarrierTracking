using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class ScaleCameraArea : MonoBehaviour
{
    public Camera EditCam;

    Vector3 minScale = new Vector3(9.60f, 0.1f, 5.40f);
    Vector3 maxScale = new Vector3(38.40f*2.0f, 0.1f, 21.60f * 2.0f);

    void OnMouseOver()
    {
        if (EditCam.enabled)
        {
            float zoomValue = Input.GetAxis("Mouse ScrollWheel");

            if (zoomValue != 0)
            {
                //transform.localScale += Vector3.one * zoomValue;
                transform.localScale += new Vector3(0.1920f, 0, 0f) * zoomValue * 5;
                transform.localScale += new Vector3(0f, 0, 0.108f) * zoomValue * 5;
                transform.localScale = Vector3.Max(transform.localScale, minScale);
                transform.localScale = Vector3.Min(transform.localScale, maxScale);
            }
        }

    }

}

