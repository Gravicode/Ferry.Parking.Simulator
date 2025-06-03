
// Define serial port settings
using System.IO.Ports;

internal class Program
{
    private static void Main(string[] args)
    {
        SerialPort serialPort = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);

        try
        {
            serialPort.Open();
            serialPort.DataReceived += SerialPort_DataReceived;
            Console.WriteLine("Listening for weighbridge data... Press any key to exit.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            serialPort.Close();
        }
    }

    static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string data = sp.ReadLine(); // Read weighbridge data
        Console.WriteLine("Data: " + data);
    }

}