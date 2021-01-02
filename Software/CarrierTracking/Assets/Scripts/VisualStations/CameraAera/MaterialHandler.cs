using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHandler : MonoBehaviour
{
    GameObject player;



    // Use this for initialization
    void Start()
    {
        player = gameObject;
        GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))

        {


            gameObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Transparent/Standard");
        }
    }
}
