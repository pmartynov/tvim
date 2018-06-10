namespace TVim.Client.Activity
{
    public class Action
    {
        public string account { get; set; }
        public string name { get; set; }
        public Authorization[] authorization { get; set; }
        public string data { get; set; }
    }

    public class Authorization
    {
        public string actor { get; set; }
        public string permission { get; set; }
    }
}