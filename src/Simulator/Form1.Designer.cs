namespace Simulator
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
            pictureBox = new PictureBox();
            groupBox1 = new GroupBox();
            BtnReset = new Button();
            BtnStop = new Button();
            BtnStart = new Button();
            TxtMaxLebar = new TextBox();
            label11 = new Label();
            TxtMinLebar = new TextBox();
            label12 = new Label();
            TxtMaxPanjang = new TextBox();
            label9 = new Label();
            TxtMinPanjang = new TextBox();
            label10 = new Label();
            TxtMaxBerat = new TextBox();
            label8 = new Label();
            TxtMinBerat = new TextBox();
            label7 = new Label();
            TxtJmlKendaraan = new TextBox();
            label6 = new Label();
            TxtJarakKendaraan = new TextBox();
            label5 = new Label();
            TxtBeratLane = new TextBox();
            label4 = new Label();
            TxtPanjangLane = new TextBox();
            label3 = new Label();
            TxtLebarLane = new TextBox();
            label2 = new Label();
            TxtJmlLane = new TextBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            TxtLogs = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Location = new Point(12, 12);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(800, 480);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(BtnReset);
            groupBox1.Controls.Add(BtnStop);
            groupBox1.Controls.Add(BtnStart);
            groupBox1.Controls.Add(TxtMaxLebar);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(TxtMinLebar);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(TxtMaxPanjang);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(TxtMinPanjang);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(TxtMaxBerat);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(TxtMinBerat);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(TxtJmlKendaraan);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(TxtJarakKendaraan);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(TxtBeratLane);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(TxtPanjangLane);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(TxtLebarLane);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(TxtJmlLane);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 498);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(800, 166);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Simulator";
            // 
            // BtnReset
            // 
            BtnReset.Location = new Point(687, 132);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(75, 23);
            BtnReset.TabIndex = 26;
            BtnReset.Text = "&Reset";
            BtnReset.UseVisualStyleBackColor = true;
            // 
            // BtnStop
            // 
            BtnStop.Location = new Point(606, 133);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new Size(75, 23);
            BtnStop.TabIndex = 25;
            BtnStop.Text = "S&top";
            BtnStop.UseVisualStyleBackColor = true;
            // 
            // BtnStart
            // 
            BtnStart.Location = new Point(525, 132);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(75, 23);
            BtnStart.TabIndex = 24;
            BtnStart.Text = "&Start";
            BtnStart.UseVisualStyleBackColor = true;
            // 
            // TxtMaxLebar
            // 
            TxtMaxLebar.Location = new Point(649, 45);
            TxtMaxLebar.Name = "TxtMaxLebar";
            TxtMaxLebar.Size = new Size(100, 23);
            TxtMaxLebar.TabIndex = 23;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(525, 48);
            label11.Name = "label11";
            label11.Size = new Size(84, 15);
            label11.TabIndex = 22;
            label11.Text = "Max Lebar (m)";
            // 
            // TxtMinLebar
            // 
            TxtMinLebar.Location = new Point(649, 16);
            TxtMinLebar.Name = "TxtMinLebar";
            TxtMinLebar.Size = new Size(100, 23);
            TxtMinLebar.TabIndex = 21;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(525, 19);
            label12.Name = "label12";
            label12.Size = new Size(82, 15);
            label12.TabIndex = 20;
            label12.Text = "Min Lebar (m)";
            // 
            // TxtMaxPanjang
            // 
            TxtMaxPanjang.Location = new Point(415, 133);
            TxtMaxPanjang.Name = "TxtMaxPanjang";
            TxtMaxPanjang.Size = new Size(100, 23);
            TxtMaxPanjang.TabIndex = 19;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(291, 136);
            label9.Name = "label9";
            label9.Size = new Size(98, 15);
            label9.TabIndex = 18;
            label9.Text = "Max Panjang (m)";
            // 
            // TxtMinPanjang
            // 
            TxtMinPanjang.Location = new Point(415, 104);
            TxtMinPanjang.Name = "TxtMinPanjang";
            TxtMinPanjang.Size = new Size(100, 23);
            TxtMinPanjang.TabIndex = 17;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(291, 107);
            label10.Name = "label10";
            label10.Size = new Size(96, 15);
            label10.TabIndex = 16;
            label10.Text = "Min Panjang (m)";
            // 
            // TxtMaxBerat
            // 
            TxtMaxBerat.Location = new Point(415, 74);
            TxtMaxBerat.Name = "TxtMaxBerat";
            TxtMaxBerat.Size = new Size(100, 23);
            TxtMaxBerat.TabIndex = 15;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(291, 77);
            label8.Name = "label8";
            label8.Size = new Size(84, 15);
            label8.TabIndex = 14;
            label8.Text = "Max Berat (kg)";
            // 
            // TxtMinBerat
            // 
            TxtMinBerat.Location = new Point(415, 45);
            TxtMinBerat.Name = "TxtMinBerat";
            TxtMinBerat.Size = new Size(100, 23);
            TxtMinBerat.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(291, 48);
            label7.Name = "label7";
            label7.Size = new Size(82, 15);
            label7.TabIndex = 12;
            label7.Text = "Min Berat (kg)";
            // 
            // TxtJmlKendaraan
            // 
            TxtJmlKendaraan.Location = new Point(415, 16);
            TxtJmlKendaraan.Name = "TxtJmlKendaraan";
            TxtJmlKendaraan.Size = new Size(100, 23);
            TxtJmlKendaraan.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(291, 19);
            label6.Name = "label6";
            label6.Size = new Size(104, 15);
            label6.TabIndex = 10;
            label6.Text = "Jumlah Kendaraan";
            // 
            // TxtJarakKendaraan
            // 
            TxtJarakKendaraan.Location = new Point(183, 133);
            TxtJarakKendaraan.Name = "TxtJarakKendaraan";
            TxtJarakKendaraan.Size = new Size(100, 23);
            TxtJarakKendaraan.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 136);
            label5.Name = "label5";
            label5.Size = new Size(170, 15);
            label5.TabIndex = 8;
            label5.Text = "Jarak Min Antar Kendaraan (m)";
            // 
            // TxtBeratLane
            // 
            TxtBeratLane.Location = new Point(183, 103);
            TxtBeratLane.Name = "TxtBeratLane";
            TxtBeratLane.Size = new Size(100, 23);
            TxtBeratLane.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 106);
            label4.Name = "label4";
            label4.Size = new Size(112, 15);
            label4.TabIndex = 6;
            label4.Text = "Berat Max Lane (kg)";
            // 
            // TxtPanjangLane
            // 
            TxtPanjangLane.Location = new Point(183, 74);
            TxtPanjangLane.Name = "TxtPanjangLane";
            TxtPanjangLane.Size = new Size(100, 23);
            TxtPanjangLane.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 77);
            label3.Name = "label3";
            label3.Size = new Size(100, 15);
            label3.TabIndex = 4;
            label3.Text = "Panjang Lane (m)";
            // 
            // TxtLebarLane
            // 
            TxtLebarLane.Location = new Point(183, 45);
            TxtLebarLane.Name = "TxtLebarLane";
            TxtLebarLane.Size = new Size(100, 23);
            TxtLebarLane.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 48);
            label2.Name = "label2";
            label2.Size = new Size(86, 15);
            label2.TabIndex = 2;
            label2.Text = "Lebar Lane (m)";
            // 
            // TxtJmlLane
            // 
            TxtJmlLane.Location = new Point(183, 16);
            TxtJmlLane.Name = "TxtJmlLane";
            TxtJmlLane.Size = new Size(100, 23);
            TxtJmlLane.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 0;
            label1.Text = "Jumlah Lane";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(TxtLogs);
            groupBox2.Location = new Point(12, 670);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(801, 100);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Logs";
            // 
            // TxtLogs
            // 
            TxtLogs.Location = new Point(6, 22);
            TxtLogs.Name = "TxtLogs";
            TxtLogs.Size = new Size(789, 67);
            TxtLogs.TabIndex = 0;
            TxtLogs.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(825, 779);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(pictureBox);
            Name = "Form1";
            Text = "Simulator Ferry Car Park";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox;
        private GroupBox groupBox1;
        private Label label1;
        private TextBox TxtBeratLane;
        private Label label4;
        private TextBox TxtPanjangLane;
        private Label label3;
        private TextBox TxtLebarLane;
        private Label label2;
        private TextBox TxtJmlLane;
        private TextBox TxtJarakKendaraan;
        private Label label5;
        private GroupBox groupBox2;
        private RichTextBox TxtLogs;
        private TextBox TxtMaxPanjang;
        private Label label9;
        private TextBox TxtMinPanjang;
        private Label label10;
        private TextBox TxtMaxBerat;
        private Label label8;
        private TextBox TxtMinBerat;
        private Label label7;
        private TextBox TxtJmlKendaraan;
        private Label label6;
        private TextBox TxtMaxLebar;
        private Label label11;
        private TextBox TxtMinLebar;
        private Label label12;
        private Button BtnReset;
        private Button BtnStop;
        private Button BtnStart;
    }
}
