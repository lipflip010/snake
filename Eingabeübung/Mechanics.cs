using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Eingabeübung
{
    class Mechanics
    {
        //Felder
        private string _richtung;
        private bool _gameover;
        private int _änderung;
        private int _höhe;
        private int _breite;
        private int _score;
     
        private Random _zufall;
 
       
        private Point _start;
        //Eigenschaftsmethoden
        public string Richtung
        {
            get { return _richtung; }
            set
            {
                switch (value)
                {
                    case "unten":
                        if (!(_richtung == "oben"))
                            _richtung = "unten";
                        break;
                        
                    case "rechts":
                        if (!(_richtung == "links"))
                            _richtung = "rechts";
                        break;
                    case "oben":
                        if (!(_richtung == "unten"))
                            _richtung = "oben";
                        break;
                    case "links":
                        if (!(_richtung == "rechts"))
                            _richtung = "links";
                        break;

                    default:
                        break;
                        

                }
            }
        }
        public int score
        {
            get { return _score; }
        }
        //Methoden
        private void KolliosionsAbfrage(ref Point[] hindernisse,ref Dictionary<int,Point> schlange, char achse, int änderung, ref int count, ref Point food)
        {
            Point nächsterPunkt = schlange[schlange.Count - 1];
            switch (achse)
            {
                case 'X':
                case 'x':
                    if ((schlange[schlange.Count-1].X + änderung + 10) > _breite)
                    {
                        nächsterPunkt.X = _start.X;
                        break;
                    }
                    if (!((schlange[schlange.Count - 1].X + änderung) >= 0))
                    {
                        nächsterPunkt.X = _breite - 10 - 5;
                        break;
                    }
                    else
                    {
                        nächsterPunkt.X += änderung;
                        break;
                    }
                case 'Y':
                case 'y':
                    if ((schlange[schlange.Count - 1].Y + änderung + 10) > _höhe)
                    {
                        nächsterPunkt.Y = _start.Y;
                        break;
                    }
                    if (!((schlange[schlange.Count - 1].Y + änderung) >= 0))
                    {
                        nächsterPunkt.Y = _höhe - 10 - 5;
                        break;
                    }
                    else
                    {
                        nächsterPunkt.Y += änderung;
                    }
                    break;

            }
            if (food == nächsterPunkt)
            {
                schlange.Add(schlange.Count, nächsterPunkt);
                bool valid = false;
                _score += 10;
                while(!valid)
                {
                    valid = true;
                    food.X = _zufall.Next(0, 60) * _änderung;
                    food.Y = _zufall.Next(0, 30) * _änderung;
                    for (int i = hindernisse.Length-1; i >= 0; i--)
                    {
                        if (hindernisse[i].X == food.X && hindernisse[i].Y == food.Y)
                        {
                            valid = false;
                            break;
                        }
                    }
                    for (int i = schlange.Count - 1; i >= 0; i--)
                    {
                        if (schlange[i].X == food.X && schlange[i].Y == food.Y)
                        {
                            valid = false;
                            break;
                        }   
                    }
                }
                
                
            }

            else //dictionary wird durchgerückt und hinterster punkt entfernt, damit es nicht nur länger wird
            {
                for (int i = 0; i <= schlange.Count - 1; i++)
                {
                    if (schlange.Count - 1 == i)
                    {
                        schlange[i] = nächsterPunkt;
                    }
                    else
                    {
                        schlange[i] = schlange[i + 1];
                    }
                }

            }
            
        }
        public void Fortbewegung(ref Point[] hindernisse,ref Dictionary<int,Point> schlange, ref int count, ref Point food)
        {
            switch (_richtung)
            {
                case "unten":
                    KolliosionsAbfrage(ref hindernisse,ref schlange,'Y', _änderung, ref count,ref food);
                    break;
                case "rechts":
                    KolliosionsAbfrage(ref hindernisse, ref schlange, 'X', _änderung, ref count, ref food);
                    break;
                case "oben":
                    KolliosionsAbfrage(ref hindernisse, ref schlange, 'Y', -_änderung, ref count, ref food);
                    break;
                case "links":
                    KolliosionsAbfrage(ref hindernisse, ref schlange, 'X', -_änderung, ref count, ref food);
                    break;
                default:
                    break;
            }
        }
        //Konstruktor
        public Mechanics(string startrichtung,int höhe, int breite,int änderung,Point start, ref bool gameover)
        {

            _score = 0;
            _zufall = new Random();          
            _gameover = gameover;
            _änderung = änderung; 
            _richtung = startrichtung;
            _höhe = höhe;
            _breite = breite;
            _start = start;
            
        }

    }
}
