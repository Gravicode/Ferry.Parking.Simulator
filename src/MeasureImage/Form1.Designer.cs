namespace MeasureImage
{
    partial class Form1
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
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            BtnCalculate = new Button();
            BtnOpen = new Button();
            BtnCalculateYolo = new Button();
            TxtLogs = new RichTextBox();
            BtnClear = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(320, 240);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            pictureBox2.Location = new Point(346, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(320, 240);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // BtnCalculate
            // 
            BtnCalculate.Location = new Point(139, 287);
            BtnCalculate.Name = "BtnCalculate";
            BtnCalculate.Size = new Size(129, 23);
            BtnCalculate.TabIndex = 2;
            BtnCalculate.Text = "&Calculate with Edge Detect";
            BtnCalculate.UseVisualStyleBackColor = true;
            // 
            // BtnOpen
            // 
            BtnOpen.Location = new Point(12, 258);
            BtnOpen.Name = "BtnOpen";
            BtnOpen.Size = new Size(75, 23);
            BtnOpen.TabIndex = 3;
            BtnOpen.Text = "&Open";
            BtnOpen.UseVisualStyleBackColor = true;
            // 
            // BtnCalculateYolo
            // 
            BtnCalculateYolo.Location = new Point(12, 287);
            BtnCalculateYolo.Name = "BtnCalculateYolo";
            BtnCalculateYolo.Size = new Size(121, 23);
            BtnCalculateYolo.TabIndex = 4;
            BtnCalculateYolo.Text = "&Calculate with Yolo";
            BtnCalculateYolo.UseVisualStyleBackColor = true;
            // 
            // TxtLogs
            // 
            TxtLogs.Location = new Point(12, 316);
            TxtLogs.Name = "TxtLogs";
            TxtLogs.Size = new Size(654, 69);
            TxtLogs.TabIndex = 5;
            TxtLogs.Text = "";
            // 
            // BtnClear
            // 
            BtnClear.Location = new Point(93, 258);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new Size(75, 23);
            BtnClear.TabIndex = 6;
            BtnClear.Text = "&Clear Logs";
            BtnClear.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(674, 393);
            Controls.Add(BtnClear);
            Controls.Add(TxtLogs);
            Controls.Add(BtnCalculateYolo);
            Controls.Add(BtnOpen);
            Controls.Add(BtnCalculate);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Calculate Car Width and Height";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button BtnCalculate;
        private Button BtnOpen;
        private Button BtnCalculateYolo;
        private RichTextBox TxtLogs;
        private Button BtnClear;
    }
}
