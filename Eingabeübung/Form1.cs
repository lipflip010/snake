using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Eingabeübung
{
    public partial class Form1 : Form
    {

        delegate void SetTextCallback(string text,Control objekt);

        Draw Bild;
        Mechanics Mechanik;
        Bitmap B_Snake = new Bitmap(File.Open(Path.Combine(Environment.CurrentDirectory, @"Assets","snake.bmp"), FileMode.Open));
        Bitmap B_Hinderniss = new Bitmap(File.Open(Path.Combine(Environment.CurrentDirectory, @"Assets\obstacle.bmp"), FileMode.Open));
        Bitmap B_Essen = new Bitmap(File.Open(Path.Combine(Environment.CurrentDirectory, @"Assets\food.bmp"), FileMode.Open));
        BufferedGraphicsContext currentContext;
        BufferedGraphics myBuffer;
        Point[] Hindernisse;
        Point Essen = new Point(14 * 15, 0);
        Dictionary<int, Point> Schlange;
        static bool Pause = false;
        bool Gameover = false;
        static bool Thread_Status = true;//true, wenn der arbeiterthread noch nicht läuft. Wenn dieser gestarted wurde, wird die der bool auf false gesetzt, damit kein weiterer thread gestartet wird.
        static int Snake_Verzögerung = 150;
        static bool Form_Status = true;


        static int Snake_PPM = 15;
        static Point start = new Point(2*Snake_PPM,4*Snake_PPM);
        Point Kopf = start;
        map Karte;
         
        
        public Form1()
        {
            InitializeComponent();
       
            InitializeCustomComponent();
            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate(canvas.CreateGraphics(), canvas.DisplayRectangle);
            Karte = new map(Path.Combine(Environment.CurrentDirectory, @"Assets\karte.bmp"), Snake_PPM);
            Essen = new Point(14 * 15, 30);//erste Essensposition
            Hindernisse = Karte.Hindernisse;
            Schlange = Karte.schlange;
            Mechanik = new Mechanics("rechts",30*Snake_PPM,60*Snake_PPM,Snake_PPM,new Point(0,0),ref Gameover);
            Bild = new Draw(myBuffer,ref Gameover);
          
          
          

  
        }
        #region Events
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Pause = false;
            Form_Status = false;
        }
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Bild.Clear();
            
            Bild.zeichne(Karte.Hindernisse, B_Hinderniss, Form_Status);
            Bild.Render();
            this.ThreadVerwaltung(true);
            
        }
        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (!(Mechanik.Richtung == "oben"))
                        Mechanik.Richtung = "unten";
                    break;
                case Keys.Right:
                    if (!(Mechanik.Richtung == "links"))
                        Mechanik.Richtung = "rechts";
                    break;
                case Keys.Up:
                    if (!(Mechanik.Richtung == "unten"))
                        Mechanik.Richtung = "oben";
                    break;
                case Keys.Left:
                    if (!(Mechanik.Richtung == "rechts"))
                        Mechanik.Richtung = "links";
                    break;
                case Keys.Space:
                    Pause = !Pause;
                    if (Pause)
                    {
                        Thread_Status = Pause;
                        ThreadVerwaltung(true);
                    }
                    break;
                case Keys.Escape:
                    Pause = !Pause;
                    Application.Exit();
                    break;
            }
            

            Thread.Sleep(Snake_Verzögerung+20);
        }
        #endregion
        #region Methoden
       
        private void ThreadVerwaltung(bool schalter)
        {
            
            if (Thread_Status)
            {
                Thread thread = new Thread(new ThreadStart(Arbeiter));
                thread.Start();
                Thread_Status = false;
                if (!schalter)
                {
                    thread.Abort();
                    Thread_Status = true;
                }
            }
        }
        private void Arbeiter()
        {
            int count = 0;
            while (Pause)
            {
                Bild.Clear();
                Mechanik.Fortbewegung(ref Hindernisse,ref Schlange,ref count , ref Essen);
                this.setText(Mechanik.score.ToString(), label2);
                
                Bild.zeichne(Schlange, B_Snake, Form_Status);
                Bild.zeichne(Karte.Hindernisse, B_Hinderniss, Form_Status);
                Bild.zeichne(Essen, B_Essen, Form_Status);
              
              
                Bild.Render();
                Gameover = false;
                count++;
                Thread.Sleep(Snake_Verzögerung);
            }
            ThreadVerwaltung(false);
        }
        //Methode zum threadsicheren verändern der eigenschaften der Steuerelemente
        private void setText(string text,Control objekt)
        {
           try // Hier tritt ein object disposed exception auf
           {
                if (objekt.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(this.setText);
                    if(!objekt.IsDisposed)
                        this.Invoke(d, new object[] { text, objekt });
                }
                else
                {
                    objekt.Text = text;
                }
            }
            catch{}
        }
        #endregion

       




    }
}
