using System;
using System.Collections.Generic;

namespace TVim.Client.Activity
{
    public class GetAccountResult
    {
        public string account_name { get; set; }
        public bool privileged { get; set; }
        public DateTime last_code_update { get; set; }
        public DateTime created { get; set; }
        public int ram_quota { get; set; }
        public int net_weight { get; set; }
        public int cpu_weight { get; set; }
        public NetLimit net_limit { get; set; }
        public CpuLimit cpu_limit { get; set; }
        public int ram_usage { get; set; }
        public List<Permission> permissions { get; set; }
        public object total_resources { get; set; }
        public object delegated_bandwidth { get; set; }
        public object voter_info { get; set; }
    }
}