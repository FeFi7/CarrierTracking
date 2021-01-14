using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AreaObjects
{
    public float[] position;
    public float[] rotation;
    public float[] scale;



    public AreaObjects(GameObject data, float[] centerLocation)
    {
        position = new float[3];
        rotation = new float[4];
        scale = new float[3];

        position[0] = data.transform.position.x - centerLocation[0];
        position[1] = data.transform.position.y - centerLocation[1];
        position[2] = data.transform.position.z - centerLocation[2];

        rotation[0] = data.transform.rotation.x;
        rotation[1] = data.transform.rotation.y;
        rotation[2] = data.transform.rotation.z;
        rotation[3] = data.transform.rotation.w;

        scale[0] = data.transform.localScale.x;
        scale[1] = data.transform.localScale.y;
        scale[2] = data.transform.localScale.z;
    }

    public GameObject ReturnToGameObject()
    {


        Vector3 position;
        position.x = this.position[0];
        position.y = this.position[1];
        position.z = this.position[2];
        GameObject data = GameObject.Instantiate(StationHandler.GetDefaultCameraArea(), position, Quaternion.identity);

        Quaternion rotation;
        rotation.x = this.rotation[0];
        rotation.y = this.rotation[1];
        rotation.z = this.rotation[2];
        rotation.w = this.rotation[3];
        data.transform.rotation = rotation;

        Vector3 scale;
        scale.x = this.scale[0];
        scale.y = this.scale[1];
        scale.z = this.scale[2];
        data.transform.localScale = scale;




        return data;


    }




}