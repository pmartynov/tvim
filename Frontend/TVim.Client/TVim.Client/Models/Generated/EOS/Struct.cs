using System.Collections.Generic;

namespace TVim.Client.Activity
{
    public class Struct
    {
        public string name { get; set; }
        public string @base { get; set; }
        public List<Field> fields { get; set; }
    }
}