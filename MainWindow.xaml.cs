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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<List<Rectangle>> full_recs = new List<List<Rectangle>>(); // двумерный массив прямоугольников
        List<List<TextBlock>> full_boxes = new List<List<TextBlock>>(); // двумерный массив символов внутри прямоугольников
        
        
        private const int MAX_X = 5;
        private const int MAX_Y = 6;

        byte indexX = 0;
        byte indexY = 0;
        char[] chars = {' ', ' ', ' ', ' ', ' '};
        string word;

        public MainWindow() // конструктор
        {
            InitializeComponent();
            MainGrid.Focus();
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

            /*
            // создание кнопки с галочкой
            Button tick = new Button
            {
                BorderThickness = new Thickness(0),
                Background = new ImageBrush(new BitmapImage(new Uri("tick.jpg", UriKind.Relative))),
                Width = 50,
                Height = 50,
                Focusable = false
            };
            Grid.SetColumn(tick, 5);
            Grid.SetRow(tick, 1);
            btnGrid.Children.Add(tick);
            */

            Rand_word();

            full_recs[0][0].Stroke = Brushes.Gray;


        }

        int Words_length()
        {
            StreamReader reader = new StreamReader(@"Words.txt");
            int res = 0;
            while (reader.ReadLine() != null) res++;
            return res;
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (indexX != 0) indexX--;
                    full_recs[indexY][indexX].Stroke = Brushes.Gray;
                    full_recs[indexY][indexX+1].Stroke = Brushes.LightGray;
                    break;
               
                case Key.Right:
                    if (indexX != MAX_X-1) indexX++;
                    full_recs[indexY][indexX].Stroke = Brushes.Gray;
                    full_recs[indexY][indexX-1].Stroke = Brushes.LightGray;
                    break;
                
                case Key.Enter:
                    char[] tmp_word = word.ToCharArray();
                    if (!chars.Contains(' '))
                    {
                        if (new string(chars) == word)
                        {
                            MessageBox.Show("Congratulations!\nPress OK or ENTER to start a New Game");
                            Restart();
                            break;
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
                            string you_lost = string.Format("You lost\nThe word was {0}\nPress OK or ENTER to start a New Game",word); 
                            MessageBox.Show(you_lost);
                            Restart();
                        }
                        

                    }
                    break;
                
                default:
                    string temp = e.Key.ToString();
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

        private void Rand_word()
        {
            Random rnd = new Random();
            StreamReader words = new StreamReader(@"Words.txt");
            int choose = rnd.Next(0, Words_length() - 1);
            for (int i = 0; i < choose; i++) words.ReadLine();
            word = words.ReadLine();
        }
    }
}
