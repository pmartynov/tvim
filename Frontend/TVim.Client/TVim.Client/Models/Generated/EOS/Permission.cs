namespace TVim.Client.Activity
{
    public class Permission
    {
        public string perm_name { get; set; }
        public string parent { get; set; }
        public RequiredAuth required_auth { get; set; }
    }
}