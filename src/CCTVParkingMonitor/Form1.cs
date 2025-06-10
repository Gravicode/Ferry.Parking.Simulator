using SkiaSharp;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace CCTVParkingMonitor
{
    public partial class Form1 : Form
    {
        string appPath;
        string filePath;
        List<SKPoint> CurrentDraw { set; get; }
        List<ParkingSpot> spots { set; get; }   
        ParkingDetector detector { set; get; }
        string fileImage { set; get; }
        public Form1()
        {
            InitializeComponent();
            Setup();
        }
        void LoadConfig()
        {
           

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                spots = JsonSerializer.Deserialize<List<ParkingSpot>>(json);
            }
            else
            {
                Console.WriteLine("Polygon file not found.");
                spots=new List<ParkingSpot>();
            }
        }
        void SaveConfig()
        {
         
            string json = JsonSerializer.Serialize(spots, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(filePath, json);
            Console.WriteLine($"Polygon saved to: {filePath}");
        }
        private Point GetPolygonCenter(List<Point> polygon)
        {
            int avgX = polygon.Sum(p => p.X) / polygon.Count;
            int avgY = polygon.Sum(p => p.Y) / polygon.Count;
            return new Point(avgX, avgY);
        }
        void Setup()
        {
            this.FormClosing += (a, b) => {
                SaveConfig();
            };
            appPath = AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(appPath, "polygon.json");
            LoadConfig();
            pictureBox1.Paint += (a, e) =>{
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                foreach (var spot in spots)
                {

                    Point captionPoint = new Point(9999,9999);
                    List<Point> polygon = new List<Point>();
                    spot.Polygon.ForEach(point => {
                        var newPoint = new Point((int)point.X, (int)point.Y);
                        polygon.Add(newPoint);
                        if(newPoint.X < captionPoint.X)
                        {
                            captionPoint = newPoint;
                        }else if(newPoint.Y < captionPoint.Y)
                        {
                            captionPoint = newPoint;
                        }

                    });
                    // Draw caption at the center of the polygon
                    Point center = GetPolygonCenter(polygon);
                    using (Font font = new Font("Arial", 12, FontStyle.Bold))
                    using (SolidBrush textBrush = new SolidBrush(Color.Black))
                    {
                        SizeF textSize = g.MeasureString(spot.Name, font);
                        g.DrawString(spot.Name, font, textBrush, center.X - textSize.Width / 2, center.Y - textSize.Height / 2);
                    }
                    // Semi-transparent green fill
                    using (SolidBrush fillBrush = new SolidBrush(spot.IsOccupied ? Color.FromArgb(128, 255, 0, 0) : Color.FromArgb(128, 0, 255, 0))) // 50% transparency
                    {
                        g.FillPolygon(fillBrush, polygon.ToArray());
                    }

                    // Bold black border
                    using (Pen borderPen = new Pen(Color.Green, 2))
                    {
                        g.DrawPolygon(borderPen, polygon.ToArray());
                    }

                   
                }
                //draw current..
                Pen dotPen = new Pen(Color.Red, 2);
                if (CurrentDraw != null)
                    if (CurrentDraw.Count == 1)
                    {
                        g.DrawRectangle(dotPen, new Rectangle((int)CurrentDraw.First().X, (int)CurrentDraw.First().Y, 1, 1));
                    }
                    else if (CurrentDraw.Count > 1)
                    {
                        var count = 0;
                        foreach (var point in CurrentDraw)
                        {
                            g.DrawRectangle(dotPen, new Rectangle((int)point.X, (int)point.Y, 1, 1));
                            count++;
                            if (count > 1)
                            {
                                var lastPoint = CurrentDraw[count - 2];
                                g.DrawLine(dotPen, lastPoint.X, lastPoint.Y, point.X, point.Y);
                            }
                        }

                    }
            };
            pictureBox1.MouseUp += (a, b) => {
                if (ChkCreateParkingSpot.Checked)
                {
                    if (CurrentDraw == null) { CurrentDraw = new(); };
                    if (b.Button == MouseButtons.Left)
                    {
                        CurrentDraw.Add(new SKPoint(b.X,b.Y));
                    }
                    if(b.Button == MouseButtons.Right)
                    {
                        CurrentDraw.Clear();
                    }
                    pictureBox1.Invalidate();
                }
            };
            BtnClear.Click += (a, b) =>
            {
                var msg = MessageBox.Show("Are you sure to clear all spot ?", "Warning", MessageBoxButtons.YesNo);
                if (msg == DialogResult.Yes)
                {
                    spots.Clear();
                    pictureBox1.Invalidate();
                    
                }
            };
                BtnAdd.Click += (a, b) =>
            {
                if (CurrentDraw != null)
                {
                    if (CurrentDraw.Count > 2)
                    {
                        var newName = $"P{spots.Count + 1}";
                        if (spots.Count > 0)
                        {
                            var lastNum = int.Parse(spots.Last().Name.Replace("P", ""));
                            lastNum++;
                            newName = $"P{lastNum}";
                        }

                        spots.Add(new(CurrentDraw, newName));
                        CurrentDraw = new();
                        pictureBox1.Invalidate();
                    }
                }
            };
                detector = new(@"D:\experiment\Ferry.Parking.Simulator\models\yolov8s.onnx");
            BtnOpen.Click += (a, b) => {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileImage = openFileDialog.FileName;
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                }
            };
            BtnCheck.Click += (a, b) => {
                if (spots.Count <= 0)
                {
                    MessageBox.Show("Set Parking Spot first !!", "Warning");
                    return;
                }
                if (string.IsNullOrEmpty(fileImage) || !File.Exists(fileImage))
                {
                    MessageBox.Show("Select parking image first !!", "Warning");
                    return;
                }
                detector.SetParkingSpots(spots);

                var res = detector.PerformDetection(fileImage);
                if (res.image is null || res.objects is null)
                {
                    MessageBox.Show("No Object Detected !!", "Warning");
                    return;
                }
                
                // Contoh kendaraan terdeteksi
                var detectedVehicles = detector.FilterObjects(res.objects, ["car","bus","truck"]);

                detector.DetectVehicles(detectedVehicles);
                spots = detector.GetParkingStatus();
                // Cek status parkiran
                foreach (var spot in spots)
                {
                    Console.WriteLine($"Area Parkir: {spot.Name} - Status: {(spot.IsOccupied ? "Terisi" : "Kosong")}");
                }
                GridView1.DataSource = spots;
                GridView1.AutoGenerateColumns = true;
                GridView1.Refresh();
                pictureBox1.Image = res.image;
            };
            BtnDelete.Click += (a, b) => {
                if (GridView1.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow selectedRow in GridView1.SelectedRows)
                    {

                        //DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                        string value = selectedRow.Cells["Name"].Value.ToString();
                        var selItem = spots.Where(x=>x.Name == value).FirstOrDefault(); 
                        if(selItem != null)
                        {
                            spots.Remove(selItem);
                        }
                        GridView1.DataSource = null;
                        GridView1.Refresh();
                        GridView1.DataSource = spots;
                        pictureBox1.Invalidate();

                    }
                }
            };
            /*
            List<ParkingSpot> spots = new List<ParkingSpot>
{
    new ParkingSpot(new List<SKPoint> { new SKPoint(10, 10), new SKPoint(50, 10), new SKPoint(50, 50), new SKPoint(10, 50) }),
    new ParkingSpot(new List<SKPoint> { new SKPoint(60, 10), new SKPoint(100, 10), new SKPoint(100, 50), new SKPoint(60, 50) })
};
            */
           
           
        }
    }
}
