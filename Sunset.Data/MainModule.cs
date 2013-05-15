using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.Data
{
    public static class MainModule
    {
        public static bool IsNullValue(this string TestVal)
        {
            return string.IsNullOrWhiteSpace(TestVal);
        }

        public static bool IsNullValue(this long TestVal)
        {
            return TestVal == Constants.NullValue;
        }

        //Public Function IsNullValue(ByVal TestVal As Long)
        //    IsNullValue = (TestVal = NullValue)
        //End Function

        public static long NullToValue(this string ConvVal)
        {
            long ParseValue;

            if (!long.TryParse(ConvVal,out ParseValue))
                ParseValue = Constants.NullValue;

            return string.IsNullOrWhiteSpace(ConvVal) ? Constants.NullValue : ParseValue;
        }

        //Public Function NullToValue(ByVal ConvVal As Variant) As Long
        //    NullToValue = IIf(IsNull(ConvVal), NullValue, ConvVal)
        //End Function

        //public string NullToString(string ConvVal)
        //{
 
        //}


//Public Function NullToString(ByVal ConvVal As Variant) As String
//    NullToString = IIf(IsNull(ConvVal), NullString, ConvVal)
//End Function

//Public Function ValueToNull(ByVal ConvVal As Long) As Variant
//    ValueToNull = IIf(ConvVal = NullValue, Null, ConvVal)
//End Function

//Public Function StringToNull(ByVal ConvVal As String) As Variant
//    StringToNull = IIf(ConvVal = "", Null, ConvVal)
//End Function

    }
}