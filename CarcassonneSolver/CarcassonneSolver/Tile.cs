using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarcassonneSolver
{
    class Tile
    {
        public int ID { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        private int rotation;

        public Position Pos { get; set; }

        public Tile(int l, int r, int t, int b, int id)
        {
            Left = l; Right = r; Top = t; Bottom = b; ID = id; rotation = 0; Pos = null;
        }

        public void Rotate()
        {
            int temp = Left;
            Left = Bottom;
            Bottom = Right;
            Right = Top;
            Top = temp;
            rotation += 1;
            rotation = rotation % 4;
        }

        public void Rotate(int rrr)
        {
            rrr = rrr % 4;
            for (int i = 0; i < rrr; i++)
                Rotate();
        }

        public String Print()
        {
            return "ID:"+ID.ToString()+
                " Left:"+Left.ToString()+
                " Right:"+Right.ToString()+
                " Top:"+Top.ToString()+
                " Bottom:"+Bottom.ToString()+
                " Rotation:"+rotation.ToString();
        }

        public int Match(int l, int r, int t, int b)
        {
            if (((l != 0 && l == Left) || l == 0) &&
                ((t != 0 && t == Top) || t == 0) &&
                ((r != 0 && r == Right) || r == 0) && 
                ((b != 0 && b == Bottom) || b == 0))
                return 0;
            if (((l != 0 && l == Bottom) || l == 0) &&
                ((t != 0 && t == Left) || t == 0) &&
                ((r != 0 && r == Top) || r == 0) &&
                ((b != 0 && b == Right) || b == 0))
                return 1;
            if (((l != 0 && l == Right) || l == 0) &&
                ((t != 0 && t == Bottom) || t == 0) &&
                ((r != 0 && r == Left) || r == 0) &&
                ((b != 0 && b == Top) || b == 0))
                return 2;
            if (((l != 0 && l == Top) || l == 0) &&
                ((t != 0 && t == Right) || t == 0) &&
                ((r != 0 && r == Bottom) || r == 0) &&
                ((b != 0 && b == Left) || b == 0))
                return 3;
            return -1;
        }
    }
}
