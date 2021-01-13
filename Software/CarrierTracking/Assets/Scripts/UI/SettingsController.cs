using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System;

public class SettingsController : MonoBehaviour
{
    //Set by inspector
    public GameObject panel;

    //Set by inspector
    public Dropdown cycleDrop;
    public Text pathText;

    private int[] dropIndex = new int[8] { 0, 30, 60, 120, 300, 1800, 3600, 8640000 };

    //Set by inspector
    public StatusController statusfield;

    //Lädt die gespeicherten Optionen (Ordnerpfad, Cycletime) in das SettingsPanel
    public void LoadSettings()
    {
        int idx = 0;
        for(int i = 0; i < dropIndex.Length; i++)
        {
            if(dropIndex[i] == GameManager.Instance.CycleTime)
            {
                idx = i;
                break;
            }
        }
        cycleDrop.value = idx;

        pathText.text = GameManager.Instance.PathToPictures;
    }

    //Öffnet Windows Explorer und lässt Ordner auswählen
    //Return den ausgewählten Ordnerpfad
    public void OpenExplorer()
    {
        pathText.text = EditorUtility.OpenFolderPanel("Load png Textures", "", "");
    }

    //Button Save Settings
    //Speichert Cycle-Time + Ordnerpfad ab
    public void SaveSettings()
    {
        try
        {
            GameManager.Instance.PathToPictures = pathText.text;
            GameManager.Instance.CycleTime = dropIndex[cycleDrop.value];
            FileHandler.Instance.setSettingsChanged();   //restart InvokeRepeat in FileHandler on changed settings
            GameManager.Instance.saveSettings();
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        statusfield.ChangeStatus("Optionen übernommen");
        panel.SetActive(false);
    }

    //Setzt Fenster Eingaben zurück
    public void CancelSettings()
    {
        panel.SetActive(false);
        pathText.text = " ";
    }

    //Öffnet das SettingsPanel
    public void OpenPanel()
    {
        panel.SetActive(true);
        LoadSettings();
    }
}
