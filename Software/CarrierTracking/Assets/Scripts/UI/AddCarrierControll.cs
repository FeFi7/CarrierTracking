using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System;

public class AddCarrierControll : MonoBehaviour
{
    //Set by inspector
    public GameObject panel; 
    public InputField carrierName;
    public InputField carrierID;
    public Dropdown dropStation;
    public Text model;
    public InputField info;

    //Set by inspector
    public StatusController statusfield;

    //Decline Button Click schließt Panel und löscht Eingaben des Benutzers
    public void DeclineButton()
    {
        ClearFields();
        ClosePanel();
    }

    //Fügt Stationnamen dem Station Dropdown Menü ein
    public void AddStationInDropDown()
    {
        List<string> list = new List<string>();

        foreach (DStation element in GameManager.Instance.Stations)
        {
            list.Add(element.name);
        }

        dropStation.ClearOptions();
        dropStation.AddOptions(list);
    }

    //Ruft CarrierController auf und erstellt neuen Button
    public void AddCarrierButton(string name, int carrierid)
    {
        CarrierController.Instance.AddNewButton(name, carrierid);
    }

    //Addet neuen Button und speichert Eingaben in Binary File
    //Eingabe Ohne Namen bzw. mit Leerzeichen als Namen ist nicht möglich
    public void AcceptButton()
    {
        if (carrierName.text == "" || carrierName.text == " " || carrierName.text == "  ")
        {
            carrierName.image.color = Color.red;
            return;
        }

        try
        {
            int carrierid = GameManager.Instance.generateCarrier(carrierName.text, info.text);

            CarrierController.Instance.AddNewButton(carrierName.text, carrierid);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        ClearFields();
        statusfield.ChangeStatus("Neuer Ladungsträger hinzugefügt");
        ClosePanel();
    }

    //Schließt Panel mit des Decline/Cancel Buttons
    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    //Öffnet Add Carrier Panel und füllt Dropdown mit Station Namen
    public void OpenPanel()
    {
        panel.SetActive(true);
        //AddStationInDropDown();
    }

    //Öffnet Windows Explorer um Model File auswählen zu können
    public void OpenExplorer()
    {
#if UNITY_EDITOR
        model.text = EditorUtility.OpenFilePanel("Load png Textures", "", "");
#endif
    }

    //Löscht Eingaben aus Panel
    public void ClearFields()
    {
        carrierName.image.color = Color.white;
        carrierName.text = "";
        carrierID.text = "";
        model.text = " ";
        info.text = "";
    }
}
