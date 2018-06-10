using System;

namespace TVim.Client.Activity
{
    public class PushTransactionArgs
    {
        public int ref_block_num;
        public long ref_block_prefix { get; set; }
        public DateTime expiration { get; set; }
        
        public string[] signatures { get; set; } = new string[0];
        public string[] context_free_data { get; set; } = new string[0];


        public int max_net_usage_words { get; set; }
        public int max_cpu_usage_ms { get; set; }
        public int delay_sec { get; set; }
        public string[] context_free_actions { get; set; } = new string[0];
        public Action[] actions { get; set; } = new Action[0];
        public string[] transaction_extensions { get; set; } = new string[0];
    }
}