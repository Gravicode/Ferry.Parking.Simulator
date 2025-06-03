using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using nanoFramework.Hardware.Esp32;
namespace Simulator
{
    public class Program
    {
        static SerialPort serialPort;
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");
            try
            {
                // get available ports
                var ports = SerialPort.GetPortNames();

                Debug.WriteLine("Available ports: ");
                foreach (string port in ports)
                {
                    Debug.WriteLine($" {port}");
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////
                // COM2 in ESP32-WROVER-KIT mapped to free GPIO pins
                // mind to NOT USE pins shared with other devices, like serial flash and PSRAM
                // also it's MANDATORY to set pin function to the appropriate COM before instantiating it

                Configuration.SetPinFunction(32, DeviceFunction.COM2_RX);
                Configuration.SetPinFunction(33, DeviceFunction.COM2_TX);

                // open COM2
                serialPort = new SerialPort("COM2");
                // set parameters
                serialPort.BaudRate = 9600;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.Handshake = Handshake.None;
                serialPort.DataBits = 8;
                // Initialize UART (COM port varies by board)
                //serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
                serialPort.Open();
                var rnd = new Random();
                Debug.WriteLine("Weighbridge Simulator Started...");

                while (true)
                {
                    // Generate random weight (1000 - 50000 kg)
                    int weight = 1000 + rnd.Next(50000);
                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                    // Format data as weighbridge output
                    string weighData = $"{timestamp}, Weight: {weight} kg";

                    // Send data via UART
                    serialPort.WriteLine(weighData);
                    Debug.WriteLine($"Sent: {weighData}");

                    // Simulate weighbridge refresh rate (e.g., 3 seconds)
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                serialPort?.Close();
            }
            //Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
