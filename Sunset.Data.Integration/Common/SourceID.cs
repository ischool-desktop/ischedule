using System.Xml.Linq;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 來源ID，欄位有ID及連線主機位置
    /// </summary>
    public class SourceID
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public SourceID()
        {
 
        }

        /// <summary>
        /// 建構式，傳入ID及連線主機位置
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DSNS"></param>
        public SourceID(string DSNS,string ID)
        {
            this.ID = ID;
            this.DSNS = DSNS;
        }

        /// <summary>
        /// 來源ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 主機連線位置
        /// </summary>
        public string DSNS { get; set; }

        /// <summary>
        /// 輸出成字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "DSNS:" + DSNS + ",ID:" + ID;
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            ID = Element.AttributeText("ID");
            DSNS = Element.AttributeText("DSNS");
        }

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("SourceID");
            Element.SetAttributeValue("ID", ID);
            Element.SetAttributeValue("DSNS", DSNS);

            return Element;
        }
    }
}