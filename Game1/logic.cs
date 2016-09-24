using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Game1
{
    public class logic : Game
    {
        int[,] field = new int[40, 40];//условимся, что 1 - голова, 2 - тело, 3 - еда, -1 - препятствия
        int x, y, right, left, down, up;
        Random rand = new Random();
        public logic()
        {
            right = 0;
            left = 0;
            down = 0;
            up = 1;
            x = rand.Next(40);
            y = rand.Next(40);
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    field[i, j] = 0;
                }
            }
            field[x, y] = 1;
        }
        public void move(int a)//1-вверх, 2-вниз, 3-влево, 4-вправо
        {
            if (a == 1)
            {
                y--;
            }
            if (a == 2)
            {
                y++;
            }
            if (a == 3)
            {
                x--;
            }
            if (a == 4)
            {
                x++;
            }
        }
    }
}
