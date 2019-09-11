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
            SerialWriteString("*AV1");
        }

        private void AH1_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH1");
        }

        private void AV2_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV2");
        }

        private void AH2_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH2");
        }

        private void AV3_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV3");
        }

        private void AH3_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH3");
        }

        private void AV4_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV4");
        }

        private void AH4_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH4");
        }

        private void AV5_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV5");
        }

        private void AH5_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH5");
        }

        private void AV6_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AV6");
        }

        private void AH6_Click(object sender, RoutedEventArgs e)
        {
            SerialWriteString("*AH6");
        }
    }
}
