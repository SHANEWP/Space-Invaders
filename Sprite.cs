using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace Space_Invaders_Shane_Panagakos
{
    public class Sprite
    {
        public Bitmap Pic { get; set; }  // used for single-frame sprites
        public List<Bitmap> Pics { get; set; }  // allows for multi-frame animation
        public List<Bitmap> LeftPics { get; set; }  // pics to use when facing left
        public List<Bitmap> RightPics { get; set; }  // pics to use when facing right
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Rectangle Bounds { get { return new Rectangle(Location.X, Location.Y, Width, Height); } }

        public Point Location = new Point(0, 0);
        private int frame = 0;
        private bool countingUp = true;

        public Sprite(Bitmap bm)
        {
            Pic = bm;
            Width = bm.Width;
            Height = bm.Height;
        }

        public void Draw(Graphics g)
        {
            if (SpeedX >= 0)
                Pics = RightPics;
            else
                Pics = LeftPics;
            if (Pics == null)
                g.DrawImageUnscaled(Pic, Location);
            else if (countingUp)
            {
                g.DrawImageUnscaled(Pics[frame++], Location);
                if (frame == Pics.Count)
                {
                    frame = Pics.Count - 2;
                    countingUp = false;
                }
            }
            else//counting down
            {
                g.DrawImageUnscaled(Pics[frame--], Location);
                if (frame == -1)
                {
                    frame = 1;
                    countingUp = true;
                }
            }
        }
        public void Update()
        {
            Location.X += SpeedX;
            Location.Y += SpeedY;
        }

    }
}