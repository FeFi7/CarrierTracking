using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CarrierController : MonoBehaviour
{
    private static CarrierController instance;

    public GameObject Panel;
    public GameObject ContentPanel;
    public GameObject UpdatePanel;

    public InputField UpdateCarrierName;
    public InputField UpdateCarrierID;
    public Dropdown UpdateStationID;
    public Text UpdateModel;
    public InputField UpdateInfo;

    public GameObject ButtonPrefab;

    public StatusController statusfield;

    GameObject newButton;

    static float contentYPos = -50.0F;
    static float contentXPos = 0.0F;
    static float contentZPos = 0.0F;

    private static float contentHeight = 40.0F;

    public void start()
    {
        LoadCarrierButtons();
    }
    public static CarrierController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CarrierController>();
            }
            return instance;
        }

    }


    public void AddNewButton(string name, int carrierid)
    {
        //Neuen Button erstellen und positionieren
        newButton = Instantiate(ButtonPrefab) as GameObject;
        newButton.GetComponent<RectTransform>().SetParent(ContentPanel.transform, false);
        RectTransform rt = ContentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, contentHeight);

        //Vergrößere Content Field um eine Buttongröße (Height +40)
        contentHeight += 40.0F;

        //Button Text zu Carrier Name ändern
        newButton.GetComponentInChildren<Text>().text = name;

        newButton.GetComponent<PrefabCarrierInfo>().CarrierID = carrierid;
        newButton.GetComponent<PrefabCarrierInfo>().CarrierName = name; 


    }

    //Löschen eines Carriers
    public void DeleteCarrier()
    {

        //Flo's Funktion um Carrier zu löschen 

        GameObject.Destroy(newButton, 1.0f);

        statusfield.ChangeStatus("Carrier wurde gelöscht");
        //ClearUpdateSettings();
        CloseUpdatePanel();
    }

    //
    public void AcceptUpdate()
    {
        //Flo Funktion müssen neue Parameter übergeben werden

        statusfield.ChangeStatus("Carrier wurde geupdatet");
        ClearUpdateSettings();
        CloseUpdatePanel();
    }

    public void CancelUpdate()
    {
        ClearUpdateSettings();
        CloseUpdatePanel();
    }

    public void ClearUpdateSettings()
    {
        UpdateCarrierName.text = "";
        UpdateCarrierID.text = "";
        //UpdateStationID
        UpdateModel.text = "";
        UpdateInfo.text = "";
    }

    public void OpenInfo(int carrierid)
    {
        Carrier carrier = GameManager.Instance.GetCarrierByID(carrierid);
        OpenUpdatePanel();

        Debug.Log(carrier.name);

        UpdateCarrierName.text = carrier.name;
        UpdateCarrierID.text = carrier.id.ToString();

        //Station ID dem richtigen Dropdown Select zuweisen
        //UpdateStationID. = carrier.station
    }

    public void OpenUpdatePanel()
    {
        if (UpdatePanel != null)
        {
            UpdatePanel.SetActive(true);
        }
    }

    public void CloseUpdatePanel()
    {
        UpdatePanel.SetActive(false);
    }

    //Öffnet Panel 
    public void OpenPanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }

    //Schließt Panel mit des Decline/Cancel Buttons
    public void ClosePanel()
    {
        Panel.SetActive(false);
    }

    public void LoadCarrierButtons()
    {
        foreach(Carrier element in GameManager.Instance.Carriers)
        {
            AddNewButton(element.name, element.id);
        }
    }
}
