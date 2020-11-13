using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{

    public class QrCode
    {
        public int X { get; }
        public int Y { get; }
        public int Degree { get; }
        public string Text { get; }

        public QrCode(int x, int y, string text)
        {
            this.X = x;
            this.Y = y;
            this.Text = text;
            this.Degree = -1;
        }

        public QrCode(int x, int y, int degree, string text)
        {
            this.X = x;
            this.Y = y;
            this.Degree = degree;
            this.Text = text;
        }

    }
}