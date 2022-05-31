using System.Drawing;

namespace MorgenGame
{
    internal interface IGameObject
    {
        int posX { get; set; }
        int posY { get; set; }

        int sizeX { get; set; }
        int sizeY { get; set; }

        Image picture { get; set; }
    }
}
