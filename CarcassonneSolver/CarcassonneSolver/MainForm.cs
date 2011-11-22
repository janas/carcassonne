using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CarcassonneSolver
{
    public partial class MainForm : Form
    {
        bool fileLoaded = false;
        enum TT { field = 1, city, road };
        List<Tile> availableTiles;
        List<Tile> fixedTiles;
        int bestPossibleScore = 0;
        int fixedMinX = Int32.MaxValue;
        int fixedMinY = Int32.MaxValue;
        Thread thread;

        public MainForm()
        {
            InitializeComponent();
        }

        private int ConvertXmlToInt(string input)
        {
            if (input.Equals("field"))
                return (int)TT.field;
            if (input.Equals("city"))
                return (int)TT.city;
            if (input.Equals("road"))
                return (int)TT.road;
            return 0;
        }

        private void loadXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            availableTiles = new List<Tile>();
            fixedTiles = new List<Tile>();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = Application.StartupPath;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                XDocument xmlDoc = XDocument.Load(ofd.FileName);
                try
                {
                    var xmlTiles = from t in xmlDoc.Descendants("tile")
                                   select new
                                   {
                                       L = t.Attribute("left").Value,
                                       R = t.Attribute("right").Value,
                                       T = t.Attribute("top").Value,
                                       B = t.Attribute("bottom").Value
                                   };
                    foreach (var t in xmlTiles)
                    {
                        availableTiles.Add(new Tile(ConvertXmlToInt(t.L),
                            ConvertXmlToInt(t.R), ConvertXmlToInt(t.T), ConvertXmlToInt(t.B), 
                            availableTiles.Count+1));
                    }
                }
                catch(Exception exc)
                {
                    fileLoaded = false;
                    MessageBox.Show("File invalid.\n"+exc.ToString());
                }
                if (availableTiles.Count > 0)
                {
                    fileLoaded = true;
                    tilesLoadedPictureBox.BackColor = Color.ForestGreen;
                    bestPossibleScore = (int)Math.Sqrt((double)availableTiles.Count);
                    drawAvailableTiles(availableTiles);
                    drawFixedTiles(availableTiles);
                }
                else
                {
                    fileLoaded = false;
                    tilesLoadedPictureBox.BackColor = Color.Red;
                    bestPossibleScore = 0;
                }
            }
        }
        
        private short getAlgorithm()
        {
            if (AccurateRadioButton.Checked)
                return 1;
            if (RosinskiRadioButton.Checked)
                return 2;
            if (JanaszekRadioButton.Checked)
                return 3;
            return 0;
        }

        private void verifyMins(Position pos)
        {
            if (pos.X < fixedMinX)
                fixedMinX = pos.X;
            if (pos.Y < fixedMinY)
                fixedMinY = pos.Y;
        }

        #region Algorithms
        private void accurateAlgorithm()
        {
            List<Tile> A1 = new List<Tile>(availableTiles);
            List<Tile> F1 = new List<Tile>(fixedTiles);
            for (int i = 0; i < A1.Count; i++)
            {
                List<Tile> A2 = new List<Tile>(A1);
                List<Tile> F2 = new List<Tile>(F1);
                Tile t2 = A2.ElementAt(i);
                A2.RemoveAt(i);
                t2.Pos = new Position(0, 0);
                F2.Add(t2);
                verifyMins(t2.Pos);
                drawTiles(A2, F2);
                Thread.Sleep((int)sleepUpDown.Value);
                for (int j = 0; j < A2.Count; j++)
                {
                    List<Tile> A3 = new List<Tile>(A2);
                    List<Tile> F3 = new List<Tile>(F2);
                    Tile t3 = A3.ElementAt(j);
                    A3.RemoveAt(j);
                    t3.Pos = new Position(1, 0);
                    F3.Add(t3);
                    verifyMins(t3.Pos);
                    drawTiles(A3, F3);
                    Thread.Sleep((int)sleepUpDown.Value);
                }
            }
            thread.Abort();
        }

        private void rosinskiAlgorithm()
        {
        }

        private void janaszekAlgorithm()
        {
        }
        #endregion

        #region Drawing
        /// <summary>Draws tiles in GUI.</summary>
        /// <param name="A">List of available tiles.</param>
        /// <param name="F">List of fixed tiles.</param>
        private void drawTiles(List<Tile> A, List<Tile> F)
        {
            drawAvailableTiles(A);
            drawFixedTiles(F);
        }

        private void drawAvailableTiles(List<Tile> A)
        {
            int boxEdge = Math.Min(availableTilesPictureBox.Width,
                availableTilesPictureBox.Height);
            int tileEdge = boxEdge / (bestPossibleScore + 1);
            Graphics g = availableTilesPictureBox.CreateGraphics();
            g.Clear(Color.DarkGray);
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Green);
            foreach (Tile t in A)
            {
                /*
                if (t.Pos != null)
                    continue;
                */
                int refX = (t.ID - 1) % (bestPossibleScore + 1);
                int refY = (t.ID - 1) / (bestPossibleScore + 1);
                brush.Color = Color.Green;
                g.FillRectangle(brush, refX * tileEdge, refY * tileEdge, 
                    tileEdge, tileEdge);
                brush.Color = Color.Brown;
                Point[] points = new Point[3];
                if (t.Left == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.Top == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.Right == 2)
                {
                    points[0] = new Point(refX * tileEdge + tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.Bottom == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge + tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                brush.Color = Color.White;
                points = new Point[5];
                if (t.Left == 3)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge + 2 * tileEdge / 5);
                    points[1] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge, refY * tileEdge + 3 * tileEdge / 5);
                    g.FillPolygon(brush, points);
                }
                if (t.Top == 3)
                {
                    points[0] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge);
                    g.FillPolygon(brush, points);
                }
                if (t.Right == 3)
                {
                    points[0] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + 2 * tileEdge / 5);
                    points[1] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + 3 * tileEdge / 5);
                    g.FillPolygon(brush, points);
                } 
                if (t.Bottom == 3)
                {
                    points[0] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + tileEdge);
                    points[1] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + tileEdge);
                    g.FillPolygon(brush, points);
                }
                g.DrawRectangle(pen, refX * tileEdge, refY * tileEdge,
                    tileEdge, tileEdge);
            }
            //g.Dispose();
        }

        private void drawFixedTiles(List<Tile> F)
        {
            int boxEdge = Math.Min(fixedTilesPictureBox.Width,
                fixedTilesPictureBox.Height);
            int tileEdge = boxEdge / (bestPossibleScore + 1);
            Graphics g = fixedTilesPictureBox.CreateGraphics();
            g.Clear(Color.DarkGray);
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Green);
            foreach (Tile t in F)
            {
                if (t.Pos == null)
                    continue;
                int refX = t.Pos.X - fixedMinX;
                int refY = t.Pos.Y - fixedMinY;
                brush.Color = Color.Green;
                g.FillRectangle(brush, refX * tileEdge, refY * tileEdge,
                    tileEdge, tileEdge);
                brush.Color = Color.Brown;
                Point[] points = new Point[3];
                if (t.Left == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.Top == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.Right == 2)
                {
                    points[0] = new Point(refX * tileEdge + tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.Bottom == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge + tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                brush.Color = Color.White;
                points = new Point[5];
                if (t.Left == 3)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge + 2 * tileEdge / 5);
                    points[1] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge, refY * tileEdge + 3 * tileEdge / 5);
                    g.FillPolygon(brush, points);
                }
                if (t.Top == 3)
                {
                    points[0] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge);
                    g.FillPolygon(brush, points);
                }
                if (t.Right == 3)
                {
                    points[0] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + 2 * tileEdge / 5);
                    points[1] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + 3 * tileEdge / 5);
                    g.FillPolygon(brush, points);
                }
                if (t.Bottom == 3)
                {
                    points[0] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + tileEdge);
                    points[1] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + tileEdge);
                    g.FillPolygon(brush, points);
                }
                g.DrawRectangle(pen, refX * tileEdge, refY * tileEdge,
                    tileEdge, tileEdge);
            }
            //g.Dispose();
        }
        #endregion

        private void goButton_Click(object sender, EventArgs e)
        {
            if (!fileLoaded)
                return;
            short algorithm = getAlgorithm();
            switch (algorithm)
            {
                case 1: 
                    thread = new Thread(new ThreadStart(accurateAlgorithm));
                    thread.Start();
                    break;
                case 2: 
                    thread = new Thread(new ThreadStart(rosinskiAlgorithm));
                    break;
                case 3: 
                    thread = new Thread(new ThreadStart(janaszekAlgorithm));
                    break;
                default: 
                    MessageBox.Show("Error:\nAlgorithm selection.");
                    break;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            thread.Abort();
        }
    }
}
