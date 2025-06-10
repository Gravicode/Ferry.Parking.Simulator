using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoloDotNet;
using YoloDotNet.Enums;
using YoloDotNet.Extensions;
using YoloDotNet.Models;

namespace CCTVParkingMonitor
{
    public class GeometryHelper
    {
        public static bool IsPointInsidePolygon(SKPoint point, List<SKPoint> polygon)
        {
            int count = polygon.Count;
            bool inside = false;

            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                    (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }

            return inside;
        }
    }
    public class ParkingSpot
    {
        Random random = new Random();
        public string Name { get; set; }
        public List<SKPoint> Polygon { get; set; } // Koordinat area parkir
        public bool IsOccupied { get; set; } // Status: Terisi atau Kosong

        public ParkingSpot(List<SKPoint> polygon, string name="")
        {
            Polygon = polygon;
            IsOccupied = false; // Default kosong
            Name = string.IsNullOrEmpty(name) ? $"P{random.Next(999)}" : name;
        }
    }
    public class ParkingDetector
    {
        public Yolo yolo { get; set; }
        public ParkingDetector(string PathToModel)
        {
            
            
            // Instantiate a new Yolo object
            yolo = new Yolo(new YoloOptions
            {
                OnnxModel = PathToModel,          // Your Yolo model in onnx format
                ModelType = ModelType.ObjectDetection,      // Set your model type
                Cuda = false,                               // Use CPU or CUDA for GPU accelerated inference. Default = true
                GpuId = 0,                                  // Select Gpu by id. Default = 0
                PrimeGpu = false,                           // Pre-allocate GPU before first inference. Default = false

                // ImageResize = ImageResize.Proportional   // Proportional = Default, Stretched = Squares the image
                // SamplingOptions =  new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None) // View benchmark-test examples: https://github.com/NickSwardh/YoloDotNet/blob/development/test/YoloDotNet.Benchmarks/ImageExtensionTests/ResizeImageTests.cs
            });


        }
        public List<SKRect> FilterObjects(List<ObjectDetection> objects, string[] Filter)
        {
            var rects = new List<SKRect>(); 
            if (Filter is null) Filter = ["car","truck","bus"];
            foreach (var item in objects)
            {
                if (Filter.Contains(item.Label.Name, StringComparer.OrdinalIgnoreCase))
                {
                    rects.Add(item.BoundingBox);
                }
            }
            return rects;
        }
        public (Image image, List<ObjectDetection> objects) PerformDetection(string ImagePath, float Confidence=0.25f, float Iou = 0.7f)
        {
            try
            {
                // Load image
                using var image = SKImage.FromEncodedData(ImagePath);

                // Run inference and get the results
                var results = yolo.RunObjectDetection(image, confidence: Confidence, iou: Iou);

                // Tip:
                // Use the extension method FilterLabels([]) on any result if you only want specific labels.
                // Example: Select only the labels you're interested in and exclude the rest.
                // var results = yolo.RunObjectDetection(image).FilterLabels(["person", "car", "cat"]);

                // Draw results
                using var resultImage = image.Draw(results);
                
                using (SKData data = resultImage.Encode(SKEncodedImageFormat.Png, 100))
                {
                    byte[] buffer = data.ToArray();
                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        return (Image.FromStream(ms), results);
                    }
                    // Now you can use the buffer array as needed
                }
                // Save to file
                //resultImage.Save(ms, SKEncodedImageFormat.Jpeg, 80);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return default;
        }
        public (Image image, List<ObjectDetection> objects) PerformDetection(byte[] imageData, float Confidence = 0.25f, float Iou = 0.7f)
        {
            try
            {
                // Load image
                using SKBitmap bitmap = SKBitmap.Decode(imageData);
                using var image = SKImage.FromBitmap(bitmap);

                // Run inference and get the results
                var results = yolo.RunObjectDetection(image, confidence: Confidence, iou: Iou);

                // Tip:
                // Use the extension method FilterLabels([]) on any result if you only want specific labels.
                // Example: Select only the labels you're interested in and exclude the rest.
                // var results = yolo.RunObjectDetection(image).FilterLabels(["person", "car", "cat"]);

                // Draw results
                using var resultImage = image.Draw(results);

                using (SKData data = resultImage.Encode(SKEncodedImageFormat.Png, 100))
                {
                    byte[] buffer = data.ToArray();
                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        return (Image.FromStream(ms),results);
                    }
                    // Now you can use the buffer array as needed
                }
                // Save to file
                //resultImage.Save(ms, SKEncodedImageFormat.Jpeg, 80);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return default;
        }
      
        private List<ParkingSpot> parkingSpots;

        public void SetParkingSpots(List<ParkingSpot> spots)
        {
            parkingSpots = spots;
        }

        public void DetectVehicles(List<SKRect> detectedVehicles)
        {
            foreach (var spot in parkingSpots)
            {
                spot.IsOccupied = detectedVehicles.Exists(vehicle => IsVehicleInSpot(vehicle, spot.Polygon));
            }
        }

        private bool IsVehicleInSpot(SKRect vehicle, List<SKPoint> polygon)
        {
            var center = new SKPoint( vehicle.MidX, vehicle.MidY);
            var fourPoints = new List<SKPoint>();   
            fourPoints.Add(new SKPoint(vehicle.MidX, vehicle.Top));
            fourPoints.Add(new SKPoint(vehicle.MidX, vehicle.Bottom));
            fourPoints.Add(new SKPoint(vehicle.Left, vehicle.MidY));
            fourPoints.Add(new SKPoint(vehicle.Right, vehicle.MidY));
            /*
            foreach (var point in polygon)
            {
                if (vehicle.Contains(point)) return true;
            }
            */
            var res = GeometryHelper.IsPointInsidePolygon(center, polygon);
            if (res) return res;
            else
            {
                foreach(var point in fourPoints)
                {
                    var res2 = GeometryHelper.IsPointInsidePolygon(point, polygon);
                    if (res2) return res2;
                }
            }
            return false;
        }

        public List<ParkingSpot> GetParkingStatus()
        {
            return parkingSpots;
        }

        void Demo()
        {
            List<ParkingSpot> spots = new List<ParkingSpot>
{
    new ParkingSpot(new List<SKPoint> { new SKPoint(10, 10), new SKPoint(50, 10), new SKPoint(50, 50), new SKPoint(10, 50) }),
    new ParkingSpot(new List<SKPoint> { new SKPoint(60, 10), new SKPoint(100, 10), new SKPoint(100, 50), new SKPoint(60, 50) })
};

            ParkingDetector detector = new ParkingDetector(".onnx");
            detector.SetParkingSpots(spots);

            // Contoh kendaraan terdeteksi
            List<SKRect> detectedVehicles = new List<SKRect>
{
    new SKRect(15, 15, 45, 45), // Kendaraan dalam area pertama
    new SKRect(70, 15, 90, 45)  // Kendaraan dalam area kedua
};

            detector.DetectVehicles(detectedVehicles);

            // Cek status parkiran
            foreach (var spot in detector.GetParkingStatus())
            {
                Console.WriteLine($"Area Parkir: {spot.Polygon[0]} - Status: {(spot.IsOccupied ? "Terisi" : "Kosong")}");
            }
        }
    }
}
