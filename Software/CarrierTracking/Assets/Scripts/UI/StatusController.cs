using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class StatusController : MonoBehaviour
{
    string status = " ";
    public Text statusfield;

    public void ChangeStatus(string status)
    {
        statusfield.text = status;
    }
}

