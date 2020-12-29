using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrierController : MonoBehaviour
{
    public GameObject Panel;
    public GameObject ContentPanel;
    public GameObject ButtonPrefab;

    static float contentYPos = -50.0F;
    static float contentXPos = 0.0F;
    static float contentZPos = 0.0F;

    static float contentHeight = 40.0F;

    public void AddNewButton(string name)
    {
        //Neuen Button erstellen und positionieren
        GameObject newButton = Instantiate(ButtonPrefab) as GameObject;
        newButton.GetComponent<RectTransform>().SetParent(ContentPanel.transform, false);
        RectTransform rt = ContentPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1404.0F, contentHeight);

        //Vergrößere Content Field um eine Buttongröße (Height +40)
        contentHeight += 40.0F;

        //Button Text zu Carrier Name ändern
        newButton.GetComponentInChildren<Text>().text = name;

        //onClick Event zu Button hinzufügen
        newButton.GetComponent<Button>().onClick.AddListener(OpenInfo);

        Debug.Log("Button wird generiert");
    }

    public void DelButton()
    {

    }

    public void OpenInfo()
    {
        //Open Info Panel with Carrier Name in text
        // Flo's Funktion fehlt
    }

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

    }
}
