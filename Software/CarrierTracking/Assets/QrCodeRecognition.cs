using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.Multi;
using ZXing.Common;
using Models;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.IO;
using System.Threading.Tasks;

public class QrCodeRecognition
{
    static int COUNT_VERTICAL_BOXES = 1;
    static int COUNT_HORIZONTAL_BOXES = 2;

    static public async Task<List<QrCode>> getCodesFromPicAsync(string file)
    {
        
        return await Task.Run(() => getCodesFromPic(file)); ;
    }

    static public List<QrCode> getCodesFromPic(string file)
    {
        Debug.Log("Qr Codes werden gelesen");
        Bitmap bmp;
        List<QrCode> codes = new List<QrCode>();

        //IBarcodeReader reader = new BarcodeReader();
        ZXing.Reader reader = new MultiFormatReader();
        // aktiviere den TRY_HARDER Modus
        GenericMultipleBarcodeReader multipleReader = new GenericMultipleBarcodeReader(reader);
        Dictionary<DecodeHintType, object> hints = new Dictionary<DecodeHintType, object>();
        //hints.Add(DecodeHintType.TRY_HARDER, true);

        // lade Bildfile in das Bitmap
        try
        {
            bmp = loadBitmap(file);
            //bmp = (Bitmap)Bitmap.FromFile(file);
        }
        catch (System.IO.FileNotFoundException)
        {
            throw new System.IO.FileNotFoundException();
        }

        var widthBox = bmp.Width / COUNT_HORIZONTAL_BOXES;
        var heightBox = bmp.Height / COUNT_VERTICAL_BOXES;

        Rectangle rect;
        Bitmap bmpCropped;

        for (int h = 0; h < COUNT_HORIZONTAL_BOXES; h++)
        {
            for (int v = 0; v < COUNT_VERTICAL_BOXES; v++)
            {
                int widthRect = widthBox;
                int heightRect = heightBox;

                // Größe der Überlappung festlegen
                int overlapWidth = widthBox / 2;
                int overlapHeight = heightBox / 2;

                // bei x und y immer minus die hälfte einer Boxhöhe/Boxbreite um die bereiche zu überlappen
                int x = h * widthBox - overlapWidth;
                int y = v * heightBox - overlapHeight;
                //Rectangle darf Größe von Bitmap nicht übersteigen
                if (x < 0)
                    x = 0;
                if (x + widthBox + overlapWidth > bmp.Width)
                {
                    x = bmp.Width - widthBox;
                    widthRect = widthBox;
                }
                else
                {
                    widthRect = widthBox + overlapWidth;
                }

                if (y < 0)
                    y = 0;
                if (y + heightBox + overlapHeight > bmp.Height)
                {
                    y = bmp.Height - heightBox;
                    heightRect = heightBox;
                }
                else
                {
                    heightRect = heightBox + overlapHeight;
                }

                rect = new System.Drawing.Rectangle(x, y, widthRect, heightRect);
                bmpCropped = cropImage(bmp, rect);

                // erkenne QRcode auf gecroppten Bitmap
                BitmapLuminanceSource src = new BitmapLuminanceSource(bmpCropped);
                HybridBinarizer binarizer = new HybridBinarizer(src);
                BinaryBitmap binBmp = new BinaryBitmap(binarizer);

                var results = multipleReader.decodeMultiple(binBmp);

                if (results != null)
                {
                    //Debug.Log(results.Length);
                    foreach (Result result in results)
                    {
                        int xFoundUpper = System.Convert.ToInt32(result.ResultPoints[result.ResultPoints.GetUpperBound(0)].X);
                        int yFoundUpper = System.Convert.ToInt32(result.ResultPoints[result.ResultPoints.GetUpperBound(0)].Y);

                        int xFoundLower = System.Convert.ToInt32(result.ResultPoints[result.ResultPoints.GetLowerBound(0)].X);
                        int yFoundLower = System.Convert.ToInt32(result.ResultPoints[result.ResultPoints.GetLowerBound(0)].Y);

                        // Mittelpunkt zwischen LowerBound und UpperBound
                        int x_middled = (Math.Max(xFoundLower, xFoundUpper) - Math.Min(xFoundLower, xFoundUpper)) / 2 + Math.Min(xFoundLower, xFoundUpper);
                        int y_middled = (Math.Max(yFoundLower, yFoundUpper) - Math.Min(yFoundLower, yFoundUpper)) / 2 + Math.Min(yFoundLower, yFoundUpper);

                        // Berechnung der Drehung
                        float xDiff = xFoundUpper - xFoundLower;
                        float yDiff = yFoundUpper - yFoundLower;
                        int angle = Convert.ToInt16((Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI) + 45.0); //+45 Grad, da 45 Grad keine Drehung bedeutet
                        if (angle < 0)
                            angle += 360;

                        //speichere pic mit markierung (falls noch nicht vorhanden)
                        if (!codes.Exists(g => g.Text == result.Text))
                        {
                            codes.Add(new QrCode(x + x_middled, y + y_middled, angle, result.Text));
                        }
                    }
                }
                else
                {
                    Debug.Log("nix gfunden");
                }
            }
        }
        return codes;
    }
    
    private static System.Drawing.Bitmap cropImage(System.Drawing.Bitmap img, System.Drawing.Rectangle cropArea)
    {
        Bitmap bmpImage = new Bitmap(img);
        return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
    }

    static byte[] bytes;
    static byte[] copyToBytes;
    static System.Drawing.Imaging.BitmapData bitmapData;
    static System.IntPtr Iptr = System.IntPtr.Zero;

    public static byte[] bmpToByteArray(Image x)
    {
        ImageConverter _imageConverter = new ImageConverter();
        byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
        return xByte;
    }

    public static Bitmap loadBitmap(string path)
    {
        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
            return new Bitmap(memoryStream);
        }
    }

    public static void saveBitmap(string content, string path)
    {
        Debug.Log("Drucke QrCode: " + content);
        BarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
        writer.Options = new EncodingOptions { Height = 600, Width = 600 };

        var result = writer.Write(content);
        var barcodeBitmap = new Bitmap(result);
        barcodeBitmap.Save(path);
    }

    public static void saveBitmap(string content, string path, int height)
    {
        BarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
        writer.Options = new EncodingOptions { Height = height, Width = height };

        var result = writer.Write(content);
        var barcodeBitmap = new Bitmap(result);
        barcodeBitmap.Save(path);
    }

}
