using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MorgenGame
{
    class Gold : IGameObject
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public int sizeX { get; set; }
        public int sizeY { get; set; }
        public Image picture { get; set; }

        private int frameCount;

        public Gold(int pX, int pY)
        {
            posX = pX;
            posY = pY;
            sizeX = 20;
            sizeY = 20;
            picture = Map.gold;
        }

        public void PlayAnimation(Graphics g)
        {
            frameCount++;
            if(frameCount > 6)
                frameCount = 0;
            g.DrawImage(picture, new Rectangle(posX, posY,sizeX, sizeY), 90 * frameCount, 0, 90, 90, GraphicsUnit.Pixel);
        }
    }
}
