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
    
    public StatusController statusfield;

    string path = " ";

    public void AcceptPrint()
    {
        //Fabis Funktion 

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
        path = EditorUtility.OpenFolderPanel("Load png Textures", "", "");
        PathText.text = path;
    }

    //Öffnet Panel um weitere Einstellungen treffen zu können, Add, Delete oder Quit Panel 
    public void OpenPanel()
    {
        if(Panel != null)
        {
            Panel.SetActive(true);
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
}
