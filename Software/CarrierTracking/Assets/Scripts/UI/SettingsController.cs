using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System;

public class SettingsController : MonoBehaviour
{
    string path = " ";
    public GameObject Panel;
    public Dropdown CycleDrop;
    public Text PathText;
    int[] DropIndex = new int[8] { 0, 30, 60, 120, 300, 1800, 3600, 8640000 };
    public StatusController statusfield;

    public void LoadSettings()
    {
        int idx = 0;
        for(int i = 0; i < DropIndex.Length; i++)
        {
            if(DropIndex[i] == GameManager.Instance.CycleTime)
            {
                idx = i;
                break;
            }
        }
        CycleDrop.value = idx;

        PathText.text = GameManager.Instance.PathToPictures;
    }

    //Öffnet Windows Explorer und lässt Ordner auswählen
    //Return den ausgewählten Ordnerpfad
    public void OpenExplorer()
    {
        path = EditorUtility.OpenFilePanel("Load png Textures", "", "");
        PathText.text = path;
    }

    //Button Save Settings
    //Speichert Cycle-Time + Ordnerpfad ab
    public void SaveSettings()
    {

        try
        {
            GameManager.Instance.PathToPictures = PathText.text;
            GameManager.Instance.CycleTime = DropIndex[CycleDrop.value];
            FileHandler.Instance.setSettingsChanged();   //restart InvokeRepeat in FileHandler on changed settings
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
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

    public void OpenPanel()
    {
        Panel.SetActive(true);
        LoadSettings();
    }
}
