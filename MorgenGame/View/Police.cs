using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MorgenGame
{
    //недоработанный класс
    class Police : IGameObject
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public int sizeX { get; set; }
        public int sizeY { get; set; }
        public Image picture { get; set; }

        public int velocity;

        public Police(int pX, int pY, int pos)
        {
            posX = pX;
            posY = pY;
            sizeX = 100;
            sizeY = 80;
            picture = pos == 0 ? Properties.Resources.bobik1 : Properties.Resources.bobik2;
            velocity = pos == 0 ? -10 : 10;
        }

        public void Move()
        {
            posX += velocity;
        }
    }
}
