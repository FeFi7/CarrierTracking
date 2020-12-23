using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierController : MonoBehaviour
{
    public GameObject Panel;
    public GameObject ScrollView; 

    public void AddNewButton(string name)
    {
        //ScrollView.AddComponent
        Debug.Log("Button wird generiert");
    }

    public void DelButton()
    {

    }

    public void OpenInfo()
    {

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
