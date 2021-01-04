using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System;

public class AddCarrierControll : MonoBehaviour
{
    string path = "";
    public GameObject Panel; 
    public InputField CarrierName;
    public InputField CarrierID;
    public Dropdown DropStation;
    public Text Model;
    public InputField Info;

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

        DropStation.ClearOptions();
        DropStation.AddOptions(list);
    }

    //Ruft CarrierController auf und erstellt neuen Button
    public void AddCarrierButton(string name, int carrierid)
    {
        CarrierController.Instance.AddNewButton(name, carrierid);
    }

    //Addet neuen Button und speichert Eingaben in Binary File
    public void AcceptButton()
    {
        try
        {
            //Carriername + stationid werden Speicherliste hinzugefügt
            //Carriername + Carrierid wird Carrier Button Prefab hinzugefügt
            //int stationid = GameManager.Instance.GetStationID(DropStation.options[DropStation.value].text);

            int carrierid = GameManager.Instance.generateCarrier(CarrierName.text, 1);

            CarrierController.Instance.AddNewButton(CarrierName.text, carrierid);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        ClearFields();
        statusfield.ChangeStatus("New Carrier added");
        ClosePanel();
    }

    //Schließt Panel mit des Decline/Cancel Buttons
    public void ClosePanel()
    {
        Panel.SetActive(false);
    }

    //Öffnet Add Carrier Panel und füllt Dropdown mit Station Namen
    public void OpenPanel()
    {
        Panel.SetActive(true);
        AddStationInDropDown();
    }

    //Öffnet Windows Explorer um Model File auswählen zu können
    public void OpenExplorer()
    {
        path = EditorUtility.OpenFilePanel("Load png Textures", "", "");
        Model.text = path;
    }

    //Löscht Eingaben aus Panel
    public void ClearFields()
    {
        CarrierName.text = "";
        CarrierID.text = "";
        Model.text = " ";
        Info.text = "";
    }
}
