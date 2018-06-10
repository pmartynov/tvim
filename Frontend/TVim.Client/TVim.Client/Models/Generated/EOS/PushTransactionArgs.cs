using System;

namespace TVim.Client.Activity
{
    public class PushTransactionArgs
    {
        public int ref_block_num;
        public long ref_block_prefix { get; set; }
        public DateTime expiration { get; set; }
        public string[] scope { get; set; }
        public Messages[] messages { get; set; }
        public string[] signatures { get; set; }
    }
}