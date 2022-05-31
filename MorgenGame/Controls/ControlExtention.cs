using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MorgenGame
{
    public static class ControlExtention
    {
        public static Label InitLabel(
            this Label label,
            int posX,
            int posY,
            Font font,
            Color backColor,
            Color foreColor,
            string text = null)
        {
            label = new Label()
            {
                Location = new Point(posX, posY),
                Font = font,
                BackColor = backColor,
                ForeColor = foreColor,
                Text = text,
                AutoSize = true
            };
            return label;
        }

        public static Button InitButton(
            this Button button,
            int posX,
            int posY,
            Font font,
            Color backColor,
            Color foreColor,
            string text,
            Action<object, EventArgs> buttonEvent)
        {
            button = new Button()
            {
                Location = new Point(posX, posY),
                Font = font,
                BackColor = backColor,
                ForeColor = foreColor,
                Text = text,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true
            };
            button.Click += new EventHandler(buttonEvent);
            return button;
        }
    }
}
