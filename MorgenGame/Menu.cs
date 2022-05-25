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
    public class Menu
    {
        public Rectangle location;
        public Color color;
        public List<Control> controls;

        public Menu()
        {
            controls = new List<Control>();
            color = Color.Brown;
        }
    }
}
