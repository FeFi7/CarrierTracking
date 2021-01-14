using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationController : MonoBehaviour
{
    private static StationController instance;

    //Set by inspector
    public GameObject addStationPanel;
    public GameObject updateStationPanel;
    public GameObject contentPanel;

    //Set by inspector
    public GameObject backStation;
    public GameObject nextStation;

    //Set by inspector
    public InputField addName;
    public InputField addID;
    public InputField addInfo;

    //Set by inspector
    public InputField updateName;
    public InputField updateID;
    public InputField updateInfo;

    //Set by inspector
    public GameObject stationButtonPrefab;

    GameObject newStationButton;
    private static List<GameObject> stationButtons = new List<GameObject>();

    //Set by inspector
    public StatusController statusfield;

    private static float contentHeight = 0.0F;

    private static bool loadStationInfo = false;

    //Öffnet die laut Liste zuvor kommende Station
    public void GoBackStation()
    {
        StationHandler.ViewPreviousStation();
    }

    //Öffnet die laut Liste nächste Station
    public void GoNextStation()
    {
        StationHandler.ViewNextStation();
    }

    //Fügt eine neue Station dem Datensatz und der Station Liste hinzu
    //Stationen ohne Namen geht nicht
    public void AddStation()
    {
        if(addName.text == "" || addName.text == " " || addName.text == "  ")
        {
            addName.image.color = Color.red;
            return; 
        }

        string stationid = "";
        try
        {
            //Station wird erstellt im GameManager -> dieser speichert die Stationen und Carrier
            //stationid = GameManager.Instance.generateStation(addName.text);

            //Station wird erstellt im StationHandler
            Station station = StationHandler.CreateStation();
            station.SetName(addName.text);
            station.SetInfo(addInfo.text);
            // station.SetID(stationid.ToString());
            stationid = station.GetID();
            //Ein Button GameObject wird der Station Liste hinzugefügt
            AddButton(addName.text, stationid);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        statusfield.ChangeStatus("Station \"" + addName.text + "\" angelegt!");

        ClearFields(addName, addID, addInfo);
        
        ClosePanel(addStationPanel);
    }

    //Der Button (GameObject), welcher auf die jeweilig angelegte Station referenziert wird angelegt 
    public void AddButton(string name, string id)
    {
        contentHeight += 40.0F;
        stationButtons.Add(Instantiate(stationButtonPrefab) as GameObject);
        int lastIndex = stationButtons.Count - 1;

        stationButtons[lastIndex].GetComponent<RectTransform>().SetParent(contentPanel.transform, false);
        RectTransform rt = contentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, contentHeight);

        stationButtons[lastIndex].GetComponentInChildren<Text>().text = name;

        stationButtons[lastIndex].GetComponent<PrefabStationInfo>().stationID = id;
        stationButtons[lastIndex].GetComponent<PrefabStationInfo>().stationName = name;
    }

    //Eingaben des Benutzers werden gelöscht und das Hinzufügen einer Station Panel wird geschlossen
    public void AddDecline()
    {
        ClearFields(addName, addID, addInfo);
        ClosePanel(addStationPanel);
    }

    //Öffnet das Info/Update Panel der fokusierten Station
    public void OpenInfo()
    { 
        Station station = StationHandler.GetSelectedStation();

        int stationid = Int32.Parse(station.GetID());

        OpenPanel(updateStationPanel);
        DStation s = GameManager.Instance.GetStationByID(stationid);
        updateName.text = s.name;
        updateID.text = s.StationID.ToString();
    }

    //Speichert neue Daten der Station ab und ändert Button Name
    public void AcceptUpdate()
    {
        if (updateName.text == "" || updateName.text == " " || updateName.text == "  ")
        {
            addName.image.color = Color.red;
            return;
        }

        //Flo's Funktion zum Updaten der Station fehlt noch

        Station station = StationHandler.GetStationList().GetStationByID(updateID.text);
        station.SetName(updateName.text);
        station.SetInfo(updateInfo.text);

        UpdateStationName(updateName.text);

        statusfield.ChangeStatus("Station wurde geupdatet");
        ClearFields(updateName, updateID, updateInfo);
        ClosePanel(updateStationPanel);
    }

    //Updatet die Station Liste mit dem neuen Namen
    public void UpdateStationName(string newName)
    {
        
        stationButtons[Int32.Parse(updateID.text)-1].GetComponentInChildren<Text>().text = newName;
        stationButtons[Int32.Parse(updateID.text)-1].GetComponent<PrefabStationInfo>().stationName = newName;
    }

    //Eingaben des Benutzers werden gelöscht und das Update/Info Panel geschlossen
    public void DeclineUpdate()
    {
        ClearFields(updateName, updateID, updateInfo);
        ClosePanel(updateStationPanel);
    }

    //Eine Station wird aus den Datensätzen und den Listen gelöscht
    public void DeleteStation()
    {
        string carrierToDelete = updateID.text;

        foreach (GameObject el in stationButtons)
        {
            if(carrierToDelete.Equals(el.GetComponent<PrefabStationInfo>().stationID))
            {
                Destroy(el);
                stationButtons.Remove(el);
                //Station aus Liste löschen
                break;
            }
        }

        Station station = StationHandler.GetStationList().GetStationByID(carrierToDelete);
        StationHandler.ViewSpecialStation(station);
        StationHandler.DeleteSelectedStation();

        contentHeight -= 40.0F;
        RectTransform rt = contentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, contentHeight);

        statusfield.ChangeStatus("Station wurde gelöscht");
        ClosePanel(updateStationPanel);
    }

    //Eingaben des Benutzers werden aus dem jeweiligen Panel gelöscht
    public void ClearFields(InputField name, InputField id, InputField info)
    {
        name.image.color = Color.white;
        name.GetComponent<InputField>().text = "";
        id.GetComponent<InputField>().text = "";
        info.GetComponent<InputField>().text = "";
    }

    //Wird aufgerufen zum Programmstart
    public void Start()
    {
        LoadStationButtons();
        loadStationInfo = true;
    }

    //Lädt zum Programmstart die gespeicherten Stationen in die Station Liste
    //Dazu werden GameObjects von jeder Station erstellt
    public void LoadStationButtons()
    {
        if (loadStationInfo == true)
        {
            return;
        }
        foreach (DStation element in GameManager.Instance.Stations)
        {
            Debug.Log(GameManager.Instance.Stations.Count);
            AddButton(element.name, element.StationID);
        }
    }

    //Öffnet das jeweilig benötigte Panel
    public void OpenPanel(GameObject Panel)
    {
        Panel.SetActive(true);
    }

    //Schließt das jeweilig offene Panel
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
