using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObject : MonoBehaviour
{
    public Camera EditCam;

    void OnMouseOver()
    {
        if (EditCam.enabled)
        {
            if (Input.GetMouseButtonDown(2))
            {
                Destroy(transform.gameObject);
            }
        }

    }
}
