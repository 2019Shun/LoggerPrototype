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
using System.Windows.Shapes;

namespace LoggerPrototype
{
    /// <summary>
    /// SelectSerialPort.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectSerialPort : Window
    {
        public Action<string, int> OpenSerialPort;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SelectSerialPort()
        {
            InitializeComponent();

            SetSerialPortName();
            SetBaudRate();
            Topmost = true;
        }

        /// <summary>
        /// 現在接続されているSerialPort名を取得し，プルダウンメニューに表示
        /// </summary>
        public void SetSerialPortName()
        {
            var CheckComNum = new System.Text.RegularExpressions.Regex("COM[1-9][0-9]?[0-9]?");

            System.Management.ManagementClass mcPnPEntity = new System.Management.ManagementClass("Win32_PnPEntity");
            System.Management.ManagementObjectCollection manageObjCol = mcPnPEntity.GetInstances();

            foreach (System.Management.ManagementObject manageObj in manageObjCol)
            {
                var namePropertyValue = manageObj.GetPropertyValue("Name");
                if (namePropertyValue == null)
                {
                    continue;
                }
                string name = namePropertyValue.ToString();

                if (CheckComNum.IsMatch(name))
                {
                    SerialComPort.Items.Add(name);
                }
            }
            SerialComPort.SelectedIndex = 0;
        }

        /// <summary>
        /// 選択可能なボーレートをプルダウンメニューに表示
        /// 必要な場合はここに追加
        /// </summary>
        public void SetBaudRate()
        {
            int[] baudRate = { 4800, 9600, 115200 };
            foreach(var i in baudRate)
            {
                SerialBaudRate.Items.Add(i.ToString());
            }
            SerialBaudRate.SelectedIndex = 1;
        }

        /// <summary>
        /// 選択したSerialPort名を取得
        /// </summary>
        /// <returns></returns>
        public string GetSelectSerialPortName()
        {
            var ExtractPortNum = new System.Text.RegularExpressions.Regex(".*(COM[1-9][0-9]?[0-9]?).*");
            if (SerialComPort.SelectedItem == null)
            {
                //textBoxTextArea.Text += "No Port Selected\n";
                return System.String.Empty;
            }
            string name = (string)SerialComPort.SelectedItem;
            return ExtractPortNum.Replace(name, "$1");
        }

        /// <summary>
        /// 選択したBaudRateを取得
        /// </summary>
        /// <returns></returns>
        public int GetSelectBaudRate()
        {
            string baudRateValue = (string)SerialBaudRate.SelectedItem;
            return int.Parse(baudRateValue);
        }

        /**** 以下イベントハンドラ ****/

        private void SerialStartBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenSerialPort(GetSelectSerialPortName(), GetSelectBaudRate());
            Close();
        }

        /// <summary>
        /// SelectSerialPort Windowを閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// SerialPortの名前を再取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            SerialComPort.Items.Clear();
            SetSerialPortName();
        }
    }
}
