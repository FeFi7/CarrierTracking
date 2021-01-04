using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationController : MonoBehaviour
{

    private static CarrierController instance;

    public GameObject AddStationPanel;
    public GameObject UpdateStationPanel;
    public GameObject ContentPanel;

    public GameObject StationButtonPrefab;

    public InputField AddName;
    public InputField AddID;
    public InputField AddInfo;

    public InputField UpdateName;
    public InputField UpdateID;
    public InputField UpdateInfo;

    GameObject newStationButton;


    public StatusController statusfield;

    string path = " ";

    private static float contentHeight = 40.0f;

    public void AddStation()
    {


        AddButton(AddName.text, AddID.text);
    }
    public void OpenInfo()
    {

    }

    public void AcceptUpdate()
    {

    }

    public void DeclineUpdate()
    {

    }

    public void DeleteStation()
    {

    }

    public void ClearFields(GameObject name, GameObject id, GameObject info)
    {
        name.GetComponent<InputField>().text = " ";
        id.GetComponent<InputField>().text = " ";
        info.GetComponent<InputField>().text = " ";
    }

    public void Start()
    {
        LoadStationButtons();
    }

    public void AddButton(string name, string id)
    {

    }

    public void LoadStationButtons()
    {
        foreach (DStation element in GameManager.Instance.Stations)
        {
            AddButton(element.name, element.StationID.ToString());
        }
    }

    public void OpenPanel(GameObject Panel)
    {
        Panel.SetActive(true);
    }

    public void ClosePanel(GameObject Panel)
    {
        Panel.SetActive(false);
    }
    public static CarrierController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CarrierController>();
            }
            return instance;
        }
    }
}
