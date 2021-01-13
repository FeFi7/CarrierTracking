using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class PrintController : MonoBehaviour
{
    //Set in Inspector
    public GameObject panel; 
    public Text pathText;
    public InputField idText;
    public Dropdown carrierSelect;
    
    //Set in Inspector
    public StatusController statusfield;

    //Befüllt das Eingabefeld CarrierID mit der richtigen ID bei jeder neuen Auswahl des Carrier Dropdowns
    public void FillID()
    {
        idText.text = GameManager.Instance.GetCarrierByName(carrierSelect.options[carrierSelect.value].text);
    }

    //Wird beim Programmstart ausgeführt
    public void Start()
    {
        carrierSelect.onValueChanged.AddListener(delegate { FillID(); });
    }

    //Speichert QR Code ab
    public void AcceptPrint()
    {
        //Fabis Funktion (ID des related Carriers + Location QR(path/PathText.text)) 
        QrCodeRecognition.saveBitmap(idText.text, pathText.text);

        statusfield.ChangeStatus("QR Code printed");
        CancelSettings();
    }

    //Schließt Panel und löscht Eingaben des Benutzers
    public void DeclinePrint()
    {
        CancelSettings();
    }

    //Öffnet den Windows Explorer
    public void OpenExplorer()
    {       
        pathText.text = EditorUtility.SaveFilePanel("Save QR Code as PNG", "", null + "Default.png","png");
    }

    //Öffnet Panel um weitere Einstellungen treffen zu können, Add, Delete oder Quit Panel 
    public void OpenPanel()
    {
        if(panel != null)
        {
            panel.SetActive(true);
            //AddCarrierDropDown();
        }
    }

    //Schließt Panel mit des Decline/Cancel Buttons
    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    //Löscht alle Eingaben des Benutzers
    public void CancelSettings()
    {
        pathText.text = "";
        idText.text = "";
        panel.SetActive(false);
    }

    //Lade Carrier in Dropdown Field Related Carriers
    public void AddCarrierDropDown()
    {
        List<string> list = new List<string>();

        list.Add("---Select----");
        
        foreach(Carrier element in GameManager.Instance.Carriers)
        {
            list.Add(element.name);
        }

        carrierSelect.ClearOptions();
        carrierSelect.AddOptions(list);
    }
}
