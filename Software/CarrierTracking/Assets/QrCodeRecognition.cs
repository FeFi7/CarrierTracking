using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.Multi;
using ZXing.Common;
using Models;
using System.Drawing;
using System.Drawing.Imaging;

public class QrCodeRecognition
{
    static int COUNT_VERTICAL_BOXES = 1;
    static int COUNT_HORIZONTAL_BOXES = 1;

    static public List<QrCode> getCodesFromPic(string file) {
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
            bmp = (Bitmap)Bitmap.FromFile(file);
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
                    Debug.Log(results.Length);
                    foreach (Result result in results)
                    {
                        int xFoundUpper = System.Convert.ToInt32(result.ResultPoints[result.ResultPoints.GetUpperBound(0)].X);
                        int yFoundUpper = System.Convert.ToInt32(result.ResultPoints[result.ResultPoints.GetUpperBound(0)].Y);

                        int xFoundLower = System.Convert.ToInt32(result.ResultPoints[result.ResultPoints.GetLowerBound(0)].X);
                        int yFoundLower = System.Convert.ToInt32(result.ResultPoints[result.ResultPoints.GetLowerBound(0)].Y);

                        //speichere pic mit markierung (falls noch nicht vorhanden)
                        if(!codes.Exists(g => g.Text == result.Text))
                        {
                            codes.Add(new QrCode(x + xFoundUpper, y + yFoundUpper, result.Text));
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

}
