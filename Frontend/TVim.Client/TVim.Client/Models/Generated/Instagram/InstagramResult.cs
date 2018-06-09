using System.Collections.Generic;

namespace TVim.Client.Models
{
    public class InstagramResult
    {
        public Pagination pagination { get; set; }
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }
    }
}