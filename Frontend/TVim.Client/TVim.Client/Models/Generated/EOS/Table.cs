using System.Collections.Generic;

namespace TVim.Client.Activity
{
    public class Table
    {
        public string name { get; set; }
        public string index_type { get; set; }
        public List<string> key_names { get; set; }
        public List<string> key_types { get; set; }
        public string type { get; set; }
    }
}