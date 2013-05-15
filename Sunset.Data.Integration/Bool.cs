using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.Data.Integration
{
    public class @Bool
    {
        static public bool Parse(string Value)
        {
            //若是空白則回傳false
            if (string.IsNullOrWhiteSpace(Value))
                return false;
            //若文字為「是」則回傳true
            else if (Value.Equals("是"))
                return true;
            //若為f開頭則回傳flase，否則回傳true
            return Value.Trim().ToLower().StartsWith("f") ? false : true;
        }
    }
}