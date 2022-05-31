using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MorgenGame
{
    class Enemy : IGameObject
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public int sizeX { get; set; }
        public int sizeY { get; set; }
        public Image picture { get; set; }

        public int moveX;
        public int moveY;

        public List<Point> movements;
        public int velocity;

        private int frameCount;
        private int animeX = 1;
        private int animeY = 0; 

        public Enemy(int pX, int pY)
        {
            picture = Properties.Resources.enemySprites;
            posX = pX;
            posY = pY;
            sizeX = 100;
            sizeY = 100;
            velocity = 1;

            frameCount = (pX + pY) % 7;
            CompleteDictionary();
        }

        public void CompleteDictionary()
        {
            movements = new List<Point>();
            movements.Add(new Point(0, -velocity));
            movements.Add(new Point(0, velocity));
            movements.Add(new Point(-velocity, 0));
            movements.Add(new Point(velocity, 0));
        }

        public void PlayAnimation(Graphics g)
        {
            frameCount++;
            GetSpritePosition();
            g.DrawImage(picture, new Rectangle(posX, posY,sizeX, sizeY), 144 * animeX, animeY, 140, 218, GraphicsUnit.Pixel);
        }

        private void GetSpritePosition()
        {
            animeX = frameCount < 6 ? frameCount % 3 : 2 - frameCount % 3;
            animeY = frameCount >= 3 && frameCount < 9 ? 218 : 0;
            if (frameCount > 11)
                frameCount = 0;
        }
    }
}
