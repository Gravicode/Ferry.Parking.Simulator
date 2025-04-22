using System.Windows.Forms;

namespace Simulator
{
   
    public partial class Form1 : Form
    {
        Simulator simulator {  get; set; }  
        public Form1()
        {
            InitializeComponent();
            Setup();
            Reset();
        }
        void Reset()
        {
            simulator.JumlahKendaraan = int.Parse(TxtJmlKendaraan.Text);
            simulator.JumlahLane = int.Parse(TxtJmlLane.Text);
            simulator.MinCarSpacing = double.Parse(TxtJarakKendaraan.Text);
            simulator.MinCarWidth = double.Parse(TxtMinLebar.Text);
            simulator.MaxCarWidth = double.Parse(TxtMaxLebar.Text);
            simulator.MinCarLength = double.Parse(TxtMinPanjang.Text);
            simulator.MaxCarLength = double.Parse(TxtMaxPanjang.Text);
            simulator.LaneLength = double.Parse(TxtPanjangLane.Text);
            simulator.LaneWidth = double.Parse(TxtLebarLane.Text);
            simulator.MaxWeightLane = double.Parse(TxtBeratLane.Text);
            simulator.MinCarWeight = int.Parse(TxtMinBerat.Text);
            simulator.MaxCarWeight = int.Parse(TxtMaxBerat.Text);
            simulator.Reset();
            TxtLogs.Clear();
            pictureBox.Image = null;
        }
        void Setup()
        {
            simulator = new Simulator();
            TxtJmlKendaraan.Text = simulator.JumlahKendaraan.ToString();
            TxtJmlLane.Text = simulator.JumlahLane.ToString();
            TxtJarakKendaraan.Text = simulator.MinCarSpacing.ToString();
            TxtMinLebar.Text = simulator.MinCarWidth.ToString();
            TxtMaxLebar.Text = simulator.MaxCarWidth.ToString();
            TxtMinPanjang.Text = simulator.MinCarLength.ToString();
            TxtMaxPanjang.Text = simulator.MaxCarLength.ToString();
            TxtPanjangLane.Text = simulator.LaneLength.ToString();
            TxtLebarLane.Text = simulator.LaneWidth.ToString();
            TxtBeratLane.Text = simulator.MaxWeightLane.ToString();
            TxtMinBerat.Text = simulator.MinCarWeight.ToString();
            TxtMaxBerat.Text = simulator.MaxCarWeight.ToString();
            LogHelper.LogReceived += (a, b) => {
                if (TxtLogs.InvokeRequired)
                {
                    TxtLogs.Invoke(new Action(() => TxtLogs.AppendText(b.ToString() + Environment.NewLine)));                 
                }
                else
                {
                    TxtLogs.AppendText(b.ToString()+Environment.NewLine);
                }
            };
            BtnReset.Click += (a, b) =>
            {
                Reset();
            };
            BtnStart.Click += (a, b) =>
            {
                simulator.Start();
            };
            BtnStop.Click += (a, b) =>
            {
                simulator.Stop();
            };
            simulator.ImageGenerated += (a, b) => {
                if (pictureBox.InvokeRequired)
                {
                    pictureBox.Invoke(new Action(() => pictureBox.Image = b.FerryImage));
                }
                else
                {
                    pictureBox.Image = b.FerryImage;
                }
            };
        }

        
    }



}
