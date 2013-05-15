using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sunset.Data.Integration
{
    internal class SSourceContent
    {
        private Dictionary<string, StringBuilder> mValues;

        public SSourceContent()
        {
            mValues = new Dictionary<string, StringBuilder>();
        }

        public void AddValue(string Source,string Value)
        {
            if (!mValues.ContainsKey(Source))
                mValues.Add(Source,new StringBuilder());

            mValues[Source].Append(Value);
        }

        public bool IsContentEqual
        {
            get
            {
                if (mValues.Keys.Count <= 1)
                    return true;

                string CompareValue = null;

                foreach (StringBuilder strBuilder in mValues.Values)
                {
                    if (CompareValue == null)
                        CompareValue = strBuilder.ToString();
                    else
                    {
                        string CurrentValue = strBuilder.ToString();

                        if (!CompareValue.Equals(CurrentValue))
                            return false;
                    }
                }

                return true;
            }
        }

        public int Count { get { return mValues.Keys.Count; } }
    }
}