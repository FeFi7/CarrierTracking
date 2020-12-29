using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Drawing;

public class CarrierHandler : MonoBehaviour
{
    public GameObject sampleCarrier;
    public GameObject AreasParent;
    public float cycleTime = 30.0f;
    public string ImgPath = "Assets//CameraPics//";

    static bool settingChanged = false;
    
    DirectoryInfo dInfo = new DirectoryInfo(@"Assets//CameraPics//");

    // Start is called before the first frame update
    void Start()
    {
        RestartInvoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (settingChanged) 
        {
            RestartInvoke();
            settingChanged = false;
        }
    }

    //restart cycle after time was changed in settings
    public void ChangeCycleTime(int seconds)
    {
        cycleTime = (float)seconds;
        RestartInvoke();
    }

    //restarty cycle after path was changed in settings
    public void ChangePathFolder (string Path)
    {
        ImgPath = Path;
        RestartInvoke();
    }

    //restart cycle manually
    void RestartInvoke()
    {
        CancelInvoke();
        InvokeRepeating("checkForPic", 5, cycleTime);
    }

    void checkForPic()
    {
        var whitelist = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };

        string[] files = Directory.GetFiles(ImgPath);

        string qr = "";

        //check all files in ImgPath
        foreach (string s in files)
        {
            FileInfo fi = null;
            try
            {
                fi = new FileInfo(s);
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e.Message);
                continue;
            }

            //if file extension in whitelist, get get qrCodes
            for (int i = 0; i <= whitelist.Length - 1; i++)
            {
                
                if (whitelist[i].Contains(fi.Extension))
                {
                    qr = ImgPath + fi.Name;

                    //get width and height of pic for relative calculation in calcQR
                    using (var fileStream = new FileStream(qr, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var image = Image.FromStream(fileStream, false, false))
                        {
                            var imgX = image.Width;
                            var imgY = image.Height;

                            fileStream.Close();

                            //calc position and place GameObject relative to the image
                            calcQR(qr, imgX, imgY);
                        }
                    }
                }
            }
        }
    }

    void calcQR(string qrImage, float imgX, float imgY)
    {
        var qrCodes = QrCodeRecognition.getCodesFromPic(qrImage);

        //float maxX = 831.0f;
        //float maxY = 605.0f;

        float maxX = imgX;
        float maxY = imgY;
        float rotation = 0.0f;

        float percentX = 1, percentY = 1;

        foreach (var qrCode in qrCodes)
        {
            percentX = 100.0f / maxX * qrCode.X;
            percentY = 100.0f / maxY * qrCode.Y;
            rotation = qrCode.Degree;
        }

        //percentX = 23;
        //percentY = 24;

        /*foreach (Transform child in AreasParent.transform)
        {
            GameObject area = child.gameObject;
            PositionRelativeTo(sampleCarrier, area, percentX, percentY, rotation);
        }
        */

        foreach (GameObject area in StationHandler.GetAllAreas())
        {
            PositionRelativeTo(sampleCarrier, area, percentX, percentY, rotation);
        }
            


        DeletePic(dInfo);
    }

    public void PositionRelativeTo(GameObject carrier, GameObject area, float percentX, float percentZ, float rotation)
    {
        float offsetX = area.transform.localScale.x / 100.0f * percentX;
        float offsetZ = area.transform.localScale.z / 100.0f * percentZ;

        float obj_rotation = area.transform.rotation.eulerAngles.y;

        offsetX -= (area.transform.localScale.x / 2);
        offsetZ -= (area.transform.localScale.z / 2);

        if (rotation == 180)
        {
            offsetX = -offsetX;
            offsetZ = -offsetZ;
        }
        else if (rotation > 0)
        {
            if (rotation == 90)
            {
                float chache = offsetX;
                offsetX = offsetZ;
                offsetZ = -chache;
            }
            else if (rotation == 270)
            {
                float chache = offsetX;
                offsetX = -offsetZ;
                offsetZ = chache;
            }
        }

        carrier.transform.position = area.transform.position;
        carrier.transform.position = new Vector3(carrier.transform.position.x + offsetX,
            area.transform.position.y + (carrier.transform.localScale.y / 2), carrier.transform.position.z - offsetZ);

        //carrier.transform.rotation = new Vector3(0.0f, 0.0f, obj_rotation);

        carrier.transform.rotation = Quaternion.Euler(0, obj_rotation, 0);

    }

    //delete all files from directory
    public void DeletePic(DirectoryInfo directory)
    {
        foreach (FileInfo file in directory.GetFiles())
            file.Delete();
    }
}
