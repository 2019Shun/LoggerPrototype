using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerPrototype
{
    /// <summary>
    /// 画面に表示する文字列を管理するクラス
    /// </summary>
    class DisplayString
    {
        /// <summary>
        /// 表示する文字列を保持するStringBuilderクラス
        /// </summary>
        private StringBuilder _contents;

        /// <summary>
        /// 表示する文字列のバッファ長
        /// </summary>
        public int BufferLength { get; set; }

        /// <summary>
        /// バッファ長を超えた場合に削除する長さ
        /// </summary>
        public int RemoveLength { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="bufferLength"></param>
        /// <param name="removeLength"></param>
        public DisplayString(int bufferLength = 1000, int removeLength = 100)
        {
            BufferLength = bufferLength;
            RemoveLength = removeLength;
            _contents = new StringBuilder();
        }

        /// <summary>
        /// 表示する文字列を追加する
        /// </summary>
        /// <param name="str"></param>
        public void Append(string str)
        {
            if(_contents.Length > BufferLength)
            {
                _contents.Remove(0, RemoveLength);
            }
            _contents.Append(str);
        }

        /// <summary>
        /// TextBox.Textに代入するstringクラスを取得
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            return _contents.ToString();
        }

        public string GetString(int num)
        {
            if(_contents.Length > num)
            {
                return _contents.ToString().Substring(_contents.Length - num);
            }
            else
            {
                return _contents.ToString();
            }
        }

        /// <summary>
        /// 表示している文字列を消去
        /// </summary>
        public void Clear()
        {
            /// StringBuilderの内容を消去
            _contents.Length = 0;
        }
    }
}
