namespace TVim.Client.Activity
{
    public class GetCodeResult
    {
        public string account_name { get; set; }
        public string code_hash { get; set; }
        public string wast { get; set; }
        public string wasm { get; set; }
        public Abi abi { get; set; }
    }
}