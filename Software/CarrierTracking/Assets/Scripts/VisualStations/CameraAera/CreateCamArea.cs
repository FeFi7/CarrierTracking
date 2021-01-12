using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCamArea : MonoBehaviour
{
    public Camera editCam;

    void OnMouseOver()
    {
        if (editCam.enabled)
        {
            if(Input.GetMouseButtonDown(1) && StationHandler.GetSelectedStation().GetAreaObjects().Count < 1) {
                
                CreateNewCameraArea(Input.mousePosition);
            }
        }
    }

    public GameObject defaultCameraArea;

    public void CreateNewCameraArea(Vector2 mousePosition)
    {
        RaycastHit hit = RayFromCamera(mousePosition, 1000.0f);
        GameObject copyedArea = GameObject.Instantiate(defaultCameraArea, hit.point, Quaternion.identity);
        GameObject areaParent = StationHandler.GetSelectedStation().GetAreasParent();
        copyedArea.transform.SetParent(areaParent.transform, true);
        copyedArea.name = "Area" + areaParent.transform.childCount;
        copyedArea.transform.position = new Vector3(copyedArea.transform.position.x, copyedArea.transform.position.y, copyedArea.transform.position.z);

        StationHandler.GetSelectedStation().RegisterCameraArea(copyedArea); 
    }

    public RaycastHit RayFromCamera(Vector3 mousePosition, float rayLength)
    {
        RaycastHit hit;
        Ray ray = editCam.ScreenPointToRay(mousePosition);
        Physics.Raycast(ray, out hit, rayLength);
        return hit;
    }
}
