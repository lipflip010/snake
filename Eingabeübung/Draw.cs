using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;


namespace Eingabeübung 
{
    class Draw : Form
    {
        //Felder
        private Graphics _grafikobjekt;
        private bool _gameover;
        private BufferedGraphics _buffer;
        private Dictionary<string,Pen> _stifte = new Dictionary<string,Pen>();
        private Dictionary<string, Image> _bilder = new Dictionary<string, Image>();
        //Eigenschaftsmethoden
        public Graphics grafikobjekt
        {
            get { return _grafikobjekt; }
            set { _grafikobjekt = value; }
        }

        //Methoden
        #region Methode zeichne +überladungen
        public void zeichne(Rectangle rect, string farbe, bool status)
        {
            if (status && !_gameover)
            {
                _grafikobjekt.DrawRectangle(_stifte[farbe], rect);
            }
            _gameover = false;
        }
        public void zeichne(Rectangle[] rect, string farbe, bool status)
        {
            if (status && !_gameover)
            {
                _grafikobjekt.DrawRectangles(_stifte[farbe], rect);
            }
            _gameover = false;

        }
        public void zeichne(Rectangle[] rect, Image bild, bool status)
        {
            if (status && !_gameover)
            {
                foreach (Rectangle item in rect)
                {
                    _grafikobjekt.DrawImage(bild, item);
                }
            }
            _gameover = false;

        }
        public void zeichne(Point punkt, Image bild, bool status)
        {
            if (status && !_gameover)
            {
                _buffer.Graphics.DrawImage(bild, punkt);
            }
            _gameover = false;
        }
        public void zeichne(Point[] punkte, Image bild, bool status)
        {
            if (status && !_gameover)
            {
                foreach (Point item in punkte)
                {
                    _buffer.Graphics.DrawImage(bild, item);
                }
            }
            _gameover = false;
        }
        public void zeichne(Dictionary<int,Point> punkte, Image bild, bool status)
        {
            if (status && !_gameover)
            {
                for (int i = punkte.Count; i >0; i--)
                {
                    _buffer.Graphics.DrawImage(bild, punkte[i-1]);
                }
            }
            _gameover = false;
        }
        #endregion
        /*public void ladeBitmap(string name, string pfad)
        {
          Bitmap bild = new Bitmap(File.Open(@name, FileMode.Open));
          _bilder.Add(name, bild);
        }*/
        public void Render()
        {
            try
            {
                _buffer.Render();
            }
            catch 
            {
              
                
            }

        }
        public void Clear()
        {
            _buffer.Graphics.Clear(Color.Khaki);
        }
        #region Konstruktor
        //Konstruktor
        public Draw()
        {
            _gameover = false;
            _stifte.Add("grün", new Pen(Color.Green));
            _stifte.Add("rot", new Pen(Color.Red));
            _stifte.Add("gelb", new Pen(Color.Yellow));
            _stifte.Add("blau", new Pen(Color.Blue));
            _stifte.Add("schwarz", new Pen(Color.Black));
        }
        public Draw(Graphics g)
        {
            _gameover = false;
            _grafikobjekt = g;
            _stifte.Add("grün", new Pen(Color.Green));
            _stifte.Add("rot", new Pen(Color.Red));
            _stifte.Add("gelb", new Pen(Color.Yellow));
            _stifte.Add("blau", new Pen(Color.Blue));
            _stifte.Add("schwarz", new Pen(Color.Black));
        }
        public Draw(BufferedGraphics b, ref bool gameover)
        {
            
            _gameover = gameover;
            if (!(b == null))
            {
                _buffer = b;
            }
            else
            {
                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                _buffer = currentContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            }
            _stifte.Add("grün", new Pen(Color.Green));
            _stifte.Add("rot", new Pen(Color.Red));
            _stifte.Add("gelb", new Pen(Color.Yellow));
            _stifte.Add("blau", new Pen(Color.Blue));
            _stifte.Add("schwarz", new Pen(Color.Black));
        }
        #endregion

    }
}
