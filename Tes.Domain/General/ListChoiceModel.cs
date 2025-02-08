using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes.Domain
{
    public class ListChoiceWithId
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
    public class ListChoiceWithIdCategory
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
    }
    public class ListChoiceWithString
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
    public class ListChoiceWithStringCategory
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
    }
    public class ListChoiceWithBool
    {
        public bool Value { get; set; }
        public string Text { get; set; }
    }
    public class ListChoiceWithObject
    {
        public object Value { get; set; }
        public string Text { get; set; }
    }
}
