using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarcassonneSolver
{
    public class BigTile
    {
        public int ID;
        public TileClass tileClass;
        public Tile LeftTop;
        public Tile RightTop;
        public Tile RightBottom;
        public Tile LeftBottom;
        public Position Pos { get; set; }
        private int rotation;

        public BigTile(TileClass tc, Tile lt, Tile rt, Tile lb, Tile rb, int id)
        {
            ID = id; 
            tileClass = tc;
            LeftTop = lt;
            RightTop = rt;
            LeftBottom = lb;
            RightBottom = rb;
            rotation = 0;
            Pos = null;
        }

        public void Rotate()
        {
            rotation++;
            rotation = rotation % 4;
            LeftTop.Rotate();
            RightTop.Rotate();
            RightBottom.Rotate();
            LeftBottom.Rotate();
        }

        public void RotateToPosition(int r)
        {
            while (rotation != r)
            {
                Rotate();
                LeftTop.Rotate();
                RightTop.Rotate();
                RightBottom.Rotate();
                LeftBottom.Rotate();
            }
        }

        public Tile getLeftTop()
        {
            switch (rotation)
            {
                case 0: return LeftTop;
                case 1: return LeftBottom;
                case 2: return RightBottom;
                case 3: return RightTop;
                default: return null;
            }
        }

        public Tile getLeftBottom()
        {
            switch (rotation)
            {
                case 0: return LeftBottom;
                case 1: return RightBottom;
                case 2: return RightTop;
                case 3: return LeftTop;
                default: return null;
            }
        }

        public Tile getRightBottom()
        {
            switch (rotation)
            {
                case 0: return RightBottom;
                case 1: return RightTop;
                case 2: return LeftTop;
                case 3: return LeftBottom;
                default: return null;
            }
        }

        public Tile getRightTop()
        {
            switch (rotation)
            {
                case 0: return RightTop;
                case 1: return LeftTop;
                case 2: return LeftBottom;
                case 3: return RightBottom;
                default: return null;
            }
        }

        public List<Tile> getTiles()
        {
            List<Tile> result = new List<Tile>();
            result.Add(getLeftTop());
            result.Add(getLeftBottom());
            result.Add(getRightTop());
            result.Add(getRightBottom());
            return result;
        }

        public Tile[] getTileArray()
        {
            Tile[] result = new Tile[4];
            result[0] = getLeftTop();
            result[1] = getLeftBottom();
            result[2] = getRightTop();
            result[3] = getRightBottom();
            return result;
        }

        /*
        public List<int> Match(int l, int r, int t, int b)
        {
            List<int> result = new List<int>();
            if (((l != 0 && l == LeftTop.getLeft() && l == LeftBottom.getLeft()) || l == 0) &&
                ((t != 0 && t == LeftTop.getTop() && t == RightTop.getTop()) || t == 0) &&
                ((r != 0 && r == RightTop.getRight() && r == RightBottom.getRight()) || r == 0) &&
                ((b != 0 && b == LeftBottom.getBottom() && b == RightBottom.getBottom()) || b == 0))
                result.Add(0);
            if (((l != 0 && l == LeftBottom.getBottom() && l == RightBottom.getBottom()) || l == 0) &&
                ((t != 0 && t == LeftTop.getLeft() && t == LeftBottom.getLeft()) || t == 0) &&
                ((r != 0 && r == LeftTop.getTop() && r == RightTop.getTop()) || r == 0) &&
                ((b != 0 && b == RightTop.getRight() && b == RightBottom.getRight()) || b == 0))
                result.Add(1);
            if (((l != 0 && l == RightTop.getRight() && l == RightBottom.getRight()) || l == 0) &&
                ((t != 0 && t == LeftBottom.getBottom() && t == RightBottom.getBottom()) || t == 0) &&
                ((r != 0 && r == LeftTop.getLeft() && r == LeftBottom.getLeft()) || r == 0) &&
                ((b != 0 && b == LeftTop.getTop() && b == RightTop.getTop()) || b == 0))
                result.Add(2);
            if (((l != 0 && l == LeftTop.getTop() && l == RightTop.getTop()) || l == 0) &&
                ((t != 0 && t == RightTop.getRight() && t == RightBottom.getRight()) || t == 0) &&
                ((r != 0 && r == LeftBottom.getBottom() && r == RightBottom.getBottom()) || r == 0) &&
                ((b != 0 && b == LeftTop.getLeft() && b == LeftBottom.getLeft()) || b == 0))
                result.Add(3);
            return result;
        }
        */
    }
}
