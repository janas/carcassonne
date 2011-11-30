using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarcassonneSolver
{
    public class Tile
    {
        public int ID { get; set; }
        public TileClass tileClass;
        private int rotation;

        public Position Pos { get; set; }

        public Tile(TileClass tc, int id)
        {
            ID = id;
            tileClass = tc; 
            rotation = 0; 
            Pos = null;
        }

        public void Rotate()
        {
            rotation++;
            rotation = rotation % 4;
        }

        public void Rotate(int times)
        {
            for (int i = 0; i < times % 4; i++)
                Rotate();
        }

        public void RotateToPosition(int r)
        {
            rotation = r;
        }

        public int getLeft()
        {
            switch (rotation)
            {
                case 0: return tileClass.Left;
                case 1: return tileClass.Bottom;
                case 2: return tileClass.Right;
                case 3: return tileClass.Top;
                default: return 0;
            }
        }

        public int getRight()
        {
            switch (rotation)
            {
                case 0: return tileClass.Right;
                case 1: return tileClass.Top;
                case 2: return tileClass.Left;
                case 3: return tileClass.Bottom;
                default: return 0;
            }
        }

        public int getTop()
        {
            switch (rotation)
            {
                case 0: return tileClass.Top;
                case 1: return tileClass.Left;
                case 2: return tileClass.Bottom;
                case 3: return tileClass.Right;
                default: return 0;
            }
        }

        public int getBottom()
        {
            switch (rotation)
            {
                case 0: return tileClass.Bottom;
                case 1: return tileClass.Right;
                case 2: return tileClass.Top;
                case 3: return tileClass.Left;
                default: return 0;
            }
        }

        /*
        public String Print()
        {
            return "ID:"+ID.ToString()+
                " Left:"+Left.ToString()+
                " Right:"+Right.ToString()+
                " Top:"+Top.ToString()+
                " Bottom:"+Bottom.ToString()+
                " Rotation:"+rotation.ToString();
        }
        */
    }
}
