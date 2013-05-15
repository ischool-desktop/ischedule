using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ischedule
{
    public class LabelContent
    {
        public string Text { get; set; }

        public string Tag { get; set; }

        public LabelContent(string Text, string Tag)
        {
            this.Text = Text;
            this.Tag = Tag;
        }
    }
}
