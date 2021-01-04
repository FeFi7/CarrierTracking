using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CarrierController : MonoBehaviour
{
    private static CarrierController instance;

    public GameObject Panel;
    public GameObject ContentPanel;
    public GameObject UpdatePanel;

    public InputField UpdateCarrierName;
    public InputField UpdateCarrierID;
    public Dropdown UpdateStationID;
    public Text UpdateModel;
    public InputField UpdateInfo;

    public GameObject ButtonPrefab;

    public StatusController statusfield;

    GameObject newButton;

    static float contentYPos = -50.0F;
    static float contentXPos = 0.0F;
    static float contentZPos = 0.0F;

    private static float contentHeight = 40.0F;

    //Wird zum Programmstart ausgeführt --> lädt alle Carrier als Buttons in die Carrier Liste 
    public void start()
    {
        LoadCarrierButtons();
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

    //Fügt neuen Button der Carrierliste hinzu, übergeben werden dazu Carrier Name und Carrier ID
    //Name und ID des Buttons werden separat in einem Skript des Button Prefabs abgespeichert
    public void AddNewButton(string name, int carrierid)
    {
        //Neuen Button erstellen und positionieren
        newButton = Instantiate(ButtonPrefab) as GameObject;
        newButton.GetComponent<RectTransform>().SetParent(ContentPanel.transform, false);
        RectTransform rt = ContentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, contentHeight);

        //Vergrößere Content Field um eine Buttongröße (Height +40)
        contentHeight += 40.0F;

        //Button Text zu Carrier Name ändern
        newButton.GetComponentInChildren<Text>().text = name;

        newButton.GetComponent<PrefabCarrierInfo>().CarrierID = carrierid;
        newButton.GetComponent<PrefabCarrierInfo>().CarrierName = name; 
    }

    //Löscht den Button und den Datensatz eines Carriers
    // ------Flo's Funktion zum löschen des Speichersatzes noch benötigt -----
    // ------Button wird noch nicht korrekt gelöscht -----------
    public void DeleteCarrier()
    {
        //Flo's Funktion um Carrier zu löschen 

        Destroy(newButton, 1.0f);

        contentHeight -= 40.0f;

        statusfield.ChangeStatus("Carrier wurde gelöscht");
        CloseUpdatePanel();
    }

    //Speichert modifizierte Carrier Daten ab und schließt das Update/Info Panel
    //------Flo's Funktion um modifizierte Daten zu Speichern fehlt noch--------
    public void AcceptUpdate()
    {
        //Flo Funktion müssen neue Parameter übergeben werden

        statusfield.ChangeStatus("Carrier wurde geupdatet");
        ClearUpdateSettings();
        CloseUpdatePanel();
    }

    //Cleart Eingaben in Update/Info Panel und schließt das Panel anschließend
    public void CancelUpdate()
    {
        ClearUpdateSettings();
        CloseUpdatePanel();
    }

    //Cleart die Eingaben aus dem Update/Info Panel
    public void ClearUpdateSettings()
    {
        UpdateCarrierName.text = "";
        UpdateCarrierID.text = "";
        //UpdateStationID
        UpdateModel.text = "";
        UpdateInfo.text = "";
    }

    //Gibt Informationen in Update/Info Panel über ausgewählten Carrier aus
    //-------Station ID wird noch nicht angezeigt ------
    public void OpenInfo(int carrierid)
    {
        Carrier carrier = GameManager.Instance.GetCarrierByID(carrierid);
        OpenUpdatePanel();

        UpdateCarrierName.text = carrier.name;
        UpdateCarrierID.text = carrier.id.ToString();

        //Station ID dem richtigen Dropdown Select zuweisen
        //UpdateStationID. = carrier.station
    }

    //Öffnet Update/Info Panel
    public void OpenUpdatePanel()
    {
        if (UpdatePanel != null)
        {
            UpdatePanel.SetActive(true);
        }
    }

    //Schließt Update/Info Panel
    public void CloseUpdatePanel()
    {
        UpdatePanel.SetActive(false);
    }

    //Öffnet Panel 
    public void OpenPanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }

    //Schließt Panel mit des Decline/Cancel Buttons
    public void ClosePanel()
    {
        Panel.SetActive(false);
    }

    //Wird zum Programmstart aufgerufen, lädt die CarrierButtons 
    public void LoadCarrierButtons()
    {
        foreach(Carrier element in GameManager.Instance.Carriers)
        {
            AddNewButton(element.name, element.id);
        }
    }
}
