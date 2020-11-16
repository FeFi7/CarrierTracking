using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObject : MonoBehaviour
{

    //public Camera EditCam;

    void OnMouseOver()
    {

         if (Input.GetMouseButtonDown(2))
            {
            //if (EditCam.enabled)
            {
                Destroy(transform.gameObject);
            }
        }

    }
}
