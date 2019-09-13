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
using System.Threading;

namespace LoggerPrototype
{
    /// <summary>
    /// HexAntenna.xaml の相互作用ロジック
    /// </summary>
    public partial class HexAntenna : Window
    {
        /// <summary>
        /// シリアル通信で文字列を送信する関数
        /// </summary>
        public Action<string> SerialWriteString;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HexAntenna()
        {
            InitializeComponent();
        }

        /**** 以下イベントハンドラ ****/
        /**** 6面＊垂直/水平で12個 ****/

        private void AV1_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV1" + Environment.NewLine);
        }

        private void AH1_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH1" + Environment.NewLine);
        }

        private void AV2_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV2" + Environment.NewLine);
        }

        private void AH2_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH2" + Environment.NewLine);
        }

        private void AV3_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV3" + Environment.NewLine);
        }

        private void AH3_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH3" + Environment.NewLine);
        }

        private void AV4_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV4" + Environment.NewLine);
        }

        private void AH4_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH4" + Environment.NewLine);
        }

        private void AV5_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV5" + Environment.NewLine);
        }

        private void AH5_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH5" + Environment.NewLine);
        }

        private void AV6_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV6" + Environment.NewLine);
        }

        private void AH6_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH6" + Environment.NewLine);
        }
    }
}
