using System.Collections.Generic;

namespace TVim.Client.Models
{
    public class Datum
    {
        public string id { get; set; }
        public User user { get; set; }
        public Images images { get; set; }
        public string created_time { get; set; }
        public Caption caption { get; set; }
        public bool user_has_liked { get; set; }
        public Likes likes { get; set; }
        public List<object> tags { get; set; }
        public string filter { get; set; }
        public Comments comments { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public object location { get; set; }
        public object attribution { get; set; }
        public List<object> users_in_photo { get; set; }
    }
}