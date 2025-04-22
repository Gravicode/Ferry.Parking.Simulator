using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator
{
    public class SimulationImageArgs:EventArgs
    {
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
        public Bitmap FerryImage { get; set; }
    }
    public class Simulator
    {
        public bool IsRunning { get; set; }
        public Random Rnd { get; set; }
        public double MinCarWidth { get; set; } = 2;
        public double MaxCarWidth { get; set; } = 2.5;
        public int MinCarWeight { get; set; } = 1500;
        public int MaxCarWeight { get; set; } = 8_000;
        public double MinCarLength { get; set; } = 2;
        public double MaxCarLength { get; set; } = 14;
        public int JumlahKendaraan { get; set; } = 100;
        public int JumlahLane { get; set; } = 3;
        public double LaneWidth { get; set; } = 2.5;
        public double LaneLength { get; set; } = 100;
        public double MaxWeightLane { get; set; } = 30_000;
        public double MinCarSpacing { get; set; } = 1;

        public event EventHandler<SimulationImageArgs> ImageGenerated;
        public Simulator()
        {
            Rnd = new Random();
        }
        List<FerryLane> lanes { set; get; }
        FerryParkingManager manager { set; get; }
        CancellationTokenSource cts { set; get; }
        CancellationToken token { set; get; }
        public void Start()
        {
            if (IsRunning) return;
           
            Thread th1 = new Thread(() =>
            {
                var totalCar = 0;
                IsRunning = true;
                while (totalCar<JumlahKendaraan) {
                    if (token.IsCancellationRequested)
                    {   
                        break;
                    }
                    var newVehicle = new Vehicle(LicensePlateGenerator.GenerateRandomPlate(), (double)Rnd.Next(MinCarWeight, MaxCarWeight), (double)MinCarLength + Rnd.NextDouble() * (MaxCarLength - MinCarLength), (double)MinCarWidth + Rnd.NextDouble() * (MaxCarWidth - MinCarWidth));
                    manager.AllocateVehicle(newVehicle);
                    totalCar++;
                    LogHelper.WriteLine($"masuk kendaraan baru: {newVehicle.Id} - lebar: {newVehicle.Width.ToString("n2")} m, panjang: {newVehicle.Length.ToString("n2")} m, berat: {newVehicle.Weight} kg");
                    var img = FerryParkingVisualizer.DrawFerryParking(lanes, 800, 400);
                    ImageGenerated?.Invoke(this, new SimulationImageArgs() { FerryImage = img });
                    Thread.Sleep(1000);
                }
                IsRunning = false;
            });
            th1.Start();
        }

        public void Stop()
        {
            if (IsRunning)
            {
                cts.Cancel();
                while (IsRunning) { Thread.Sleep(500); }
                
            }
        }
        public void Reset()
        {
            Stop();
            
            lanes = new List<FerryLane>();
            for (int i = 0; i < JumlahLane; i++)
            {
                lanes.Add(new FerryLane(i, LaneLength, LaneWidth, MaxWeightLane, MinCarSpacing));
            }

            manager = new FerryParkingManager(lanes);

            List<Vehicle> vehicles = new List<Vehicle>();
            cts = new CancellationTokenSource();
            token = cts.Token;
            //IsRunning = false;
        }
    }
    public class LicensePlateGenerator
    {
        private static readonly List<string> regionCodes = new List<string>
    {
        "B", "D", "F", "Z", "AB", "AD", "AE", "L", "DK", "BA"
    };

        private static readonly Random random = new Random();

        public static string GenerateRandomPlate()
        {
            string region = regionCodes[random.Next(regionCodes.Count)];
            int number = random.Next(1000, 9999);
            string letters = $"{(char)random.Next('A', 'Z')}{(char)random.Next('A', 'Z')}{(char)random.Next('A', 'Z')}";

            return $"{region} {number} {letters}";
        }
    }
    internal class SimulationHelpers
    {
    }

    public class FerryParkingVisualizer
    {
        public static Bitmap DrawFerryParking(List<FerryLane> lanes, int imageWidth, int imageHeight)
        {
            Bitmap bitmap = new Bitmap(imageWidth, imageHeight);
            using Graphics g = Graphics.FromImage(bitmap);

            g.Clear(Color.White);

            int laneHeight = imageHeight / lanes.Count;
            int margin = 10; // Jarak antar kendaraan

            for (int i = 0; i < lanes.Count; i++)
            {
                int yPos = i * laneHeight;
                g.FillRectangle(Brushes.LightBlue, 0, yPos, imageWidth, laneHeight);

                int xPos = margin;

                foreach (var vehicle in lanes[i].Vehicles)
                {
                    int vehicleWidth = (int)(vehicle.Width * 50);  // Skala ukuran kendaraan
                    int vehicleLength = (int)(vehicle.Length * 10);

                    if (xPos + vehicleLength > imageWidth - margin)
                        break;

                    g.FillRectangle(Brushes.Red, xPos, yPos + margin, vehicleLength, laneHeight - 2 * margin);
                    g.DrawString($"{vehicle.Id} ({vehicle.Weight} kg)", new Font("Arial", 10), Brushes.White, xPos + 5, yPos + 5);

                    xPos += vehicleLength + margin;
                }
            }

            return bitmap;
        }
    }
    public class Vehicle
    {
        public string Id { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }

        public Vehicle(string id, double weight, double length, double width)
        {
            Id = id;
            Weight = weight;
            Length = length;
            Width = width;
        }
    }

    public class FerryLane
    {
        public int LaneId { get; set; }
        public double MaxLength { get; set; }
        public double MaxWidth { get; set; }
        public double MaxWeight { get; set; }
        public double MinSpacing { get; set; }  // Jarak minimum antar kendaraan
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        public FerryLane(int laneId, double maxLength, double maxWidth, double maxWeight, double minSpacing)
        {
            LaneId = laneId;
            MaxLength = maxLength;
            MaxWidth = maxWidth;
            MaxWeight = maxWeight;
            MinSpacing = minSpacing;
        }

        public bool CanFit(Vehicle vehicle)
        {
            double currentWeight = Vehicles.Sum(v => v.Weight);
            double currentLength = Vehicles.Sum(v => v.Length) + (Vehicles.Count * MinSpacing);

            return (currentWeight + vehicle.Weight <= MaxWeight) &&
                   (currentLength + vehicle.Length <= MaxLength);
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if (CanFit(vehicle))
            {
                Vehicles.Add(vehicle);
                LogHelper.WriteLine($"Kendaraan {vehicle.Id} ditempatkan di Lane {LaneId}");
            }
            else
            {
                LogHelper.WriteLine($"Kendaraan {vehicle.Id} tidak dapat ditempatkan di Lane {LaneId}");
            }
        }
    }

    public class FerryParkingManager
    {
        private List<FerryLane> Lanes;

        public FerryParkingManager(List<FerryLane> lanes)
        {
            Lanes = lanes;
        }

        public void AllocateVehicle(Vehicle vehicle)
        {
            foreach (var lane in Lanes.OrderBy(l => l.Vehicles.Count))
            {
                if (lane.CanFit(vehicle))
                {
                    lane.AddVehicle(vehicle);
                    return;
                }
            }
            LogHelper.WriteLine($"Tidak ada lane tersedia untuk kendaraan {vehicle.Id}");
        }
    }
}
