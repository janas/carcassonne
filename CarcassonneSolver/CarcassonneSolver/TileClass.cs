using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarcassonneSolver
{
    public class TileClass
    {
        public int ClassID { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int avaCount { get; set; }
        
        public List<int> Match(int l, int r, int t, int b)
        {
            List<int> result = new List<int>();
            if (((l != 0 && l == Left) || l == 0) &&
                ((t != 0 && t == Top) || t == 0) &&
                ((r != 0 && r == Right) || r == 0) &&
                ((b != 0 && b == Bottom) || b == 0))
                result.Add(0);
            if (((l != 0 && l == Bottom) || l == 0) &&
                ((t != 0 && t == Left) || t == 0) &&
                ((r != 0 && r == Top) || r == 0) &&
                ((b != 0 && b == Right) || b == 0))
                result.Add(1);
            if (((l != 0 && l == Right) || l == 0) &&
                ((t != 0 && t == Bottom) || t == 0) &&
                ((r != 0 && r == Left) || r == 0) &&
                ((b != 0 && b == Top) || b == 0))
                result.Add(2);
            if (((l != 0 && l == Top) || l == 0) &&
                ((t != 0 && t == Right) || t == 0) &&
                ((r != 0 && r == Bottom) || r == 0) &&
                ((b != 0 && b == Left) || b == 0))
                result.Add(3);
            return result;
        }

        public TileClass(int l, int r, int t, int b, int classID)
        {
            Left = l; Right = r; Top = t; Bottom = b; ClassID = classID; avaCount = 0;
        }

        public int CitiesNumber()
        {
            int result = 0;
            if (Left == (int)TT.city)
                result++;
            if (Right == (int)TT.city)
                result++;
            if (Top == (int)TT.city)
                result++;
            if (Bottom == (int)TT.city)
                result++;
            return result;
        }

        public int FieldsNumber()
        {
            int result = 0;
            if (Left == (int)TT.field)
                result++;
            if (Right == (int)TT.field)
                result++;
            if (Top == (int)TT.field)
                result++;
            if (Bottom == (int)TT.field)
                result++;
            return result;
        }

        public int RoadsNumber()
        {
            int result = 0;
            if (Left == (int)TT.road)
                result++;
            if (Right == (int)TT.road)
                result++;
            if (Top == (int)TT.road)
                result++;
            if (Bottom == (int)TT.road)
                result++;
            return result;
        }

        public bool Equals(TileClass tc)
        {
            if (this.Left != tc.Left)
                return false;
            if (this.Right != tc.Right)
                return false;
            if (this.Top != tc.Top)
                return false;
            if (this.Bottom != tc.Bottom)
                return false;
            return true;
        }

        public bool Equals(int l, int r, int t, int b)
        {
            if (this.Left != l)
                return false;
            if (this.Right != r)
                return false;
            if (this.Top != t)
                return false;
            if (this.Bottom != b)
                return false;
            return true;
        }
    }
}
