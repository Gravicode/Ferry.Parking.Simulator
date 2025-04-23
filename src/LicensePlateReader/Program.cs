namespace LicensePlateReader
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //var path = @"C:\experiment\Ferry.Parking.Simulator\src\Dependencies\Aspose.Total.NET.lic";
            //new Aspose.OCR.License().SetLicense(path);
            Application.Run(new Form1());
        }
    }
}