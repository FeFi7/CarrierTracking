using Models;
using System;
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
    static List<GameObject> carrierButtons = new List<GameObject>(); 
    
    static float contentYPos = -50.0F;
    static float contentXPos = 0.0F;
    static float contentZPos = 0.0F;

    private static float carriercontentHeight = 0.0F;
    private static int carrierCount = 1;

    //Wird zum Programmstart ausgeführt --> lädt alle Carrier als Buttons in die Carrier Liste 
    public void Start()
    {
        //LoadCarrierButtons();
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
        //Vergrößere Content Field um eine Buttongröße (Height +40)
        carriercontentHeight += 40.0F;
        carrierCount += 1;

        carrierButtons.Add(Instantiate(ButtonPrefab) as GameObject);
        int lastIndex = carrierButtons.Count - 1;

        carrierButtons[lastIndex].GetComponent<RectTransform>().SetParent(ContentPanel.transform, false);
        RectTransform rt = ContentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, carriercontentHeight);

        //Button Text zu Carrier Name ändern
        carrierButtons[lastIndex].GetComponentInChildren<Text>().text = name;

        carrierButtons[lastIndex].GetComponent<PrefabCarrierInfo>().CarrierID = carrierid;
        carrierButtons[lastIndex].GetComponent<PrefabCarrierInfo>().CarrierName = name;

    }

    public void Toggle_Changed(bool newValue)
    {
        if (newValue == true)
        {
            ShowCarrierToStation();
        }
        else
        {
            ShowAllCarrier();
        }
    }

    public void ShowCarrierToStation()
    {
        ClearCarrierList();

        Station station = StationHandler.GetSelectedStation();

        string sid = station.GetID();
        List<QrCode> carrierList = CarrierHandler.Instance.getQrCodesForStation(sid);
        
        foreach(QrCode el in carrierList)
        {
            Carrier carrier = GameManager.Instance.GetCarrierByID(Int32.Parse(el.Text));
            AddNewButton(carrier.name, carrier.id);
        }
    }

    public void ShowAllCarrier()
    {
        ClearCarrierList();
        LoadCarrierButtons();
    }

    public void ClearCarrierList()
    {
        foreach(GameObject el in carrierButtons)
        {
            Destroy(el);
            carrierButtons.Remove(el);
        }

        carriercontentHeight = 0.0F;
        carrierCount = 0;
        RectTransform rt = ContentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, carriercontentHeight);
    }

    //Löscht den Button und den Datensatz eines Carriers
    // ------Flo's Funktion zum löschen des Speichersatzes noch benötigt -----
    public void DeleteCarrier()
    {
        //Flo's Funktion um Carrier zu löschen 

        int carrierIdToDelete = Int32.Parse(UpdateCarrierID.text);
        foreach (GameObject el in carrierButtons)
        {
            if (el.GetComponent<PrefabCarrierInfo>().CarrierID == carrierIdToDelete)
            {
                Destroy(el);
                carrierButtons.Remove(el);
                break;
            }
        }

        carriercontentHeight -= 40.0F;
        carrierCount -= 1;
        RectTransform rt = ContentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, carriercontentHeight);

        statusfield.ChangeStatus("Carrier wurde gelöscht");
        CloseUpdatePanel();
    }

    //Speichert modifizierte Carrier Daten ab und schließt das Update/Info Panel
    //------Flo's Funktion um modifizierte Daten zu Speichern fehlt noch--------
    public void AcceptUpdate()
    {
        if (UpdateCarrierName.text == "" || UpdateCarrierName.text == " " || UpdateCarrierName.text == "  ")
        {
            UpdateCarrierName.image.color = Color.red;
            return;
        }
        //Flo Funktion müssen neue Parameter übergeben werden

        UpdateCarrierNameData(UpdateCarrierName.text);

        statusfield.ChangeStatus("Carrier wurde geupdatet");
        ClearUpdateSettings();
        CloseUpdatePanel();
    }

    //Updatet bei Namensupdate den Namen des Buttons
    public void UpdateCarrierNameData(string newName)
    {
        carrierButtons[Int32.Parse(UpdateCarrierID.text) - 1].GetComponentInChildren<Text>().text = newName;
        carrierButtons[Int32.Parse(UpdateCarrierID.text) - 1].GetComponent<PrefabCarrierInfo>().CarrierName = newName;
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
        UpdateCarrierName.image.color = Color.white;
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
