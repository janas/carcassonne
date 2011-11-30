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
        List<BigTile> availableBigTiles;
        List<BigTile> fixedBigTiles;
        int bestPossibleScore = 0;
        int bestBigTilePossibleScore = 0;
        List<Tile> BRSavailableTiles;
        List<Tile> BRSfixedTiles;
        List<BigTile> BRSavailableBigTiles;
        List<BigTile> BRSfixedBigTiles;
        int BRSscore = 0;
        int currentScore = 0;
        Thread thread;
        DateTime startTime;
        DateTime stopTime;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stops background thread.
        /// </summary>
        private void threadStart()
        {
            thread.Start();
            BRSscore = 0;
            currentScore = 0;
            performanceLabel.Invoke((MethodInvoker)(() => performanceLabel.Text = ""));
            executionProgressBar.Style = ProgressBarStyle.Marquee;
            executionProgressBar.MarqueeAnimationSpeed = 30;
            startTime = DateTime.Now;
        }

        /// <summary>
        /// Starts background thread.
        /// </summary>
        private void threadStop()
        {
            stopTime = DateTime.Now;
            StopProgressBar(executionProgressBar);
            TimeSpan executionTime = stopTime - startTime;
            /*
            DateTime bday = new DateTime(1989, 3, 15, 15, 10, 0);
            executionTime = DateTime.Now - bday;
            */
            String performanceInfo = "Time: ";
            if (executionTime.Days > 0)
                performanceInfo += executionTime.Days.ToString() + " d ";
            if (executionTime.Days > 0 || executionTime.Hours > 0)
                performanceInfo += executionTime.Hours.ToString() + " h ";
            if (executionTime.Days > 0 || executionTime.Hours > 0 || executionTime.Minutes > 0)
                performanceInfo += executionTime.Minutes.ToString() + " m ";
            if (executionTime.Days > 0 || executionTime.Hours > 0 || executionTime.Minutes > 0
                || executionTime.Seconds > 0)
                performanceInfo += executionTime.Seconds.ToString() + " s ";
            performanceInfo += executionTime.Milliseconds.ToString() + " ms";
            performanceLabel.Invoke((MethodInvoker)(() => performanceLabel.Text = performanceInfo));
            thread.Abort();
        }

        /// <summary>
        /// Converts input string from XML file to int.
        /// </summary>
        /// <param name="input">Input string from XML file.</param>
        /// <returns>Corresponding number in TT enum.</returns>
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

        /// <summary>
        /// Lists distinct TileClasses from list of Tiles.
        /// </summary>
        /// <param name="L">List of tiles.</param>
        /// <returns>List of distinct TileClasses.</returns>
        private List<TileClass> listTileClasses(List<Tile> L)
        {
            List<TileClass> result = new List<TileClass>();
            foreach (Tile t in L)
                if (!tileClassExists(t.tileClass, result))
                    result.Add(t.tileClass);
            return result;
        }

        /// <summary>
        /// Lists distinct TileClasses from list of BigTiles.
        /// </summary>
        /// <param name="L">List of big tiles.</param>
        /// <returns>List of distinct TileClasses.</returns>
        private List<TileClass> listBigTileClasses(List<BigTile> L)
        {
            List<TileClass> result = new List<TileClass>();
            foreach (BigTile t in L)
                if (!tileClassExists(t.tileClass, result))
                    result.Add(t.tileClass);
            return result;
        }

        /// <summary>
        /// Finds TileClass matching edge types in list of Tiles.
        /// </summary>
        /// <param name="l">Left edge type.</param>
        /// <param name="r">Right edge type.</param>
        /// <param name="t">Top edge type.</param>
        /// <param name="b">Bottom edge type.</param>
        /// <param name="L">List of tiles.</param>
        /// <returns>Matching TileClass, if exists; newly created TileClass, otherwise.</returns>
        private TileClass findEqualTileClass(int l, int r, int t, int b, List<Tile> L)
        {
            foreach (Tile tile in L)
                if (tile.tileClass.Equals(l, r, t, b))
                    return tile.tileClass;
            return new TileClass(l, r, t, b, 0);
        }

        /// <summary>
        /// Finds TileClass matching edge types in list of BigTiles.
        /// </summary>
        /// <param name="l">Left edge type.</param>
        /// <param name="r">Right edge type.</param>
        /// <param name="t">Top edge type.</param>
        /// <param name="b">Bottom edge type.</param>
        /// <param name="L">List of tiles.</param>
        /// <returns>Matching TileClass, if exists; newly created TileClass, otherwise.</returns>
        private TileClass findEqualTileClass(int l, int r, int t, int b, List<BigTile> L)
        {
            foreach (BigTile tile in L)
                if (tile.tileClass.Equals(l, r, t, b))
                    return tile.tileClass;
            return new TileClass(l, r, t, b, 0);
        }

        /// <summary>
        /// Checks whether TileClass matching edge types exists in list of tiles.
        /// </summary>
        /// <param name="l">Left edge type.</param>
        /// <param name="r">Right edge type.</param>
        /// <param name="t">Top edge type.</param>
        /// <param name="b">Bottom edge type.</param>
        /// <param name="L">List of tiles.</param>
        /// <returns>True, when TileClass exists in the list; false, otherwise.</returns>
        private bool tileClassExists(int l, int r, int t, int b, List<Tile> L)
        {
            foreach (Tile tile in L)
                if (tile.tileClass.Equals(l, r, t, b))
                    return true;
            return false;
        }

        /// <summary>
        /// Checks whether TileClass exists in list of TileClasses.
        /// </summary>
        /// <param name="tc">Searched TileClass.</param>
        /// <param name="L">List of TileClasses.</param>
        /// <returns>True, when TileClass exists in the list; false, otherwise.</returns>
        private bool tileClassExists(TileClass tc, List<TileClass> L)
        {
            foreach (TileClass tileClass in L)
                if (tileClass.Equals(tc))
                    return true;
            return false;
        }

        /// <summary>
        /// Loads input from XMl file and adds tiles to list of available tiles.
        /// </summary>
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
                        TileClass tc = findEqualTileClass(ConvertXmlToInt(t.L), ConvertXmlToInt(t.R), 
                            ConvertXmlToInt(t.T), ConvertXmlToInt(t.B), availableTiles);
                        availableTiles.Add(new Tile(tc, availableTiles.Count + 1));
                        tc.avaCount++;
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
                    //drawFixedTiles(availableTiles);
                }
                else
                {
                    fileLoaded = false;
                    tilesLoadedPictureBox.BackColor = Color.Red;
                    bestPossibleScore = 0;
                }
            }
        }
        
        /// <summary>
        /// Checks which algorithm was chosen by the user.
        /// </summary>
        /// <returns>Number corresponding to chosen algorithm.</returns>
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
        
        /// <summary>
        /// Checks whether list of fixed tiles contains tile of specific position.
        /// </summary>
        /// <param name="x">X-coordinate of tile position.</param>
        /// <param name="y">Y-coordinate of tile position.</param>
        private bool fixedListContainsElement(int x, int y)
        {
            foreach (Tile t in fixedTiles)
                if (t.Pos.X == x && t.Pos.Y == y)
                    return true;
            return false;
        }

        /// <summary>
        /// Checks whether list of fixed big tiles contains tile of specific position.
        /// </summary>
        /// <param name="x">X-coordinate of tile position.</param>
        /// <param name="y">Y-coordinate of tile position.</param>
        private bool fixedBigTilesListContainsElement(int x, int y)
        {
            foreach (BigTile bt in fixedBigTiles)
                if (bt.Pos.X == x && bt.Pos.Y == y)
                    return true;
            return false;
        }

        /// <summary>
        /// Checks whether list contains square of tiles.
        /// </summary>
        /// <param name="x">X-coordinate of left-top-most tile position.</param>
        /// <param name="y">Y-coordinate of left-top-most tile position.</param>
        /// <param name="edge">Length of square edge.</param>
        private bool fixedListContainsSquare(int x, int y, int edge)
        {
            for (int i = x; i < edge; i++)
                for (int j = y; j < edge; j++)
                    if (!fixedListContainsElement(i, j))
                        return false;
            return true;
        }

        /// <summary>
        /// Checks whether list contains square of big tiles.
        /// </summary>
        /// <param name="x">X-coordinate of left-top-most big tile position.</param>
        /// <param name="y">Y-coordinate of left-top-most big tile position.</param>
        /// <param name="edge">Length of square edge.</param>
        private bool fixedBigTilesListContainsSquare(int x, int y, int edge)
        {
            for (int i = x; i < edge; i++)
                for (int j = y; j < edge; j++)
                    if (!fixedBigTilesListContainsElement(i, j))
                        return false;
            return true;
        }

        /// <summary>
        /// Counts number of tiles in the list with at least one edge of city type.
        /// </summary>
        /// <param name="L">List of tiles.</param>
        /// <returns>Number of tiles with city type edge.</returns>
        private int countCityTiles(List<Tile> L)
        {
            int result = 0;
            foreach (Tile t in L)
                if (t.tileClass.CitiesNumber() > 0)
                    result++;
            return result;
        }

        /// <summary>
        /// Counts number of big tiles in the list with at least one edge of city type.
        /// </summary>
        /// <param name="L">List of big tiles.</param>
        /// <returns>Number of big tiles with city type edge.</returns>
        private int countCityTiles(List<BigTile> L)
        {
            int result = 0;
            foreach (BigTile bt in L)
                if (bt.tileClass.CitiesNumber() > 0)
                    result++;
            return result;
        }

        /// <summary>
        /// Counts number of tiles in the list with at least one edge of field type.
        /// </summary>
        /// <param name="L">List of tiles.</param>
        /// <returns>Number of tiles with field type edge.</returns>
        private int countFieldTiles(List<Tile> L)
        {
            int result = 0;
            foreach (Tile t in L)
                if (t.tileClass.FieldsNumber() > 0)
                    result++;
            return result;
        }

        /// <summary>
        /// Counts number of big tiles in the list with at least one edge of field type.
        /// </summary>
        /// <param name="L">List of big tiles.</param>
        /// <returns>Number of big tiles with field type edge.</returns>
        private int countFieldTiles(List<BigTile> L)
        {
            int result = 0;
            foreach (BigTile bt in L)
                if (bt.tileClass.FieldsNumber() > 0)
                    result++;
            return result;
        }

        /// <summary>
        /// Counts number of tiles in the list with at least one edge of road type.
        /// </summary>
        /// <param name="L">List of tiles.</param>
        /// <returns>Number of tiles with road type edge.</returns>
        private int countRoadTiles(List<Tile> L)
        {
            int result = 0;
            foreach (Tile t in L)
                if (t.tileClass.RoadsNumber() > 0)
                    result++;
            return result;
        }

        /// <summary>
        /// Counts number of big tiles in the list with at least one edge of road type.
        /// </summary>
        /// <param name="L">List of big tiles.</param>
        /// <returns>Number of big tiles with road type edge.</returns>
        private int countRoadTiles(List<BigTile> L)
        {
            int result = 0;
            foreach (BigTile bt in L)
                if (bt.tileClass.RoadsNumber() > 0)
                    result++;
            return result;
        }

        private void getNeighboursTypes(ref int left, ref int right, ref int top, ref int bottom, int x, int y)
        {
            foreach (Tile t in fixedTiles)
            {
                if (t.Pos.X == x - 1 && t.Pos.Y == y)
                    left = t.getRight();
                if (t.Pos.X == x + 1 && t.Pos.Y == y)
                    right = t.getLeft();
                if (t.Pos.X == x && t.Pos.Y == y + 1)
                    bottom = t.getTop();
                if (t.Pos.X == x && t.Pos.Y == y - 1)
                    top = t.getBottom();
            }
        }

        private void getBigTilesNeighboursTypes(ref int left, ref int right, ref int top, ref int bottom, int x, int y)
        {
            foreach (BigTile t in fixedBigTiles)
            {
                if (t.Pos.X == x - 1 && t.Pos.Y == y)
                    left = t.getRightTop().getRight();
                if (t.Pos.X == x + 1 && t.Pos.Y == y)
                    right = t.getLeftBottom().getLeft();
                if (t.Pos.X == x && t.Pos.Y == y + 1)
                    bottom = t.getRightTop().getTop();
                if (t.Pos.X == x && t.Pos.Y == y - 1)
                    top = t.getLeftBottom().getBottom();
            }
        }

        private void saveBRS(int score, List<Tile> A, List<Tile> F)
        {
            BRSscore = score;
            BRSavailableTiles = new List<Tile>();
            BRSfixedTiles = new List<Tile>();
            foreach (Tile tA in A)
                BRSavailableTiles.Add(tA);
            foreach (Tile tF in F)
                BRSfixedTiles.Add(tF);
        }

        private void saveBRS(int score, List<BigTile> A, List<BigTile> F)
        {
            BRSscore = score;
            BRSavailableBigTiles = new List<BigTile>();
            BRSfixedBigTiles = new List<BigTile>();
            foreach (BigTile tA in A)
                BRSavailableBigTiles.Add(tA);
            foreach (BigTile tF in F)
                BRSfixedBigTiles.Add(tF);
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
            if (result > BRSscore)
                saveBRS(result, availableTiles, fixedTiles);
            scoreLabel.Invoke((MethodInvoker)(() => scoreLabel.Text = result.ToString()
                + " / " + bestPossibleScore.ToString()));
            return result;
        }

        private int verifyScore(List<BigTile> F)
        {
            if (F.Count == 0)
                return 0;
            int result = 1;
            while (true)
            {
                if (fixedBigTilesListContainsSquare(0, 0, result + 1))
                    result++;
                else
                    break;
            }
            if (result > BRSscore)
                saveBRS(result, availableBigTiles, fixedBigTiles);
            scoreLabel.Invoke((MethodInvoker)(() => scoreLabel.Text = result.ToString()
                + " / " + bestPossibleScore.ToString()));
            return result;
        }

        private void stopExecution(List<Tile> A, List<Tile> F)
        {
            drawTiles(A, F);
            threadStop();
        }

        private void stopExecution(List<BigTile> A, List<BigTile> F)
        {
            drawBigTiles(A, F);
            threadStop();
        }

        private Tile getFirstTileOfClass(TileClass tc, List<Tile> L)
        {
            foreach (Tile t in L)
                if (t.tileClass.Equals(tc))
                    return t;
            return null;
        }

        private BigTile getFirstBigTileOfClass(TileClass tc, List<BigTile> L)
        {
            foreach (BigTile bt in L)
                if (bt.tileClass.Equals(tc))
                    return bt;
            return null;
        }

        public void orderList(List<TileClass> L)
        {
            //int oftenType;
            int r = countRoadTiles(availableTiles);
            int c = countCityTiles(availableTiles);
            int f = countFieldTiles(availableTiles);
            int max = Math.Max(Math.Max(r, c), f);
            if (max == r)
                L.Sort(new TileClassRoadComparer());
            else if (max == f)
                L.Sort(new TileClassFieldComparer());
            else
                L.Sort(new TileClassCityComparer());
        }

        public void orderBigTileList(List<TileClass> L)
        {
            //int oftenType;
            int r = countRoadTiles(availableBigTiles);
            int c = countCityTiles(availableBigTiles);
            int f = countFieldTiles(availableBigTiles);
            int max = Math.Max(Math.Max(r, c), f);
            if (max == r)
                L.Sort(new TileClassRoadComparer());
            else if (max == f)
                L.Sort(new TileClassFieldComparer());
            else
                L.Sort(new TileClassCityComparer());
        }

        /*
        private Tile getCityTileOfClass(TileClass tc, List<Tile> L)
        {
            List<Tile> cityL = new List<Tile>();
            foreach (Tile t in L)
                cityL.Add(t);
            if (countCityTiles(L) > (2 * currentScore - 1))
                cityL.Sort(new TileCityComparer());
            else
                cityL.Sort(new TileNotCityComparer());
            for (int i = 0; i < cityL.Count; i++)
                if (cityL.ElementAt(i).tileClass.Equals(tc))
                    return cityL.ElementAt(i);
            return null;
        }
        */

        #region Algorithms
        private List<Position> getPositions()
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

        private List<Position> getBigTilesPositions()
        {
            List<Position> result = new List<Position>();
            for (int i = 0; i < currentScore + 1; i++)
            {
                for (int j = 0; j < currentScore + 1; j++)
                {
                    if (!fixedBigTilesListContainsElement(i, j))
                        result.Add(new Position(i, j));
                }
            }
            if (result.Count == 0)
                result.Add(new Position(0, 0));
            return result;
        }

        /*
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
                    if ((rotations = availableTiles.ElementAt(j).tileClass.Match(left, right, top, bottom)).Count > 0)
                    {
                        positionUsed = true;
                        foreach (int rotation in rotations)
                        {
                            // Moving one tile from A to F
                            Tile t = availableTiles.ElementAt(j);
                            availableTiles.RemoveAt(j);
                            t.Pos = new Position(p.X, p.Y);

                            t.RotateToPosition(rotation);
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
                            t.RotateToPosition(0);
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

        private void accurateLevelDeeper2()
        {
            // Check if it is the end
            if (availableTiles.Count == 0)
                return;
            List<Position> positions = accurateGetPositions();
            foreach (Position p in positions)
            {
                bool positionUsed = false;
                List<TileClass> availableTileClasses = listTileClasses(availableTiles);
                if (availableTileClasses.Count == 0)
                {
                    MessageBox.Show("Something gone wrong:\nThere are available tiles,\n"
                        + "but there is no available tile class.");
                    return;
                }
                for (int j = 0; j < availableTileClasses.Count; j++)
                {
                    // Matching
                    int left = 0, right = 0, top = 0, bottom = 0;
                    List<int> rotations;
                    getNeighboursTypes(ref left, ref right, ref top, ref bottom, p.X, p.Y);
                    if ((rotations = availableTileClasses.ElementAt(j).Match(left, right, top, bottom)).Count > 0)
                    {
                        positionUsed = true;
                        foreach (int rotation in rotations)
                        {
                            // Moving one tile from A to F
                            Tile t = getFirstTileOfClass(availableTileClasses.ElementAt(j), availableTiles);
                            if (t == null)
                            {
                                MessageBox.Show("Something gone wrong:\nThere is available tile class,\n"
                                    + "but there is no available tile of this class.");
                                return;
                            }
                            availableTiles.Remove(t);
                            t.tileClass.avaCount--;
                            t.Pos = new Position(p.X, p.Y);

                            t.RotateToPosition(rotation);
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
                            accurateLevelDeeper2();

                            // Moving tile back from F to A
                            fixedTiles.Remove(t);
                            //t.Pos = null;
                            t.RotateToPosition(0);
                            availableTiles.Add(t);
                            t.tileClass.avaCount++;
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
        */

        private void levelDeeper(short mode, int type)
        {
            // Check if it is the end
            if (availableTiles.Count == 0)
                return;
            List<Position> positions = getPositions();
            foreach (Position p in positions)
            {
                bool positionUsed = false;
                List<TileClass> availableTileClasses = listTileClasses(availableTiles);
                if (mode == 1)
                {
                    orderList(availableTileClasses);
                }
                if (mode == 2)
                {
                    if (type == (int)TT.field)
                    {
                        if (countFieldTiles(availableTiles) > (2 * currentScore - 1))
                            availableTileClasses.Sort(new TileClassFieldComparer());
                        else
                            availableTileClasses.Sort(new TileClassNotFieldComparer());
                    }
                    else if (type == (int)TT.road)
                    {
                        if (countRoadTiles(availableTiles) > (2 * currentScore - 1))
                            availableTileClasses.Sort(new TileClassRoadComparer());
                        else
                            availableTileClasses.Sort(new TileClassNotRoadComparer());
                    }
                    else
                    {
                        if (countCityTiles(availableTiles) > (2 * currentScore - 1))
                            availableTileClasses.Sort(new TileClassCityComparer());
                        else
                            availableTileClasses.Sort(new TileClassNotCityComparer());
                    }
                }
                if (availableTileClasses.Count == 0)
                {
                    MessageBox.Show("Something gone wrong:\nThere are available tiles,\n"
                        + "but there is no available tile class.");
                    return;
                }
                //for (int j = 0; j < availableTileClasses.Count; j++)
                foreach (TileClass tc in availableTileClasses)
                {
                    // Matching
                    int left = 0, right = 0, top = 0, bottom = 0;
                    List<int> rotations;
                    getNeighboursTypes(ref left, ref right, ref top, ref bottom, p.X, p.Y);
                    //if ((rotations = availableTileClasses.ElementAt(j).Match(left, right, top, bottom)).Count > 0)
                    if ((rotations = tc.Match(left, right, top, bottom)).Count > 0)
                    {
                        positionUsed = true;
                        foreach (int rotation in rotations)
                        {
                            // Moving one tile from A to F
                            //Tile t = getFirstTileOfClass(availableTileClasses.ElementAt(j), availableTiles);
                            Tile t = getFirstTileOfClass(tc, availableTiles);
                            if (t == null)
                            {
                                MessageBox.Show("Something gone wrong:\nThere is available tile class,\n"
                                    + "but there is no available tile of this class.");
                                return;
                            }
                            availableTiles.Remove(t);
                            t.tileClass.avaCount--;
                            t.Pos = new Position(p.X, p.Y);

                            t.RotateToPosition(rotation);
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
                            levelDeeper(mode, type);

                            // Moving tile back from F to A
                            fixedTiles.Remove(t);
                            //t.Pos = null;
                            t.RotateToPosition(0);
                            availableTiles.Add(t);
                            t.tileClass.avaCount++;
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
            levelDeeper(1, 0);
            threadStop();
            drawTiles(BRSavailableTiles, BRSfixedTiles);
            scoreLabel.Invoke((MethodInvoker)(() => scoreLabel.Text = BRSscore.ToString()
                + " / " + bestPossibleScore.ToString()));
        }

        private void rosinskiAlgorithmCityFirst()
        {
            levelDeeper(2, (int)TT.city);
            threadStop();
            drawTiles(BRSavailableTiles, BRSfixedTiles);
            scoreLabel.Invoke((MethodInvoker)(() => scoreLabel.Text = BRSscore.ToString() 
                + " / " + bestPossibleScore.ToString()));
        }

        private void rosinskiAlgorithmRoadFirst()
        {
            levelDeeper(2, (int)TT.road);
            threadStop();
        }
        
        private void rosinskiAlgorithmFieldFirst()
        {
            levelDeeper(2, (int)TT.field);
            threadStop();
        }

        private void bigTilesLevelDeeper(short mode, int type)
        {
            // Check if it is the end
            if (availableBigTiles.Count == 0)
                return;
            List<Position> positions = getBigTilesPositions();
            foreach (Position p in positions)
            {
                bool positionUsed = false;
                List<TileClass> availableBigTileClasses = listBigTileClasses(availableBigTiles);
                if (mode == 1)
                {
                    orderBigTileList(availableBigTileClasses);
                }
                if (mode == 2)
                {
                    if (type == (int)TT.field)
                    {
                        if (countFieldTiles(availableTiles) > (2 * currentScore - 1))
                            availableBigTileClasses.Sort(new TileClassFieldComparer());
                        else
                            availableBigTileClasses.Sort(new TileClassNotFieldComparer());
                    }
                    else if (type == (int)TT.road)
                    {
                        if (countRoadTiles(availableTiles) > (2 * currentScore - 1))
                            availableBigTileClasses.Sort(new TileClassRoadComparer());
                        else
                            availableBigTileClasses.Sort(new TileClassNotRoadComparer());
                    }
                    else
                    {
                        if (countCityTiles(availableTiles) > (2 * currentScore - 1))
                            availableBigTileClasses.Sort(new TileClassCityComparer());
                        else
                            availableBigTileClasses.Sort(new TileClassNotCityComparer());
                    }
                }
                if (availableBigTileClasses.Count == 0)
                {
                    MessageBox.Show("Something gone wrong:\nThere are available tiles,\n"
                        + "but there is no available tile class.");
                    return;
                }
                foreach (TileClass tc in availableBigTileClasses)
                {
                    // Matching
                    int left = 0, right = 0, top = 0, bottom = 0;
                    List<int> rotations;
                    getBigTilesNeighboursTypes(ref left, ref right, ref top, ref bottom, p.X, p.Y);
                    if ((rotations = tc.Match(left, right, top, bottom)).Count > 0)
                    {
                        positionUsed = true;
                        foreach (int rotation in rotations)
                        {
                            // Moving one tile from A to F
                            BigTile bt = getFirstBigTileOfClass(tc, availableBigTiles);
                            if (bt == null)
                            {
                                MessageBox.Show("Something gone wrong:\nThere is available tile class,\n"
                                    + "but there is no available tile of this class.");
                                return;
                            }
                            availableBigTiles.Remove(bt);
                            bt.tileClass.avaCount--;
                            bt.Pos = new Position(p.X, p.Y);

                            bt.RotateToPosition(rotation);
                            fixedBigTiles.Add(bt);

                            // Drawing pictures
                            if (sleepUpDown.Value > 0)
                            {
                                drawBigTiles(availableBigTiles, fixedBigTiles);
                                Thread.Sleep((int)sleepUpDown.Value);
                            }

                            // Check if we need to look further
                            currentScore = verifyScore(fixedBigTiles);
                            if (currentScore == bestBigTilePossibleScore)
                                stopExecution(availableBigTiles, fixedBigTiles);

                            // Next level
                            bigTilesLevelDeeper(mode, type);

                            // Moving tile back from F to A
                            fixedBigTiles.Remove(bt);
                            //t.Pos = null;
                            bt.RotateToPosition(0);
                            availableBigTiles.Add(bt);
                            bt.tileClass.avaCount++;
                            currentScore = verifyScore(fixedBigTiles);

                            // Drawing pictures
                            if (sleepUpDown.Value > 0)
                            {
                                drawBigTiles(availableBigTiles, fixedBigTiles);
                                Thread.Sleep((int)sleepUpDown.Value / 5);
                            }
                        }
                    }
                }
                if (!positionUsed)
                    return;
            }
        }

        private void getNextTile(ref List<Tile> tiles)
        {
            List<TileClass> availableTileClasses = listTileClasses(availableTiles);
            foreach (TileClass tc in availableTileClasses)
            {
                List<int> rotations = null;
                if (tiles.Count == 0)
                    rotations = tc.Match(0, 0, 0, 0);
                else if (tiles.Count == 1)
                    rotations = tc.Match(tiles[0].getRight(), 0, 
                        tiles[0].getTop(), 0);
                else if (tiles.Count == 2)
                    rotations = tc.Match(tiles[0].getLeft(), 0, 
                        tiles[0].getBottom(), 0);
                else if (tiles.Count == 3)
                    rotations = tc.Match(tiles[2].getRight(), tiles[1].getRight(), 
                        tiles[1].getBottom(), tiles[2].getBottom());
                else
                    return;
                if (rotations.Count == 0)
                    return;
                foreach (int rotation in rotations)
                {
                    Tile t = getFirstTileOfClass(tc, availableTiles);
                    if (t == null)
                    {
                        MessageBox.Show("Something gone wrong:\nThere is available TileClass,\n"
                            + "but there is no available BigTile of this TileClass.\nlt");
                        return;
                    }
                    availableTiles.Remove(t);
                    t.tileClass.avaCount--;
                    t.RotateToPosition(rotation);
                    
                    tiles.Add(t);
                    getNextTile(ref tiles);
                    if (tiles.Count == 4)
                        return;
                    tiles.Remove(t);

                    t.RotateToPosition(0);
                    t.tileClass.avaCount++;
                    availableTiles.Add(t);
                }
            }
        }

        private void generateNextBigTile()
        {
            if (availableTiles.Count < 4)
                return;
            List<Tile> tiles = new List<Tile>();
            getNextTile(ref tiles);
            //MessageBox.Show(tiles.Count.ToString() + " elements to create big tile");
            if (tiles.Count < 4)
                return;
            TileClass tileClass = findEqualTileClass(tiles[0].getLeft(), tiles[1].getRight(),
                tiles[2].getTop(), tiles[3].getBottom(), availableBigTiles);
            tileClass.avaCount++;
            BigTile bt = new BigTile(tileClass, tiles[0], tiles[1], tiles[2], tiles[3], 
                availableBigTiles.Count + 1);
            availableBigTiles.Add(bt);

            generateNextBigTile();
        }

        private void janaszekAlgorithm()
        {
            availableBigTiles = new List<BigTile>();
            fixedBigTiles = new List<BigTile>();
            generateNextBigTile();
            bestBigTilePossibleScore = (int)Math.Sqrt((double)availableBigTiles.Count);
            drawAvailableBigTiles(availableBigTiles);
            bigTilesLevelDeeper(1, 0);
            threadStop();
        }
        #endregion

        #region Drawing
        /// <summary>
        /// Draws tiles in GUI.
        /// </summary>
        /// <param name="A">List of available tiles.</param>
        /// <param name="F">List of fixed tiles.</param>
        private void drawTiles(List<Tile> A, List<Tile> F)
        {
            drawAvailableTiles(A);
            drawFixedTiles(F);
        }

        private void drawBigTiles(List<BigTile> A, List<BigTile> F)
        {
            drawAvailableBigTiles(A);
            drawFixedBigTiles(F);
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
                int refX = (t.ID - 1) % (bestPossibleScore + 1);
                int refY = (t.ID - 1) / (bestPossibleScore + 1);
                brush.Color = Color.Green;
                g.FillRectangle(brush, refX * tileEdge, refY * tileEdge, 
                    tileEdge, tileEdge);
                brush.Color = Color.Brown;
                Point[] points = new Point[3];
                if (t.getLeft() == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.getTop() == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.getRight() == 2)
                {
                    points[0] = new Point(refX * tileEdge + tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.getBottom() == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge + tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                brush.Color = Color.White;
                points = new Point[5];
                if (t.getLeft() == 3)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge + 2 * tileEdge / 5);
                    points[1] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge, refY * tileEdge + 3 * tileEdge / 5);
                    g.FillPolygon(brush, points);
                }
                if (t.getTop() == 3)
                {
                    points[0] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge);
                    g.FillPolygon(brush, points);
                }
                if (t.getRight() == 3)
                {
                    points[0] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + 2 * tileEdge / 5);
                    points[1] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + 3 * tileEdge / 5);
                    g.FillPolygon(brush, points);
                }
                if (t.getBottom() == 3)
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
                Position min = findMinPosition(F);
                int refX = t.Pos.X - min.X;
                int refY = t.Pos.Y - min.Y;
                brush.Color = Color.Green;
                g.FillRectangle(brush, refX * tileEdge, refY * tileEdge,
                    tileEdge, tileEdge);
                brush.Color = Color.Brown;
                Point[] points = new Point[3];
                if (t.getLeft() == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.getTop() == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.getRight() == 2)
                {
                    points[0] = new Point(refX * tileEdge + tileEdge, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                if (t.getBottom() == 2)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge + tileEdge);
                    points[1] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + tileEdge);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    g.FillPolygon(brush, points);
                }
                brush.Color = Color.White;
                points = new Point[5];
                if (t.getLeft() == 3)
                {
                    points[0] = new Point(refX * tileEdge, refY * tileEdge + 2 * tileEdge / 5);
                    points[1] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge, refY * tileEdge + 3 * tileEdge / 5);
                    g.FillPolygon(brush, points);
                }
                if (t.getTop() == 3)
                {
                    points[0] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge);
                    points[1] = new Point(refX * tileEdge + 2 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge);
                    g.FillPolygon(brush, points);
                }
                if (t.getRight() == 3)
                {
                    points[0] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + 2 * tileEdge / 5);
                    points[1] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 2 * tileEdge / 5);
                    points[2] = new Point(refX * tileEdge + tileEdge / 2, refY * tileEdge + tileEdge / 2);
                    points[3] = new Point(refX * tileEdge + 3 * tileEdge / 5, refY * tileEdge + 3 * tileEdge / 5);
                    points[4] = new Point(refX * tileEdge + tileEdge, refY * tileEdge + 3 * tileEdge / 5);
                    g.FillPolygon(brush, points);
                }
                if (t.getBottom() == 3)
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

        private void drawAvailableBigTiles(List<BigTile> A)
        {
            int boxEdge = Math.Min(availableTilesPictureBox.Width,
                availableTilesPictureBox.Height);
            int tileEdge = boxEdge / (bestPossibleScore + 2);
            Graphics g = availableTilesPictureBox.CreateGraphics();
            g.Clear(Color.DarkGray);
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Green);
            foreach (BigTile bt in A)
            {
                int refX = 2 * ((bt.ID - 1) % (bestPossibleScore / 2 + bestPossibleScore % 2));
                int refY = 2 * ((bt.ID - 1) / (bestPossibleScore / 2 + bestPossibleScore % 2));
                
                List<Tile> tiles = bt.getTiles();
                for (int i = 0; i < tiles.Count; i++)
                {
                    Tile t = tiles[i];
                    int rX, rY;
                    if (i == 0)
                    {
                        rX = refX;
                        rY = refY;
                    }
                    else if (i == 1)
                    {
                        rX = refX;
                        rY = refY + 1;
                    }
                    else if (i == 2)
                    {
                        rX = refX + 1;
                        rY = refY;
                    }
                    else
                    {
                        rX = refX + 1;
                        rY = refY + 1;
                    }
                    brush.Color = Color.Green;
                    g.FillRectangle(brush, rX * tileEdge, rY * tileEdge,
                        tileEdge, tileEdge);
                    brush.Color = Color.Brown;
                    Point[] points = new Point[3];

                    if (t.getLeft() == 2)
                    {
                        points[0] = new Point(rX * tileEdge, rY * tileEdge);
                        points[1] = new Point(rX * tileEdge, rY * tileEdge + tileEdge);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getTop() == 2)
                    {
                        points[0] = new Point(rX * tileEdge, rY * tileEdge);
                        points[1] = new Point(rX * tileEdge + tileEdge, rY * tileEdge);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getRight() == 2)
                    {
                        points[0] = new Point(rX * tileEdge + tileEdge, rY * tileEdge);
                        points[1] = new Point(rX * tileEdge + tileEdge, rY * tileEdge + tileEdge);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getBottom() == 2)
                    {
                        points[0] = new Point(rX * tileEdge, rY * tileEdge + tileEdge);
                        points[1] = new Point(rX * tileEdge + tileEdge, rY * tileEdge + tileEdge);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        g.FillPolygon(brush, points);
                    }
                    brush.Color = Color.White;
                    points = new Point[5];
                    if (t.getLeft() == 3)
                    {
                        points[0] = new Point(rX * tileEdge, rY * tileEdge + 2 * tileEdge / 5);
                        points[1] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge + 2 * tileEdge / 5);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        points[3] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge + 3 * tileEdge / 5);
                        points[4] = new Point(rX * tileEdge, rY * tileEdge + 3 * tileEdge / 5);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getTop() == 3)
                    {
                        points[0] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge);
                        points[1] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge + 2 * tileEdge / 5);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        points[3] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge + 2 * tileEdge / 5);
                        points[4] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getRight() == 3)
                    {
                        points[0] = new Point(rX * tileEdge + tileEdge, rY * tileEdge + 2 * tileEdge / 5);
                        points[1] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge + 2 * tileEdge / 5);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        points[3] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge + 3 * tileEdge / 5);
                        points[4] = new Point(rX * tileEdge + tileEdge, rY * tileEdge + 3 * tileEdge / 5);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getBottom() == 3)
                    {
                        points[0] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge + tileEdge);
                        points[1] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge + 3 * tileEdge / 5);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        points[3] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge + 3 * tileEdge / 5);
                        points[4] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge + tileEdge);
                        g.FillPolygon(brush, points);
                    }
                    pen = new Pen(Color.Gray);
                    g.DrawRectangle(pen, rX * tileEdge, rY * tileEdge,
                        tileEdge, tileEdge);
                }
                pen = new Pen(Color.Black);
                g.DrawRectangle(pen, refX * tileEdge, refY * tileEdge,
                        tileEdge * 2, tileEdge * 2);
            }
            //g.Dispose();
        }

        private void drawFixedBigTiles(List<BigTile> F)
        {
            int boxEdge = Math.Min(fixedTilesPictureBox.Width,
                fixedTilesPictureBox.Height);
            int tileEdge = boxEdge / (bestPossibleScore + 1);
            Graphics g = fixedTilesPictureBox.CreateGraphics();
            g.Clear(Color.DarkGray);
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Green);
            foreach (BigTile bt in F)
            {
                if (bt.Pos == null)
                    continue;
                Position min = findMinPosition(F);

                int refX = (bt.Pos.X - min.X) * 2;
                int refY = (bt.Pos.Y - min.Y) * 2;

                List<Tile> tiles = bt.getTiles();
                for (int i = 0; i < tiles.Count; i++)
                {
                    Tile t = tiles[i];
                    int rX, rY;
                    if (i == 0)
                    {
                        rX = refX;
                        rY = refY;
                    }
                    else if (i == 1)
                    {
                        rX = refX;
                        rY = refY + 1;
                    }
                    else if (i == 2)
                    {
                        rX = refX + 1;
                        rY = refY;
                    }
                    else
                    {
                        rX = refX + 1;
                        rY = refY + 1;
                    }
                    brush.Color = Color.Green;
                    g.FillRectangle(brush, rX * tileEdge, rY * tileEdge,
                        tileEdge, tileEdge);
                    brush.Color = Color.Brown;
                    Point[] points = new Point[3];
                    if (t.getLeft() == 2)
                    {
                        points[0] = new Point(rX * tileEdge, rY * tileEdge);
                        points[1] = new Point(rX * tileEdge, rY * tileEdge + tileEdge);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getTop() == 2)
                    {
                        points[0] = new Point(rX * tileEdge, rY * tileEdge);
                        points[1] = new Point(rX * tileEdge + tileEdge, rY * tileEdge);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getRight() == 2)
                    {
                        points[0] = new Point(rX * tileEdge + tileEdge, rY * tileEdge);
                        points[1] = new Point(rX * tileEdge + tileEdge, rY * tileEdge + tileEdge);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getBottom() == 2)
                    {
                        points[0] = new Point(rX * tileEdge, rY * tileEdge + tileEdge);
                        points[1] = new Point(rX * tileEdge + tileEdge, rY * tileEdge + tileEdge);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        g.FillPolygon(brush, points);
                    }
                    brush.Color = Color.White;
                    points = new Point[5];
                    if (t.getLeft() == 3)
                    {
                        points[0] = new Point(rX * tileEdge, rY * tileEdge + 2 * tileEdge / 5);
                        points[1] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge + 2 * tileEdge / 5);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        points[3] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge + 3 * tileEdge / 5);
                        points[4] = new Point(rX * tileEdge, rY * tileEdge + 3 * tileEdge / 5);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getTop() == 3)
                    {
                        points[0] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge);
                        points[1] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge + 2 * tileEdge / 5);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        points[3] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge + 2 * tileEdge / 5);
                        points[4] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getRight() == 3)
                    {
                        points[0] = new Point(rX * tileEdge + tileEdge, rY * tileEdge + 2 * tileEdge / 5);
                        points[1] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge + 2 * tileEdge / 5);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        points[3] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge + 3 * tileEdge / 5);
                        points[4] = new Point(rX * tileEdge + tileEdge, rY * tileEdge + 3 * tileEdge / 5);
                        g.FillPolygon(brush, points);
                    }
                    if (t.getBottom() == 3)
                    {
                        points[0] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge + tileEdge);
                        points[1] = new Point(rX * tileEdge + 2 * tileEdge / 5, rY * tileEdge + 3 * tileEdge / 5);
                        points[2] = new Point(rX * tileEdge + tileEdge / 2, rY * tileEdge + tileEdge / 2);
                        points[3] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge + 3 * tileEdge / 5);
                        points[4] = new Point(rX * tileEdge + 3 * tileEdge / 5, rY * tileEdge + tileEdge);
                        g.FillPolygon(brush, points);
                    }
                    pen = new Pen(Color.Gray);
                    g.DrawRectangle(pen, rX * tileEdge, rY * tileEdge,
                        tileEdge, tileEdge);
                }
                pen = new Pen(Color.Black);
                g.DrawRectangle(pen, refX * tileEdge, refY * tileEdge,
                    tileEdge * 2, tileEdge * 2);
            }
            //g.Dispose();
        }
        
        /// <summary>
        /// Finds position containing X-coordinate of left-most tile and Y-coordinate od top-most tile.
        /// </summary>
        /// <param name="F">List of tiles.</param>
        /// <returns>Left-top-most position.</returns>
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

        /// <summary>
        /// Finds position containing X-coordinate of left-most big tile and Y-coordinate of top-most big tile.
        /// </summary>
        /// <param name="F">List of tiles.</param>
        /// <returns>Left-top-most position.</returns>
        private Position findMinPosition(List<BigTile> F)
        {
            Position result = new Position(Int32.MaxValue, Int32.MaxValue);
            foreach (BigTile t in F)
            {
                result.X = Math.Min(t.Pos.X, result.X);
                result.Y = Math.Min(t.Pos.Y, result.Y);
            }
            return result;
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
                    int typeSelected = getSelectedType();
                    thread = new Thread(selectThreadStart(typeSelected));
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

        private ThreadStart selectThreadStart(int type)
        {
            if (type == (int)TT.field)
                return new ThreadStart(rosinskiAlgorithmFieldFirst);
            else if (type == (int)TT.road)
                return new ThreadStart(rosinskiAlgorithmRoadFirst);
            else
                return new ThreadStart(rosinskiAlgorithmCityFirst);
        }

        private int getSelectedType()
        {
            // FIX
            // Here, it always returns city type
            return (int)TT.city;
            string error = "You have not selected type.\nI will select it for you.\nCity";
            int type = -1;
            using (SetTypeForm stf = new SetTypeForm())
            {
                DialogResult dr = stf.ShowDialog();
                if (dr == DialogResult.OK)
                    type = stf.getType();
                else
                    MessageBox.Show(error);
            }
            if (type == -1)
                MessageBox.Show(error);
            return type;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (thread != null)
                threadStop();
        }
        
        /*
        public class TileCityComparer : IComparer<Tile>
        {
            public int Compare(Tile t1, Tile t2)
            {
                int t1cities = t1.tileClass.CitiesNumber();
                int t2cities = t2.tileClass.CitiesNumber();
                if (t1cities == t2cities)
                    return 0;
                else
                    return t2cities - t1cities; 
            }
        }

        public class TileNotCityComparer : IComparer<Tile>
        {
            public int Compare(Tile t1, Tile t2)
            {
                int t1cities = t1.tileClass.CitiesNumber();
                int t2cities = t2.tileClass.CitiesNumber();
                if (t1cities == t2cities)
                    return 0;
                else
                    return t1cities - t2cities;
            }
        }
        */

        public class TileClassCityComparer : IComparer<TileClass>
        {
            public int Compare(TileClass tc1, TileClass tc2)
            {
                int n1 = tc1.CitiesNumber();
                int n2 = tc2.CitiesNumber();
                if (n1 == n2)
                    return 0;
                else
                    return n2 - n1;
            }
        }

        public class TileClassNotCityComparer : IComparer<TileClass>
        {
            public int Compare(TileClass tc1, TileClass tc2)
            {
                int n1 = tc1.CitiesNumber();
                int n2 = tc2.CitiesNumber();
                if (n1 == n2)
                    return 0;
                else
                    return n1 - n2;
            }
        }

        public class TileClassFieldComparer : IComparer<TileClass>
        {
            public int Compare(TileClass tc1, TileClass tc2)
            {
                int n1 = tc1.FieldsNumber();
                int n2 = tc2.FieldsNumber();
                if (n1 == n2)
                    return 0;
                else
                    return n2 - n1;
            }
        }

        public class TileClassNotFieldComparer : IComparer<TileClass>
        {
            public int Compare(TileClass tc1, TileClass tc2)
            {
                int n1 = tc1.FieldsNumber();
                int n2 = tc2.FieldsNumber();
                if (n1 == n2)
                    return 0;
                else
                    return n1 - n2;
            }
        }

        public class TileClassRoadComparer : IComparer<TileClass>
        {
            public int Compare(TileClass tc1, TileClass tc2)
            {
                int n1 = tc1.RoadsNumber();
                int n2 = tc2.RoadsNumber();
                if (n1 == n2)
                    return 0;
                else
                    return n2 - n1;
            }
        }

        public class TileClassNotRoadComparer : IComparer<TileClass>
        {
            public int Compare(TileClass tc1, TileClass tc2)
            {
                int n1 = tc1.RoadsNumber();
                int n2 = tc2.RoadsNumber();
                if (n1 == n2)
                    return 0;
                else
                    return n1 - n2;
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

        private void availableTileClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (availableTiles == null)
                MessageBox.Show("availableTiles is null");
            else
                MessageBox.Show(listTileClasses(availableTiles).Count.ToString());
        }
    }
}
