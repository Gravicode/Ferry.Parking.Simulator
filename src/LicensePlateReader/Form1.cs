using OpenCvSharp;
using Tesseract;

namespace LicensePlateReader
{
    public partial class Form1 : Form
    {
        string SelectedFile { get; set; }
        public Form1()
        {
            InitializeComponent();
            BtnLoad.Click += (a, b) => { OpenFile(); };
            BtnRead.Click += (a, b) => { ReadTesseract(); };
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

        void ReadTesseract()
        {

            if (!File.Exists(SelectedFile))
            {
                MessageBox.Show("Please choose image first");
                return;
            }

            string outputFolder = "C:\\experiment\\Ferry.Parking.Simulator\\src\\output"; // Folder to save cropped plates

            // Read the image
            Mat img = Cv2.ImRead(SelectedFile);

            // Convert the image to grayscale
            Mat gray = new Mat();
            Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);

            // Apply Gaussian blur to reduce noise
            Mat blurred = new Mat();
            Cv2.GaussianBlur(gray, blurred, new OpenCvSharp.Size(5, 5), 0);

            // Use the Canny edge detector
            Mat edges = new Mat();
            Cv2.Canny(blurred, edges, 50, 150);

            // Find contours
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            string outputPath = "";
            // Filter contours based on area
            foreach (var cnt in contours)
            {
                if (Cv2.ContourArea(cnt) > 1000)
                {
                    var boundingBox = Cv2.BoundingRect(cnt);
                    Mat licensePlate = new Mat(img, boundingBox);

                    // Save cropped license plate image
                    outputPath = $"{outputFolder}/license_plate_{boundingBox.X}_{boundingBox.Y}.png";
                    Cv2.ImWrite(outputPath, licensePlate);

                    // Draw contours on the original image
                    Cv2.Rectangle(img, boundingBox, Scalar.Green, 2);
                }
            }

            // Display the original image with contours
            //Cv2.ImShow("Contours", img);
            //Cv2.WaitKey(0);
            //Cv2.DestroyAllWindows();
            if (!File.Exists(outputPath))
            {
                return;
            }
            Mat image = Cv2.ImRead(outputPath, ImreadModes.Grayscale);
            //Mat image = new Mat();
            //Cv2.CvtColor(img, image, ColorConversionCodes.BGR2GRAY);

            // Preprocessing: Resize and apply threshold
            //Cv2.Resize(image, image, new OpenCvSharp.Size(500, 200));
            Cv2.Threshold(image, image, 0, 255, ThresholdTypes.Otsu);

            // Save processed image
            string processedImagePath = "processed_plate.jpg";
            Cv2.ImWrite(processedImagePath, image);

            // OCR using Tesseract
            var tessdataPath = @"C:\experiment\Ferry.Parking.Simulator\src\tessdata";
            var ocrEngine = new TesseractEngine(tessdataPath, "ind", EngineMode.Default);
            var img2 = Pix.LoadFromFile(processedImagePath);
            var result = ocrEngine.Process(img2);

            Console.WriteLine("Detected License Plate Number: " + result.GetText());
            TxtPlate.Text = result.GetText();
            /*
            // Initialize Tesseract OCR Engine
            using var ocrEngine = new TesseractEngine(@"C:\experiment\Ferry.Parking.Simulator\src\tessdata", "eng", EngineMode.Default);

            // Load the image into a Pix object
            using var img = Pix.LoadFromFile(SelectedFile);

            // Perform OCR
            var result = ocrEngine.Process(img);

            // Output the recognized text
            Console.WriteLine("Detected Plate Number: " + result.GetText());
            TxtPlate.Text = result.GetText();
            */
        }
        void Read()
        {
            if (!File.Exists(SelectedFile))
            {
                MessageBox.Show("Please choose image first");
                return;
            }
            Aspose.OCR.AsposeOcr recognitionEngine = new Aspose.OCR.AsposeOcr();
            // Add images to OcrInput object
            Aspose.OCR.OcrInput input = new Aspose.OCR.OcrInput(Aspose.OCR.InputType.SingleImage);
            input.Add(SelectedFile);
           
            // Recognition settings
            Aspose.OCR.CarPlateRecognitionSettings recognitionSettings = new Aspose.OCR.CarPlateRecognitionSettings();
            recognitionSettings.Language = Aspose.OCR.Language.Eng;
            // Recognize license plates
            var results = recognitionEngine.RecognizeCarPlate(input, recognitionSettings);
            foreach (Aspose.OCR.RecognitionResult result in results)
            {
                Console.WriteLine(result.RecognitionText);
                TxtPlate.Text = result.RecognitionText;
            }

        }
    }
}
