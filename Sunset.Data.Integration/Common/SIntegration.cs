using System.Collections.Generic;
using System.Text;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 從來源學校下載資料資訊
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SIntegrationResult<T>
    {
        //內部使用訊息記錄器
        private StringBuilder MessageBuilder;

        /// <summary>
        /// 建構式，初始化屬性值
        /// </summary>
        public SIntegrationResult()
        {
            IsSuccess = true;
            Data = new List<T>();
            MessageBuilder = new StringBuilder();
        }

        /// <summary>
        /// 加入資料下載訊息
        /// </summary>
        /// <param name="Message"></param>
        internal void AddMessage(string Message)
        {
            if (!string.IsNullOrEmpty(Message))
                MessageBuilder.AppendLine(Message);
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; internal set; }

        /// <summary>
        /// 下載訊息
        /// </summary>
        public string Message { get { return MessageBuilder.ToString(); } }

        /// <summary>
        /// 結果
        /// </summary>
        public List<T> Data { get; internal set; }
    }
}