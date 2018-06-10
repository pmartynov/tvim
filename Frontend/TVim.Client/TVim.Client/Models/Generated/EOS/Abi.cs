using System.Collections.Generic;

namespace TVim.Client.Activity
{
    public class Abi
    {
        public string version { get; set; }
        public List<Type> types { get; set; }
        public List<Struct> structs { get; set; }
        public List<Action> actions { get; set; }
        public List<Table> tables { get; set; }
        public List<object> ricardian_clauses { get; set; }
        public List<object> error_messages { get; set; }
        public List<object> abi_extensions { get; set; }
    }
}