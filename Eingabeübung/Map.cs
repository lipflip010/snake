using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Eingabeübung
{
    class map
    {
        //Felder
        private static Bitmap _karte;
        private  Point[]  _hindernisse;
        private static int _PixelPerMove;
      
        private Dictionary<int, Point> _schlange;
        
      
        //Konstruktor
        public map(string filepath, int PPM)
        {
            FileStream Source = File.Open(filepath, FileMode.Open);
            _karte = new Bitmap(Source);
            _PixelPerMove = PPM;
            _hindernisse = ObjekteList(Color.Black);
            _schlange = ObjekteDict(Color.Green);
        }
        //Eigenschaftsmethoden
        public Dictionary<int, Point> schlange 
        {
            get { return _schlange; }
        }
        public int Height
        {
            get { return _karte.Height; }
            
        }
        public Point[] Hindernisse
        {
            get { return _hindernisse; }
        }

        //Methoden
        public Color Farbe(int x, int y)
        {
            return _karte.GetPixel(x, y);
        }

        static public int NumberOf(Color farbe)
        {
            int number = 0;

            for (int y = 0; y < _karte.Height; y++)
            {
                for (int x = 0; x < _karte.Width; x++)
                {

                    if (_karte.GetPixel(x, y).ToArgb() == farbe.ToArgb())
                    {
                        number++;
                    }
                }
            }

            return number;
        }
        static Point[] ObjekteList(Color farbe)
        {
            Point[] punkte = new Point[NumberOf(farbe)];
            
            int count = 0;
            for (int y = 0; y < _karte.Height; y++)
            {
                for (int x = 0; x < _karte.Width; x++)
                {

                    if (_karte.GetPixel(x, y).ToArgb() == farbe.ToArgb())
                    {
                        punkte[count] = new Point(x*_PixelPerMove,y*_PixelPerMove);
                        count++;
                        
                    }
                }
            }
            
            return punkte;
        }
        static Dictionary<int,Point>  ObjekteDict(Color farbe)
        {
            Dictionary<int,Point> dict = new Dictionary<int,Point>(NumberOf(farbe));

            int count = 0;
            for (int y = 0; y < _karte.Height; y++)
            {
                for (int x = 0; x < _karte.Width; x++)
                {

                    if (_karte.GetPixel(x, y).ToArgb() == farbe.ToArgb())
                    {
                        dict.Add(count, new Point(x * _PixelPerMove, y * _PixelPerMove));
                        count++;

                    }
                }
            }

            return dict;
        }

    }
}
