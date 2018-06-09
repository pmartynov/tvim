namespace TVim.Client.Models.Errors
{
    public class ErrorBase
    {
        public string Msg { get; set; }

        public ErrorBase() { }

        public ErrorBase(string msg)
        {
            Msg = msg;
        }
    }
}