using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunset.Data
{
    /// <summary>
    /// 節次集合，集合依照星期及節次對應的時間來排序
    /// </summary>
    public class Periods :IEnumerable<Period>
    {
        private List<Period> mPeriods;
        private List<Period> mRandomPeriods;
        private int mRandomIndex = 0;

        /// <summary>
        /// 建構式
        /// </summary>
        public Periods()
        {
            //初始化建構式
            mPeriods = new List<Period>();
        }

        /// <summary>
        /// 將節次打亂
        /// </summary>
        public void RandomPeriods()
        {
            mRandomPeriods = new List<Period>();
            mRandomPeriods.AddRange(mPeriods.FindAll(x=>!x.Disable)); //將原本節次新增
            mRandomPeriods.Shuffle();          //打亂節次
            mRandomIndex = 0;                  //將索引重設
        }

        /// <summary>
        /// 取得亂數節次
        /// </summary>
        /// <returns></returns>
        public Period GetNextRandomPeriod()
        {
            try
            {
                return mRandomPeriods[mRandomIndex++];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 新增節次
        /// </summary>
        /// <param name="NewPeriod">節次物件</param>
        /// <returns>新增的節次物件，若是沒有新增成功則回傳null。</returns>
        /// <remarks>需再重寫，根據星期及時間排序。</remarks>
        public Period Add(Period NewPeriod)
        {
            int nPos = this.GetSortOrder(NewPeriod);

            if (nPos >= mPeriods.Count)
                mPeriods.Add(NewPeriod);
            else
                mPeriods.Insert(nPos, NewPeriod);

            return NewPeriod;

            //留意，在新版中未考慮到Period重覆的情況

            #region VB
            //Public Function Add(NewPeriod As Period) As Period
            //    Dim sKey As String
            //    Dim sInsKey As String
            //    Dim prdMember As Period

            //    'determine sKey
            //    sKey = "W" & NewPeriod.WeekDay & "P" & NewPeriod.PeriodNo

            //    'Determine insert position
            //    For Each prdMember In mCol
            //        If NewPeriod.WeekDay + CDbl(NewPeriod.BeginTime) _
            //            < prdMember.WeekDay + CDbl(prdMember.BeginTime) Then
            //            sInsKey = "W" & prdMember.WeekDay & "P" & prdMember.PeriodNo
            //            Exit For
            //        End If
            //    Next

            //    On Error GoTo DupPeriod
            //    'set the properties passed into the method
            //    If sInsKey = "" Then
            //        mCol.Add NewPeriod, sKey
            //    Else
            //        mCol.Add NewPeriod, sKey, sInsKey
            //    End If

            //    ' Recalculate MaxWeekDay & MinWeekDay
            //    mMinWeekDay = mCol(1).WeekDay
            //    mMaxWeekDay = mCol(mCol.Count).WeekDay

            //    'return the object created
            //    Set Add = NewPeriod
            //    Exit Function
            //DupPeriod:
            //    Set Add = Nothing
            //End Function
            #endregion
        }

        /// <summary>
        /// 重設節次集合
        /// </summary>
        public void Clear()
        {
            //將節次集合清空，並且新增新的節次集合
            mPeriods.Clear();
            mPeriods = new List<Period>();

            #region VB
            //Public Sub Clear()
            //    Set mCol = Nothing
            //    Set mCol = New Collection
            //    mMinWeekDay = 0
            //    mMaxWeekDay = 0
            //End Sub
            #endregion
        }

        /// <summary>
        /// 根據星期幾及顯示節次取得節次物件
        /// </summary>
        /// <param name="WeekDay">星期幾</param>
        /// <param name="DisplayPeriod">顯示節次</param>
        /// <returns>節次物件，若是沒有對應的則傳回null。</returns>
        public Period GetDisplayPeriod(int WeekDay, int DisplayPeriod)
        {
            return mPeriods.Find(x => x.WeekDay == WeekDay 
                && x.DisplayPeriod == DisplayPeriod 
                && !x.Disable);

            #region VB
            //Public Function GetDispPeriod(ByVal WeekDay As Integer, ByVal DispPeriod As Integer) As Period
            //    Dim prdMember As Period

            //    Set GetDispPeriod = Nothing
            //    For Each prdMember In mCol
            //        If prdMember.WeekDay > WeekDay Then Exit For
            //        If prdMember.DispPeriod = DispPeriod Then
            //            Set GetDispPeriod = prdMember
            //            Exit For
            //        End If
            //    Next
            //End Function
            #endregion
        }

        /// <summary>
        /// 根據星期幾及節次編號取得節次物件
        /// </summary>
        /// <param name="WeekDay">星期幾</param>
        /// <param name="PeriodNo">節次編號</param>
        /// <returns>節次物件，若是沒有對應的則傳回null。</returns>
        public Period GetPeriod(int WeekDay, int PeriodNo)
        {
            return mPeriods.Find(x => x.WeekDay == WeekDay 
                && x.PeriodNo == PeriodNo
                && !x.Disable);

            #region VB
            //Public Function GetPeriod(ByVal WeekDay As Integer, ByVal PeriodNo As Integer) As Period
            //    On Error GoTo NoSuchPeriod
            //    Set GetPeriod = mCol("W" & WeekDay & "P" & PeriodNo)
            //    Exit Function
            //NoSuchPeriod:
            //    Set GetPeriod = Nothing
            //End Function
            #endregion
        }

        public Period GetPeriodIgroneDisable(int Weekday, int PeriodNo)
        {
            return mPeriods.Find(x => x.WeekDay == Weekday
               && x.PeriodNo == PeriodNo);
        }

        /// <summary>
        /// 節次集合中最大的星期
        /// </summary>
        public int MaxWeekDay 
        {
            get 
            {
                return mPeriods.Count>0? mPeriods.Select(x=>x.WeekDay).Max():0;

                #region VB
                //Public Function GetMaxWeekDay() As Integer
                //    GetMaxWeekDay = mMaxWeekDay
                //End Function
                #endregion
            } 
        }

        /// <summary>
        /// 節次集合中最小的星期
        /// </summary>
        public int MinWeekDay
        {
            get
            {
                return mPeriods.Count >0? mPeriods.Select(x=>x.WeekDay).Min():0;

                #region VB
                //Public Function GetMinWeekDay() As Integer
                //    GetMinWeekDay = mMinWeekDay
                //End Function
                #endregion
            }
        }

        /// <summary>
        /// 節次集合中最大的節次編號
        /// </summary>
        public int MaxPeriodNo 
        {
            get
            {
                return mPeriods.Count >0? mPeriods.Select(x => x.PeriodNo).Max():0;

                #region VB
                //Public Function GetMaxPeriodNo() As Integer
                //    Dim prdMember As Period
                //    Dim nMaxPN As Integer

                //    For Each prdMember In mCol
                //        If prdMember.PeriodNo > nMaxPN Then nMaxPN = prdMember.PeriodNo
                //    Next
                //    GetMaxPeriodNo = nMaxPN
                //End Function
                #endregion
            }
        }

        /// <summary>
        /// 節次集合中最大的顯示節次
        /// </summary>
        public int MaxDisplayPeriod 
        {
            get
            {
                return mPeriods.Count >0? mPeriods.Select(x => x.DisplayPeriod).Max():0;

                #region VB
                //Public Function GetMaxDispPeriod() As Integer
                //    Dim prdMember As Period
                //    Dim nMaxDP As Integer

                //    For Each prdMember In mCol
                //        If prdMember.DispPeriod > nMaxDP Then nMaxDP = prdMember.DispPeriod
                //    Next
                //    GetMaxDispPeriod = nMaxDP
                //End Function
                #endregion
            }
        }

        private int Parse(string Value)
        {
            int i;

            if (int.TryParse(Value, out i))
                return i;
            else
                return 0;
        }

        /// <summary>
        /// 根據鍵值移除對應的節次
        /// </summary>
        /// <param name="Key">鍵值組合：W+星期幾+P+節次編號</param>
        public void Remove(string Key)
        {
            //鍵值的組合一定會大於等於4
            if (Key.Length >= 4)
            {
                try
                {
                    //取得星期幾，由於星期幾只會在1~7，故只有一位數
                    int W = Parse(Key.Substring(1, 1));

                    //取得節次編號，節次一定從第4個位置開始
                    int P = Parse(Key.Substring(3, Key.Length - 3));

                    //根據星期幾及節次編號取得節次
                    Period Period = mPeriods.Find(x => x.WeekDay == W && x.PeriodNo == P);

                    //假設節次不為null就移除該節次
                    if (Period != null)
                        mPeriods.Remove(Period);
                }
                catch (Exception e)
                {
                    //待撰寫...
                }
            }

            #region VB
            //Public Sub Remove(vntIndexKey As Variant)
            //    mCol.Remove vntIndexKey

            //    ' Recalculate MaxWeekDay & MinWeekDay
            //    If mCol.Count = 0 Then
            //        mMaxWeekDay = 0
            //        mMinWeekDay = 0
            //    Else
            //        mMinWeekDay = mCol(1).WeekDay
            //        mMaxWeekDay = mCol(mCol.Count).WeekDay
            //    End If
            //End Sub
            #endregion
        }

        /// <summary>
        /// 根據星期條件取得中午休息時間節次
        /// </summary>
        /// <param name="WeekDay">星期幾</param>
        /// <returns>節次物件</returns>
        public Period GetBreakPeriod(int WeekDay)
        {
            //在內部儲存中午休息時間節次編號為0
            return GetPeriod(WeekDay, 0);

            #region VB
            //Public Function GetBreakPeriod(ByVal WeekDay As Integer) As Period
            //    Set GetBreakPeriod = GetPeriod(WeekDay, 0)
            //End Function
            #endregion
        }

        /// <summary>
        /// 根據星期條件取得第一節（依節次編號）；假設星期條件為0, 根據最小的星期來取得；否則根據指定的星期。
        /// </summary>
        /// <param name="WeekDay">星期幾</param>
        /// <returns>節次物件</returns>
        public Period FirstPeriod(int WeekDay)
        {
            //假設星期條件為0, 根據最小的星期來取得；否則根據指定的星期
            return WeekDay == 0 ? GetPeriod(MinWeekDay, 1) : GetPeriod(WeekDay, 1);

            #region VB
            //Public Function FirstPeriod(ByVal WeekDay As Integer) As Period
            //    If WeekDay = 0 Then
            //        Set FirstPeriod = GetPeriod(mMinWeekDay, 1)
            //    Else
            //        Set FirstPeriod = GetPeriod(WeekDay, 1)
            //    End If
            //End Function
            #endregion
        }

        /// <summary>
        /// 根據星期條件取得最後一節（依節次編號）
        /// </summary>
        /// <param name="WeekDay">星期幾</param>
        /// <returns>節次物件</returns>
        public Period LastPeriod(int WeekDay)
        {
            //假設星期為0，則將星期設為集合中最大的星期
            if (WeekDay == 0)
                WeekDay = MaxWeekDay;

            //根據星期篩選出節次，並依節次編號排序，然後取得最後一個元素
            try
            {
                return mPeriods.FindAll(x => x.WeekDay == WeekDay).OrderBy(x => x.PeriodNo).Last();
            }
            catch
            {
                return null;
            }

            #region VB
            //Public Function LastPeriod(ByVal WeekDay As Integer) As Period
            //    Dim prdTest As Period
            //    Dim prdSave As Period

            //    If WeekDay = 0 Then WeekDay = mMaxWeekDay
            //    Set prdTest = FirstPeriod(WeekDay)
            //    If prdTest Is Nothing Then
            //        Set LastPeriod = Nothing
            //    Else
            //        Do
            //            Set prdSave = prdTest
            //            Set prdTest = GetPeriod(WeekDay, prdTest.PeriodNo + 1)
            //        Loop Until prdTest Is Nothing
            //        Set LastPeriod = prdSave
            //    End If
            //End Function
            #endregion
        }

        /// <summary>
        /// 取得下一節
        /// </summary>
        /// <param name="WeekDay">星期幾</param>
        /// <param name="PeriodNo">節次編號</param>
        /// <returns>節次物件</returns>
        public Period NextPeriod(int WeekDay, int PeriodNo)
        {
            //依星期及節次編號取得下一節
            Period prdTest = GetPeriod(WeekDay, PeriodNo + 1);

            //假設回傳的為null
            if (prdTest == null)
            {
                //將星期加1
                WeekDay++;

                //當星期大於最大星期或是節次不為null時跳出迴圈
                while((WeekDay <= MaxWeekDay) && (prdTest==null))
                {
                    prdTest = FirstPeriod(WeekDay);
                    WeekDay++;
                };
            }

            return prdTest;

            #region VB
            //Public Function NextPeriod(ByVal WeekDay As Integer, ByVal PeriodNo As Integer) As Period
            //    Dim prdTest As Period

            //    Set prdTest = GetPeriod(WeekDay, PeriodNo + 1)
            //    If prdTest Is Nothing Then
            //        WeekDay = WeekDay + 1
            //        Do Until (WeekDay > mMaxWeekDay) Or (Not (prdTest Is Nothing))
            //            Set prdTest = FirstPeriod(WeekDay)
            //            WeekDay = WeekDay + 1
            //        Loop
            //    End If
            //    Set NextPeriod = prdTest
            //End Function
            #endregion
        }

        /// <summary>
        /// 取得前一節
        /// </summary>
        /// <param name="WeekDay">星期幾</param>
        /// <param name="PeriodNo">節次編號</param>
        /// <returns>節次物件</returns>
        public Period PrevPeriod(int WeekDay, int PeriodNo)
        {
            Period prdTest = null;

            //假設節次大於1，取得前一前節次；節次0為中午休息節次，故不列入考量。
            if (PeriodNo > 1)
                prdTest = GetPeriod(WeekDay, PeriodNo - 1);

            if (prdTest == null)
            {
                WeekDay--;

                //當星期小於最小星期或是節次不為null時跳出迴圈
                while((WeekDay >= MinWeekDay) || (prdTest==null))
                {
                    prdTest = LastPeriod(WeekDay);
                    WeekDay--;
                } 
            }

            return prdTest;

            #region VB
            //Public Function PrevPeriod(ByVal WeekDay As Integer, ByVal PeriodNo As Integer) As Period
            //    Dim prdTest As Period

            //    If PeriodNo > 1 Then
            //        Set prdTest = GetPeriod(WeekDay, PeriodNo - 1)
            //    End If

            //    If prdTest Is Nothing Then
            //        WeekDay = WeekDay - 1
            //        Do Until (WeekDay < mMinWeekDay) Or (Not (prdTest Is Nothing))
            //            Set prdTest = LastPeriod(WeekDay)
            //            WeekDay = WeekDay - 1
            //        Loop
            //    End If
            //    Set PrevPeriod = prdTest
            //End Function
            #endregion
        }

        /// <summary>
        /// 根據鍵值取得節次
        /// </summary>
        /// <param name="Key">鍵值為星期及節次編號的組合</param>
        /// <returns>節次物件，若是鍵值不存在則傳回null</returns>
        public Period this[string Key]
        {
            get 
            {
                if (Key.Length >= 4)
                {
                    try
                    {
                        //取得星期
                        int W = Parse(Key.Substring(1, 1));

                        //取得節次編號
                        int P = Parse(Key.Substring(3, Key.Length - 3));

                        return mPeriods.Find(x => x.WeekDay == W && x.PeriodNo == P);
                    }
                    catch
                    {
                        return null;
                    }
                } 
                
                return null;
            }
        }

        public Period this[int Index]
        {
            get
            {
                return Index>=0 && Index<Count ? mPeriods[Index] : null;

                #region VB
                //Public Property Get Item(vntIndexKey As Variant) As Period
                //    Set Item = mCol(vntIndexKey)
                //End Property
                #endregion
            }
        }

        /// <summary>
        /// 節次集合數量
        /// </summary>
        public int Count 
        { 
            get 
            { 
                return mPeriods.Count;

                #region VB
                //Public Property Get Count() As Long
                //    Count = mCol.Count
                //End Property
                #endregion
            }
        }

        #region IEnumerable<Period> Members

        public IEnumerator<Period> GetEnumerator()
        {
            return mPeriods.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mPeriods.GetEnumerator();
        }

        #endregion
    }
}