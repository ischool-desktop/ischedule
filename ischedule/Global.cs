using System.Collections.Generic;
using FISCA.DSAClient;

namespace ischedule
{
    /// <summary>
    /// 全域變數
    /// </summary>
    public class Global
    {
        /// <summary>
        /// 是否為合法的Passport
        /// </summary>
        /// <returns></returns>
        public static bool IsValidatePassport()
        {
            if (Passport == null)
                return false;

            try
            {
                Connection Conn = new Connection();

                Conn.Connect(FISCA.Authentication.DSAServices.GreeningAccessPoint,
                    FISCA.Authentication.DSAServices.GreeningContract,
                    Passport);
            }catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Passport
        /// </summary>
        public static PassportToken Passport { get; set; }

        /// <summary>
        /// 可連線的學校清單
        /// </summary>
        public static List<SchoolDSNSName> AvailDSNSNames { get; set; }

        /// <summary>
        /// 登入帳號
        /// </summary>
        public static string EMail { get; set; }
    }

    /// <summary>
    /// 學校名稱對應DSNS名稱
    /// </summary>
    public class SchoolDSNSName
    {
        /// <summary>
        /// 學校名稱
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// DSNS名稱
        /// </summary>
        public string DSNSName { get; set; }
    }
}