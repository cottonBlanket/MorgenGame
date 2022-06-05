using System.Drawing;

namespace MorgenGame
{
    /// <summary>
    /// интерфейс игрового объекта
    /// </summary>
    internal interface IGameObject
    {
        /// <summary>
        /// позиция по абсцисее
        /// </summary>
        int posX { get; set; }
        /// <summary>
        /// позиция по ординате
        /// </summary>
        int posY { get; set; }
        /// <summary>
        /// размеры по абсциссе
        /// </summary>
        int sizeX { get; set; }
        /// <summary>
        /// размеры по ординате
        /// </summary>
        int sizeY { get; set; }
        /// <summary>
        /// картинка объекта
        /// </summary>
        Image picture { get; set; }
    }
}
