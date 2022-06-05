using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MorgenGame
{
    class Shop : IGameObject
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

        /// <summary>
        /// конструктор класса
        /// для инициализации игрока с заданными значениями координат
        /// </summary>
        /// <param name="pX">начальная позиция по абсциссе</param>
        /// <param name="pY">начальная позиция по ординате</param>
        public Shop(int pX, int pY)
        {
            posX = pX;
            posY = pY;
            sizeX = 20;
            sizeY = 50;
        }
    }
}
