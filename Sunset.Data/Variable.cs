
using System;
namespace Sunset.Data
{
    /// <summary>
    /// 星期及節次變數
    /// </summary>
    public class Variable
    {
        //'local variable(s) to hold property value(s)
        private string mName;
        private long[,] mValues;
        private int mCount;
        private int mValueNo;
        private int mWhich;

        #region VB
        //Private mName As String
        //Private mValues() As Long
        //Private mCount As Integer
        //Private mValueNo As Integer
        //Private mWhich As Integer
        #endregion

        /// <summary>
        /// 值為CEvent的WeekDayVar或是PeriodVar，例如(a3)
        /// </summary>
        /// <param name="Expression"></param>
        public Variable(string Expression)
        {
            //解析左右括號的位置
            int nLeftP = Expression.IndexOf("(",0);
            int nRightP = Expression.IndexOf(")",0);

            //假設有左括號
            if (nLeftP >= 0)
            {
                //右括號的位置不能小於左括號位置
                if (nRightP < nLeftP) return;

                //左括號位置不能小於1
                if (nLeftP < 1)
                    return;
                else
                    mName = Expression;

                int Count;

                if (int.TryParse(Expression.Substring(nLeftP + 1, nRightP - nLeftP - 1), out Count))
                {
                    mCount = Count;
                    mValueNo = -1;
                    mValues = new long[mCount, 2];
                }
                else
                {
                    mCount = 1;
                    mWhich = 0;
                    mValueNo = -1;
                    mValues = new long[1, 2];
                }
            }
            else
            {
                mName = Expression;
                mCount = 1;
                mWhich = 0;
                mValueNo = -1;
                mValues = new long[1,2];
            }

            #region VB
            //Public Sub SetVariable(ByVal Expression As String)
            //    Dim nLeftP As Integer
            //    Dim nRightP As Integer

            //    nLeftP = InStr(1, Expression, "(")
            //    nRightP = InStr(1, Expression, ")")
            //    If nLeftP > 0 Then
            //        If nRightP < nLeftP Then Exit Sub
            //        If nLeftP < 2 Then
            //            Exit Sub
            //        Else
            //            mName = Expression
            //        End If
            //        mCount = CInt(Mid(Expression, nLeftP + 1, nRightP - nLeftP - 1))
            //        mWhich = 1
            //        mValueNo = 0
            //        ReDim mValues(1 To mCount, 1 To 2) As Long
            //    Else
            //        mName = Expression
            //        mCount = 1
            //        mWhich = 1
            //        mValueNo = 0
            //        ReDim mValues(1 To 1, 1 To 2) As Long
            //    End If
            //End Sub
            #endregion
        }

        public void UseValue(int Which)
        {
            if (Which < 0) Which = 0;

            if (Which > mValueNo)
                mWhich = mValueNo;
            else
                mWhich = Which;

            #region VB
            //Public Sub UseValue(ByVal Which As Integer)
            //    If Which < 1 Then Which = 1
            //    If Which > mValueNo Then
            //        mWhich = mValueNo
            //    Else
            //        mWhich = Which
            //    End If
            //End Sub
            #endregion
        }

        public bool Fit(long TestValue)
        {
            if (mValueNo < mCount) return true;

            for (int i = 0; i <= mValueNo; i++)
            {
                if (mValues[i, 0] == TestValue)
                    return true;
            }

            return false;

            #region VB
            //Public Function Fit(ByVal TestValue As Long) As Boolean
            //    Dim nIndex As Integer

            //    Fit = True
            //    If mValueNo < mCount Then Exit Function
            //    For nIndex = 1 To mValueNo
            //        If mValues(nIndex, 1) = TestValue Then Exit Function
            //    Next nIndex

            //    Fit = False
            //End Function
            #endregion
        }

        public bool IsValid()
        {
            return mCount > 0;

            #region VB
            //Public Function IsValid() As Boolean
            //    IsValid = (mCount > 0)
            //End Function
            #endregion
        }

        public void AddValue(long NewValue)
        {
            if (!IsValid()) return;

            try
            {

                int nIndex = 0;

                while (nIndex <= mValueNo)
                {
                    if (mValues[nIndex, 0] == NewValue)
                    {
                        mValues[nIndex, 1]++;
                        break;
                    }

                    nIndex++;
                }

                if (nIndex > mValueNo)
                {
                    if (mValueNo < mCount)
                    {
                        mValueNo++;
                        mValues[mValueNo, 0] = NewValue;
                        mValues[mValueNo, 1] = 1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            #region VB
            //Public Sub AddValue(ByVal NewValue As Long)
            //    Dim nIndex As Integer
            //    If Not IsValid() Then Exit Sub
            //    nIndex = 1
            //    Do Until nIndex > mValueNo
            //        If mValues(nIndex, 1) = NewValue Then
            //            mValues(nIndex, 2) = mValues(nIndex, 2) + 1
            //            Exit Do
            //        End If
            //        nIndex = nIndex + 1
            //    Loop
            //    If nIndex > mValueNo Then
            //        If mValueNo < mCount Then
            //            mValueNo = mValueNo + 1
            //            mValues(mValueNo, 1) = NewValue
            //            mValues(mValueNo, 2) = 1
            //        End If
            //    End If
            //End Sub
            #endregion
        }

        public void DelValue(long OldValue)
        {
            if (!IsValid()) return;

            int nIndex = 0;

            while (nIndex <= mValueNo)
            {
                if (mValues[nIndex, 0] == OldValue)
                {
                    mValues[nIndex, 1] = mValues[nIndex, 1] - 1;
                    if (mValues[nIndex, 1] == 0)
                    {
                        mValueNo--;
                        if (nIndex < mCount)
                        {
                            for (int i = nIndex; i <= mValueNo; i++)
                            {
                                mValues[nIndex, 0] = mValues[nIndex + 1, 0];
                                mValues[nIndex, 1] = mValues[nIndex + 1, 1];
                            }
                        }
                    }
                }
                nIndex++;
            }

            #region VB
            //Public Sub DelValue(ByVal OldValue As Long)
            //    Dim nIndex As Integer
            //    Dim i As Integer

            //    If Not IsValid() Then Exit Sub
            //    nIndex = 1
            //    Do Until nIndex > mValueNo
            //        If mValues(nIndex, 1) = OldValue Then
            //            mValues(nIndex, 2) = mValues(nIndex, 2) - 1
            //            If mValues(nIndex, 2) = 0 Then
            //                mValueNo = mValueNo - 1
            //                If nIndex < mCount Then
            //                    For i = nIndex To mValueNo
            //                        mValues(nIndex, 1) = mValues(nIndex + 1, 1)
            //                        mValues(nIndex, 2) = mValues(nIndex + 1, 2)
            //                    Next i
            //                End If
            //            End If
            //        End If
            //        nIndex = nIndex + 1
            //    Loop
            //End Sub
            #endregion
        }

        public int ValueNo 
        {
            get
            {
                return mValueNo;

                #region VB
                //Public Property Get ValueNo() As Integer
                //    ValueNo = mValueNo
                //End Property
                #endregion
            }
        }

        public int Count 
        { 
            get 
            { 
                return mCount;

                #region VB
                //Public Property Get Count() As Integer
                //    Count = mCount
                //End Property
                #endregion
            } 
        }

        public long Value
        {
            get
            {
                if (IsValid())
                {
                    return mValueNo < mWhich ? 0 : mValues[mWhich, 0];
                }

                return 0;

                #region VB
                //Public Property Get Value() As Long
                //    If IsValid() Then
                //        If mValueNo < mWhich Then
                //            Value = 0
                //        Else
                //            Value = mValues(mWhich, 1)
                //        End If
                //    Else
                //        Value = 0
                //    End If
                //End Property
                #endregion
            }
        }

        public long Ref
        {
            get
            {
                if (IsValid())
                {
                    return mValueNo < mWhich ? 0 : mValues[mWhich, 1];
                }

                return 0;

                #region VB
                //Public Property Get Ref() As Long
                //    If IsValid() Then
                //        If mValueNo < mWhich Then
                //            Ref = 0
                //        Else
                //            Ref = mValues(mWhich, 2)
                //        End If
                //    Else
                //        Ref = 0
                //    End If
                //End Property
                #endregion
            }
        }

        public string Name
        {
            get
            {
                return IsValid() ? mName : "Error";

                #region VB
                //Public Property Get Name() As String
                //    If IsValid() Then
                //        Name = mName
                //    Else
                //        Name = "Error!"
                //    End If
                //End Property
                #endregion
            }
        }

        public int Which
        {
            get
            {
                return IsValid() ? mWhich : 0;

                #region VB
                //Public Property Get Which() As Integer
                //    If IsValid() Then
                //        Which = mWhich
                //    Else
                //        Which = 0
                //    End If
                //End Property
                #endregion
            }
        }
    }
}