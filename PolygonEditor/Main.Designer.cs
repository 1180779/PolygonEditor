namespace PolygonEditor
{
    partial class Main
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
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            menuStrip1 = new MenuStrip();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            canvas = new PictureBox();
            controlCenter = new TabControl();
            tabPage1 = new TabPage();
            groupBox2 = new GroupBox();
            radioConC1 = new RadioButton();
            radioConG1 = new RadioButton();
            radioConG0 = new RadioButton();
            relations = new GroupBox();
            buttonLineToBezier = new Button();
            radioRestrConst = new RadioButton();
            radioRestrVertical = new RadioButton();
            radioRestrHorizontal = new RadioButton();
            radioRestrNoRestr = new RadioButton();
            groupBox1 = new GroupBox();
            buttonRemoveVertex = new Button();
            radioAlgMy = new RadioButton();
            buttonAddVertex = new Button();
            radioAlgLib = new RadioButton();
            tabPage2 = new TabPage();
            groupBox3 = new GroupBox();
            label1 = new Label();
            textBoxRadius = new TextBox();
            buttonNewPolygon = new Button();
            label2 = new Label();
            textBoxVerteciesC = new TextBox();
            buttonRemovePolygon = new Button();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            controlCenter.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox2.SuspendLayout();
            relations.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(886, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // canvas
            // 
            canvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            canvas.Location = new Point(0, 27);
            canvas.Name = "canvas";
            canvas.Size = new Size(749, 419);
            canvas.TabIndex = 2;
            canvas.TabStop = false;
            canvas.Paint += canvas_Paint;
            canvas.MouseDown += canvas_MouseDown;
            canvas.MouseMove += canvas_MouseMove;
            canvas.MouseUp += canvas_MouseUp;
            // 
            // controlCenter
            // 
            controlCenter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            controlCenter.Controls.Add(tabPage1);
            controlCenter.Controls.Add(tabPage2);
            controlCenter.Location = new Point(755, 27);
            controlCenter.Name = "controlCenter";
            controlCenter.SelectedIndex = 0;
            controlCenter.Size = new Size(131, 419);
            controlCenter.TabIndex = 3;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(relations);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(123, 391);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Edycja";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(radioConC1);
            groupBox2.Controls.Add(radioConG1);
            groupBox2.Controls.Add(radioConG0);
            groupBox2.Location = new Point(3, 294);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(117, 94);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "ciągłość";
            // 
            // radioConC1
            // 
            radioConC1.AutoSize = true;
            radioConC1.Enabled = false;
            radioConC1.Location = new Point(6, 72);
            radioConC1.Name = "radioConC1";
            radioConC1.Size = new Size(39, 19);
            radioConC1.TabIndex = 2;
            radioConC1.TabStop = true;
            radioConC1.Text = "C1";
            radioConC1.UseVisualStyleBackColor = true;
            radioConC1.CheckedChanged += radioCon_CheckedChanged;
            // 
            // radioConG1
            // 
            radioConG1.AutoSize = true;
            radioConG1.Enabled = false;
            radioConG1.Location = new Point(6, 47);
            radioConG1.Name = "radioConG1";
            radioConG1.Size = new Size(39, 19);
            radioConG1.TabIndex = 1;
            radioConG1.TabStop = true;
            radioConG1.Text = "G1";
            radioConG1.UseVisualStyleBackColor = true;
            radioConG1.CheckedChanged += radioCon_CheckedChanged;
            // 
            // radioConG0
            // 
            radioConG0.AutoSize = true;
            radioConG0.Enabled = false;
            radioConG0.Location = new Point(6, 22);
            radioConG0.Name = "radioConG0";
            radioConG0.Size = new Size(39, 19);
            radioConG0.TabIndex = 0;
            radioConG0.TabStop = true;
            radioConG0.Text = "G0";
            radioConG0.UseVisualStyleBackColor = true;
            radioConG0.CheckedChanged += radioCon_CheckedChanged;
            // 
            // relations
            // 
            relations.Controls.Add(buttonLineToBezier);
            relations.Controls.Add(radioRestrConst);
            relations.Controls.Add(radioRestrVertical);
            relations.Controls.Add(radioRestrHorizontal);
            relations.Controls.Add(radioRestrNoRestr);
            relations.Location = new Point(3, 148);
            relations.Name = "relations";
            relations.Size = new Size(117, 140);
            relations.TabIndex = 5;
            relations.TabStop = false;
            relations.Text = "ograniczenia";
            // 
            // buttonLineToBezier
            // 
            buttonLineToBezier.Location = new Point(0, 113);
            buttonLineToBezier.Margin = new Padding(3, 2, 3, 2);
            buttonLineToBezier.Name = "buttonLineToBezier";
            buttonLineToBezier.Size = new Size(112, 22);
            buttonLineToBezier.TabIndex = 4;
            buttonLineToBezier.Text = "bezier";
            buttonLineToBezier.UseVisualStyleBackColor = true;
            buttonLineToBezier.Click += buttonLineToBezier_Click;
            // 
            // radioRestrConst
            // 
            radioRestrConst.AutoSize = true;
            radioRestrConst.Enabled = false;
            radioRestrConst.Location = new Point(4, 88);
            radioRestrConst.Margin = new Padding(3, 2, 3, 2);
            radioRestrConst.Name = "radioRestrConst";
            radioRestrConst.Size = new Size(65, 19);
            radioRestrConst.TabIndex = 3;
            radioRestrConst.TabStop = true;
            radioRestrConst.Text = "stała dł.";
            radioRestrConst.UseVisualStyleBackColor = true;
            radioRestrConst.CheckedChanged += radioRestr_CheckedChanged;
            // 
            // radioRestrVertical
            // 
            radioRestrVertical.AutoSize = true;
            radioRestrVertical.Enabled = false;
            radioRestrVertical.Location = new Point(4, 65);
            radioRestrVertical.Margin = new Padding(3, 2, 3, 2);
            radioRestrVertical.Name = "radioRestrVertical";
            radioRestrVertical.Size = new Size(71, 19);
            radioRestrVertical.TabIndex = 2;
            radioRestrVertical.TabStop = true;
            radioRestrVertical.Text = "pionowa";
            radioRestrVertical.UseVisualStyleBackColor = true;
            radioRestrVertical.CheckedChanged += radioRestr_CheckedChanged;
            // 
            // radioRestrHorizontal
            // 
            radioRestrHorizontal.AutoSize = true;
            radioRestrHorizontal.Enabled = false;
            radioRestrHorizontal.Location = new Point(6, 43);
            radioRestrHorizontal.Margin = new Padding(3, 2, 3, 2);
            radioRestrHorizontal.Name = "radioRestrHorizontal";
            radioRestrHorizontal.Size = new Size(71, 19);
            radioRestrHorizontal.TabIndex = 1;
            radioRestrHorizontal.TabStop = true;
            radioRestrHorizontal.Text = "pozioma";
            radioRestrHorizontal.UseVisualStyleBackColor = true;
            radioRestrHorizontal.CheckedChanged += radioRestr_CheckedChanged;
            // 
            // radioRestrNoRestr
            // 
            radioRestrNoRestr.AutoSize = true;
            radioRestrNoRestr.Enabled = false;
            radioRestrNoRestr.Location = new Point(6, 20);
            radioRestrNoRestr.Margin = new Padding(3, 2, 3, 2);
            radioRestrNoRestr.Name = "radioRestrNoRestr";
            radioRestrNoRestr.Size = new Size(80, 19);
            radioRestrNoRestr.TabIndex = 0;
            radioRestrNoRestr.TabStop = true;
            radioRestrNoRestr.Text = "bez ogran.";
            radioRestrNoRestr.UseVisualStyleBackColor = true;
            radioRestrNoRestr.CheckedChanged += radioRestr_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(buttonRemoveVertex);
            groupBox1.Controls.Add(radioAlgMy);
            groupBox1.Controls.Add(buttonAddVertex);
            groupBox1.Controls.Add(radioAlgLib);
            groupBox1.Location = new Point(3, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(117, 136);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "wierzch.";
            // 
            // buttonRemoveVertex
            // 
            buttonRemoveVertex.Enabled = false;
            buttonRemoveVertex.Location = new Point(4, 22);
            buttonRemoveVertex.Name = "buttonRemoveVertex";
            buttonRemoveVertex.Size = new Size(109, 23);
            buttonRemoveVertex.TabIndex = 0;
            buttonRemoveVertex.Text = "usuń wierzch.";
            buttonRemoveVertex.UseVisualStyleBackColor = true;
            buttonRemoveVertex.Click += buttonRemoveVertex_Click;
            // 
            // radioAlgMy
            // 
            radioAlgMy.AutoSize = true;
            radioAlgMy.Location = new Point(6, 105);
            radioAlgMy.Name = "radioAlgMy";
            radioAlgMy.Size = new Size(88, 19);
            radioAlgMy.TabIndex = 3;
            radioAlgMy.Text = "własna imp.";
            radioAlgMy.UseVisualStyleBackColor = true;
            radioAlgMy.CheckedChanged += radioAlgMy_CheckedChanged;
            // 
            // buttonAddVertex
            // 
            buttonAddVertex.Enabled = false;
            buttonAddVertex.Location = new Point(5, 51);
            buttonAddVertex.Name = "buttonAddVertex";
            buttonAddVertex.Size = new Size(109, 23);
            buttonAddVertex.TabIndex = 1;
            buttonAddVertex.Text = "dodaj wierzch.";
            buttonAddVertex.UseVisualStyleBackColor = true;
            buttonAddVertex.Click += buttonAddVertex_Click;
            // 
            // radioAlgLib
            // 
            radioAlgLib.AutoSize = true;
            radioAlgLib.Checked = true;
            radioAlgLib.Location = new Point(6, 80);
            radioAlgLib.Name = "radioAlgLib";
            radioAlgLib.Size = new Size(89, 19);
            radioAlgLib.TabIndex = 2;
            radioAlgLib.TabStop = true;
            radioAlgLib.Text = "biblioteczny";
            radioAlgLib.UseVisualStyleBackColor = true;
            radioAlgLib.CheckedChanged += radioAlgLib_CheckedChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox3);
            tabPage2.Controls.Add(buttonRemovePolygon);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(123, 391);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Wielokąty";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label1);
            groupBox3.Controls.Add(textBoxRadius);
            groupBox3.Controls.Add(buttonNewPolygon);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(textBoxVerteciesC);
            groupBox3.Location = new Point(5, 5);
            groupBox3.Margin = new Padding(3, 2, 3, 2);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 2, 3, 2);
            groupBox3.Size = new Size(114, 127);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "nowy wielokąt";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(5, 17);
            label1.Name = "label1";
            label1.Size = new Size(83, 15);
            label1.TabIndex = 4;
            label1.Text = "liczba wierzch.";
            // 
            // textBoxRadius
            // 
            textBoxRadius.Location = new Point(5, 74);
            textBoxRadius.Margin = new Padding(3, 2, 3, 2);
            textBoxRadius.Name = "textBoxRadius";
            textBoxRadius.Size = new Size(104, 23);
            textBoxRadius.TabIndex = 2;
            // 
            // buttonNewPolygon
            // 
            buttonNewPolygon.Location = new Point(4, 99);
            buttonNewPolygon.Margin = new Padding(3, 2, 3, 2);
            buttonNewPolygon.Name = "buttonNewPolygon";
            buttonNewPolygon.Size = new Size(104, 22);
            buttonNewPolygon.TabIndex = 0;
            buttonNewPolygon.Text = "generuj";
            buttonNewPolygon.UseVisualStyleBackColor = true;
            buttonNewPolygon.Click += buttonNewPolygon_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 57);
            label2.Name = "label2";
            label2.Size = new Size(52, 15);
            label2.TabIndex = 5;
            label2.Text = "promień";
            // 
            // textBoxVerteciesC
            // 
            textBoxVerteciesC.Location = new Point(6, 34);
            textBoxVerteciesC.Margin = new Padding(3, 2, 3, 2);
            textBoxVerteciesC.Name = "textBoxVerteciesC";
            textBoxVerteciesC.Size = new Size(103, 23);
            textBoxVerteciesC.TabIndex = 3;
            // 
            // buttonRemovePolygon
            // 
            buttonRemovePolygon.Location = new Point(5, 136);
            buttonRemovePolygon.Margin = new Padding(3, 2, 3, 2);
            buttonRemovePolygon.Name = "buttonRemovePolygon";
            buttonRemovePolygon.Size = new Size(112, 22);
            buttonRemovePolygon.TabIndex = 1;
            buttonRemovePolygon.Text = "usuń";
            buttonRemovePolygon.UseVisualStyleBackColor = true;
            buttonRemovePolygon.Click += buttonRemovePolygon_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(886, 450);
            Controls.Add(controlCenter);
            Controls.Add(canvas);
            Controls.Add(menuStrip1);
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            Name = "Main";
            Text = "Main";
            KeyDown += Main_KeyDown;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            controlCenter.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            relations.ResumeLayout(false);
            relations.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private PictureBox canvas;
        private TabControl controlCenter;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private RadioButton radioAlgMy;
        private RadioButton radioAlgLib;
        private Button buttonAddVertex;
        private Button buttonRemoveVertex;
        private GroupBox groupBox1;
        private GroupBox relations;
        private GroupBox groupBox2;
        private RadioButton radioConC1;
        private RadioButton radioConG1;
        private RadioButton radioConG0;
        private RadioButton radioRestrNoRestr;
        private RadioButton radioRestrVertical;
        private RadioButton radioRestrHorizontal;
        private RadioButton radioRestrConst;
        private TextBox textBoxRadius;
        private Button buttonRemovePolygon;
        private Button buttonNewPolygon;
        private Label label1;
        private TextBox textBoxVerteciesC;
        private GroupBox groupBox3;
        private Label label2;
        private Button buttonLineToBezier;
    }
}