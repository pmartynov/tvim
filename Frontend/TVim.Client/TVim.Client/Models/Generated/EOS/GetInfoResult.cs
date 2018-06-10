using System;

namespace TVim.Client.Activity
{
    public class GetInfoResult
    {
        public string server_version { get; set; }
        public string chain_id { get; set; }
        public int head_block_num { get; set; }
        public int last_irreversible_block_num { get; set; }
        public string last_irreversible_block_id { get; set; }
        public string head_block_id { get; set; }
        public DateTime head_block_time { get; set; }
        public string head_block_producer { get; set; }
        public int virtual_block_cpu_limit { get; set; }
        public int virtual_block_net_limit { get; set; }
        public int block_cpu_limit { get; set; }
        public int block_net_limit { get; set; }
    }
}