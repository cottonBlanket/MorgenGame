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
    public class Wall
    {
        public Rectangle position;
        public Image picture;

        public Wall(Image pic, Rectangle pos)
        {
            position = pos;
            picture = pic;
        }
    }
}
