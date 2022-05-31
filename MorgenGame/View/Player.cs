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
    class Player : IGameObject
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
        //Dictionary<char, Tuple<Bitmap, Bitmap>> sprites;
        //Dictionary<Bitmap, List<int>> imagesSize;

        public Player(int pX, int pY)
        {
            posX = pX;
            posY = pY;
            sizeX = 60;
            sizeY = 100;
            picture = Properties.Resources.playerSprite2;
            health = 500;

            //sprites = new Dictionary<char, Tuple<Bitmap, Bitmap>>();
            //CompleteSpriteDictionary();

            //imagesSize = new Dictionary<Bitmap, List<int>>();
            //CompleteAnimationDictionary();
        }

        public void Move()
        {
            posX += moveX;
            posY += moveY;
        }


        //private void CompleteSpriteDictionary()
        //{
        //    sprites.Add('W', Tuple.Create(Map.playerSprite3, Map.stayedSprite3));
        //    sprites.Add('S', Tuple.Create(Map.playerSprite1, Map.stayedSprite4));
        //    sprites.Add('A', Tuple.Create(Map.playerSprite4, Map.stayedSprite2));
        //    sprites.Add('D', Tuple.Create(Map.playerSprite2, Map.stayedSprite1));
        //}

        //private void CompleteAnimationDictionary()
        //{
        //    imagesSize.Add(Map.playerSprite1, new List<int> { 45, 0, 42, 63 });
        //    imagesSize.Add(Map.playerSprite2, new List<int> { 45, 0, 42, 63 });
        //    imagesSize.Add(Map.playerSprite3, new List<int> { 35, 0, 35, 63 });
        //    imagesSize.Add(Map.playerSprite4, new List<int> { 45, 0, 42, 63 });
        //    imagesSize.Add(Map.stayedSprite1, new List<int> { 45, 0, 42, 63 });
        //    imagesSize.Add(Map.stayedSprite2, new List<int> { 45, 0, 42, 63 });
        //    imagesSize.Add(Map.stayedSprite3, new List<int> { 45, 0, 42, 63 });
        //    imagesSize.Add(Map.stayedSprite4, new List<int> { 45, 0, 42, 63 });
        //}

        public void PlayAnimation(Graphics g, char button)
        {
            if (isMoving)
                anime++;
            if (anime > 7 && (button == 'D' || button == 'A'))
                anime = 1;
            else if (button == 'W' && anime > 5)
                anime = 0;
            else if(button == 'S' && anime > 4)
                anime = 0;

            //picture = isMoving ? sprites[button].Item1 : sprites[button].Item2;
            //var imageSize = imagesSize[(Bitmap)picture];
            //g.DrawImage(picture, new Rectangle(posX, posY, sizeX, sizeY),
                //imageSize[0] * anime, imageSize[1], imageSize[2], imageSize[3], GraphicsUnit.Pixel);
            switch (button)
            {
                case 'D':
                    picture = Properties.Resources.playerSprite2;
                    g.DrawImage(picture, new Rectangle(posX, posY, sizeX, sizeY), 45 * anime, 0, 42, 63, GraphicsUnit.Pixel);
                    break;
                case 'A':
                    picture = Properties.Resources.playerSprite4;
                    g.DrawImage(picture, new Rectangle(posX, posY, sizeX, sizeY), 45 * (anime + 8), 0, 42, 63, GraphicsUnit.Pixel);
                    break;
                case 'W':
                    picture = Properties.Resources.playerSprite3;
                    g.DrawImage(picture, new Rectangle(posX, posY, sizeX, sizeY), 35 * anime, 0, 35, 63, GraphicsUnit.Pixel);
                    break;
                case 'S':
                    picture = Properties.Resources.playerSprite1;
                    g.DrawImage(picture, new Rectangle(posX, posY, sizeX, sizeY), 45 * (anime % 2), 0, 42, 63, GraphicsUnit.Pixel);
                    break;
            }
        }

        //private void PlayStayedAnimation(Graphics g, char button)
        //{
        //    anime++;
        //    switch (button)
        //    {
        //        case 'D':
        //            picture = Map.stayedSprite1;
        //            g.DrawImage(picture, new Rectangle(posX, posY, sizeX - 20, sizeY - 20), 0, 0, 66, 115, GraphicsUnit.Pixel);
        //            break;
        //        case 'A':
        //            picture = Map.playerSprite4;
        //            g.DrawImage(picture, new Rectangle(posX, posY, sizeX, sizeY), 45 * (anime + 8), 0, 42, 63, GraphicsUnit.Pixel);
        //            break;
        //        case 'W':
        //            picture = Map.playerSprite3;
        //            g.DrawImage(picture, new Rectangle(posX, posY, sizeX, sizeY), 35 * anime, 0, 35, 63, GraphicsUnit.Pixel);
        //            break;
        //        case 'S':
        //            picture = Map.playerSprite1;
        //            g.DrawImage(picture, new Rectangle(posX, posY, sizeX, sizeY), 45 * (anime % 2), 0, 42, 63, GraphicsUnit.Pixel);
        //            break;
        //    }
        //}
    }
}
