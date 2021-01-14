using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public string PathToPictures = "/CarrierTracking/Software/CarrierTracking/Assets/CameraPics";
    public int CycleTime= 0;

    public GameSettings() {
        PathToPictures = GameManager.Instance.PathToPictures;
        CycleTime = GameManager.Instance.CycleTime;

    
    }
}
