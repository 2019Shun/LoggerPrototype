using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;

namespace LoggerPrototype
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //LogTextScroll.ScrollToEnd();

            var selectSerial = new SelectSerialPort();
            selectSerial.OpenSerialPort = OpenSerialPortTest;
            selectSerial.Show();
        }

        public void OpenSerialPortTest(string port, int baud)
        {
            SerialPort serialPort = new SerialPort();
            serialPort.PortName = port;
            serialPort.BaudRate = baud;
            serialPort.NewLine = Environment.NewLine;
            //SerialPort.Parity = System.IO.Ports.Parity.None;
            //SerialPort.DataBits = 8;
            //SerialPort.StopBits = System.IO.Ports.StopBits.One;
            //SerialPort.Handshake = System.IO.Ports.Handshake.None;
            serialPort.Open();

            serialPort.DiscardInBuffer();
            SerialLogTextBox.Document.Blocks.Clear();
            PrintInfo("Started communication with " + serialPort.PortName);

            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceivedHandler);

        }

        /// <summary>
        /// 文字の出力：白
        /// </summary>
        /// <param name="str"></param>
        public void Print(string str)
        {
            TextRange rangeOfText = new TextRange(SerialLogTextBox.Document.ContentEnd, SerialLogTextBox.Document.ContentEnd);
            rangeOfText.Text = str;
            rangeOfText.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.White);
            SerialLogTextScroll.ScrollToEnd();
        }

        /// <summary>
        /// 文字の出力：青
        /// 最後に改行を入れる
        /// </summary>
        /// <param name="str"></param>
        public void PrintInfo(string str)
        {
            TextRange rangeOfText = new TextRange(SerialLogTextBox.Document.ContentEnd, SerialLogTextBox.Document.ContentEnd);
            rangeOfText.Text = "[INFO] " + str;
            rangeOfText.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            SerialLogTextScroll.ScrollToEnd();
            SerialLogTextBox.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// 文字の出力：赤
        /// 最後に改行を入れる
        /// </summary>
        /// <param name="str"></param>
        public void PrintWarning(string str)
        {
            TextRange rangeOfText = new TextRange(SerialLogTextBox.Document.ContentEnd, SerialLogTextBox.Document.ContentEnd);
            rangeOfText.Text = "[WARN] " + str;
            rangeOfText.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            SerialLogTextScroll.ScrollToEnd();
            SerialLogTextBox.AppendText(Environment.NewLine);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //SerialLogTextBox.Text += "test\ntest\ntest\ntest\ntest\n";
            //SerialLogTextBox.AppendText("test");

            PrintWarning("test ");
        }

        private void SerialPort_DataReceivedHandler(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialLogTextBox.Dispatcher.Invoke(
                    new Action(() =>
                    {
                        SerialPort serialPort = (SerialPort)sender;
                        string str = serialPort.ReadExisting();
                        Print(str);
                    })
                );
            }
            catch
            {
                SerialLogTextBox.Dispatcher.Invoke(
                    new Action(() =>
                    {
                        SerialPort serialPort = (SerialPort)sender;
                        PrintWarning("Failed communication with " + serialPort.PortName);
                    })
                );
            }
        }

        private void MenuTextItemClear_Click(object sender, RoutedEventArgs e)
        {
            SerialLogTextBox.Document.Blocks.Clear();
        }

        private void MenuFinish_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
