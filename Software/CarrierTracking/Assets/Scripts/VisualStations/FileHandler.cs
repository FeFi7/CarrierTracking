using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


// Kontrolliert regelmäßig zu jeder Station, ob neue Bilder 
// vorhanden sind, liest die QrCodes aus und melden diese an den CarrierHandler
[System.Serializable]
public class FileHandler : MonoBehaviour
{
    private static FileHandler instance;

    string[] whitelist = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
    float cycleTime = 8.0f;
    string imgPath = "Assets//CameraPics//";
    DirectoryInfo dInfo = new DirectoryInfo(@"Assets//CameraPics//");
    bool _settingChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!Directory.Exists(imgPath))
        {
            Directory.CreateDirectory(imgPath);
        }
        // Unterordner für jeden Carrier erstellen, falls noch nicht vorhanden
        foreach (DStation station in GameManager.Instance.Stations)
        {
            var stationPath = imgPath + station.name + "_" + station.StationID + "//";

            if (!Directory.Exists(stationPath))
            {
                Directory.CreateDirectory(stationPath);
            }
        }

        RestartInvoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (_settingChanged)
        {
            RestartInvoke();
        }
    }

    public static FileHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<FileHandler>();
            }
            return instance;
        }
    }

    //Möglichkeit aus anderer Klasse zu sagen, dass Settings geändert wurden
    void setSettingsChanged()
    {
        _settingChanged = true;
    }

    //restart cycle manually
    void RestartInvoke()
    {
        CancelInvoke("checkForNewPics");
        if (_settingChanged)
        {
            cycleTime = GameManager.Instance.CycleTime;
            imgPath = GameManager.Instance.PathToPictures;

            // Falls Unterordner für neuen Pfad noch nicht vorhanden 
            foreach (DStation station in GameManager.Instance.Stations)
            {
                var stationPath = imgPath + station.name + "_" + station.StationID + "//";

                if (!Directory.Exists(stationPath))
                {
                    Directory.CreateDirectory(stationPath);
                }
            }
            Debug.Log("Settings were changed!");
        }
        // Checke alle X Sekunden, ob neue Bilder vorhanden sind
        InvokeRepeating("checkForNewPics", 10.0f, cycleTime);
        _settingChanged = false;
    }

    void checkForNewPics()
    {
        
        string qrPath = "";
        foreach (DStation station in GameManager.Instance.Stations)
        {
            var stationPath = imgPath + station.name + "_" + station.StationID + "//";
            if (!Directory.Exists(stationPath)) //Falls Ordner plötzlich nicht mehr vorhanden
            {
                Directory.CreateDirectory(stationPath);
            }

            var stationDirectory = new DirectoryInfo(stationPath);
            FileInfo newestFile;
            if (stationDirectory.GetFiles().Select(f => whitelist.Contains(f.Extension.ToLower())).Count() > 0)
                newestFile = stationDirectory.GetFiles().OrderByDescending(f => f.LastWriteTime).First(f => whitelist.Contains(f.Extension.ToLower()));
            else //Falls keine Datei im Ordner, breche Schleife ab
                break;

            //if file extension in whitelist, get qrCodes
            if (whitelist.Contains(newestFile.Extension.ToLower()))
            {
                qrPath = stationPath + newestFile.Name;

                //get width and height of pic for relative calculation in calcQR
                using (var fileStream = new FileStream(qrPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var image = Image.FromStream(fileStream, false, false))
                    {
                        var imgX = image.Width;
                        var imgY = image.Height;

                        fileStream.Close();

                        CarrierHandler.Instance.setCarrierPicPixelSizes(imgX, imgY, station.StationID.ToString());
                        readQrCodesFromPic(qrPath, station);
                    }
                }
            }

        }
    }

    // lest qr Codes vom Bild aus und gibt diese an CarrierHandler weiter
    private async void readQrCodesFromPic(string qrPath, DStation station)
    {
        string _stationId = station.StationID.ToString();
        var _qrCodes = await QrCodeRecognition.getCodesFromPicAsync(qrPath);

        CarrierHandler.Instance.setQrCodesForStation(_qrCodes, _stationId);

        cleanDirectory(new FileInfo(qrPath).Directory);
    }

    private void cleanDirectory(DirectoryInfo directory)
    {
        // Lösche alle Files im Ordner bis auf Neueste
        try
        {
            var newestFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First(f => whitelist.Contains(f.Extension.ToLower()));

            foreach (FileInfo fi in directory.GetFiles())
            {
                if (!newestFile.Name.Equals(fi.Name))
                {
                    fi.Delete();
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }
}