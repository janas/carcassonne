namespace CarcassonneSolver
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.algorithmGroupBox = new System.Windows.Forms.GroupBox();
            this.RosinskiRadioButton = new System.Windows.Forms.RadioButton();
            this.JanaszekRadioButton = new System.Windows.Forms.RadioButton();
            this.AccurateRadioButton = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accurateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rosińskiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.janaszekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tilesLoadedGroupBox = new System.Windows.Forms.GroupBox();
            this.tilesLoadedPictureBox = new System.Windows.Forms.PictureBox();
            this.availableTilesPictureBox = new System.Windows.Forms.PictureBox();
            this.availableTilesLabel = new System.Windows.Forms.Label();
            this.visualizationGroupBox = new System.Windows.Forms.GroupBox();
            this.fixedTilesLabel = new System.Windows.Forms.Label();
            this.fixedTilesPictureBox = new System.Windows.Forms.PictureBox();
            this.executionGroupBox = new System.Windows.Forms.GroupBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.goLabel = new System.Windows.Forms.Label();
            this.goButton = new System.Windows.Forms.Button();
            this.sleepGroupBox = new System.Windows.Forms.GroupBox();
            this.sleepUpDown = new System.Windows.Forms.NumericUpDown();
            this.scoreGroupBox = new System.Windows.Forms.GroupBox();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.executionProgressBar = new System.Windows.Forms.ProgressBar();
            this.testingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.availableTileClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.performanceLabel = new System.Windows.Forms.Label();
            this.algorithmGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tilesLoadedGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tilesLoadedPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availableTilesPictureBox)).BeginInit();
            this.visualizationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fixedTilesPictureBox)).BeginInit();
            this.executionGroupBox.SuspendLayout();
            this.sleepGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sleepUpDown)).BeginInit();
            this.scoreGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // algorithmGroupBox
            // 
            this.algorithmGroupBox.Controls.Add(this.RosinskiRadioButton);
            this.algorithmGroupBox.Controls.Add(this.JanaszekRadioButton);
            this.algorithmGroupBox.Controls.Add(this.AccurateRadioButton);
            this.algorithmGroupBox.Location = new System.Drawing.Point(101, 27);
            this.algorithmGroupBox.Name = "algorithmGroupBox";
            this.algorithmGroupBox.Size = new System.Drawing.Size(237, 42);
            this.algorithmGroupBox.TabIndex = 1;
            this.algorithmGroupBox.TabStop = false;
            this.algorithmGroupBox.Text = "Algorithm selection";
            // 
            // RosinskiRadioButton
            // 
            this.RosinskiRadioButton.AutoSize = true;
            this.RosinskiRadioButton.Location = new System.Drawing.Point(80, 19);
            this.RosinskiRadioButton.Name = "RosinskiRadioButton";
            this.RosinskiRadioButton.Size = new System.Drawing.Size(72, 17);
            this.RosinskiRadioButton.TabIndex = 1;
            this.RosinskiRadioButton.TabStop = true;
            this.RosinskiRadioButton.Text = "Rosiński\'s";
            this.RosinskiRadioButton.UseVisualStyleBackColor = true;
            // 
            // JanaszekRadioButton
            // 
            this.JanaszekRadioButton.AutoSize = true;
            this.JanaszekRadioButton.Location = new System.Drawing.Point(158, 19);
            this.JanaszekRadioButton.Name = "JanaszekRadioButton";
            this.JanaszekRadioButton.Size = new System.Drawing.Size(77, 17);
            this.JanaszekRadioButton.TabIndex = 2;
            this.JanaszekRadioButton.TabStop = true;
            this.JanaszekRadioButton.Text = "Janaszek\'s";
            this.JanaszekRadioButton.UseVisualStyleBackColor = true;
            // 
            // AccurateRadioButton
            // 
            this.AccurateRadioButton.AutoSize = true;
            this.AccurateRadioButton.Location = new System.Drawing.Point(6, 19);
            this.AccurateRadioButton.Name = "AccurateRadioButton";
            this.AccurateRadioButton.Size = new System.Drawing.Size(68, 17);
            this.AccurateRadioButton.TabIndex = 0;
            this.AccurateRadioButton.TabStop = true;
            this.AccurateRadioButton.Text = "Accurate";
            this.AccurateRadioButton.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.algorithmToolStripMenuItem,
            this.testingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadXMLToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadXMLToolStripMenuItem
            // 
            this.loadXMLToolStripMenuItem.Name = "loadXMLToolStripMenuItem";
            this.loadXMLToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.loadXMLToolStripMenuItem.Text = "Load XML";
            this.loadXMLToolStripMenuItem.Click += new System.EventHandler(this.loadXMLToolStripMenuItem_Click);
            // 
            // algorithmToolStripMenuItem
            // 
            this.algorithmToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accurateToolStripMenuItem,
            this.rosińskiToolStripMenuItem,
            this.janaszekToolStripMenuItem});
            this.algorithmToolStripMenuItem.Name = "algorithmToolStripMenuItem";
            this.algorithmToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.algorithmToolStripMenuItem.Text = "Algorithm";
            // 
            // accurateToolStripMenuItem
            // 
            this.accurateToolStripMenuItem.Name = "accurateToolStripMenuItem";
            this.accurateToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.accurateToolStripMenuItem.Text = "Accurate";
            // 
            // rosińskiToolStripMenuItem
            // 
            this.rosińskiToolStripMenuItem.Name = "rosińskiToolStripMenuItem";
            this.rosińskiToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.rosińskiToolStripMenuItem.Text = "Rosiński\'s";
            // 
            // janaszekToolStripMenuItem
            // 
            this.janaszekToolStripMenuItem.Name = "janaszekToolStripMenuItem";
            this.janaszekToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.janaszekToolStripMenuItem.Text = "Janaszek\'s";
            // 
            // tilesLoadedGroupBox
            // 
            this.tilesLoadedGroupBox.Controls.Add(this.tilesLoadedPictureBox);
            this.tilesLoadedGroupBox.Location = new System.Drawing.Point(13, 27);
            this.tilesLoadedGroupBox.Name = "tilesLoadedGroupBox";
            this.tilesLoadedGroupBox.Size = new System.Drawing.Size(82, 42);
            this.tilesLoadedGroupBox.TabIndex = 3;
            this.tilesLoadedGroupBox.TabStop = false;
            this.tilesLoadedGroupBox.Text = "Tiles loaded";
            // 
            // tilesLoadedPictureBox
            // 
            this.tilesLoadedPictureBox.BackColor = System.Drawing.Color.Red;
            this.tilesLoadedPictureBox.Location = new System.Drawing.Point(7, 18);
            this.tilesLoadedPictureBox.Name = "tilesLoadedPictureBox";
            this.tilesLoadedPictureBox.Size = new System.Drawing.Size(63, 17);
            this.tilesLoadedPictureBox.TabIndex = 0;
            this.tilesLoadedPictureBox.TabStop = false;
            // 
            // availableTilesPictureBox
            // 
            this.availableTilesPictureBox.BackColor = System.Drawing.Color.DarkGray;
            this.availableTilesPictureBox.Location = new System.Drawing.Point(9, 32);
            this.availableTilesPictureBox.Name = "availableTilesPictureBox";
            this.availableTilesPictureBox.Size = new System.Drawing.Size(450, 430);
            this.availableTilesPictureBox.TabIndex = 4;
            this.availableTilesPictureBox.TabStop = false;
            // 
            // availableTilesLabel
            // 
            this.availableTilesLabel.AutoSize = true;
            this.availableTilesLabel.Location = new System.Drawing.Point(388, 16);
            this.availableTilesLabel.Name = "availableTilesLabel";
            this.availableTilesLabel.Size = new System.Drawing.Size(71, 13);
            this.availableTilesLabel.TabIndex = 5;
            this.availableTilesLabel.Text = "Available tiles";
            // 
            // visualizationGroupBox
            // 
            this.visualizationGroupBox.Controls.Add(this.fixedTilesLabel);
            this.visualizationGroupBox.Controls.Add(this.fixedTilesPictureBox);
            this.visualizationGroupBox.Controls.Add(this.availableTilesLabel);
            this.visualizationGroupBox.Controls.Add(this.availableTilesPictureBox);
            this.visualizationGroupBox.Location = new System.Drawing.Point(13, 75);
            this.visualizationGroupBox.Name = "visualizationGroupBox";
            this.visualizationGroupBox.Size = new System.Drawing.Size(959, 475);
            this.visualizationGroupBox.TabIndex = 6;
            this.visualizationGroupBox.TabStop = false;
            this.visualizationGroupBox.Text = "Visualization";
            // 
            // fixedTilesLabel
            // 
            this.fixedTilesLabel.AutoSize = true;
            this.fixedTilesLabel.Location = new System.Drawing.Point(500, 16);
            this.fixedTilesLabel.Name = "fixedTilesLabel";
            this.fixedTilesLabel.Size = new System.Drawing.Size(53, 13);
            this.fixedTilesLabel.TabIndex = 7;
            this.fixedTilesLabel.Text = "Fixed tiles";
            // 
            // fixedTilesPictureBox
            // 
            this.fixedTilesPictureBox.BackColor = System.Drawing.Color.DarkGray;
            this.fixedTilesPictureBox.Location = new System.Drawing.Point(503, 32);
            this.fixedTilesPictureBox.Name = "fixedTilesPictureBox";
            this.fixedTilesPictureBox.Size = new System.Drawing.Size(450, 430);
            this.fixedTilesPictureBox.TabIndex = 6;
            this.fixedTilesPictureBox.TabStop = false;
            // 
            // executionGroupBox
            // 
            this.executionGroupBox.Controls.Add(this.performanceLabel);
            this.executionGroupBox.Controls.Add(this.executionProgressBar);
            this.executionGroupBox.Controls.Add(this.cancelButton);
            this.executionGroupBox.Controls.Add(this.goLabel);
            this.executionGroupBox.Controls.Add(this.goButton);
            this.executionGroupBox.Location = new System.Drawing.Point(342, 28);
            this.executionGroupBox.Name = "executionGroupBox";
            this.executionGroupBox.Size = new System.Drawing.Size(443, 41);
            this.executionGroupBox.TabIndex = 7;
            this.executionGroupBox.TabStop = false;
            this.executionGroupBox.Text = "Execution";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(79, 14);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(67, 20);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // goLabel
            // 
            this.goLabel.AutoSize = true;
            this.goLabel.Location = new System.Drawing.Point(6, 20);
            this.goLabel.Name = "goLabel";
            this.goLabel.Size = new System.Drawing.Size(0, 13);
            this.goLabel.TabIndex = 8;
            // 
            // goButton
            // 
            this.goButton.BackColor = System.Drawing.Color.SteelBlue;
            this.goButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.goButton.ForeColor = System.Drawing.Color.White;
            this.goButton.Location = new System.Drawing.Point(6, 14);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(67, 20);
            this.goButton.TabIndex = 0;
            this.goButton.Text = "Start";
            this.goButton.UseVisualStyleBackColor = false;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // sleepGroupBox
            // 
            this.sleepGroupBox.Controls.Add(this.sleepUpDown);
            this.sleepGroupBox.Location = new System.Drawing.Point(901, 28);
            this.sleepGroupBox.Name = "sleepGroupBox";
            this.sleepGroupBox.Size = new System.Drawing.Size(71, 41);
            this.sleepGroupBox.TabIndex = 8;
            this.sleepGroupBox.TabStop = false;
            this.sleepGroupBox.Text = "Sleep";
            // 
            // sleepUpDown
            // 
            this.sleepUpDown.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.sleepUpDown.Location = new System.Drawing.Point(6, 15);
            this.sleepUpDown.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.sleepUpDown.Name = "sleepUpDown";
            this.sleepUpDown.Size = new System.Drawing.Size(57, 20);
            this.sleepUpDown.TabIndex = 0;
            this.sleepUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // scoreGroupBox
            // 
            this.scoreGroupBox.Controls.Add(this.scoreLabel);
            this.scoreGroupBox.Location = new System.Drawing.Point(840, 28);
            this.scoreGroupBox.Name = "scoreGroupBox";
            this.scoreGroupBox.Size = new System.Drawing.Size(55, 41);
            this.scoreGroupBox.TabIndex = 9;
            this.scoreGroupBox.TabStop = false;
            this.scoreGroupBox.Text = "Score";
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Location = new System.Drawing.Point(6, 20);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(13, 13);
            this.scoreLabel.TabIndex = 0;
            this.scoreLabel.Text = "0";
            // 
            // executionProgressBar
            // 
            this.executionProgressBar.Enabled = false;
            this.executionProgressBar.Location = new System.Drawing.Point(152, 12);
            this.executionProgressBar.Name = "executionProgressBar";
            this.executionProgressBar.Size = new System.Drawing.Size(100, 23);
            this.executionProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.executionProgressBar.TabIndex = 10;
            // 
            // testingToolStripMenuItem
            // 
            this.testingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.availableTileClassesToolStripMenuItem});
            this.testingToolStripMenuItem.Name = "testingToolStripMenuItem";
            this.testingToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.testingToolStripMenuItem.Text = "Testing";
            // 
            // availableTileClassesToolStripMenuItem
            // 
            this.availableTileClassesToolStripMenuItem.Name = "availableTileClassesToolStripMenuItem";
            this.availableTileClassesToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.availableTileClassesToolStripMenuItem.Text = "# available tile classes";
            this.availableTileClassesToolStripMenuItem.Click += new System.EventHandler(this.availableTileClassesToolStripMenuItem_Click);
            // 
            // performanceLabel
            // 
            this.performanceLabel.AutoSize = true;
            this.performanceLabel.Location = new System.Drawing.Point(258, 20);
            this.performanceLabel.Name = "performanceLabel";
            this.performanceLabel.Size = new System.Drawing.Size(0, 13);
            this.performanceLabel.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.scoreGroupBox);
            this.Controls.Add(this.sleepGroupBox);
            this.Controls.Add(this.executionGroupBox);
            this.Controls.Add(this.visualizationGroupBox);
            this.Controls.Add(this.tilesLoadedGroupBox);
            this.Controls.Add(this.algorithmGroupBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Carcassonne Solver";
            this.algorithmGroupBox.ResumeLayout(false);
            this.algorithmGroupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tilesLoadedGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tilesLoadedPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availableTilesPictureBox)).EndInit();
            this.visualizationGroupBox.ResumeLayout(false);
            this.visualizationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fixedTilesPictureBox)).EndInit();
            this.executionGroupBox.ResumeLayout(false);
            this.executionGroupBox.PerformLayout();
            this.sleepGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sleepUpDown)).EndInit();
            this.scoreGroupBox.ResumeLayout(false);
            this.scoreGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox algorithmGroupBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadXMLToolStripMenuItem;
        private System.Windows.Forms.RadioButton JanaszekRadioButton;
        private System.Windows.Forms.RadioButton RosinskiRadioButton;
        private System.Windows.Forms.RadioButton AccurateRadioButton;
        private System.Windows.Forms.ToolStripMenuItem algorithmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accurateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rosińskiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem janaszekToolStripMenuItem;
        private System.Windows.Forms.GroupBox tilesLoadedGroupBox;
        private System.Windows.Forms.PictureBox tilesLoadedPictureBox;
        private System.Windows.Forms.PictureBox availableTilesPictureBox;
        private System.Windows.Forms.Label availableTilesLabel;
        private System.Windows.Forms.GroupBox visualizationGroupBox;
        private System.Windows.Forms.PictureBox fixedTilesPictureBox;
        private System.Windows.Forms.Label fixedTilesLabel;
        private System.Windows.Forms.GroupBox executionGroupBox;
        private System.Windows.Forms.Label goLabel;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.GroupBox sleepGroupBox;
        private System.Windows.Forms.NumericUpDown sleepUpDown;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox scoreGroupBox;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.ProgressBar executionProgressBar;
        private System.Windows.Forms.ToolStripMenuItem testingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem availableTileClassesToolStripMenuItem;
        private System.Windows.Forms.Label performanceLabel;
    }
}

