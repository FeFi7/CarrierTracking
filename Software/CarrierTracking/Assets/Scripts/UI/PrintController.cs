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

    //Befüllt das Eingabefeld CarrierID mit der richtigen ID bei jeder neuen Auswahl des Carrier Dropdowns
    public void FillID()
    {
        IDText.text = GameManager.Instance.GetCarrierByName(CarrierSelect.options[CarrierSelect.value].text);
    }

    //Wird beim Programmstart ausgeführt
    public void Start()
    {
        CarrierSelect.onValueChanged.AddListener(delegate { FillID(); });
    }

    //Speichert QR Code ab
    public void AcceptPrint()
    {
        //Fabis Funktion (ID des related Carriers + Location QR(path/PathText.text)) 
        QrCodeRecognition.saveBitmap(IDText.text, PathText.text);

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
        path = EditorUtility.SaveFilePanel("Save QR Code as PNG", "", null + "Default.png","png");
        PathText.text = path;
    }

    //Öffnet Panel um weitere Einstellungen treffen zu können, Add, Delete oder Quit Panel 
    public void OpenPanel()
    {
        if(Panel != null)
        {
            Panel.SetActive(true);
            AddCarrierDropDown();
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
    public void AddCarrierDropDown()
    {
        List<string> list = new List<string>();

        list.Add("---Select----");
        
        foreach(Carrier element in GameManager.Instance.Carriers)
        {
            list.Add(element.name);
        }

        CarrierSelect.ClearOptions();
        CarrierSelect.AddOptions(list);
    }
}
