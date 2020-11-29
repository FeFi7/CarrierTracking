using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void changeScene(string Scenename)
    {
        Application.LoadLevel(Scenename);
    }
}
