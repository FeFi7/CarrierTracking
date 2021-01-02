using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateWall : MonoBehaviour
{
    bool creating;

    public GameObject start;
    public GameObject end;
    public GameObject wallPrefab;

    GameObject wall;

    public Camera EditCameta;
    public Camera MainCamera;

    

    void Start()
    {
        EditCameta.enabled = true;
        MainCamera.enabled = false;
    }

    void Update()
    {
        GetInput();
        if (MainCamera.enabled)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                MainCamera.transform.Rotate(new Vector3(0.0f, 0.0f, 0.05f));

            if (Input.GetKey(KeyCode.RightArrow))
                MainCamera.transform.Rotate(new Vector3(0.0f, 0.0f, -0.05f));
        }
    }

    void OnMouseOver()
    {
        if (EditCameta.enabled)
        {
            if (Input.GetMouseButtonDown(0))
                SetStart();
        }
    }

    void GetInput()
    {
        if (EditCameta.enabled && creating)
        {
            if (Input.GetMouseButtonUp(0))
                SetEnd();
            else
            {
               Adjust();
            }
        }
    }

    void SetStart()
    {
        creating = true;
        start.transform.position = GetWorldPoint();
        wall = (GameObject)Instantiate(wallPrefab, start.transform.position, Quaternion.identity);
        GameObject WallParent = StationHandler.GetSelectedStation().GetWallsParent();
        wall.transform.SetParent(WallParent.transform, false);
        wall.name = "Wall" + WallParent.transform.childCount;
        wall.AddComponent<RemoveObject>();
    }

    void SetEnd()
    {
        creating = false;
        end.transform.position = GetWorldPoint();
        
    }

    private void Adjust()
    {
        end.transform.position = GetWorldPoint();
        AdjustWall();
    }

    private void AdjustWall()
    {
        start.transform.LookAt(end.transform.position);
        end.transform.LookAt(start.transform.position);
        float distance = Vector3.Distance(start.transform.position, end.transform.position);
        wall.transform.position = start.transform.position + distance / 2 * start.transform.forward;
        wall.transform.rotation = start.transform.rotation;
        wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance);
    }

    Vector3 GetWorldPoint()
    {
        Ray ray = EditCameta.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.point;
        return Vector3.zero;
    }
}
