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
    /// PacketSend.xaml の相互作用ロジック
    /// </summary>
    public partial class PacketSend : Window
    {
        /// <summary>
        /// シリアル通信で文字列を送信する関数
        /// </summary>
        public Action<string> SerialWriteString;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PacketSend()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 送信ボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString(SendTextBox.Text);
        }

        /// <summary>
        /// クリアボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            SendTextBox.Clear();
        }
    }
}
