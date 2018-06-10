using System.Collections.Generic;

namespace TVim.Client.Activity
{
    public class RequiredAuth
    {
        public int threshold { get; set; }
        public List<Key> keys { get; set; }
        public List<object> accounts { get; set; }
        public List<object> waits { get; set; }
    }
}