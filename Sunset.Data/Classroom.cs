using System.Collections.Generic;

namespace Sunset.Data
{
    /// <summary>
    /// 場地
    /// </summary>
    public class Classroom
    {
        private string mWhereID;
        private string mName;
        private int mCapacity;
        private string mLocID;
        private bool mLocOnly;
        private List<Appointments> mAppointmentsList;
        private Appointments mAppointments;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="WhereID">場地編號</param>
        /// <param name="Name">場地名稱</param>
        /// <param name="Capacity">行事曆數量</param>
        /// <param name="LocID">地點編號</param>
        /// <param name="LocOnly">場地無行事曆</param>
        public Classroom(string WhereID,string Name,int Capacity,string LocID,bool LocOnly)
        {
            //指定場地編號、場地名稱、地點編號以及場地無行事曆
            this.mWhereID = WhereID;
            this.mName = Name;
            this.mLocID = LocID;
            this.mLocOnly = LocOnly;

            //新增行事曆列表
            this.mAppointmentsList = new List<Appointments>();

            //若是場地無行事曆，代表這個場地可以被無限制的使用，沒有時間限制；例如指定地點為忠信學校。
            if (LocOnly)
                mCapacity = 0;
            else
            {
                //若是行事曆數量小於1，則設行事曆數量為1，至少有1個預設行事曆
                if (Capacity < 1)
                    Capacity = 1;

                this.mCapacity = Capacity;

                //根據行事曆數量來新增行事曆
                for (int i = 0; i < this.mCapacity; i++)
                    mAppointmentsList.Add(new Appointments());

                //將使用中的行事曆設為第1個行事曆（C#索引從0開始）
                mAppointments = mAppointmentsList[0];
            }

            #region VB
            //Dim nIndex As Integer    
            //mWhereID = WhereID
            //mName = Name
            //mLocID = LocID
            //mLocOnly = LocOnly
            //If LocOnly Then
            //    Capacity = 0
            //Else
            //    If Capacity < 1 Then Capacity = 1
            //    mCapacity = Capacity
            //    ReDim mApps(1 To Capacity) As Appointments
            //    For nIndex = 1 To Capacity
            //        Set mApps(nIndex) = New Appointments
            //    Next nIndex
            //    Set mApp = mApps(1)
            //End If
            #endregion
        }

        /// <summary>
        /// 切換行事曆
        /// </summary>
        /// <param name="nWhich">行事曆索引</param>
        public void UseAppointments(int nWhich)
        {
            //若是索引小於0，那麼設為0
            if (nWhich < 0) nWhich = 0;

            //若是索引大於等於行事曆數量，將索引設為行事曆數量減1（C#索引從0開始）
            if (nWhich >= mCapacity) nWhich = mCapacity-1;

            //切換使用中的行事歷（根據nWhich變數）
            mAppointments = mAppointmentsList[nWhich];

            #region VB
            //If nWhich < 1 Then nWhich = 1
            //If nWhich > mCapacity Then nWhich = mCapacity
            //Set mApp = mApps(nWhich)
            #endregion
        }

        /// <summary>
        /// 場地編號
        /// </summary>
        public string WhereID { get { return mWhereID; } }

        /// <summary>
        /// 場地名稱
        /// </summary>
        public string Name { get { return mName; } }

        /// <summary>
        /// 行事曆數量
        /// </summary>
        public int Capacity { get { return mCapacity; } }

        /// <summary>
        /// 地點編號
        /// </summary>
        public string LocID { get { return mLocID; } }

        /// <summary>
        /// 地點無行事曆代表這個資源在被排程時不需『排定約會』，也就是行事曆數量為0；例如排在忠信。
        /// </summary>
        public bool LocOnly { get { return mLocOnly; } }

        /// <summary>
        /// 場地總時數
        /// </summary>
        public int TotalHour { get; set; }

        /// <summary>
        /// 已排課時數
        /// </summary>
        public int AllocHour { get; set; }

        /// <summary>
        /// 約會集合
        /// </summary>
        public Appointments Appointments { get { return mAppointments; } }
    }
}