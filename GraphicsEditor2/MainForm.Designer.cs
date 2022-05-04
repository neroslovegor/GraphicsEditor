
namespace GraphicsEditor2
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tsbUndo = new System.Windows.Forms.ToolStripButton();
            this.tsbRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCut = new System.Windows.Forms.ToolStripButton();
            this.tsbCopy = new System.Windows.Forms.ToolStripButton();
            this.tsbPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbMouse = new System.Windows.Forms.ToolStripButton();
            this.tsbLine = new System.Windows.Forms.ToolStripButton();
            this.tsbTriangle = new System.Windows.Forms.ToolStripButton();
            this.tsbRectangle = new System.Windows.Forms.ToolStripButton();
            this.tsbModes = new System.Windows.Forms.ToolStrip();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbSelectAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbBringToFront = new System.Windows.Forms.ToolStripButton();
            this.tsbSendToBack = new System.Windows.Forms.ToolStripButton();
            this.tsbTurnLeft = new System.Windows.Forms.ToolStripButton();
            this.tsbTurnRight = new System.Windows.Forms.ToolStripButton();
            this.tsbReflectVertically = new System.Windows.Forms.ToolStripButton();
            this.tsbReflectHorizontally = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAngle = new System.Windows.Forms.ToolStripTextBox();
            this.tsbCube = new System.Windows.Forms.ToolStripButton();
            this.tsbMorphing = new System.Windows.Forms.ToolStripButton();
            this.tsbT = new System.Windows.Forms.ToolStripTextBox();
            this.panelForScroll = new System.Windows.Forms.Panel();
            this.pbCanvas = new System.Windows.Forms.PictureBox();
            this.openFiguresFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFiguresFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tsbModes.SuspendLayout();
            this.panelForScroll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCanvas)).BeginInit();
            this.SuspendLayout();
            // 
            // tsbUndo
            // 
            this.tsbUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUndo.Image = ((System.Drawing.Image)(resources.GetObject("tsbUndo.Image")));
            this.tsbUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUndo.Name = "tsbUndo";
            this.tsbUndo.Size = new System.Drawing.Size(23, 22);
            this.tsbUndo.Text = "Undo";
            this.tsbUndo.Click += new System.EventHandler(this.tsbUndo_Click);
            // 
            // tsbRedo
            // 
            this.tsbRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRedo.Image = ((System.Drawing.Image)(resources.GetObject("tsbRedo.Image")));
            this.tsbRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRedo.Name = "tsbRedo";
            this.tsbRedo.Size = new System.Drawing.Size(23, 22);
            this.tsbRedo.Text = "Redo";
            this.tsbRedo.Click += new System.EventHandler(this.tsbRedo_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCut
            // 
            this.tsbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCut.Image = ((System.Drawing.Image)(resources.GetObject("tsbCut.Image")));
            this.tsbCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCut.Name = "tsbCut";
            this.tsbCut.Size = new System.Drawing.Size(23, 22);
            this.tsbCut.Text = "Cut";
            this.tsbCut.Click += new System.EventHandler(this.tsbCut_Click);
            // 
            // tsbCopy
            // 
            this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsbCopy.Image")));
            this.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.Size = new System.Drawing.Size(23, 22);
            this.tsbCopy.Text = "Copy";
            this.tsbCopy.Click += new System.EventHandler(this.tsbCopy_Click);
            // 
            // tsbPaste
            // 
            this.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPaste.Image = ((System.Drawing.Image)(resources.GetObject("tsbPaste.Image")));
            this.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPaste.Name = "tsbPaste";
            this.tsbPaste.Size = new System.Drawing.Size(23, 22);
            this.tsbPaste.Text = "Paste";
            this.tsbPaste.Click += new System.EventHandler(this.tsbPaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbMouse
            // 
            this.tsbMouse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMouse.Image = ((System.Drawing.Image)(resources.GetObject("tsbMouse.Image")));
            this.tsbMouse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMouse.Name = "tsbMouse";
            this.tsbMouse.Size = new System.Drawing.Size(23, 22);
            this.tsbMouse.Text = "Mouse";
            this.tsbMouse.Click += new System.EventHandler(this.tsbSelectMode_Click);
            // 
            // tsbLine
            // 
            this.tsbLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLine.Image = ((System.Drawing.Image)(resources.GetObject("tsbLine.Image")));
            this.tsbLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLine.Name = "tsbLine";
            this.tsbLine.Size = new System.Drawing.Size(23, 22);
            this.tsbLine.Text = "Line";
            this.tsbLine.Click += new System.EventHandler(this.tsbSelectMode_Click);
            // 
            // tsbTriangle
            // 
            this.tsbTriangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTriangle.Image = ((System.Drawing.Image)(resources.GetObject("tsbTriangle.Image")));
            this.tsbTriangle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTriangle.Name = "tsbTriangle";
            this.tsbTriangle.Size = new System.Drawing.Size(23, 22);
            this.tsbTriangle.Text = "Triangle";
            this.tsbTriangle.Click += new System.EventHandler(this.tsbSelectMode_Click);
            // 
            // tsbRectangle
            // 
            this.tsbRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRectangle.Image = ((System.Drawing.Image)(resources.GetObject("tsbRectangle.Image")));
            this.tsbRectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRectangle.Name = "tsbRectangle";
            this.tsbRectangle.Size = new System.Drawing.Size(23, 22);
            this.tsbRectangle.Text = "Rectangle";
            this.tsbRectangle.Click += new System.EventHandler(this.tsbSelectMode_Click);
            // 
            // tsbModes
            // 
            this.tsbModes.BackColor = System.Drawing.Color.AliceBlue;
            this.tsbModes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsbModes.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsbModes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpen,
            this.tsbSave,
            this.tsbUndo,
            this.tsbRedo,
            this.toolStripSeparator2,
            this.tsbCut,
            this.tsbCopy,
            this.tsbPaste,
            this.tsbSelectAll,
            this.toolStripSeparator1,
            this.tsbMouse,
            this.tsbLine,
            this.tsbTriangle,
            this.tsbRectangle,
            this.toolStripSeparator3,
            this.tsbBringToFront,
            this.tsbSendToBack,
            this.tsbTurnLeft,
            this.tsbTurnRight,
            this.tsbReflectVertically,
            this.tsbReflectHorizontally,
            this.toolStripSeparator4,
            this.tsbAngle,
            this.tsbMorphing,
            this.tsbT,
            this.tsbCube});
            this.tsbModes.Location = new System.Drawing.Point(0, 0);
            this.tsbModes.Name = "tsbModes";
            this.tsbModes.Size = new System.Drawing.Size(929, 25);
            this.tsbModes.TabIndex = 2;
            this.tsbModes.Text = "tsbModes";
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpen.Image")));
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(23, 22);
            this.tsbOpen.Text = "Open file";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbSave.Image")));
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "Save";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbSelectAll
            // 
            this.tsbSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbSelectAll.Image")));
            this.tsbSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSelectAll.Name = "tsbSelectAll";
            this.tsbSelectAll.Size = new System.Drawing.Size(23, 22);
            this.tsbSelectAll.Text = "Select all";
            this.tsbSelectAll.Click += new System.EventHandler(this.tsbSelectAll_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbBringToFront
            // 
            this.tsbBringToFront.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbBringToFront.Image = ((System.Drawing.Image)(resources.GetObject("tsbBringToFront.Image")));
            this.tsbBringToFront.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBringToFront.Name = "tsbBringToFront";
            this.tsbBringToFront.Size = new System.Drawing.Size(23, 22);
            this.tsbBringToFront.Text = "Bring to front";
            this.tsbBringToFront.Click += new System.EventHandler(this.tsbBringToFront_Click);
            // 
            // tsbSendToBack
            // 
            this.tsbSendToBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSendToBack.Image = ((System.Drawing.Image)(resources.GetObject("tsbSendToBack.Image")));
            this.tsbSendToBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSendToBack.Name = "tsbSendToBack";
            this.tsbSendToBack.Size = new System.Drawing.Size(23, 22);
            this.tsbSendToBack.Text = "Send to back";
            this.tsbSendToBack.Click += new System.EventHandler(this.tsbSendToBack_Click);
            // 
            // tsbTurnLeft
            // 
            this.tsbTurnLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTurnLeft.Image = ((System.Drawing.Image)(resources.GetObject("tsbTurnLeft.Image")));
            this.tsbTurnLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTurnLeft.Name = "tsbTurnLeft";
            this.tsbTurnLeft.Size = new System.Drawing.Size(23, 22);
            this.tsbTurnLeft.Text = "Turn left";
            this.tsbTurnLeft.Click += new System.EventHandler(this.tsbTurnLeft_Click);
            // 
            // tsbTurnRight
            // 
            this.tsbTurnRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTurnRight.Image = ((System.Drawing.Image)(resources.GetObject("tsbTurnRight.Image")));
            this.tsbTurnRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTurnRight.Name = "tsbTurnRight";
            this.tsbTurnRight.Size = new System.Drawing.Size(23, 22);
            this.tsbTurnRight.Text = "Turn right";
            this.tsbTurnRight.Click += new System.EventHandler(this.tsbTurnRight_Click);
            // 
            // tsbReflectVertically
            // 
            this.tsbReflectVertically.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbReflectVertically.Image = ((System.Drawing.Image)(resources.GetObject("tsbReflectVertically.Image")));
            this.tsbReflectVertically.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReflectVertically.Name = "tsbReflectVertically";
            this.tsbReflectVertically.Size = new System.Drawing.Size(23, 22);
            this.tsbReflectVertically.Text = "Reflect vertically";
            this.tsbReflectVertically.Click += new System.EventHandler(this.tsbReflectVertically_Click);
            // 
            // tsbReflectHorizontally
            // 
            this.tsbReflectHorizontally.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbReflectHorizontally.Image = ((System.Drawing.Image)(resources.GetObject("tsbReflectHorizontally.Image")));
            this.tsbReflectHorizontally.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReflectHorizontally.Name = "tsbReflectHorizontally";
            this.tsbReflectHorizontally.Size = new System.Drawing.Size(23, 22);
            this.tsbReflectHorizontally.Text = "Reflect horizontally";
            this.tsbReflectHorizontally.Click += new System.EventHandler(this.tsbReflectHorizontally_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbAngle
            // 
            this.tsbAngle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tsbAngle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsbAngle.Name = "tsbAngle";
            this.tsbAngle.Size = new System.Drawing.Size(60, 25);
            this.tsbAngle.Text = "90";
            // 
            // tsbCube
            // 
            this.tsbCube.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCube.Image = ((System.Drawing.Image)(resources.GetObject("tsbCube.Image")));
            this.tsbCube.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCube.Name = "tsbCube";
            this.tsbCube.Size = new System.Drawing.Size(23, 22);
            this.tsbCube.Text = "Cube";
            this.tsbCube.Click += new System.EventHandler(this.tsbCube_Click);
            // 
            // tsbMorphing
            // 
            this.tsbMorphing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMorphing.Image = ((System.Drawing.Image)(resources.GetObject("tsbMorphing.Image")));
            this.tsbMorphing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMorphing.Name = "tsbMorphing";
            this.tsbMorphing.Size = new System.Drawing.Size(23, 22);
            this.tsbMorphing.Text = "Morphing";
            this.tsbMorphing.Click += new System.EventHandler(this.tsbMorphing_Click);
            // 
            // tsbT
            // 
            this.tsbT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tsbT.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsbT.Name = "tsbT";
            this.tsbT.Size = new System.Drawing.Size(60, 25);
            this.tsbT.Text = "0,5";
            // 
            // panelForScroll
            // 
            this.panelForScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelForScroll.AutoScroll = true;
            this.panelForScroll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelForScroll.Controls.Add(this.pbCanvas);
            this.panelForScroll.Location = new System.Drawing.Point(1, 23);
            this.panelForScroll.Name = "panelForScroll";
            this.panelForScroll.Size = new System.Drawing.Size(927, 540);
            this.panelForScroll.TabIndex = 5;
            // 
            // pbCanvas
            // 
            this.pbCanvas.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbCanvas.BackgroundImage")));
            this.pbCanvas.Location = new System.Drawing.Point(-1, -1);
            this.pbCanvas.Name = "pbCanvas";
            this.pbCanvas.Size = new System.Drawing.Size(618, 481);
            this.pbCanvas.TabIndex = 0;
            this.pbCanvas.TabStop = false;
            // 
            // openFiguresFileDialog
            // 
            this.openFiguresFileDialog.DefaultExt = "gr";
            this.openFiguresFileDialog.Filter = "*.gr|*.gr";
            // 
            // saveFiguresFileDialog
            // 
            this.saveFiguresFileDialog.DefaultExt = "gr";
            this.saveFiguresFileDialog.Filter = "*.gr|*.gr";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(929, 564);
            this.Controls.Add(this.panelForScroll);
            this.Controls.Add(this.tsbModes);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Graphics Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tsbModes.ResumeLayout(false);
            this.tsbModes.PerformLayout();
            this.panelForScroll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCanvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton tsbUndo;
        private System.Windows.Forms.ToolStripButton tsbRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbCut;
        private System.Windows.Forms.ToolStripButton tsbCopy;
        private System.Windows.Forms.ToolStripButton tsbPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbMouse;
        private System.Windows.Forms.ToolStripButton tsbLine;
        private System.Windows.Forms.ToolStripButton tsbTriangle;
        private System.Windows.Forms.ToolStripButton tsbRectangle;
        private System.Windows.Forms.ToolStrip tsbModes;
        private System.Windows.Forms.Panel panelForScroll;
        private System.Windows.Forms.PictureBox pbCanvas;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbSendToBack;
        private System.Windows.Forms.ToolStripButton tsbBringToFront;
        private System.Windows.Forms.ToolStripButton tsbTurnRight;
        private System.Windows.Forms.ToolStripButton tsbTurnLeft;
        private System.Windows.Forms.ToolStripButton tsbReflectVertically;
        private System.Windows.Forms.ToolStripButton tsbReflectHorizontally;
        private System.Windows.Forms.ToolStripButton tsbSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripTextBox tsbT;
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.OpenFileDialog openFiguresFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFiguresFileDialog;
        private System.Windows.Forms.ToolStripButton tsbCube;
        private System.Windows.Forms.ToolStripTextBox tsbAngle;
        private System.Windows.Forms.ToolStripButton tsbMorphing;
    }
}

