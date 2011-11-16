using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CarcassonneSolver
{
    public partial class MainForm : Form
    {
        enum TT { field = 1, city, road };
        List<Tile> availableTiles;
        short algorithm = 1;
        int bestPossibleScore = 0;

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
                catch
                {
                    MessageBox.Show("File invalid.");
                }
                if (availableTiles.Count > 0)
                {
                    tilesLoadedPictureBox.BackColor = Color.ForestGreen;
                    bestPossibleScore = (int)Math.Sqrt((double)availableTiles.Count);
                }
                else
                {
                    tilesLoadedPictureBox.BackColor = Color.Red;
                    bestPossibleScore = 0;
                }
            }
        }

        private void drawAvailableTiles()
        {
            int boxEdge = Math.Min(availableTilesPictureBox.Width,
                availableTilesPictureBox.Height);
            int tileEdge = boxEdge / (bestPossibleScore + 1);
            Graphics g = availableTilesPictureBox.CreateGraphics();
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Green);
            foreach (Tile t in availableTiles)
            {
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
            g.Dispose();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            drawAvailableTiles();
        }
    }
}
