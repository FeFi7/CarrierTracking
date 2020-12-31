using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

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

    public CarrierController carrierController;

    //Decline Button schließt Panel und löscht Eingaben
    public void DeclineButton()
    {
        ClearFields();
        ClosePanel();
    }

    public void AddCarrierDropDown()
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
    public void AddCarrierButton(string name)
    {
        carrierController.AddNewButton(name);
    }

    //Addet neuen Button und speichert Eingaben in Binary File
    public void AcceptButton()
    {
        try
        {
            //Button hinzufügen
            AddCarrierButton(CarrierName.text);

            //Carriername + stationid werden Speicherliste hinzugefügt
            int stationid = GameManager.Instance.GetStationID(DropStation.options[DropStation.value].text);

            GameManager.Instance.generateCarrier(CarrierName.text, stationid);
        }
        catch
        {
            Debug.Log("Ein Fehler ist aufgetreten");
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

    public void OpenPanel()
    {
        Panel.SetActive(true);
        AddCarrierDropDown();
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
