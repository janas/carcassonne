using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarcassonneSolver
{
    class Tile
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        private int rotation;

        public Tile(int l, int r, int t, int b)
        {
            Left = l; Right = r; Top = t; Bottom = b; rotation = 0;
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

        public String Print()
        {
            return "Left:"+Left.ToString()+
                " Right:"+Right.ToString()+
                " Top:"+Top.ToString()+
                " Bottom:"+Bottom.ToString()+
                " Rotation:"+rotation.ToString();
        }

    }
}
