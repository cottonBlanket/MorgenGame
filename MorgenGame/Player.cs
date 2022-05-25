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
    public class Player : IGameObject
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public int sizeX { get; set; }
        public int sizeY { get; set; }
        public Image picture { get; set; }

        public int moveX;
        public int moveY;
        public bool isMoving;

        public int health;

        public int anime;
        public int frameCount = 0;

        public Player()
        {
            posX = 20;
            posY = 340;
            sizeX = 100;
            sizeY = 150;
            picture = Map.playerSprite2;
            health = 1000;
        }

        public void Move()
        {
            posX += moveX;
            posY += moveY;
        }

        public void PlayAnimation(Graphics g, char button)
        {
            anime++;
            if (anime > 7 && (button == 'D' || button == 'A'))
                anime = 1;
            else if (button == 'W' && anime > 6)
                anime = 0;
            else if(button == 'S' && anime > 2)
                anime = 0;
            switch(button)
            {
                case 'D':
                    picture = Map.playerSprite2;
                    g.DrawImage(picture, new Rectangle(new Point(posX, posY),
                        new Size(sizeX, sizeY)), 45 * anime, 0, 42, 63, GraphicsUnit.Pixel);
                    break;
                case 'A':
                    picture = Map.playerSprite4;
                    g.DrawImage(picture, new Rectangle(new Point(posX, posY),
                        new Size(sizeX, sizeY)), 45 * (anime + 8), 0, 42, 63, GraphicsUnit.Pixel);
                    break;
                case 'W':
                    picture = Map.playerSprite3;
                    g.DrawImage(picture, new Rectangle(new Point(posX, posY),
                        new Size(sizeX, sizeY)), 40 * anime, 0, 35, 63, GraphicsUnit.Pixel);
                    break;
                case 'S':
                    picture = Map.playerSprite1;
                    g.DrawImage(picture, new Rectangle(new Point(posX, posY),
                        new Size(sizeX, sizeY)), 45 * anime, 0, 42, 63, GraphicsUnit.Pixel);
                    break;

            }
        }
    }
}
