namespace GrafikaKomputerowaDrawer
{
    partial class GKDrawer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            splitContainer1 = new SplitContainer();
            Canvas = new Display();
            flowLayoutPanel1 = new FlowLayoutPanel();
            PolygonButton = new RadioButton();
            LineButton = new RadioButton();
            PointButton = new RadioButton();
            SelectButton = new RadioButton();
            SnapBox = new CheckBox();
            LockBox = new CheckBox();
            DrawingAlgorithmBox = new GroupBox();
            BresButton = new RadioButton();
            DefButton = new RadioButton();
            ConstraintBox = new GroupBox();
            RemoveConstraintButton = new Button();
            HorizontalConst1 = new RadioButton();
            VerticalConst1 = new RadioButton();
            ConstraintButton = new Button();
            OffsetPolyBox = new GroupBox();
            OffsetInput = new NumericUpDown();
            menuStrip1 = new MenuStrip();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            constrintsOnSegmentToolStripMenuItem = new ToolStripMenuItem();
            xMLSerializationToolStripMenuItem = new ToolStripMenuItem();
            serializeToolStripMenuItem1 = new ToolStripMenuItem();
            deserializeToolStripMenuItem1 = new ToolStripMenuItem();
            clearDrawerToolStripMenuItem = new ToolStripMenuItem();
            drawOffsetPolygonToolStripMenuItem = new ToolStripMenuItem();
            ConstraintErrorProvider = new ErrorProvider(components);
            OffsetErrorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            DrawingAlgorithmBox.SuspendLayout();
            ConstraintBox.SuspendLayout();
            OffsetPolyBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)OffsetInput).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ConstraintErrorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)OffsetErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(Canvas);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.AutoScroll = true;
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
            splitContainer1.Panel2.Controls.Add(menuStrip1);
            splitContainer1.Size = new Size(800, 485);
            splitContainer1.SplitterDistance = 610;
            splitContainer1.TabIndex = 0;
            // 
            // Canvas
            // 
            Canvas.Dock = DockStyle.Fill;
            Canvas.Location = new Point(0, 0);
            Canvas.Name = "Canvas";
            Canvas.Size = new Size(610, 485);
            Canvas.TabIndex = 0;
            Canvas.Click += Canvas_Click;
            Canvas.Paint += Canvas_Paint;
            Canvas.MouseDoubleClick += Canvas_MouseDoubleClick;
            Canvas.MouseDown += Canvas_MouseDown;
            Canvas.MouseMove += Canvas_MouseMove;
            Canvas.MouseUp += Canvas_MouseUp;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(PolygonButton);
            flowLayoutPanel1.Controls.Add(LineButton);
            flowLayoutPanel1.Controls.Add(PointButton);
            flowLayoutPanel1.Controls.Add(SelectButton);
            flowLayoutPanel1.Controls.Add(SnapBox);
            flowLayoutPanel1.Controls.Add(LockBox);
            flowLayoutPanel1.Controls.Add(DrawingAlgorithmBox);
            flowLayoutPanel1.Controls.Add(ConstraintBox);
            flowLayoutPanel1.Controls.Add(OffsetPolyBox);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 28);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(163, 593);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // PolygonButton
            // 
            PolygonButton.AutoSize = true;
            PolygonButton.Checked = true;
            PolygonButton.Location = new Point(3, 3);
            PolygonButton.Name = "PolygonButton";
            PolygonButton.Size = new Size(83, 24);
            PolygonButton.TabIndex = 1001;
            PolygonButton.TabStop = true;
            PolygonButton.Text = "Polygon";
            PolygonButton.UseVisualStyleBackColor = true;
            PolygonButton.CheckedChanged += PolygonButton_CheckedChanged;
            // 
            // LineButton
            // 
            LineButton.AutoSize = true;
            LineButton.Location = new Point(3, 33);
            LineButton.Name = "LineButton";
            LineButton.Size = new Size(57, 24);
            LineButton.TabIndex = 1002;
            LineButton.Text = "Line";
            LineButton.UseVisualStyleBackColor = true;
            LineButton.CheckedChanged += LineButton_CheckedChanged;
            // 
            // PointButton
            // 
            PointButton.AutoSize = true;
            PointButton.Location = new Point(3, 63);
            PointButton.Name = "PointButton";
            PointButton.Size = new Size(63, 24);
            PointButton.TabIndex = 1003;
            PointButton.Text = "Point";
            PointButton.UseVisualStyleBackColor = true;
            PointButton.CheckedChanged += PointButton_CheckedChanged;
            // 
            // SelectButton
            // 
            SelectButton.AutoSize = true;
            SelectButton.Location = new Point(3, 93);
            SelectButton.Name = "SelectButton";
            SelectButton.Size = new Size(70, 24);
            SelectButton.TabIndex = 1004;
            SelectButton.Text = "Select";
            SelectButton.UseVisualStyleBackColor = true;
            SelectButton.CheckedChanged += SelectButton_CheckedChanged;
            // 
            // SnapBox
            // 
            SnapBox.AutoSize = true;
            SnapBox.Checked = true;
            SnapBox.CheckState = CheckState.Checked;
            SnapBox.Location = new Point(3, 123);
            SnapBox.Name = "SnapBox";
            SnapBox.Size = new Size(121, 24);
            SnapBox.TabIndex = 4;
            SnapBox.Text = "Snap to point";
            SnapBox.UseVisualStyleBackColor = true;
            // 
            // LockBox
            // 
            LockBox.AutoSize = true;
            LockBox.Checked = true;
            LockBox.CheckState = CheckState.Checked;
            LockBox.Location = new Point(3, 153);
            LockBox.Name = "LockBox";
            LockBox.Size = new Size(113, 24);
            LockBox.TabIndex = 6;
            LockBox.Text = "Lock objects";
            LockBox.UseVisualStyleBackColor = true;
            // 
            // DrawingAlgorithmBox
            // 
            DrawingAlgorithmBox.CausesValidation = false;
            DrawingAlgorithmBox.Controls.Add(BresButton);
            DrawingAlgorithmBox.Controls.Add(DefButton);
            DrawingAlgorithmBox.Location = new Point(3, 183);
            DrawingAlgorithmBox.Name = "DrawingAlgorithmBox";
            DrawingAlgorithmBox.Size = new Size(148, 86);
            DrawingAlgorithmBox.TabIndex = 1000;
            DrawingAlgorithmBox.TabStop = false;
            DrawingAlgorithmBox.Text = "Drawing";
            // 
            // BresButton
            // 
            BresButton.AutoSize = true;
            BresButton.Location = new Point(14, 53);
            BresButton.Name = "BresButton";
            BresButton.Size = new Size(112, 24);
            BresButton.TabIndex = 1;
            BresButton.Text = "Bresenham's";
            BresButton.UseVisualStyleBackColor = true;
            BresButton.CheckedChanged += BresButton_CheckedChanged;
            // 
            // DefButton
            // 
            DefButton.AutoSize = true;
            DefButton.Checked = true;
            DefButton.Location = new Point(14, 26);
            DefButton.Name = "DefButton";
            DefButton.Size = new Size(79, 24);
            DefButton.TabIndex = 0;
            DefButton.TabStop = true;
            DefButton.Text = "Default";
            DefButton.UseVisualStyleBackColor = true;
            DefButton.CheckedChanged += DefButton_CheckedChanged;
            // 
            // ConstraintBox
            // 
            ConstraintBox.Controls.Add(RemoveConstraintButton);
            ConstraintBox.Controls.Add(HorizontalConst1);
            ConstraintBox.Controls.Add(VerticalConst1);
            ConstraintBox.Controls.Add(ConstraintButton);
            ConstraintBox.Location = new Point(3, 275);
            ConstraintBox.Name = "ConstraintBox";
            ConstraintBox.Size = new Size(157, 146);
            ConstraintBox.TabIndex = 0;
            ConstraintBox.TabStop = false;
            ConstraintBox.Text = "Constraint";
            // 
            // RemoveConstraintButton
            // 
            RemoveConstraintButton.Location = new Point(12, 117);
            RemoveConstraintButton.Name = "RemoveConstraintButton";
            RemoveConstraintButton.Size = new Size(114, 29);
            RemoveConstraintButton.TabIndex = 11;
            RemoveConstraintButton.Text = "Remove";
            RemoveConstraintButton.UseVisualStyleBackColor = true;
            RemoveConstraintButton.Click += RemoveConstraintButton_Click;
            // 
            // HorizontalConst1
            // 
            HorizontalConst1.AutoSize = true;
            HorizontalConst1.Location = new Point(12, 56);
            HorizontalConst1.Name = "HorizontalConst1";
            HorizontalConst1.Size = new Size(100, 24);
            HorizontalConst1.TabIndex = 10;
            HorizontalConst1.Text = "Horizontal";
            HorizontalConst1.UseVisualStyleBackColor = true;
            // 
            // VerticalConst1
            // 
            VerticalConst1.AutoSize = true;
            VerticalConst1.Checked = true;
            VerticalConst1.Location = new Point(12, 26);
            VerticalConst1.Name = "VerticalConst1";
            VerticalConst1.Size = new Size(79, 24);
            VerticalConst1.TabIndex = 9;
            VerticalConst1.TabStop = true;
            VerticalConst1.Text = "Vertical";
            VerticalConst1.UseVisualStyleBackColor = true;
            // 
            // ConstraintButton
            // 
            ConstraintButton.Location = new Point(12, 86);
            ConstraintButton.Name = "ConstraintButton";
            ConstraintButton.Size = new Size(114, 29);
            ConstraintButton.TabIndex = 8;
            ConstraintButton.Text = "Add constraint";
            ConstraintButton.UseVisualStyleBackColor = true;
            ConstraintButton.Click += ConstraintButton_Click;
            // 
            // OffsetPolyBox
            // 
            OffsetPolyBox.Controls.Add(OffsetInput);
            OffsetPolyBox.Location = new Point(3, 427);
            OffsetPolyBox.Name = "OffsetPolyBox";
            OffsetPolyBox.Size = new Size(148, 62);
            OffsetPolyBox.TabIndex = 1005;
            OffsetPolyBox.TabStop = false;
            OffsetPolyBox.Text = "OffsetPolygon";
            OffsetPolyBox.Visible = false;
            // 
            // OffsetInput
            // 
            OffsetInput.Location = new Point(6, 26);
            OffsetInput.Maximum = new decimal(new int[] { 150, 0, 0, 0 });
            OffsetInput.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            OffsetInput.Name = "OffsetInput";
            OffsetInput.Size = new Size(150, 27);
            OffsetInput.TabIndex = 0;
            OffsetInput.Value = new decimal(new int[] { 10, 0, 0, 0 });
            OffsetInput.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(165, 28);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { constrintsOnSegmentToolStripMenuItem, xMLSerializationToolStripMenuItem, clearDrawerToolStripMenuItem, drawOffsetPolygonToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(75, 24);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // constrintsOnSegmentToolStripMenuItem
            // 
            constrintsOnSegmentToolStripMenuItem.Checked = true;
            constrintsOnSegmentToolStripMenuItem.CheckOnClick = true;
            constrintsOnSegmentToolStripMenuItem.CheckState = CheckState.Checked;
            constrintsOnSegmentToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            constrintsOnSegmentToolStripMenuItem.Name = "constrintsOnSegmentToolStripMenuItem";
            constrintsOnSegmentToolStripMenuItem.Size = new Size(239, 26);
            constrintsOnSegmentToolStripMenuItem.Text = "Constrints on segment";
            constrintsOnSegmentToolStripMenuItem.CheckedChanged += constrintsOnSegmentToolStripMenuItem_CheckedChanged;
            // 
            // xMLSerializationToolStripMenuItem
            // 
            xMLSerializationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { serializeToolStripMenuItem1, deserializeToolStripMenuItem1 });
            xMLSerializationToolStripMenuItem.Name = "xMLSerializationToolStripMenuItem";
            xMLSerializationToolStripMenuItem.Size = new Size(239, 26);
            xMLSerializationToolStripMenuItem.Text = "XML Serialization";
            // 
            // serializeToolStripMenuItem1
            // 
            serializeToolStripMenuItem1.Name = "serializeToolStripMenuItem1";
            serializeToolStripMenuItem1.Size = new Size(165, 26);
            serializeToolStripMenuItem1.Text = "Serialize";
            serializeToolStripMenuItem1.Click += serializeToolStripMenuItem_Click;
            // 
            // deserializeToolStripMenuItem1
            // 
            deserializeToolStripMenuItem1.Name = "deserializeToolStripMenuItem1";
            deserializeToolStripMenuItem1.Size = new Size(165, 26);
            deserializeToolStripMenuItem1.Text = "Deserialize";
            deserializeToolStripMenuItem1.Click += deserializeToolStripMenuItem_Click;
            // 
            // clearDrawerToolStripMenuItem
            // 
            clearDrawerToolStripMenuItem.Name = "clearDrawerToolStripMenuItem";
            clearDrawerToolStripMenuItem.Size = new Size(239, 26);
            clearDrawerToolStripMenuItem.Text = "Clear drawer";
            clearDrawerToolStripMenuItem.Click += clearDrawerToolStripMenuItem_Click;
            // 
            // drawOffsetPolygonToolStripMenuItem
            // 
            drawOffsetPolygonToolStripMenuItem.CheckOnClick = true;
            drawOffsetPolygonToolStripMenuItem.Name = "drawOffsetPolygonToolStripMenuItem";
            drawOffsetPolygonToolStripMenuItem.Size = new Size(239, 26);
            drawOffsetPolygonToolStripMenuItem.Text = "Draw offset polygon";
            drawOffsetPolygonToolStripMenuItem.CheckedChanged += drawOffsetPolygonToolStripMenuItem_CheckedChanged;
            // 
            // ConstraintErrorProvider
            // 
            ConstraintErrorProvider.ContainerControl = this;
            // 
            // OffsetErrorProvider
            // 
            OffsetErrorProvider.ContainerControl = this;
            // 
            // GKDrawer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 485);
            Controls.Add(splitContainer1);
            DoubleBuffered = true;
            KeyPreview = true;
            Name = "GKDrawer";
            Text = "GKDrawer";
            KeyDown += GKDrawer_KeyDown;
            KeyUp += GKDrawer_KeyUp;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            DrawingAlgorithmBox.ResumeLayout(false);
            DrawingAlgorithmBox.PerformLayout();
            ConstraintBox.ResumeLayout(false);
            ConstraintBox.PerformLayout();
            OffsetPolyBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)OffsetInput).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ConstraintErrorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)OffsetErrorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Display Canvas;
        private CheckBox SnapBox;
        private CheckBox LockBox;
        private Button ConstraintButton;
        private GroupBox ConstraintBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem constrintsOnSegmentToolStripMenuItem;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox DrawingAlgorithmBox;
        private RadioButton BresButton;
        private RadioButton DefButton;
        private RadioButton HorizontalConst1;
        private RadioButton VerticalConst1;
        private ErrorProvider ConstraintErrorProvider;
        private RadioButton PolygonButton;
        private RadioButton LineButton;
        private RadioButton PointButton;
        private RadioButton SelectButton;
        private Button RemoveConstraintButton;
        private ToolStripMenuItem xMLSerializationToolStripMenuItem;
        private ToolStripMenuItem serializeToolStripMenuItem1;
        private ToolStripMenuItem deserializeToolStripMenuItem1;
        private ToolStripMenuItem clearDrawerToolStripMenuItem;
        private GroupBox OffsetPolyBox;
        private ToolStripMenuItem drawOffsetPolygonToolStripMenuItem;
        private ErrorProvider OffsetErrorProvider;
        private NumericUpDown OffsetInput;
    }
}