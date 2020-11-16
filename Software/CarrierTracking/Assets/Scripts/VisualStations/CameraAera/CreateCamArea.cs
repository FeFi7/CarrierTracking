using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCamArea : MonoBehaviour
{
    public Camera EditCam;
    public GameObject AreaParent;

    void OnMouseOver()
    {
        if (EditCam.enabled)
        {
            if(Input.GetMouseButtonDown(1)) {
                CreateNewCameraArea(Input.mousePosition);
            }
        }
    }

    public GameObject DefaultCameraArea;

    public void CreateNewCameraArea(Vector2 mousePosition)
    {
        RaycastHit hit = RayFromCamera(mousePosition, 1000.0f);
        GameObject copyedArea = GameObject.Instantiate(DefaultCameraArea, hit.point, Quaternion.identity);
        copyedArea.transform.SetParent(AreaParent.transform, true);
        copyedArea.transform.position = new Vector3(copyedArea.transform.position.x, copyedArea.transform.position.y, copyedArea.transform.position.z);
        
    }

    public RaycastHit RayFromCamera(Vector3 mousePosition, float rayLength)
    {
        RaycastHit hit;
        Ray ray = EditCam.ScreenPointToRay(mousePosition);
        Physics.Raycast(ray, out hit, rayLength);
        return hit;
    }
}
