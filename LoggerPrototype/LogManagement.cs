﻿using System;
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
        /// LogControlに保存容量を表示するための関数
        /// </summary>
        public Action<int> SetSaveFileCapacity;

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

            File.AppendAllText(_saveFileName, str);
            Encoding sjisEnc = Encoding.GetEncoding("UTF-8");
            _saveFileCapacity += sjisEnc.GetByteCount(str);
            SetSaveFileCapacity(_saveFileCapacity);
        }

        public void SetEnableWriting(bool isEnable)
        {
            _saveFileCapacity = 0;
            _enableWriting = isEnable;
        }

        public void SetSaveFilePath(string fn)
        {
            _saveFileName = fn;
        }

        public int GetSaveFileCapacity()
        {
            return _saveFileCapacity;
        }
    }
}