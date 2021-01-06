using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationController : MonoBehaviour
{

    private static StationController instance;

    public GameObject AddStationPanel;
    public GameObject UpdateStationPanel;
    public GameObject ContentPanel;

    public InputField AddName;
    public InputField AddID;
    public InputField AddInfo;

    public InputField UpdateName;
    public InputField UpdateID;
    public InputField UpdateInfo;

    public GameObject StationButtonPrefab;
    GameObject newStationButton;
    static List<GameObject> stationButtons = new List<GameObject>();

    public StatusController statusfield;

    string path = " ";

    private static float contentHeight = 0.0F;

    public void AddStation()
    {
        int stationid = -1;
        try
        {
            stationid = GameManager.Instance.generateStation(AddName.text);
            AddButton(AddName.text, stationid);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        Station station = StationHandler.CreateStation();
        station.SetName(AddName.text);
        station.SetInfo(AddInfo.text);
        station.SetID(stationid.ToString());
        ///station.SetInfo(AddName.info);
        

        ClearFields(AddName, AddID, AddInfo);
        statusfield.ChangeStatus("Neue Station angelegt");
        ClosePanel(AddStationPanel);
    }

    public void AddButton(string name, int id)
    {
        contentHeight += 40.0F;
        stationButtons.Add(Instantiate(StationButtonPrefab) as GameObject);
        int lastIndex = stationButtons.Count - 1;

        stationButtons[lastIndex].GetComponent<RectTransform>().SetParent(ContentPanel.transform, false);
        RectTransform rt = ContentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, contentHeight);

        stationButtons[lastIndex].GetComponentInChildren<Text>().text = name;

        stationButtons[lastIndex].GetComponent<PrefabStationInfo>().stationID = id;
        stationButtons[lastIndex].GetComponent<PrefabStationInfo>().stationName = name;
    }

    public void AddDecline()
    {
        ClearFields(AddName, AddID, AddInfo);
        ClosePanel(AddStationPanel);
    }

    public void OpenInfo(int stationid)
    {
        OpenPanel(UpdateStationPanel);
        DStation station = GameManager.Instance.GetStationByID(stationid);
        UpdateName.text = station.name;
        UpdateID.text = station.StationID.ToString();
        //UpdateInfo.text = ---fehlt---
    }

    public void AcceptUpdate()
    {
        //Flo's Funktion zum Updaten der Station fehlt noch

        UpdateStationName(UpdateName.text);

        statusfield.ChangeStatus("Station wurde geupdatet");
        ClearFields(UpdateName, UpdateID, UpdateInfo);
        ClosePanel(UpdateStationPanel);
    }

    public void UpdateStationName(string newName)
    {
        stationButtons[Int32.Parse(UpdateID.text)-1].GetComponentInChildren<Text>().text = newName;
        stationButtons[Int32.Parse(UpdateID.text)-1].GetComponent<PrefabStationInfo>().stationName = newName;
    }

    public void DeclineUpdate()
    {
        ClearFields(UpdateName, UpdateID, UpdateInfo);
        ClosePanel(UpdateStationPanel);
    }

    public void DeleteStation()
    {
        int carrierToDelete = Int32.Parse(UpdateID.text);
        foreach(GameObject el in stationButtons)
        {
            Destroy(el);
            stationButtons.Remove(el);
            break;
        }

        Station station = StationHandler.GetStationList().GetStationByID(carrierToDelete.ToString());
        StationHandler.ViewSpecialStation(station);
        StationHandler.DeleteSelectedStation();

        contentHeight -= 40.0F;
        RectTransform rt = ContentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, contentHeight);

        statusfield.ChangeStatus("Station wurde gelöscht");
        ClosePanel(UpdateStationPanel);
    }

    public void ClearFields(InputField name, InputField id, InputField info)
    {
        name.GetComponent<InputField>().text = "";
        id.GetComponent<InputField>().text = "";
        info.GetComponent<InputField>().text = "";
    }

    public void start()
    {
        LoadStationButtons();
    }

    public void LoadStationButtons()
    {
        foreach (DStation element in GameManager.Instance.Stations)
        {
            AddButton(element.name, element.StationID);
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
    public static StationController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<StationController>();
            }
            return instance;
        }
    }
}
