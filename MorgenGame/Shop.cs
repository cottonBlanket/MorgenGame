using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MorgenGame
{
    class Shop : IGameObject
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public int sizeX { get; set; }
        public int sizeY { get; set; }
        public Image picture { get; set; }

        public Shop(int pX, int pY)
        {
            posX = pX;
            posY = pY;
            sizeX = 20;
            sizeY = 50;
        }
    }
}
