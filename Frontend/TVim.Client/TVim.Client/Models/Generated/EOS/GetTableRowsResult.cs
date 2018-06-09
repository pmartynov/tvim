using System.Collections.Generic;

namespace TVim.Client.Activity
{
    public class GetTableRowsResult
    {
        public List<Row> rows { get; set; }
        public bool more { get; set; }
    }
}