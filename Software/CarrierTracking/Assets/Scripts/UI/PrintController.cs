using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class PrintController : MonoBehaviour
{
    public GameObject Panel; 
    public Text PathText;
    public InputField IDText;
    public Dropdown CarrierSelect;
    
    public StatusController statusfield;

    string path = " ";

    public void AcceptPrint()
    {
        //GameManager Funktion um StationID mithilfe des Stationnamens zu finden
        int stationid = GameManager.Instance.GetStationID(CarrierSelect.options[CarrierSelect.value].text);

        //Fabis Funktion (ID des related Carriers + Location QR(path/PathText.text)) 
        QrCodeRecognition.saveBitmap(stationid.ToString(), PathText.text);

        statusfield.ChangeStatus("QR Code printed");
        CancelSettings();
    }

    public void DeclinePrint()
    {
        CancelSettings();
    }

    //Öffnet den Windows Explorer
    public void OpenExplorer()
    {       
        path = EditorUtility.SaveFilePanel("Save QR Code as PNG", "", null + "Default.png","png");
        PathText.text = path;
    }

    //Öffnet Panel um weitere Einstellungen treffen zu können, Add, Delete oder Quit Panel 
    public void OpenPanel()
    {
        if(Panel != null)
        {
            Panel.SetActive(true);
            AddStationDropDown();
        }
    }

    //Schließt Panel mit des Decline/Cancel Buttons
    public void ClosePanel()
    {
        Panel.SetActive(false);
    }

    //Löscht alle Eingaben des Benutzers
    public void CancelSettings()
    {
        PathText.text = "";
        IDText.text = "";
        Panel.SetActive(false);
    }

    //Lade Carrier in Dropdown Field Related Carriers
    public void AddStationDropDown()
    {
        List<string> list = new List<string>();
        
        foreach(Carrier element in GameManager.Instance.Carriers)
        {
            list.Add(element.name);
        }

        CarrierSelect.ClearOptions();
        CarrierSelect.AddOptions(list);
    }
}
