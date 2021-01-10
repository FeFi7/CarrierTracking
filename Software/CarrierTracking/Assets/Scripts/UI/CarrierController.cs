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

    //Set by inspector
    public GameObject panel;
    public GameObject contentPanel;
    public GameObject updatePanel;
    public GameObject printPanel;

    //Set by inspector
    public InputField updateCarrierName;
    public InputField updateCarrierID;
    public Dropdown updateStationID;
    public Text updateModel;
    public InputField updateInfo;

    //Set by inspector
    public Text printCarrier;
    public InputField printID;

    //Set by inspector
    public GameObject buttonPrefab;

    //Set by inspector
    public StatusController statusfield;

    private GameObject newButton;
    private static List<GameObject> carrierButtons = new List<GameObject>(); 

    private static float carriercontentHeight = 0.0F;
    private static int carrierCount = 1;

    private static bool loadCarrierInfo = false;

    //Befüllt im Print Panel das Feld der ID mit der zugehörigen Carrier ID
    public void PrintFillID()
    {
        printID.text = updateCarrierID.text;
    }

    //Speichert den QR Code an der angegebenen File Path ab
    //Der File path darf dazu nicht leer oder mit Leerzeichen befüllt sein
    public void AcceptPrint()
    {
        if(printCarrier.text == ("") || printCarrier.text == (" ") || printCarrier.text == ("  "))
        {
            return;
        }

        QrCodeRecognition.saveBitmap(printID.text, printCarrier.text);

        ChangePrintToUpdate();
        statusfield.ChangeStatus("QR Code für Carrier gespeichert");
    }

    //Wechselt zurück zum Update/Info Panel ohne ein QR Code zu speichern
    public void DeclinePrint()
    {
        ChangePrintToUpdate();
    }

    //Schließt das Info/Update Panel und wechselt zum Print Panel
    public void ChangeUpdateToPrint()
    {
        updatePanel.SetActive(false);
        printPanel.SetActive(true);
        PrintFillID();
    }

    //Schließt das Print Panel und wechselt zum Info/Update Panel
    public void ChangePrintToUpdate()
    {
        updatePanel.SetActive(true);
        printPanel.SetActive(false);
    }

    //Wird zum Programmstart ausgeführt --> lädt alle Carrier als Buttons in die Carrier Liste 
    public void Start()
    {
        LoadCarrierButtons();
        loadCarrierInfo = true;
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

        carrierButtons.Add(Instantiate(buttonPrefab) as GameObject);
        int lastIndex = carrierButtons.Count - 1;

        carrierButtons[lastIndex].GetComponent<RectTransform>().SetParent(contentPanel.transform, false);
        RectTransform rt = contentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, carriercontentHeight);

        carrierButtons[lastIndex].GetComponentInChildren<Text>().text = name;

        carrierButtons[lastIndex].GetComponent<PrefabCarrierInfo>().CarrierID = carrierid;
        carrierButtons[lastIndex].GetComponent<PrefabCarrierInfo>().CarrierName = name;

    }

    //Wenn Toggle Feld ausgefüllt ist werden nur Carrier dargestellt, welche in der Station erkannt wurden
    //Wenn Toggle Feld nicht ausgefüllt ist werden alle Carrier in der Liste dargestellt
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

    //Carrier Buttons Liste zeigt nur noch in Station erkannte Carrier an 
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

    //Leert die Carrier Liste erst und fügt danach alle vorhandenen Carrier der Liste und der carrierButtons Liste neu hinzu
    public void ShowAllCarrier()
    {
        ClearCarrierList();
        LoadCarrierButtons();
    }

    //Löscht alle Buttons(GameObject) aus der Carrier Liste und leert die carrierButtons Liste
    public void ClearCarrierList()
    {
        foreach(GameObject el in carrierButtons)
        {
            Destroy(el);
            carrierButtons.Remove(el);
        }

        carriercontentHeight = 0.0F;
        carrierCount = 0;
        RectTransform rt = contentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, carriercontentHeight);
    }

    //Löscht den Carrier Button(GameObject), den Datensatz eines Carriers und verkleinert das Contentpanel der Carrier Liste
    public void DeleteCarrier()
    {
        int delCarrier = Int32.Parse(updateCarrierID.text);

        foreach (GameObject el in carrierButtons)
        {
            if (el.GetComponent<PrefabCarrierInfo>().CarrierID == delCarrier)
            {
                Destroy(el);
                carrierButtons.Remove(el);
                GameManager.Instance.deleteCarrierByID(delCarrier);
                break;
            }
        }

        carriercontentHeight -= 40.0F;
        carrierCount -= 1;
        RectTransform rt = contentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, carriercontentHeight);

        statusfield.ChangeStatus("Carrier wurde gelöscht");
        CloseUpdatePanel();
    }

    //Speichert modifizierte Carrier Daten ab, aktualisiert Carrier Button Name und schließt das Update/Info Panel
    public void AcceptUpdate()
    {
        if (updateCarrierName.text == "" || updateCarrierName.text == " " || updateCarrierName.text == "  ")
        {
            updateCarrierName.image.color = Color.red;
            return;
        }

        int carrierId = Int32.Parse(updateCarrierID.text);

        GameManager.Instance.modifyCarrierByID(carrierId, updateCarrierName.text);

        UpdateCarrierNameData(updateCarrierName.text);

        statusfield.ChangeStatus("Carrier wurde geupdatet");
        ClearUpdateSettings();
        CloseUpdatePanel();
    }

    //Updatet bei Namensupdate den Namen des Buttons
    public void UpdateCarrierNameData(string newName)
    {
        carrierButtons[Int32.Parse(updateCarrierID.text) - 1].GetComponentInChildren<Text>().text = newName;
        carrierButtons[Int32.Parse(updateCarrierID.text) - 1].GetComponent<PrefabCarrierInfo>().CarrierName = newName;
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
        updateCarrierName.image.color = Color.white;
        updateCarrierName.text = "";
        updateCarrierID.text = "";
        updateModel.text = "";
        updateInfo.text = "";
    }

    //Gibt Informationen in Update/Info Panel über ausgewählten Carrier aus
    public void OpenInfo(int carrierid)
    {
        Carrier carrier = GameManager.Instance.GetCarrierByID(carrierid);
        OpenUpdatePanel();

        updateCarrierName.text = carrier.name;
        updateCarrierID.text = carrier.id.ToString();
    }

    //Öffnet Update/Info Panel
    public void OpenUpdatePanel()
    {
        if (updatePanel != null)
        {
            updatePanel.SetActive(true);
        }
    }

    //Schließt Update/Info Panel
    public void CloseUpdatePanel()
    {
        updatePanel.SetActive(false);
    }

    //Öffnet Panel 
    public void OpenPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    //Schließt Panel mit des Decline/Cancel Buttons
    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    //Wird zum Programmstart aufgerufen, lädt die CarrierButtons 
    public void LoadCarrierButtons()
    {
        if(loadCarrierInfo == true)
        {
            return;
        }
        foreach(Carrier element in GameManager.Instance.Carriers)
        {
            AddNewButton(element.name, element.id);
        }
    }
}
