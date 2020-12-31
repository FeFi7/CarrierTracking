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

            //Carrier wird Speicherliste hinzugefügt + //StationID ÄNDERN//

            //GameManager.Instance.generateCarrier(CarrierName.text, DropStation.options[DropStation.value].text);
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
