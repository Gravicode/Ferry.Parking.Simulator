using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using DTK.LPR.Lib;
using ReadWriteMemory;
using System.IO;

namespace sample
{
    public partial class FormVideo : Form
    {
        VideoCapture videoCap = null;
        LPREngine engine = null;
        LPRParams lprParams = new LPRParams();

        public FormVideo()
        {
            InitializeComponent();

            this.Text += " (Library version:" + DTKLPR4.GetLibraryVersion() + ")";

            ((DataGridViewImageColumn)dataGridView1.Columns["colPlateImage"]).ImageLayout = DataGridViewImageCellLayout.Zoom;

            numMinCharHeight.Value = lprParams.MinCharHeight;
            numMaxCharHeight.Value = lprParams.MaxCharHeight;
            txtCountryCodes.Text = lprParams.Countries;
            numFPSLimit.Value = lprParams.FPSLimit;
            numNumThreads.Value = lprParams.NumThreads;
            cbRecognitionOnMotion.Checked = lprParams.RecognitionOnMotion;
            txtBurnString.Text = lprParams.BurnFormatString;
            cmbBurnPos.SelectedIndex = lprParams.BurnPosition;
        }

        private void CreateLPREngine(bool bVideo)
        {
            if (engine != null)
            {
                engine.Dispose();
            }

            // Set LPR settings
            lprParams.MinCharHeight = (int)numMinCharHeight.Value;
            lprParams.MaxCharHeight = (int)numMaxCharHeight.Value;
            lprParams.Countries = txtCountryCodes.Text;
            lprParams.FPSLimit = (int)numFPSLimit.Value;
            lprParams.NumThreads = (int)numNumThreads.Value;
            lprParams.RecognitionOnMotion = cbRecognitionOnMotion.Checked;
            lprParams.BurnFormatString = txtBurnString.Text;
            lprParams.BurnPosition = cmbBurnPos.SelectedIndex;

            // Create LPREngine object
            if (bVideo)
                engine = new LPREngine(lprParams, true, OnLicensePlateDetected);
            else
                engine = new LPREngine(lprParams, false, null);

            if (engine.IsLicensed == 1)
                MessageBox.Show("The computer have not runtime license activated. The LPR will be disabled.\nPlease contact at support@dtksoft.com to request trial license.", "No License", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            if (engine.IsLicensed == 2)
                MessageBox.Show("The runtime license do not allow more channels. The LPR will be disabled.", "No License", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            if (!patched)
            {
                //if 32bit use:
               // if (DoPatch32Bit(Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName), "DTKLPR4.dll"))
                if (DoPatch64Bit(Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName), "DTKLPR4.dll"))

                {
                    patched = true;
                    //  MessageBox.Show("Patched!");
                }
                else
                {
                    patched = false;
                    MessageBox.Show("Not patched!");
                }
            }

            CreateLPREngine(true);

            if (videoCap != null)
            {
                videoCap.StopCapture();
                videoCap.Dispose();
            }

            // Create video capture object
            videoCap = new VideoCapture(OnFrameCaptured, OnCaptureError, engine);

            // Start capture from selected video source
            if (tabControl1.SelectedIndex == 0)
                videoCap.StartCaptureFromIPCamera(txtIPCameraURL.Text);
            if (tabControl1.SelectedIndex == 1)
                videoCap.StartCaptureFromFile(txtVideoFile.Text, (int)numVideoFileRepeatCount.Value);
            if (tabControl1.SelectedIndex == 2)
                videoCap.StartCaptureFromDevice((int)numDeviceIndex.Value, (int)numDeviceResX.Value, (int)numDeviceResY.Value);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (videoCap != null)
                videoCap.StopCapture();
        }

        public void OnFrameCaptured(VideoCapture videoCap, IntPtr pBuffer, int width, int height, int stride, PIXFMT pixelFormat, long timestamp, object customObject)
        {
            LPREngine engine = (LPREngine)customObject;
            engine.PutFrameImageBuffer(pBuffer, width, height, stride, pixelFormat, timestamp, 0);

            Bitmap bmp = DTKLPR4.CreateBitmapFromBuffer(pBuffer, width, height, stride, pixelFormat);

            SetFrame(bmp);
        }

        public void OnCaptureError(VideoCapture videoCap, ERR_CAPTURE errorCode, object customObject)
        {
            if (errorCode == ERR_CAPTURE.EOF)
            {
                SetFrame(null);
            }
        }

        public void OnLicensePlateDetected(LPREngine engine, LicensePlate plate)
        {
            AddPlate(plate);
        }

        public void AddPlate(LicensePlate plate)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate { AddPlate(plate); });
                return;
            }
            DataGridViewRow row = dataGridView1.Rows[dataGridView1.Rows.Add()];
            row.Cells["colDT"].Value = plate.DateTime;
            row.Cells["colPlate"].Value = plate.Text + ((plate.CountryCode.Length > 0) ? " (" + plate.CountryCode + ")" : "");
            row.Cells["colDirection"].Value = plate.Direction.ToString();
            row.Cells["colPlateImage"].Value = plate.PlateImage;
            row.Tag = plate.Image;
            dataGridView1.ClearSelection();
            row.Selected = true;
            dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
        }

        public void SetFrame(Image image)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate { SetFrame(image);});
                return;
            }
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = image;
        }

        private void btnBrowseVideoFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtVideoFile.Text = openFileDialog1.FileName;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                pictureBox2.Image = (Image)dataGridView1.SelectedRows[0].Tag;
            }
        }

        private void btnLicInfo_Click(object sender, EventArgs e)
        {
            FormLic dlg = new FormLic();
            dlg.ShowDialog();
        }

        private void btnClearResults_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
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


            if (!(buff[0] == 0x85 && buff[1] == 0xC0 && buff[2] == 0x74 && buff[3] == 0x34))
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
