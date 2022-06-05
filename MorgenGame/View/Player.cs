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
    /// <summary>
    /// класс, отвечающий за игрока
    /// </summary>
    class Player : IGameObject
    {
        /// <summary>
        /// позиция по абсцисее
        /// </summary>
        public int posX { get; set; }
        /// <summary>
        /// позиция по ординате
        /// </summary>
        public int posY { get; set; }
        /// <summary>
        /// размеры по абсциссе
        /// </summary>
        public int sizeX { get; set; }
        /// <summary>
        /// размеры по ординате
        /// </summary>
        public int sizeY { get; set; }
        /// <summary>
        /// картинка объекта
        /// </summary>
        public Image picture { get; set; }

        public int moveX;//изменение положения по абсциссе
        public int moveY;//изменение положения по ординате
        public bool isMoving;//двигается игрок или нет

        public int health;//здоровье игрока

        public int anime;//переменная для анимации
        public int frameCount = 0;//количество тиков
        //Dictionary<char, Tuple<Bitmap, Bitmap>> sprites;
        //Dictionary<Bitmap, List<int>> imagesSize;

        /// <summary>
        /// конструктор класса
        /// для инициализации игрока с заданными значениями координат
        /// </summary>
        /// <param name="pX">начальная позиция по абсциссе</param>
        /// <param name="pY">начальная позиция по ординате</param>
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

        /// <summary>
        /// изменяет положение игрока
        /// </summary>
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

        /// <summary>
        /// отрисовывает анимацию движения игрока
        /// </summary>
        /// <param name="g">объект рисования</param>
        /// <param name="button">последняя нажатая клавиша</param>
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
