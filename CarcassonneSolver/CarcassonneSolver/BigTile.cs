using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarcassonneSolver
{
    public class BigTile
    {
        public int ID;
        public Tile LeftTop;
        public Tile RightTop;
        public Tile RightBottom;
        public Tile LeftBottom;
        private int rotation;

        public void Rotate()
        {
            Tile temp = LeftTop;
            LeftTop = LeftBottom;
            LeftBottom = RightBottom;
            RightBottom = RightTop;
            RightTop = temp;
            LeftTop.Rotate();
            RightTop.Rotate();
            RightBottom.Rotate();
            LeftBottom.Rotate();
            rotation += 1;
            rotation = rotation % 4;
        }

        public List<int> Match(int l, int r, int t, int b)
        {
            List<int> result = new List<int>();
            if (((l != 0 && l == LeftTop.Left && l == LeftBottom.Left) || l == 0) &&
                ((t != 0 && t == LeftTop.Top && t == RightTop.Top) || t == 0) &&
                ((r != 0 && r == RightTop.Right && r == RightBottom.Right) || r == 0) &&
                ((b != 0 && b == LeftBottom.Bottom && b == RightBottom.Bottom) || b == 0))
                result.Add(0);
            if (((l != 0 && l == LeftBottom.Bottom && l == RightBottom.Bottom) || l == 0) &&
                ((t != 0 && t == LeftTop.Left && t == LeftBottom.Left) || t == 0) &&
                ((r != 0 && r == LeftTop.Top && r == RightTop.Top) || r == 0) &&
                ((b != 0 && b == RightTop.Right && b == RightBottom.Right) || b == 0))
                result.Add(1);
            if (((l != 0 && l == RightTop.Right && l == RightBottom.Right) || l == 0) &&
                ((t != 0 && t == LeftBottom.Bottom && t == RightBottom.Bottom) || t == 0) &&
                ((r != 0 && r == LeftTop.Left && r == LeftBottom.Left) || r == 0) &&
                ((b != 0 && b == LeftTop.Top && b == RightTop.Top) || b == 0))
                result.Add(2);
            if (((l != 0 && l == LeftTop.Top && l == RightTop.Top) || l == 0) &&
                ((t != 0 && t == RightTop.Right && t == RightBottom.Right) || t == 0) &&
                ((r != 0 && r == LeftBottom.Bottom && r == RightBottom.Bottom) || r == 0) &&
                ((b != 0 && b == LeftTop.Left && b == LeftBottom.Left) || b == 0))
                result.Add(3);
            return result;
        }
    }
}
