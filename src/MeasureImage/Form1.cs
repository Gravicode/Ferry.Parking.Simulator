using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace MeasureImage
{
    public partial class Form1 : Form
    {
        string SelectedFile { get; set; }
        string modelPath { set; get; }
        string configPath { set; get; }
        string classNamesPath { set; get; }
        string YoloDir = @"C:\experiment\Ferry.Parking.Simulator\src\Yolo3";
        //random assign color to each label
        Scalar[] Colors; //Enumerable.Repeat(false, 80).Select(x => Scalar.RandomColor()).ToArray();

        //get labels from coco.names
        string[] Labels; //File.ReadAllLines(Path.Combine(YoloDir, classNamesPath)).ToArray();

        public Form1()
        {
            InitializeComponent();
         
            modelPath = Path.Combine(YoloDir, "yolov3.weights");
            configPath = Path.Combine(YoloDir, "yolov3.cfg");
            classNamesPath = Path.Combine(YoloDir, "coco.names");
            Colors = Enumerable.Repeat(false, 80).Select(x => Scalar.RandomColor()).ToArray();

            //get labels from coco.names
            Labels=File.ReadAllLines(Path.Combine(YoloDir, classNamesPath)).ToArray();

            BtnOpen.Click += (a, b) => { OpenFile(); };
            BtnCalculate.Click += (a, b) => { Measure(); };
            BtnCalculateYolo.Click += (a, b) => { Test(); };
            BtnClear.Click += (a, b) => { TxtLogs.Clear(); };
        }
        void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select an Image";
            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                SelectedFile = selectedFilePath;
                pictureBox1.Image = Image.FromFile(selectedFilePath);
                Console.WriteLine($"Selected file: {selectedFilePath}");
            }

        }
        void Measure()
        {
            if (!File.Exists(SelectedFile))
            {
                MessageBox.Show("Please choose image first");
                return;
            }
            // Load the image
            Mat src = Cv2.ImRead(SelectedFile);

            // Convert to grayscale
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // Apply Gaussian blur
            Mat blurred = new Mat();
            Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

            // Edge detection using Canny
            Mat edged = new Mat();
            Cv2.Canny(blurred, edged, 50, 150);

            // Find contours
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(edged, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // Assume the largest contour is the object
            double maxArea = 0;
            Point[] largestContour = null;
            var count = 0;
            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area > maxArea)
                {
                    maxArea = area;
                    largestContour = contour;
                   
                }
               
            }
            
            if (largestContour != null)
            {
                // Draw the largest contour
                Cv2.DrawContours(src, new[] { largestContour }, -1, Scalar.Red, 2);

                // Calculate bounding box
                Rect boundingBox = Cv2.BoundingRect(largestContour);
                Cv2.Rectangle(src, boundingBox, Scalar.Green, 2);

                // Calculate size (width and height)
                double width = boundingBox.Width;
                double height = boundingBox.Height;
                TxtLogs.AppendText($"object - Height: {height}, Width: {width}\n");

                Console.WriteLine($"Width: {width}, Height: {height}");
            }
            
            if (src.Empty())
                return;

            Bitmap bitmap = BitmapConverter.ToBitmap(src);
            pictureBox2.Image = bitmap;
            // Display the result
            //Cv2.ImShow("Result", src);
            //Cv2.WaitKey(0);
        }

        void Test()
        {
            #region parameter
            var image = SelectedFile;//Path.Combine(YoloDir, "kite.jpg");
            var cfg = Path.Combine(YoloDir, configPath);
            var model = Path.Combine(YoloDir, modelPath);
            const float threshold = 0.5f;       //for confidence 
            const float nmsThreshold = 0.3f;    //threshold for nms
            #endregion

            //get image
            var org = new Mat(image);

            //setting blob, size can be:320/416/608
            //opencv blob setting can check here https://github.com/opencv/opencv/tree/master/samples/dnn#object-detection
            var blob = CvDnn.BlobFromImage(org, 1.0 / 255, new Size(416, 416), new Scalar(), true, false);

            //load model and config, if you got error: "separator_index < line.size()", check your cfg file, must be something wrong.
            var net = CvDnn.ReadNetFromDarknet(cfg, model);
            #region set preferable
            net.SetPreferableBackend(Backend.OPENCV);
            /*
            0:DNN_BACKEND_DEFAULT 
            1:DNN_BACKEND_HALIDE 
            2:DNN_BACKEND_INFERENCE_ENGINE
            3:DNN_BACKEND_OPENCV 
             */
            net.SetPreferableTarget(Target.CPU);
            /*
            0:DNN_TARGET_CPU 
            1:DNN_TARGET_OPENCL
            2:DNN_TARGET_OPENCL_FP16
            3:DNN_TARGET_MYRIAD 
            4:DNN_TARGET_FPGA 
             */
            #endregion

            //input data
            net.SetInput(blob);

            //get output layer name
            var outNames = net.GetUnconnectedOutLayersNames();
            //create mats for output layer
            var outs = outNames.Select(_ => new Mat()).ToArray();

            #region forward model
            Stopwatch sw = new Stopwatch();
            sw.Start();

            net.Forward(outs, outNames);

            sw.Stop();
            Console.WriteLine($"Runtime:{sw.ElapsedMilliseconds} ms");
            #endregion

            //get result from all output
            GetResult(outs, org, threshold, nmsThreshold);
            Bitmap bitmap = BitmapConverter.ToBitmap(org);
            pictureBox2.Image = bitmap;
        }
        void MeasureYolo()
        {
            if (!File.Exists(SelectedFile))
            {
                MessageBox.Show("Please choose image first");
                return;
            }
            using var net = CvDnn.ReadNetFromDarknet(configPath, modelPath);
            net.SetPreferableBackend(Backend.OPENCV);
            net.SetPreferableTarget(Target.CPU);

            using var image = Cv2.ImRead(SelectedFile);
            using var blob = CvDnn.BlobFromImage(image, 1 / 255.0, new Size(416, 416), new Scalar(), true, false);
            net.SetInput(blob);

            var outputNames = net.GetUnconnectedOutLayersNames();
            var outputs = new Mat[outputNames.Length];
            net.Forward(outputs, outputNames);

            foreach (var output in outputs)
            {
                for (int i = 0; i < output.Rows; i++)
                {
                    var data = output.Row(i);
                    float confidence = data.At<float>(4);
                    if (confidence > 0.5)
                    {
                        int x = (int)(data.At<float>(0) * image.Width);
                        int y = (int)(data.At<float>(1) * image.Height);
                        int width = (int)(data.At<float>(2) * image.Width);
                        int height = (int)(data.At<float>(3) * image.Height);

                        Cv2.Rectangle(image, new Rect(x, y, width, height), Scalar.Red, 2);
                    }
                }
            }
            Bitmap bitmap = BitmapConverter.ToBitmap(image);
            pictureBox2.Image = bitmap;
            //Cv2.ImShow("Detected Cars", image);
            //Cv2.WaitKey(0);
        }
        /// <summary>
        /// Get result form all output
        /// </summary>
        /// <param name="output"></param>
        /// <param name="image"></param>
        /// <param name="threshold"></param>
        /// <param name="nmsThreshold">threshold for nms</param>
        /// <param name="nms">Enable Non-maximum suppression or not</param>
        private void GetResult(IEnumerable<Mat> output, Mat image, float threshold, float nmsThreshold, bool nms = true)
        {
            //for nms
            var classIds = new List<int>();
            var confidences = new List<float>();
            var probabilities = new List<float>();
            var boxes = new List<Rect2d>();

            var w = image.Width;
            var h = image.Height;
            /*
             YOLO3 COCO trainval output
             0 1 : center                    2 3 : w/h
             4 : confidence                  5 ~ 84 : class probability 
            */
            const int prefix = 5;   //skip 0~4

            foreach (var prob in output)
            {
                for (var i = 0; i < prob.Rows; i++)
                {
                    var confidence = prob.At<float>(i, 4);
                    if (confidence > threshold)
                    {
                        //get classes probability
                        Cv2.MinMaxLoc(prob.Row(i).ColRange(prefix, prob.Cols), out _, out Point max);
                        var classes = max.X;
                        var probability = prob.At<float>(i, classes + prefix);

                        if (probability > threshold) //more accuracy, you can cancel it
                        {
                            //get center and width/height
                            var centerX = prob.At<float>(i, 0) * w;
                            var centerY = prob.At<float>(i, 1) * h;
                            var width = prob.At<float>(i, 2) * w;
                            var height = prob.At<float>(i, 3) * h;

                            if (!nms)
                            {
                                // draw result (if don't use NMSBoxes)
                                Draw(image, classes, confidence, probability, centerX, centerY, width, height);
                                continue;
                            }

                            //put data to list for NMSBoxes
                            classIds.Add(classes);
                            confidences.Add(confidence);
                            probabilities.Add(probability);
                            boxes.Add(new Rect2d(centerX, centerY, width, height));
                        }
                    }
                }
            }

            if (!nms) return;

            //using non-maximum suppression to reduce overlapping low confidence box
            CvDnn.NMSBoxes(boxes, confidences, threshold, nmsThreshold, out int[] indices);

            Console.WriteLine($"NMSBoxes drop {confidences.Count - indices.Length} overlapping result.");

            foreach (var i in indices)
            {
                var box = boxes[i];
                Draw(image, classIds[i], confidences[i], probabilities[i], box.X, box.Y, box.Width, box.Height);
                TxtLogs.AppendText($"{Labels[classIds[i]]} - Height: {box.Height}, Width: {box.Width}\n");
            }

        }

        /// <summary>
        /// Draw result to image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="classes"></param>
        /// <param name="confidence"></param>
        /// <param name="probability"></param>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void Draw(Mat image, int classes, float confidence, float probability, double centerX, double centerY, double width, double height)
        {
            //label formating
            var label = $"{Labels[classes]} {probability * 100:0.00}%";
            Console.WriteLine($"confidence {confidence * 100:0.00}% {label}");
            var x1 = (centerX - width / 2) < 0 ? 0 : centerX - width / 2; //avoid left side over edge
            //draw result
            image.Rectangle(new Point(x1, centerY - height / 2), new Point(centerX + width / 2, centerY + height / 2), Colors[classes], 2);
            var textSize = Cv2.GetTextSize(label, HersheyFonts.HersheyTriplex, 0.5, 1, out var baseline);
            Cv2.Rectangle(image, new Rect(new Point(x1, centerY - height / 2 - textSize.Height - baseline),
                new Size(textSize.Width, textSize.Height + baseline)), Colors[classes], Cv2.FILLED);
            var textColor = Cv2.Mean(Colors[classes]).Val0 < 70 ? Scalar.White : Scalar.Black;
            Cv2.PutText(image, label, new Point(x1, centerY - height / 2 - baseline), HersheyFonts.HersheyTriplex, 0.5, textColor);
        }

    }
}
