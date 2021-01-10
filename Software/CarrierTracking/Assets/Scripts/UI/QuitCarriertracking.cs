using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitCarriertracking : MonoBehaviour
{
    //Schließt das Carriertracking Programm endgültig
    public void doquitProgramm()
    {
        Debug.Log("Carriertacking has been closed.");
        Application.Quit();
    }
}
