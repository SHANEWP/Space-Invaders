//Shane Panagakos
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Space_Invaders_Shane_Panagakos
{
    public partial class Form1 : Form
    {
        SoundPlayer shoot = new SoundPlayer("Laser.wav");
        SoundPlayer hit = new SoundPlayer("Gunshot.wav");
        Bitmap background;
        Bitmap bullet;
        Sprite player;
        List<Sprite> bullets = new List<Sprite>();
        List<Sprite> aliens = new List<Sprite>();
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            using (Image im = Image.FromFile(Path.GetFullPath(@"..\..\Images\SpaceBackground.jpg")))
            {
                background = new Bitmap(im, 800, 500);
            }
            using (Image im = Image.FromFile(Path.GetFullPath(@"..\..\Images\satellite4.png")))
            {
                bullet = new Bitmap(im, 12, 12);
            }
            createAliens("bug", 0);
            createAliens("spaceship", 1);
            createAliens("satellite", 2);
            createAliens("star", 3);
            createAliens("watchit", 4);
            createAliens("flyingsaucer", 5);
            using (Image im = Image.FromFile(Path.GetFullPath(@"..\..\Images\player.png")))
            {
                Bitmap playerImage = new Bitmap(im);
                player = new Sprite(playerImage);
                player.Location = new Point(375, 450);
                player.SpeedX = 0;
                player.SpeedY = 0;
            }
        }
        private void createAliens(string file, int row)
        {
            List<Bitmap> picList = new List<Bitmap>();
            for (int i=1; i<=4; i++)
                using (Image im = Image.FromFile(Path.GetFullPath(@"..\..\Images\" + file + i.ToString() + ".png")))
                {
                    Bitmap pose = new Bitmap(im);
                    picList.Add(pose);
                }
            for (int i = 0; i < 15; i++)
            {
                Point location = new Point(i*60, row*60);
                Sprite sprt = new Sprite(picList[0]) { Location = location };
                sprt.LeftPics = picList;
                sprt.RightPics = picList;
                sprt.SpeedX = 2; 
                sprt.SpeedY = 0;
                aliens.Add(sprt);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            e.Graphics.DrawImage(background, 0, 0);

            foreach (Sprite et in aliens)
            {
                et.Draw(g);
            }
            foreach (Sprite b in bullets)
            {
                b.Draw(g);
            }
            player.Draw(g);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach(Sprite et in aliens)
            {
                et.Update();
            }
            foreach(Sprite b in bullets)
            {
                b.Update();
            }
            var alienResults =
                from Sprite et in aliens
                orderby et.Location.X
                select et;
            if (alienResults.Count() == 0)
                gameEnded(true);
            else if (alienResults.Last().Location.X > 860)
                foreach (Sprite et in aliens)
                    et.SpeedX = -et.SpeedX;
            else if (alienResults.First().Location.X < -120)
                foreach (Sprite et in aliens)
                {
                    et.SpeedX = -et.SpeedX;
                    if (et.SpeedX >= 0)
                        et.SpeedX += 3;
                    else
                        et.SpeedX -= 3;
                    et.Location.Y += 25;
                }
            var invasionResults =
                from Sprite et in aliens
                where et.Location.Y > 460 || et.Bounds.IntersectsWith(player.Bounds)
                select et;
            if (invasionResults.Count() > 0)
                gameEnded(false);
            List<Sprite> usedBullets = new List<Sprite>();
            foreach (Sprite b in bullets)
            {
                var bulletResults =
                    from Sprite et in aliens
                    where b.Bounds.IntersectsWith(et.Bounds)
                    select et;
                if (bulletResults.Count() > 0)
                {
                    aliens.RemoveAt(aliens.IndexOf(bulletResults.First()));
                    hit.Play();
                    usedBullets.Add(b);
                }
                if (b.Location.Y < 10)
                    usedBullets.Add(b);
            }
            foreach (Sprite b in usedBullets)
                bullets.RemoveAt(bullets.IndexOf(b));
            Refresh();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            Sprite bulletSprite = new Sprite(bullet);
            bulletSprite.Location.X = player.Location.X + 25;
            bulletSprite.Location.Y = 450;
            bulletSprite.SpeedX = 0;
            bulletSprite.SpeedY = -15;
            bullets.Add(bulletSprite);
            shoot.Play();
        }  
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            player.Location.X = e.X - 25;
        }
        private void gameEnded(bool isWin)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            if (isWin)
                MessageBox.Show("Yay! You saved the planet!");
            else
                MessageBox.Show("Oh, no! You have been invaded!");
            Close();
        }
        
    }
}
