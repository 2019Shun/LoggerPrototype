using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// LogControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LogControl : Window
    {
        /// <summary>
        /// ログを取る際の許可
        /// </summary>
        public Action<bool> SetEnableWriting;

        /// <summary>
        /// ログを保存するパス名
        /// </summary>
        public Action<string> SetSaveFilePath;

        /// <summary>
        /// Mainの右側画面に情報を表示する
        /// </summary>
        public Action<string> PrintInfo;

        /// <summary>
        /// 現在のコース番号
        /// </summary>
        private int _courseNum;
        
        /// <summary>
        /// 現在のコース番号をstringで扱う際のプロパティ
        /// </summary>
        private string courseNum
        {
            get { return _courseNum.ToString(); }
            set
            {
                try
                {
                    _courseNum = int.Parse(value);
                    SaveCourse.Text = value;
                }
                catch
                {
                    _courseNum = 0;
                    SaveCourse.Text = "0";
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LogControl()
        {
            InitializeComponent();

            LogTemplBtn.IsEnabled = false;
            LogEndBtn.IsEnabled = false;

            courseNum = "0";
        }

        /// <summary>
        /// 保存したファイルの容量を画面に表示
        /// </summary>
        /// <param name="value"></param>
        public void SetSaveFileCapacity(int value)
        {
            SaveCapacity.Dispatcher.Invoke(
                new Action(() =>
                {
                    SaveCapacity.Text = value.ToString() + " [Byte]";
                })
            );
            
        }

        /// <summary>
        /// 画面の情報から保存パスを取得
        /// </summary>
        /// <returns></returns>
        private string GetSaveFilePath()
        {
            return SaveFolderTextBox.Text + "\\" + SaveFilePrefix.Text + courseNum + ".log";
        }

        /**** 以下イベントハンドラ ****/

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            // フォルダー参照ダイアログのインスタンスを生成
            var dlg = new System.Windows.Forms.FolderBrowserDialog();

            // 説明文を設定
            dlg.Description = "フォルダーを選択してください。";

            // ダイアログを表示
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたフォルダーパスをメッセージボックスに表示
                SaveFolderTextBox.Text = dlg.SelectedPath;
            }
        }

        private void LogStartBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SaveFolderTextBox.Text == "")
            {
                MessageBox.Show("保存するフォルダを選択してください．", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SaveCourse.Text == "")
            {
                MessageBox.Show("コースの開始番号を入力してください", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LogStartBtn.IsEnabled = false;
            LogTemplBtn.IsEnabled = true;
            LogEndBtn.IsEnabled = true;
            SetSaveFilePath(GetSaveFilePath());
            SetEnableWriting(true);

            PrintInfo("パス名\"" + GetSaveFilePath() + "\"で保存を開始します．");
        }

        private void LogTemplBtn_Click(object sender, RoutedEventArgs e)
        {
            LogStartBtn.IsEnabled = true;
            LogTemplBtn.IsEnabled = false;
            courseNum = (_courseNum + 1).ToString();
            SetEnableWriting(false);

            PrintInfo("測定を一時停止します．");
        }

        private void LogEndBtn_Click(object sender, RoutedEventArgs e)
        {
            LogStartBtn.IsEnabled = true;
            LogTemplBtn.IsEnabled = false;
            LogEndBtn.IsEnabled = false;
            courseNum = "0";
            SetEnableWriting(false);

            PrintInfo("測定を終了します．");
        }


        private void SaveCourse_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 0-9のみ
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }

        private void SaveCourse_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // 貼り付けを許可しない
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void SaveFilePrefix_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //ファイル名に使用できない文字を取得
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            e.Handled = !(e.Text.IndexOfAny(invalidChars) < 0);
        }

        private void SaveCourse_TextChanged(object sender, TextChangedEventArgs e)
        {
            courseNum = SaveCourse.Text;
        }
    }
}
