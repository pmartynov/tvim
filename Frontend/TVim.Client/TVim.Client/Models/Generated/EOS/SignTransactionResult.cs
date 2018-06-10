using System;
using System.Collections.Generic;

namespace TVim.Client.Activity
{
    public partial class MainActivity
    {
        public class SignTransactionResult
        {
            public DateTime expiration { get; set; }
            public int ref_block_num { get; set; }
            public long ref_block_prefix { get; set; }
            public int max_net_usage_words { get; set; }
            public int max_cpu_usage_ms { get; set; }
            public int delay_sec { get; set; }
            public List<object> context_free_actions { get; set; }
            public List<object> actions { get; set; }
            public List<object> transaction_extensions { get; set; }
            public List<string> signatures { get; set; }
            public List<object> context_free_data { get; set; }
        }
    }
}
