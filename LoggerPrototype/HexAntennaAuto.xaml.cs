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
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace LoggerPrototype
{
    /// <summary>
    /// HexAntennaAuto.xaml の相互作用ロジック
    /// </summary>
    public partial class HexAntennaAuto : Window
    {
        /// <summary>
        /// シリアル通信で文字列を送信する関数
        /// </summary>
        public Action<string> SerialWriteString;


        /// <summary>
        /// タイマ割り込み用
        /// </summary>
        Timer GlobalTimer;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HexAntennaAuto()
        {
            InitializeComponent();

            AutoHAEndBtn.IsEnabled = false;

            NoSignalEnable.IsChecked = true;
            VerticalEnable.IsChecked = true;
            HorizontalEnable.IsChecked = false;

            //初期値代入
            NoSignalValue.Text = (100).ToString();
            VerticalValue.Text = (100).ToString();
            HorizontalValue.Text = (100).ToString();
        }

        /// <summary>
        /// 6面体アンテナ用コマンド送信関数
        /// </summary>
        /// <param name="vorh">0:vertiacal, 1:horizontal</param>
        /// <param name="n">0で無信号区間の生成</param>
        private void SendHexAntennaCmd(uint vorh, uint n = 0)
        {
            if (1 <= n && n <= 6)
                SerialWriteString("*A" + (vorh == 0 ? "V" : "H") + n.ToString());
            else
                SerialWriteString("*AVM");
        }

        /// <summary>
        /// 6面体アンテナ用コマンド送信関数
        /// </summary>
        /// <param name="HAS"></param>
        private void SendHexAntennaCmd(HexAntStr HAS)
        {
            SendHexAntennaCmd(HAS.VorH, HAS.N);
        }

        /**** 以下イベントハンドラ ****/

        /// <summary>
        /// 開始ボタンのクリック処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoHAStartBtn_Click(object sender, RoutedEventArgs e)
        {
            AutoHAStartBtn.IsEnabled = false;
            AutoHAEndBtn.IsEnabled = true;

            NoSignalEnable.IsEnabled = false;
            VerticalEnable.IsEnabled = false;
            HorizontalEnable.IsEnabled = false;

            DateTime dateTime = DateTime.Now;
            ulong timeStamp = 0;
            int num = 0;
            uint noSignalTime = uint.Parse(NoSignalValue.Text);
            uint verticalTime = uint.Parse(VerticalValue.Text);
            uint horizontalTime = uint.Parse(HorizontalValue.Text);
           
            var antList = new List<HexAntStr>();
            if (NoSignalEnable.IsChecked == true)
            {
                antList.Add(new HexAntStr(0, 0, noSignalTime));
            }
            if(VerticalEnable.IsChecked == true)
            {
                for(uint i = 1; i <= 6; i++)
                    antList.Add(new HexAntStr(0, i, verticalTime));
            }
            if (HorizontalEnable.IsChecked == true)
            {
                for (uint i = 1; i <= 6; i++)
                    antList.Add(new HexAntStr(1, i, horizontalTime));
            }

            TimerCallback cb = state =>
            {
                var ts = DateTime.Now - dateTime;
                if(ts.TotalMilliseconds > timeStamp)
                {
                    timeStamp += antList[num].Time;
                    SendHexAntennaCmd(antList[num]);

                    num++;
                    num %= antList.Count();
                }
            };

            GlobalTimer = new Timer(cb, null, 0, 1);
        }

        /// <summary>
        /// 終了ボタンのクリック処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoHAEndBtn_Click(object sender, RoutedEventArgs e)
        {
            AutoHAStartBtn.IsEnabled = true;
            AutoHAEndBtn.IsEnabled = false;

            NoSignalEnable.IsEnabled = true;
            VerticalEnable.IsEnabled = true;
            HorizontalEnable.IsEnabled = true;

            GlobalTimer.Dispose();
        }

        /// <summary>
        /// 数字に制限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoSignalTexitBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 0-9のみ
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }

        /// <summary>
        /// 数字に制限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VerticalTexitBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 0-9のみ
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }

        /// <summary>
        /// 数字に制限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HorizontalTexitBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 0-9のみ
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }
    }

    public class HexAntStr
    {
        public uint VorH;
        public uint N;
        public uint Time;
        public HexAntStr(uint vorh, uint n, uint time)
        {
            VorH = vorh;
            N = n;
            Time = time;
        }
    }
}
