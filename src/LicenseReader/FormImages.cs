using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using DTK.LPR.Lib;
using ReadWriteMemory;

namespace sample
{
    public partial class FormImages : Form
    {
        private LPRParams lprParams = new LPRParams();
        private Thread worker = null;
        private bool workerStopFlag = false;

        public FormImages()
        {
            InitializeComponent();

            this.Text += " (Library version:" + DTKLPR4.GetLibraryVersion() + ")";

            colPlateImage.DefaultCellStyle.NullValue = new Bitmap(1, 1);

            // Load defaul settings
            numMinCharHeight.Value = lprParams.MinCharHeight;
            numMaxCharHeight.Value = lprParams.MaxCharHeight;
            txtCountryCodes.Text = lprParams.Countries;
        }

        private void btnLoadImages_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPEG Files|*.jpg;*.jpeg|BMP Files|*.bmp|PNG Files|*.png|All Image Files|*.jpg;*.jpeg;*.bmp;*.png";
            openFileDialog1.FilterIndex = 4;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFileDialog1.FileNames)
                {
                    int nRow = dataGridView1.Rows.Add();
                    dataGridView1.Rows[nRow].Cells[colFileName.Index].Value = Path.GetFileName(fileName);
                    dataGridView1.Rows[nRow].Tag = fileName; // store full file path in Tag
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {


            if (!patched)
            {

                //if 32bit use : 
              // if (DoPatch32Bit(Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName), "DTKLPR4.dll"))
                if (DoPatch64Bit(Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName), "DTKLPR4.dll"))

                {
                    patched = true;

                }
                else
                {
                    patched = false;

                }
            }


            workerStopFlag = false;
            worker = new Thread(() => Worker());
            worker.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            workerStopFlag = true;
        }

        private void Worker()
        {
            lprParams.MinCharHeight = (int)numMinCharHeight.Value;
            lprParams.MaxCharHeight = (int)numMaxCharHeight.Value;
            lprParams.Countries = txtCountryCodes.Text;

            LPREngine engine = new LPREngine(lprParams, false, null);
            if (engine.IsLicensed != 0)
            {
                MessageBox.Show("No License");
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (workerStopFlag)
                    break;

                string fileName = row.Tag.ToString();

                Bitmap image = (Bitmap)Image.FromFile(fileName);

                int time = Environment.TickCount;

                // Read from image buffer
                // BitmapData bmpdata = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
                // List<LicensePlate> plates = engine.ReadFromImageBuffer(bmpdata.Scan0, image.Width, image.Height, bmpdata.Stride, PIXFMT.RGB24);

                List<LicensePlate> plates = engine.ReadFromFile(fileName);

                time = Environment.TickCount - time;

                string result = "";
                Image plates_img = null;
                foreach (LicensePlate plate in plates)
                {
                    if (result.Length > 0) result += ",";
                    result += plate.Text;
                    if (plate.CountryCode.Length > 0)
                        result += " (" + plate.CountryCode + ")";

                    Rectangle bbox = new Rectangle(plate.X, plate.Y, plate.Width, plate.Height);
                    Image plateImage = bbox.IsEmpty ? GetDemoPlateImage(plate.Text) : cropImage(image, bbox);

                    if (plates_img == null)
                        plates_img = plateImage;
                    else
                    {
                        Image image1 = new Bitmap(plates_img.Width + plateImage.Width, Math.Max(plates_img.Height, plateImage.Height));
                        Graphics g = Graphics.FromImage(image1);
                        g.DrawImage(plates_img, 0, 0);
                        g.DrawImage(plateImage, plates_img.Width, 0);
                        plates_img = image1;
                    }
                }

                SetResult(row, result, time, plates_img);
            }
        }


        private void SetResult(DataGridViewRow row, string text, int time, Image img)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    SetResult(row, text, time, img);
                });
                return;
            }

            row.Cells[colResult.Index].Value = text;
            row.Cells[colTime.Index].Value = time;
            row.Cells[colPlateImage.Index].Value = img;

            int dispCount = dataGridView1.DisplayedRowCount(true);
            if (row.Index >= dispCount)
                dataGridView1.FirstDisplayedScrollingRowIndex = row.Index - dispCount + 2;

            dataGridView1.ClearSelection();
            row.Selected = true;
        }

        private Image GetDemoPlateImage(string text)
        {
            Bitmap bmp = new Bitmap(200, 50);
            Graphics gr = Graphics.FromImage(bmp);
            gr.DrawRectangle(Pens.Black, new Rectangle(2, 2, 196, 46));
            gr.DrawString(text, new Font("Arial", 22), Brushes.Black, new Rectangle(5, 5, 190, 40));
            gr.Dispose();
            return bmp;
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            return ((Bitmap)img).Clone(cropArea, img.PixelFormat);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Tag == null)
                return;
            pictureBox1.Image = Image.FromFile(dataGridView1.SelectedRows[0].Tag.ToString());
        }

        private void FormImages_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker != null)
            {
                workerStopFlag = true;
                while (worker.IsAlive)
                    Thread.Sleep(10);
            }
        }

        private void btnLicInfo_Click(object sender, EventArgs e)
        {
            FormLic dlg = new FormLic();
            dlg.ShowDialog();
        }

        private bool patched;
        private bool DoPatch64Bit(string appExe, string dllName)
        {


            ProcessMemory Mem = new ProcessMemory(appExe);



            if (!Mem.CheckProcess())
            {
                //"Is Running ?"

                return false;
            }
            else

                Mem.StartProcess();



            byte[] buff;

            IntPtr baseoffset;
            IntPtr offset;
            baseoffset = Mem.DllImageAddress(dllName);

           
            offset = baseoffset + 23905279;


            buff = Mem.ReadMem(offset, 4);


            if (!(buff[0] == 0x85  && buff[1] == 0xC0 && buff[2] == 0x74 && buff[3] == 0x34))
            {

                //"Not Found!"
                return false;
            }


            buff[0] = 0x33;
            buff[1] = 0xFF;
            buff[2] = 0xFF;
            buff[3] = 0xC7;
            Mem.WriteMem(offset, buff);


          



            return true;
        }


        private bool DoPatch32Bit(string appExe, string dllName)
        {

            ProcessMemory Mem = new ProcessMemory(appExe);



            if (!Mem.CheckProcess())
            {
                //"Is Running ?"
                return false;
            }
            else

                Mem.StartProcess();



            byte[] buff;

            IntPtr baseoffset;
            IntPtr offset;
            baseoffset = Mem.DllImageAddress(dllName);

            offset = baseoffset + 13598312;
            

            buff = Mem.ReadMem(offset, 10);


            if (!(buff[0] == 0xC7 && buff[1] == 0x05 && buff[8] == 0x00 && buff[9] == 0x00))
            {

                //"Not Found!"
                return false;
            }


            buff[6] = 0x01;
            
            Mem.WriteMem(offset, buff);

            

            return true;
        }
    }
}
