using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class StatusController : MonoBehaviour
{
    //Set by inspector
    public Text statusfield;

    //Setzt die neue Statusmeldung in das Status Panel
    public void ChangeStatus(string status)
    {
        statusfield.text = status;
    }
}

