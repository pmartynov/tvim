using System;
using System.Collections.Generic;

namespace TVim.Client.Activity
{
    public class GetBlockResult
    {
        public DateTime timestamp { get; set; }
        public string producer { get; set; }
        public int confirmed { get; set; }
        public string previous { get; set; }
        public string transaction_mroot { get; set; }
        public string action_mroot { get; set; }
        public int schedule_version { get; set; }
        public object new_producers { get; set; }
        public List<object> header_extensions { get; set; }
        public string producer_signature { get; set; }
        public List<object> transactions { get; set; }
        public List<object> block_extensions { get; set; }
        public string id { get; set; }
        public int block_num { get; set; }
        public long ref_block_prefix { get; set; }
    }
}