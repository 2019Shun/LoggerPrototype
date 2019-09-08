﻿using System;
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
using System.IO;
using System.IO.Ports;

namespace LoggerPrototype
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 画面に表示する文字列を管理するDisplayString
        /// </summary>
        private DisplayString _displayString;

        /// <summary>
        /// C#のシリアル通信ライブラリ
        /// </summary>
        private SerialPort _serialPort;

        /// <summary>
        /// 自動スクロールのオンオフを管理するプロパティ
        /// </summary>
        private bool _enableAutoScroll
        {
            get { return MenuItemAutoScroll.IsChecked; }
            set { MenuItemAutoScroll.IsChecked = value; }
        }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ConnectSerialInterface();

            _displayString = new DisplayString();
            _enableAutoScroll = true;
        }

        /// <summary>
        /// シリアルポートと接続する処理
        /// 選択ウィンドウを表示する
        /// </summary>
        public void ConnectSerialInterface()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort = null;
            }
            var selectSerial = new SelectSerialPort();
            selectSerial.OpenSerialPort = OpenSerialPort;
            selectSerial.Show();
        }

        /// <summary>
        /// 指定されたシリアルポートを開く
        /// </summary>
        /// <param name="port">ポート名(ex."COM4")</param>
        /// <param name="baud">ボーレート</param>
        public void OpenSerialPort(string port, int baud)
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort = null;
            }
            _serialPort = new SerialPort();
            _serialPort.PortName = port;
            _serialPort.BaudRate = baud;
            _serialPort.NewLine = Environment.NewLine;
            //SerialPort.Parity = System.IO.Ports.Parity.None;
            //SerialPort.DataBits = 8;
            //SerialPort.StopBits = System.IO.Ports.StopBits.One;
            //SerialPort.Handshake = System.IO.Ports.Handshake.None;
            _serialPort.Open();
            _serialPort.DiscardInBuffer();

            DisplayClear();
            PrintInfo("Started communication with " + _serialPort.PortName);

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceivedHandler);

        }

        /// <summary>
        /// シリアルポートが開かれていた場合，strを送信
        /// 右側画面に送信した文字列を出力
        /// </summary>
        /// <param name="str"></param>
        private void SerialWriteString(string str)
        {
            if (_serialPort.IsOpen == false)
            {
                PrintWarning("Serial port is not opened.");
                return;
            }

            _serialPort.Write(str);
            PrintData(str);
        }

        /// <summary>
        /// 文字の出力：白
        /// </summary>
        /// <param name="str"></param>
        public void Print(string str)
        {
            _displayString.Append(str);

            SerialLogTextBox.Text = _displayString.GetString();

            if (_enableAutoScroll)
            {
                SerialLogTextScroll.ScrollToEnd();
            }
        }

        /// <summary>
        /// 文字の出力：青，右側の画面
        /// </summary>
        /// <param name="str"></param>
        public void PrintData(string str)
        {
            TextRange rangeOfText = new TextRange(SerialLogTextBox2.Document.ContentEnd, SerialLogTextBox2.Document.ContentEnd);
            rangeOfText.Text = "[SEND] " + str;
            rangeOfText.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.AntiqueWhite);
            SerialLogTextScroll2.ScrollToEnd();
            SerialLogTextBox2.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// 文字の出力：青，右側の画面
        /// </summary>
        /// <param name="str"></param>
        public void PrintInfo(string str)
        {
            TextRange rangeOfText = new TextRange(SerialLogTextBox2.Document.ContentEnd, SerialLogTextBox2.Document.ContentEnd);
            rangeOfText.Text = "[INFO] " + str;
            rangeOfText.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Aquamarine);
            SerialLogTextScroll2.ScrollToEnd();
            SerialLogTextBox2.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// 文字の出力：赤，右側の画面
        /// </summary>
        /// <param name="str"></param>
        public void PrintWarning(string str)
        {
            TextRange rangeOfText = new TextRange(SerialLogTextBox2.Document.ContentEnd, SerialLogTextBox2.Document.ContentEnd);
            rangeOfText.Text = "[WARN] " + str;
            rangeOfText.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.IndianRed);
            SerialLogTextScroll2.ScrollToEnd();
            SerialLogTextBox2.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// 出力画面のクリア
        /// </summary>
        private void DisplayClear()
        {
            _displayString.Clear();
            SerialLogTextBox.Text = "";

            ///右側の画面出力はクリアしなくてもいい？
            //SerialLogTextBox2.Document.Blocks.Clear();
        }

        /**** 以下イベントハンドラ ****/

        /// <summary>
        /// デバッグ用．後で消す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PrintWarning("test");
        }

        /// <summary>
        /// シリアルポートからデータを受信した際に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 表示している文字列を消去
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuTextItemClear_Click(object sender, RoutedEventArgs e)
        {
            DisplayClear();
        }

        /// <summary>
        /// アプリケーションの終了 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuApplicationShutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// シリアルポートを切断する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemSerialPortClose_Click(object sender, RoutedEventArgs e)
        {
            _serialPort.Close();
            PrintInfo("Disconnected with " + _serialPort.PortName);
        }

        /// <summary>
        /// シリアルポートと接続するための処理を実行．
        /// もともとシリアルポートと接続していた場合は切断する．
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemConnectSerialPort_Click(object sender, RoutedEventArgs e)
        {
            ConnectSerialInterface();
        }

        /// <summary>
        /// 入力受け取り
        /// たぶん黒い画面にフォーカスがあってればこのイベントハンドラに入るはず
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UniformGrid_TextInput(object sender, TextCompositionEventArgs e)
        {
            SerialWriteString(e.Text);
        }
    }
}
