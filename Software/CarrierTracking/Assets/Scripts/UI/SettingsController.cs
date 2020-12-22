using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class SettingsController : MonoBehaviour
{
    string path;
    public GameObject Panel;
    public Dropdown CycleDrop;
    public Text PathText;
    int[] DropIndex = new int[8] { 0, 30, 60, 120, 300, 1800, 3600, 8640000 };
    public StatusController statusfield;

    public CarrierHandler CarrierHandler;

    //Öffnet Windows Explorer und lässt Ordner auswählen
    //Return den ausgewählten Ordnerpfad
    public void OpenExplorer()
    {
        path = EditorUtility.OpenFolderPanel("Load png Textures", "", "");
        PathText.text = path;
    }

    //Button Save Settings
    //Speichert Cycle-Time + Ordnerpfad ab
    public void SaveSettings()
    {
        try
        {
            CarrierHandler.ChangeCycleTime(DropIndex[CycleDrop.value]);
            CarrierHandler.ChangePathFolder(path);
        }
        catch
        {
            Debug.Log("Ein Fehler ist aufgetreten - Eingaben wurden NICHT verarbeitet.");
        }

        statusfield.ChangeStatus("Settings accepted");
        Panel.SetActive(false);
    }

    //Setzt Fenster Eingaben zurück
    public void CancelSettings()
    {
        Panel.SetActive(false);
        PathText.text = " ";
    }
}
