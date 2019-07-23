using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FortuneSimulation.Managers
{
    public class MapManager
    {
        public const int GRID_W = 16;
        public const int GRID_H = 16;
        private static Size GRID_SIZE = new Size(800, 800);   // square

        public class SizeInt
        {
            public int width;
            public int height;
            public SizeInt(int w, int h)
            {
                width = w;
                height = h;
            }
        }
        /// <summary>
        /// Grid I generated, 50x50
        /// </summary>
        public static SizeInt MAP_SIZE = new SizeInt((int)GRID_SIZE.Width / GRID_W, (int)GRID_SIZE.Height / GRID_H);   // square
        //public static Border[,] Money_stack_UI_arr = new Border[MAP_SIZE.width, MAP_SIZE.height];

        /// <summary>
        /// Stores how many stacks are in 1 location
        /// </summary>
        public static int[,] Money_stack_data_arr = new int[MAP_SIZE.width, MAP_SIZE.height];
        /// <summary>
        /// Stores how much money are in 1 location
        /// </summary>
        public static int[,] Money_abs_data_arr = new int[MAP_SIZE.width, MAP_SIZE.height];
        public const int MONEY_PER_STACK = 1000;             // every piece of red dot equals 10,000 dollars.
        
        public static void GenerateBank(Grid grid, int layers, Point center)
        {
            int currentLayer = layers;
            int currentDistance = 0;

            while (currentLayer > 0)
            {
                currentDistance++;
                for (int i = 0; i < MAP_SIZE.width; i++)
                {
                    for (int j = 0; j < MAP_SIZE.height; j++)
                    {
                        if (GF.GetDistanceFromCenter(i, j, center) < currentDistance)
                        {
                            AddStackToUI(grid, i, j);
                            Money_stack_data_arr[i, j] += 1;
                            Money_abs_data_arr[i, j] = Money_stack_data_arr[i, j] * MONEY_PER_STACK;
                        }
                    }
                }
                currentLayer--;
            }
        }

        public static void UpdateBank(Grid grid, int[,] data)
        {
            for (int i = 0; i < MAP_SIZE.width; i++)
            {
                for (int j = 0; j < MAP_SIZE.height; j++)
                {
                    int m = data[i, j];
                    while (m > 0)
                    {
                        AddStackToUI(grid, i, j);
                        m--;
                    }
                }
            }
        }

        private static void AddStackToUI(Grid grid, int i, int j)
        {
            Border money_stack = new Border();
            money_stack.Width = GRID_W;
            money_stack.Height = GRID_H;
            money_stack.BorderThickness = new Thickness(0.5);
            money_stack.Background = new SolidColorBrush(Color.FromArgb(30, 255, 0, 0));

            grid.Children.Add(money_stack);
            Grid.SetColumn(money_stack, i);
            Grid.SetRow(money_stack, j);
        }

    }
}
