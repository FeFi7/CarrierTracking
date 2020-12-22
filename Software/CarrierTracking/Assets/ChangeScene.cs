using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void changeScene(string Scenename)
    {
        SceneManager.LoadScene("StationsScene", LoadSceneMode.Single);
        //Application.LoadLevel(Scenename);
    }
}
