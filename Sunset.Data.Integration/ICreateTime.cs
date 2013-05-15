using System;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 建立日期，用來排序調代課記錄
    /// </summary>
    public interface ICreateTime
    {
        /// <summary>
        /// 建立日期
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}