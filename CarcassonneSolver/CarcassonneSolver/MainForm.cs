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
    enum TT { field = 1, city, road };

    public partial class MainForm : Form
    {
        bool fileLoaded = false;
        List<Tile> availableTiles;
        List<Tile> fixedTiles;
        int bestPossibleScore = 0;
        int currentScore = 0;
        Thread thread;
        DateTime startTime;
        DateTime stopTime;

        public MainForm()
        {
            InitializeComponent();
        }

        private void threadStart()
        {
            thread.Start();
            executionProgressBar.Style = ProgressBarStyle.Marquee;
            executionProgressBar.MarqueeAnimationSpeed = 30;
            startTime = DateTime.Now;
        }

        private void threadStop()
        {
            stopTime = DateTime.Now;
            StopProgressBar(executionProgressBar);
            TimeSpan executionTime = stopTime - startTime;
            String performanceInfo = "Execution time:\n" +
                    executionTime.Days.ToString() + " d " +
                    executionTime.Hours.ToString() + " h " +
                    executionTime.Minutes.ToString() + " m " +
                    executionTime.Seconds.ToString() + " s " +
                    executionTime.Milliseconds.ToString() + " ms";
            MessageBox.Show(performanceInfo);
            thread.Abort();
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
        
        /// <summary>Checks whether list contains specific tile.</summary>
        /// <param name="x">X-position of tile.</param>
        /// <param name="y">Y-position of tile.</param>
        private bool fixedListContainsElement(int x, int y)
        {
            foreach (Tile t in fixedTiles)
                if (t.Pos.X == x && t.Pos.Y == y)
                    return true;
            return false;
        }

        /// <summary>Checks whether list contains square of tiles.</summary>
        /// <param name="x">X-position of left-top-most tile.</param>
        /// <param name="y">Y-position of left-top-most tile.</param>
        /// <param name="edge">Length of square edge.</param>
        private bool fixedListContainsSquare(int x, int y, int edge)
        {
            for (int i = x; i < edge; i++)
                for (int j = y; j < edge; j++)
                    if (!fixedListContainsElement(i, j))
                        return false;
            return true;
        }

        private void getNeighboursTypes(ref int left, ref int right, ref int top, ref int bottom, int x, int y)
        {
            foreach (Tile t in fixedTiles)
            {
                if (t.Pos.X == x - 1 && t.Pos.Y == y)
                    left = t.Right;
                if (t.Pos.X == x + 1 && t.Pos.Y == y)
                    right = t.Left;
                if (t.Pos.X == x && t.Pos.Y == y + 1)
                    bottom = t.Top;
                if (t.Pos.X == x && t.Pos.Y == y - 1)
                    top = t.Bottom;
            }
        }

        private int verifyScore(List<Tile> F)
        {
            if (F.Count == 0)
                return 0;
            int result = 1;
            /*
            foreach (Tile t in F)
            {
                while (true)
                {
                    if (fixedListContainsSquare(t.Pos.X, t.Pos.Y, result+1))
                        result++;
                    else
                        break;
                }
            }
            */ 
            while (true)
            {
                if (fixedListContainsSquare(0, 0, result+1))
                    result++;
                else
                    break;
            }
            //MessageBox.Show(result.ToString());
            return result;
        }

        private void stopExecution(List<Tile> A, List<Tile> F)
        {
            drawTiles(A, F);
            threadStop();
        }

        #region Algorithms
        private List<Position> accurateGetPositions()
        {
            List<Position> result = new List<Position>();
            for (int i = 0; i < currentScore + 1; i++)
            {
                for (int j = 0; j < currentScore + 1; j++)
                {
                    if (!fixedListContainsElement(i, j))
                        result.Add(new Position(i, j));
                }
            }
            if (result.Count == 0)
                result.Add(new Position(0, 0));
            return result;
        }

        private void accurateLevelDeeper()
        {
            // Check if it is the end
            if (availableTiles.Count == 0)
                return;
            List<Position> positions = accurateGetPositions();
            foreach (Position p in positions)
            {
                bool positionUsed = false;
                for (int j = 0; j < availableTiles.Count; j++)
                {
                    // Matching
                    int left = 0, right = 0, top = 0, bottom = 0;
                    List<int> rotations;
                    getNeighboursTypes(ref left, ref right, ref top, ref bottom, p.X, p.Y);
                    if ((rotations = availableTiles.ElementAt(j).Match(left, right, top, bottom)).Count > 0)
                    {
                        positionUsed = true;
                        foreach (int rotation in rotations)
                        {
                            // Moving one tile from A to F
                            Tile t = availableTiles.ElementAt(j);
                            availableTiles.RemoveAt(j);
                            t.Pos = new Position(p.X, p.Y);

                            t.Rotate(rotation);
                            fixedTiles.Add(t);

                            // Drawing pictures
                            if (sleepUpDown.Value > 0)
                            {
                                drawTiles(availableTiles, fixedTiles);
                                Thread.Sleep((int)sleepUpDown.Value);
                            }

                            // Check if we need to look further
                            currentScore = verifyScore(fixedTiles);
                            if (currentScore == bestPossibleScore)
                                stopExecution(availableTiles, fixedTiles);

                            // Next level
                            accurateLevelDeeper();

                            // Moving tile back from F to A
                            fixedTiles.Remove(t);
                            //t.Pos = null;
                            t.Rotate(4 - rotation);
                            availableTiles.Insert(j, t);
                            currentScore = verifyScore(fixedTiles);

                            // Drawing pictures
                            if (sleepUpDown.Value > 0)
                            {
                                drawTiles(availableTiles, fixedTiles);
                                Thread.Sleep((int)sleepUpDown.Value / 5);
                            }
                        }
                    }
                }
                if (!positionUsed)
                    return;
            }
        }

        private void accurateAlgorithm()
        {
            accurateLevelDeeper();
            threadStop();
        }
        
        private void rosinskiAlgorithm()
        {
            TileCityComparer tc = new TileCityComparer();
            availableTiles.Sort(tc);
            accurateLevelDeeper();
            threadStop();
        }

        private void createBigTiles()
        {
            List<Tile> bigTiles = new List<Tile>();
            
        }

        private void janaszekAlgorithm()
        {
            createBigTiles();
            threadStop();
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

        private Position findMinPosition(List<Tile> F)
        {
            Position result = new Position(Int32.MaxValue, Int32.MaxValue);
            foreach (Tile t in F)
            {
                result.X = Math.Min(t.Pos.X, result.X);
                result.Y = Math.Min(t.Pos.Y, result.Y);
            }
            return result;
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
                Position min = findMinPosition(F);
                int refX = t.Pos.X - min.X;
                int refY = t.Pos.Y - min.Y;
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
                    threadStart();
                    break;
                case 2:
                    thread = new Thread(new ThreadStart(rosinskiAlgorithm));
                    threadStart();
                    break;
                case 3:
                    thread = new Thread(new ThreadStart(janaszekAlgorithm));
                    threadStart();
                    break;
                default: 
                    MessageBox.Show("Error:\nAlgorithm selection.");
                    break;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (thread != null)
                threadStop();
        }

        public class TileCityComparer : IComparer<Tile>
        {
            public int Compare(Tile t1, Tile t2)
            {
                int t1cities = t1.Cities();
                int t2cities = t2.Cities();
                if (t1cities == t2cities)
                    return 0;
                else
                    return t1cities - t2cities; 
            }
        }

        delegate void CallProgressBarStop(ProgressBar myProgressBar);
        private void StopProgressBar(ProgressBar myProgressBar)
        {
            if (myProgressBar.InvokeRequired)
            {
                CallProgressBarStop del = StopProgressBar;
                myProgressBar.Invoke(del, new object[] { myProgressBar });
                return;
            }

            myProgressBar.Style = ProgressBarStyle.Continuous;
            myProgressBar.MarqueeAnimationSpeed = 0;
        }
    }
}
