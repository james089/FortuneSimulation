using FortuneSimulation.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FortuneSimulation
{
    class GF
    {
        public enum Axis
        {
            x,
            y
        }
        public static double GetDistanceFromCenter(int x, int y, Point center)
        {
            return Math.Sqrt(Math.Pow(x - center.X, 2) + Math.Pow(y - center.Y, 2));
        }

        public static int CheckBoundary(int val, Axis axis)
        {
            if (axis == Axis.x)
            {
                if (val < 0) return 0;
                else if (val > MapManager.MAP_SIZE.width) return MapManager.MAP_SIZE.width;
                else return val;
            }
            else
            {
                if (val < 0) return 0;
                else if (val > MapManager.MAP_SIZE.height) return MapManager.MAP_SIZE.height;
                else return val;
            }
        }
    }
}
