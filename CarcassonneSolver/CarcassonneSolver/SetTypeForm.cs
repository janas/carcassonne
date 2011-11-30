using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CarcassonneSolver
{
    public partial class SetTypeForm : Form
    {
        private int type;

        public SetTypeForm()
        {
            InitializeComponent();
        }

        public int getType()
        {
            return type;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (typeComboBox.SelectedIndex == 0)
                type = (int)TT.city;
            else if (typeComboBox.SelectedIndex == 1)
                type = (int)TT.road;
            else if (typeComboBox.SelectedIndex == 2)
                type = (int)TT.field;
            else
                type = -1;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
