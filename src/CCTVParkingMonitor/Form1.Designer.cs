namespace CCTVParkingMonitor
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
            groupBox1 = new GroupBox();
            BtnAdd = new Button();
            BtnOpen = new Button();
            BtnCheck = new Button();
            BtnDelete = new Button();
            GridView1 = new DataGridView();
            ChkCreateParkingSpot = new CheckBox();
            BtnClear = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridView1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(776, 417);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(BtnClear);
            groupBox1.Controls.Add(BtnAdd);
            groupBox1.Controls.Add(BtnOpen);
            groupBox1.Controls.Add(BtnCheck);
            groupBox1.Controls.Add(BtnDelete);
            groupBox1.Controls.Add(GridView1);
            groupBox1.Controls.Add(ChkCreateParkingSpot);
            groupBox1.Location = new Point(12, 435);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(776, 247);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Parking Monitoring";
            // 
            // BtnAdd
            // 
            BtnAdd.Location = new Point(469, 105);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(130, 23);
            BtnAdd.TabIndex = 5;
            BtnAdd.Text = "&Add Parking Spot";
            BtnAdd.UseVisualStyleBackColor = true;
            // 
            // BtnOpen
            // 
            BtnOpen.Location = new Point(469, 47);
            BtnOpen.Name = "BtnOpen";
            BtnOpen.Size = new Size(130, 23);
            BtnOpen.TabIndex = 4;
            BtnOpen.Text = "&Open Parking Image";
            BtnOpen.UseVisualStyleBackColor = true;
            // 
            // BtnCheck
            // 
            BtnCheck.Location = new Point(469, 76);
            BtnCheck.Name = "BtnCheck";
            BtnCheck.Size = new Size(130, 23);
            BtnCheck.TabIndex = 3;
            BtnCheck.Text = "&Check Parking Spots";
            BtnCheck.UseVisualStyleBackColor = true;
            // 
            // BtnDelete
            // 
            BtnDelete.Location = new Point(6, 212);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(130, 23);
            BtnDelete.TabIndex = 2;
            BtnDelete.Text = "&Delete Spot";
            BtnDelete.UseVisualStyleBackColor = true;
            // 
            // GridView1
            // 
            GridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridView1.Location = new Point(6, 22);
            GridView1.Name = "GridView1";
            GridView1.Size = new Size(457, 184);
            GridView1.TabIndex = 1;
            // 
            // ChkCreateParkingSpot
            // 
            ChkCreateParkingSpot.AutoSize = true;
            ChkCreateParkingSpot.Location = new Point(469, 22);
            ChkCreateParkingSpot.Name = "ChkCreateParkingSpot";
            ChkCreateParkingSpot.Size = new Size(130, 19);
            ChkCreateParkingSpot.TabIndex = 0;
            ChkCreateParkingSpot.Text = "Create Parking Spot";
            ChkCreateParkingSpot.UseVisualStyleBackColor = true;
            // 
            // BtnClear
            // 
            BtnClear.Location = new Point(469, 134);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new Size(130, 23);
            BtnClear.TabIndex = 6;
            BtnClear.Text = "C&lear All Spot";
            BtnClear.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 694);
            Controls.Add(groupBox1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Check Parking Spots";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private GroupBox groupBox1;
        private Button BtnDelete;
        private DataGridView GridView1;
        private CheckBox ChkCreateParkingSpot;
        private Button BtnOpen;
        private Button BtnCheck;
        private Button BtnAdd;
        private Button BtnClear;
    }
}
