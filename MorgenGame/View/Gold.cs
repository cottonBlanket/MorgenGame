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

        private int frameCount;//количество тиков

        /// <summary>
        /// конструктор класса
        /// для инициализации игрока с заданными значениями координат
        /// </summary>
        /// <param name="pX">начальная позиция по абсциссе</param>
        /// <param name="pY">начальная позиция по ординате</param>
        public Gold(int pX, int pY)
        {
            posX = pX;
            posY = pY;
            sizeX = 20;
            sizeY = 20;
            picture = Properties.Resources.gold;
        }

        /// <summary>
        /// отрисовывает анимацию золота
        /// </summary>
        /// <param name="g">объект рисования</param>
        public void PlayAnimation(Graphics g)
        {
            frameCount++;
            if(frameCount > 6)
                frameCount = 0;
            g.DrawImage(picture, new Rectangle(posX, posY,sizeX, sizeY), 90 * frameCount, 0, 90, 90, GraphicsUnit.Pixel);
        }
    }
}
