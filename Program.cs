using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace Minesweeper3
{
    internal class Program
    {
        private static Board _board;
        private static Grid _grid;
        [STAThread]
        public static void Main(string[] args)
        {
            int rows = 9;
            int cols = 9;
            int mines = 10;
            _board = new Board(rows, cols, mines);
            var app = new Application();
            var window = new Window();
            _grid = _board.CreateGrid();
            CreateButtons();

            window.Content = _grid;
            app.Run(window);
        }

        private static void CreateButtons()
        {
            for (int i = 0; i < _board.Height; i++)
            {
                for (int j = 0; j < _board.Width; j++)
                {
                    var label = new Button
                    {
                        FontSize = 20,
                        Tag = new Point(i, j),
                        IsEnabled = true,
                    };
                    label.Click += Button_Click;
                    label.MouseRightButtonDown += Button_RightClick;
                    _grid.Children.Add(label);
                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                }
            }
        }

        private static void UpdateView()
        {
            for (int i = 0; i < _board.Height; i++)
            {
                for (int j = 0; j < _board.Width; j++)
                {
                    var content = _board.Cells[i, j].IsOpen ? Content(i, j) : "";
                    if (_board.Cells[i, j].HasFlag)
                    {
                        content = "\ud83d\udea9";
                    }
                    var color = content switch
                    {
                        "1" => System.Windows.Media.Color.FromRgb(50, 50, 220),
                        "2" => System.Windows.Media.Color.FromRgb(40, 150, 70),
                        "3" => System.Windows.Media.Color.FromRgb(220, 50, 50),
                        "4" => System.Windows.Media.Color.FromRgb(10, 10, 150),
                        "5" => System.Windows.Media.Color.FromRgb(140, 80, 5),
                        "6" => System.Windows.Media.Color.FromRgb(10, 200, 220),
                        "8" => System.Windows.Media.Color.FromRgb(100, 100, 100),
                        _ => System.Windows.Media.Color.FromRgb(0, 0, 0),
                    };

                    var label = (Button)_grid.Children[i * _board.Width + j];
                    label.Content = content;
                    label.Foreground = new SolidColorBrush(color);
                    label.IsEnabled = !_board.Cells[i, j].IsOpen;
                }
            }
        }
        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Point point = (Point)button.Tag;
            int row = (int)point.X;
            int col = (int)point.Y;
            if (_board.Cells[row, col].IsMine)
            {
                button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(220, 50, 50));
                _board.OpenAllMines();
                MessageBox.Show("Spelet er over. Du traff ei mine.");
                UpdateView();
            }
            else
            {
                _board.OpenCells( row,col);
                if (_board.HasWon())
                {
                    MessageBox.Show("Gratulerer. Du vann!");
                }
                UpdateView();
            }
        }

        private static void Button_RightClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Point point = (Point)button.Tag;
            int row = (int)point.X;
            int col = (int)point.Y;
            if (_board.Cells[row, col].HasFlag)
            {
                _board.Cells[row, col].HasFlag = false;
            }
            else
            {
                _board.Cells[row, col].HasFlag = true;
            }
            UpdateView();
        }
        public static string Content(int row, int column)
        {
            if (_board.Cells[row, column].IsMine) return "\ud83d\udca3";
            else if (_board.Cells[row, column].MinesNearby == 0) return "";
            else return _board.Cells[row, column].MinesNearby.ToString();
        }
    }
}
