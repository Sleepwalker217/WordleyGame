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

namespace WordleyGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        List<Rectangle> recs = new List<Rectangle>();
        List<TextBlock> boxes = new List<TextBlock>();
        public MainWindow() // конструктор
        {
            InitializeComponent();

            for(int i=0; i<5; i++)
            {
                Rectangle curr_Rect = new Rectangle();
                curr_Rect.Fill = Brushes.Transparent;
                curr_Rect.Height = 50;
                curr_Rect.Width = 50;
                curr_Rect.RadiusX = 10;
                curr_Rect.RadiusY = 10;
                curr_Rect.Stroke = Brushes.LightGray;
                curr_Rect.StrokeThickness = 6;
                recs.Add(curr_Rect);
                Grid.SetColumn(recs[i], i);
                Grid.SetRow(recs[i], 1);

                btnGrid.Children.Add(recs[i]);


                TextBlock curr_Text = new TextBlock();
                curr_Text.Text = i.ToString();
                curr_Text.FontSize = 40;
                curr_Text.VerticalAlignment = VerticalAlignment.Center;
                curr_Text.HorizontalAlignment = HorizontalAlignment.Center;
                curr_Text.Margin = new Thickness(0, 0, 0, 5);
                curr_Text.FontWeight = FontWeights.Medium;
                boxes.Add(curr_Text);
                Grid.SetColumn(boxes[i], i);
                Grid.SetRow(boxes[i], 1);
                btnGrid.Children.Add(boxes[i]);
            }
        }
    }
}
