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

        private void XMLbutton_Click(object sender, EventArgs e)
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
                            ConvertXmlToInt(t.R), ConvertXmlToInt(t.T), ConvertXmlToInt(t.B)));
                    }
                }
                catch
                {
                    MessageBox.Show("File invalid.");
                }
                MessageBox.Show("File loaded succesfully.\nThere are "
                    +availableTiles.Count.ToString()+" available tiles now.");
            }
        }
    }
}
