using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WordleyGame
{
    public partial class MainWindow : Window
    {

        List<List<Rectangle>> full_recs = new List<List<Rectangle>>(); // двумерный массив прямоугольников
        List<List<TextBlock>> full_boxes = new List<List<TextBlock>>(); // двумерный массив символов внутри прямоугольников
        
               
        private const int MAX_X = 5;
        private const int MAX_Y = 6;

        // Глобальные переменные
        byte indexX = 0;
        byte indexY = 0;
        char[] chars = {' ', ' ', ' ', ' ', ' '};
        string word;

        public MainWindow() // конструктор
        {
            InitializeComponent();
            MainGrid.Focus();
            this.Topmost = true;
            // создание элементов
            for (int i = 0; i < MAX_Y; i++)
            {
                List<Rectangle> recs = new List<Rectangle>();
                List<TextBlock> boxes = new List<TextBlock>();
                for (int j = 0; j < MAX_X; j++)
                {
                    Rectangle curr_Rect = new Rectangle
                    {
                        Fill = Brushes.Transparent,
                        Height = 60,
                        Width = 60,
                        RadiusX = 15,
                        RadiusY = 15,
                        Stroke = Brushes.LightGray,
                        StrokeThickness = 6
                    };
                    recs.Add(curr_Rect);
                    Grid.SetColumn(recs[j], j);
                    Grid.SetRow(recs[j], i + 1);

                    btnGrid.Children.Add(recs[j]);


                    TextBlock curr_Text = new TextBlock
                    {
                        FontSize = 40,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 0, 0, 5),
                        FontWeight = FontWeights.Medium
                    };
                    boxes.Add(curr_Text);
                    Grid.SetColumn(boxes[j], j);
                    Grid.SetRow(boxes[j], i + 1);
                    btnGrid.Children.Add(boxes[j]);
                }
                full_recs.Add(recs);
                full_boxes.Add(boxes);
            }
            Rand_word();
            full_recs[0][0].Stroke = Brushes.Gray;
        }

        // Длина слова (для дальнейшего расширения до более 6 букв в слове)
        int Words_length()
        {
            StreamReader reader = new StreamReader(@"Words.txt");
            int res = 0;
            while (reader.ReadLine() != null) res++;
            return res;
        }

        // Отлов нажатия клавиш
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                //Перемещение влево
                case Key.Left:
                    if (indexX != 0) indexX--;
                    full_recs[indexY][indexX].Stroke = Brushes.Gray;
                    full_recs[indexY][indexX+1].Stroke = Brushes.LightGray;
                    break;

                //Перемещение вправо
                case Key.Right:
                    if (indexX != MAX_X-1) indexX++;
                    full_recs[indexY][indexX].Stroke = Brushes.Gray;
                    full_recs[indexY][indexX-1].Stroke = Brushes.LightGray;
                    break;
                
                //Применить (enter)
                case Key.Enter:
                    char[] tmp_word = word.ToCharArray();
                    if (!chars.Contains(' '))
                    {

                        if (new string(chars) == word)
                        {
                            string fin = string.Format("The word was {0}", word);
                            Msg.Visibility = Visibility.Visible;
                            MsgText.Text = "You Win!";
                            MsgWord.Text = fin;
                        }
                        for (int i = 0; i < MAX_X; i++)
                        {
                            if (chars[i] == tmp_word[i])
                                full_recs[indexY][i].Fill = Brushes.LightGreen;
                            else if (word.Contains(chars[i]))
                                full_recs[indexY][i].Fill = Brushes.Yellow;
                            chars[i] = ' ';
                        }
                            
                        
                        full_recs[indexY][indexX].Stroke = Brushes.LightGray;

                        if (indexY < MAX_Y-1)
                        {
                            indexY++;
                            indexX = 0;
                            full_recs[indexY][indexX].Stroke = Brushes.Gray;
                        }
                        else 
                        {
                            if (new string(chars) != word)
                            {
                                string fin = string.Format("The word was {0}", word);
                                Msg.Visibility = Visibility.Visible;
                                MsgText.Text = "You Lose!";
                                MsgWord.Text = fin;
                            }
                        
                        }
                    }
                    break;

                //Остальные клваиши
                default:
                    string temp = e.Key.ToString();
                    //Проверка является ли нажатая клавиша буквой
                    if (temp.Length == 1)
                    {
                        full_boxes[indexY][indexX].Text = temp;
                        chars[indexX] = temp.ToCharArray()[0];

                        if (indexX != MAX_X - 1) indexX++;
                        full_recs[indexY][indexX].Stroke = Brushes.Gray;
                        full_recs[indexY][indexX - 1].Stroke = Brushes.LightGray;
                    }
                    break;
            }
        }
        // Перезапуск игры
        private void Restart()
        {
            indexX = 0;
            indexY = 0;
            for (int i = 0; i < MAX_Y; i++)
            {
                for (int j = 0; j < MAX_X; j++)
                {
                    full_boxes[i][j].Text = " ";
                    full_recs[i][j].Fill = Brushes.Transparent;
                    full_recs[i][j].Stroke = Brushes.LightGray;
                    chars[j] = ' ';
                    full_recs[indexY][indexX].Stroke = Brushes.Gray;
                }
            }   
            Rand_word();
        }

        //Выбор рандомного слова
        private void Rand_word()
        {
            Random rnd = new Random();
            StreamReader words = new StreamReader(@"Words.txt");
            int choose = rnd.Next(0, Words_length() - 1);
            for (int i = 0; i < choose; i++) words.ReadLine();
            word = words.ReadLine();
        }

        //Перетягивание окна
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        //Нажатие кнопки выхода
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Нажатие кнопки свернуть
        private void minBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Нажатие кнопки New Game 
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Restart();
            Msg.Visibility = Visibility.Hidden;
            MainGrid.Focus();
        }

        //Нажатие кнопки Exit
        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
