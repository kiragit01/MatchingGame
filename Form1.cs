using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // firstClicked указывает на первый элемент управления Label 
        // на котором щелкнул игрок, но это значение будет равно null 
        // если игрок еще не нажимал на ярлык
        Label firstClicked = null;

        // secondClicked указывает на второй элемент управления Label 
        // на который нажимает игрок
        Label secondClicked = null;


        //Используйте этот объект Random для выбора случайных значков для квадратов
        Random random = new Random();

        // Каждая из этих букв представляет собой интересный значок
        // в шрифте Webdings,
        // и каждый значок встречается в этом списке дважды
        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "$", "$",
            "b", "b", "v", "v", "w", "w", "l", "l"
        };

        /// <summary>
        /// Присвойте каждой иконке из списка иконок случайный квадрат
        /// </summary>
        private void AssignIconsToSquares()
        {
            // Панель TableLayoutPanel имеет 16 ярлыков,
            // а в списке иконок 16 иконок,
            // поэтому из списка случайным образом выбирается иконка
            // и добавляется к каждой метке
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor; // делает цвет значков таким же, как и цвет фона
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            AssignIconsToSquares();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Событие Click каждой метки обрабатывается этим обработчиком события
        /// </summary>
        /// <param name="sender">Этикетка, по которой был произведен щелчок</param>
        /// <param name="e"></param>
        private void label1_Click_1(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // Если метка, на которую вы нажимаете, черная, то игрок нажал на
                // иконку, которая уже была раскрыта...
                // игнорируйте щелчок
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                // Если firstClicked равен null, то это первая иконка 
                // в паре, на которую нажал игрок,
                // поэтому установите firstClicked на ярлык, на котором игрок 
                // щелкнул, измените его цвет на черный и верните
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                // Если игрок зашел так далеко, таймер не
                // запущен, а firstClicked не равно null,
                // так что это должна быть вторая иконка, на которую нажал игрок
                // Установите его цвет на черный
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                // Проверяем, выиграл ли игрок
                CheckForWinner();

                // Если игрок нажал на две одинаковые иконки, сохраните их 
                // черными и сбросьте значения firstClicked и secondClicked 
                // чтобы игрок мог нажать на другую иконку
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // Если игрок зашел так далеко, то он 
                // щелкнул по двум разным иконкам, поэтому запустите 
                // таймер (который подождет 0.25 секунды, а затем скроет иконки)
                timer1.Start();
            }
        }

        /// <summary>
        /// Этот таймер запускается, когда игрок нажимает две несовпадающие иконки, 
        /// поэтому он отсчитывает 0.25 секунды, а затем выключается и скрывает обе иконки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Остановка таймера
            timer1.Stop();

            // Скрыть оба значка
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Сбросьте значения firstClicked и secondClicked 
            // чтобы в следующий раз, когда ярлык будет
            // щелчок, программа знает, что это первый щелчок
            firstClicked = null;
            secondClicked = null;
        }

        /// <summary>
        /// Проверяет каждый значок на соответствие, 
        /// сравнивая его цвет переднего плана с цветом фона. 
        /// Если все значки совпадают, игрок выигрывает.
        /// </summary>
        private void CheckForWinner()
        {
            // Пройдитесь по всем ярлыкам на панели TableLayoutPanel, 
            // проверяя каждую из них на соответствие ее значку
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            // Если цикл не вернулся, значит, он не нашел
            // ни одной несопоставимой иконки.
            // Это означает, что пользователь победил. Покажите сообщение и закройте форму
            MessageBox.Show("Вы подобрали все значки!", "Поздравляем"); 
            Close();
        }


    }
}
