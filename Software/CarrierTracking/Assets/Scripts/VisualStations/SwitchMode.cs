using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchMode : MonoBehaviour
{
    public Camera editCam;
    public Camera mainCam;
    public Button SwitchCameraButton = null;

    void Start()
    {
        SwitchCameraButton = gameObject.GetComponent<Button>();
        SwitchCameraButton.onClick.AddListener(switchCamera);
    }

    //Switch camnera on button-click and change its text
    void switchCamera()
    {
        if (editCam.enabled)
        {
            editCam.enabled = false;
            mainCam.enabled = true;

            mainCam.transform.rotation = Quaternion.Euler(90, 0, 0);

            GameManager.Instance.LoadStationsFromList();
            GameManager.Instance.save();

            GameObject.Find("ButtonModusWechseln").GetComponentInChildren<Text>().text = "zu Bearbeitungsmodus wechseln";
        }
        else
        {
            editCam.enabled = true;
            mainCam.enabled = false;
            GameObject.Find("ButtonModusWechseln").GetComponentInChildren<Text>().text = "zurück zur Übersicht";
        }
    }
}
