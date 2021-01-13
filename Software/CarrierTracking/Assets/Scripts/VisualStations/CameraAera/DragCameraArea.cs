using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCameraArea : MonoBehaviour
{
    public Camera EditCam;

    private Vector3 mOffset;
    private float mZCoord;

    //void OnMouseDown()
    void OnMouseOver()
    {
        if(EditCam.enabled) {
            if(Input.GetMouseButtonDown(0)) {
                mZCoord = EditCam.WorldToScreenPoint(gameObject.transform.position).z;
                mOffset = gameObject.transform.position - GetMouseWorldPos();
            }
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        if(EditCam.enabled) {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = mZCoord;
            return EditCam.ScreenToWorldPoint(mousePoint);
        }
        return new Vector3(0.0f, 0.0f, 0.0f);
    }  
      
    void OnMouseDrag()
    {
        if (EditCam.enabled)
        {
            transform.position = new Vector3(GetMouseWorldPos().x + mOffset.x, transform.position.y, GetMouseWorldPos().z + mOffset.z);
        }
    }

}
