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

        public List<Point> movements;//список возможных случайных перемещений
        public int velocity;//скорость врагов

        private int frameCount;//количество тиков
        private int animeX = 1;//переменная для сдвига отрисовывания спрайта по абсциссе
        private int animeY = 0; //переменная для сдвига отрисовывания спрайта по ординате

        /// <summary>
        /// конструктор класса
        /// для инициализации игрока с заданными значениями координат
        /// </summary>
        /// <param name="pX">начальная позиция по абсциссе</param>
        /// <param name="pY">начальная позиция по ординате</param>
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

        /// <summary>
        /// заполняет список случайных перемещений
        /// </summary>
        public void CompleteDictionary()
        {
            movements = new List<Point>();
            movements.Add(new Point(0, -velocity));
            movements.Add(new Point(0, velocity));
            movements.Add(new Point(-velocity, 0));
            movements.Add(new Point(velocity, 0));
        }

        /// <summary>
        /// отрисовывает анимацию движения врагов
        /// </summary>
        /// <param name="g">объект рисования</param>
        public void PlayAnimation(Graphics g)
        {
            frameCount++;
            GetSpritePosition();
            g.DrawImage(picture, new Rectangle(posX, posY,sizeX, sizeY), 144 * animeX, animeY, 140, 218, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// определяет движение отрисовщика по картинке спрайтов
        /// </summary>
        private void GetSpritePosition()
        {
            animeX = frameCount < 6 ? frameCount % 3 : 2 - frameCount % 3;
            animeY = frameCount >= 3 && frameCount < 9 ? 218 : 0;
            if (frameCount > 11)
                frameCount = 0;
        }
    }
}
