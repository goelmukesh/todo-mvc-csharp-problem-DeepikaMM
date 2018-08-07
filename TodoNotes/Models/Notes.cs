using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoNotes.Models
{
    public class Notes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool PinStatus { get; set; }
        public string PlainText { get; set; }
        public List<CheckList> checkList { get; set; }
        public List<Label> label { get; set; }
    }
}
