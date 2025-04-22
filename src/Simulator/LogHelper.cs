using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class LogReceivedCls : EventArgs
    {
        public enum MessageTypes { INFO, WARNING, ERROR }
        public MessageTypes MessageType { get; set; } = MessageTypes.INFO;
        public DateTime Created { get; set; } = DateTime.Now;
        public string Message { get; set; }

        public override string ToString()
        {
            return $"[{MessageType}][{Created.ToString("dd-MMM-yyyy HH:mm")}] -> {Message}";
        }
    }
    public class LogHelper
    {
        public static event EventHandler<LogReceivedCls> LogReceived;

        public static void WriteLog(string Message, LogReceivedCls.MessageTypes messageTypes = LogReceivedCls.MessageTypes.INFO)
        {
            var msg = new LogReceivedCls() { Message = Message, MessageType = messageTypes };
            LogReceived?.Invoke(null, msg);
            Debug.WriteLine(msg.ToString());
            //Console.WriteLine(msg.ToString());
        }

        public static void WriteLog(Exception ex)
        {
            WriteLog(ex.ToString(), LogReceivedCls.MessageTypes.ERROR);
        }
        public static void WriteLine(string Message, params object[] parameters)
        {
            Message = string.Format(Message, parameters);
            WriteLog(Message, LogReceivedCls.MessageTypes.INFO);
        }
        public static void WriteLine(string Message, LogReceivedCls.MessageTypes messageTypes = LogReceivedCls.MessageTypes.INFO)
        {
            WriteLog(Message, messageTypes);
        }
        public static void WriteLine(object MessageObj)
        {
            var Message = MessageObj.ToString();
            WriteLog(Message, LogReceivedCls.MessageTypes.INFO);
        }
        public static void WriteLine()
        {
            var Message = Environment.NewLine;
            WriteLog(Message, LogReceivedCls.MessageTypes.INFO);
        }
        public static void WriteLine(Exception ex)
        {
            WriteLog(ex.ToString(), LogReceivedCls.MessageTypes.ERROR);
        }
    }
}
