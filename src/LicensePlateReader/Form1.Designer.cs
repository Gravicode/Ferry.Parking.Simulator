namespace LicensePlateReader
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
            label1 = new Label();
            TxtPlate = new TextBox();
            BtnRead = new Button();
            BtnLoad = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(640, 480);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(658, 12);
            label1.Name = "label1";
            label1.Size = new Size(122, 15);
            label1.TabIndex = 1;
            label1.Text = "License Plate Number";
            // 
            // TxtPlate
            // 
            TxtPlate.Location = new Point(658, 30);
            TxtPlate.Name = "TxtPlate";
            TxtPlate.Size = new Size(192, 23);
            TxtPlate.TabIndex = 2;
            // 
            // BtnRead
            // 
            BtnRead.Location = new Point(739, 59);
            BtnRead.Name = "BtnRead";
            BtnRead.Size = new Size(75, 23);
            BtnRead.TabIndex = 3;
            BtnRead.Text = "&Read";
            BtnRead.UseVisualStyleBackColor = true;
            // 
            // BtnLoad
            // 
            BtnLoad.Location = new Point(658, 59);
            BtnLoad.Name = "BtnLoad";
            BtnLoad.Size = new Size(75, 23);
            BtnLoad.TabIndex = 4;
            BtnLoad.Text = "&Load";
            BtnLoad.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(862, 504);
            Controls.Add(BtnLoad);
            Controls.Add(BtnRead);
            Controls.Add(TxtPlate);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "License Plate Reader";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private TextBox TxtPlate;
        private Button BtnRead;
        private Button BtnLoad;
    }
}
