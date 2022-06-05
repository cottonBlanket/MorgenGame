using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MorgenGame
{
    /// <summary>
    /// класс расширение для класса Control
    /// </summary>
    public static class ControlExtention
    {
        /// <summary>
        /// инициализирует Label
        /// </summary>
        /// <param name="label">инициализируемый лэйбл</param>
        /// <param name="posX">позиция по абсциссе</param>
        /// <param name="posY">позиция по ординате</param>
        /// <param name="font">щрифт текста</param>
        /// <param name="backColor">задний фон</param>
        /// <param name="foreColor">цвет текста</param>
        /// <param name="text">текст</param>
        /// <returns>инициализированный лэйбл с заданными параметрами</returns>
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

        /// <summary>
        /// инициализирует кнопку
        /// </summary>
        /// <param name="button">инициализируемая кнопка</param>
        /// <param name="posX">позиция по абсциссе</param>
        /// <param name="posY">позиция по ординате</param>
        /// <param name="font">щрифт текста</param>
        /// <param name="backColor">задний фон</param>
        /// <param name="foreColor">цвет текста</param>
        /// <param name="text">текст</param>
        /// <param name="buttonEvent">событие по нажатию кнопки</param>
        /// <returns>инициализированная кнопка с заданными параметрами</returns>
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
