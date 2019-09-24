using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LoggerPrototype
{
    class LogManagement
    {
        /// <summary>
        /// ログを取る際の許可
        /// </summary>
        private bool _enableWriting;

        /// <summary>
        /// 保存するファイル名
        /// </summary>
        private string _saveFileName;

        /// <summary>
        /// 新しいファイルになってからの保存容量
        /// </summary>
        private int _saveFileCapacity;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LogManagement()
        {
            _enableWriting = false;
            _saveFileCapacity = 0;
        }

        /// <summary>
        /// Mainから呼び出して文字列を受け取る
        /// 書き込むか否かはLogControlからの指示による
        /// </summary>
        /// <param name="str"></param>
        public void LogManagementString(string str)
        {
            if(_enableWriting == false)
            {
                return;
            }

            SaveTextFile(str);
        }

        /// <summary>
        /// ファイル書き込みの許可
        /// </summary>
        /// <param name="isEnable"></param>
        public void SetEnableWriting(bool isEnable)
        {
            _saveFileCapacity = 0;
            _enableWriting = isEnable;
        }

        /// <summary>
        /// ファイル名を設定する
        /// </summary>
        /// <param name="fn"></param>
        public void SetSaveFilePath(string fn)
        {
            _saveFileName = fn;
        }

        /// <summary>
        /// 保存したファイルサイズを取得
        /// </summary>
        /// <returns></returns>
        public int GetSaveFileCapacity()
        {
            return _saveFileCapacity;
        }

        private void SaveTextFile(string str)
        {
            File.AppendAllText(_saveFileName, str);
            Encoding sjisEnc = Encoding.GetEncoding("UTF-8");
            _saveFileCapacity += sjisEnc.GetByteCount(str);
        }
    }
}
