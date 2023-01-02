using System;
using System.Collections.Generic;
using System.Text;

namespace Xpert.Common.DAO
{
    public class QueryInfos
    {
        public string StringSelections { get; set; }
        public string StringCondition { get; set; }
        public string StringJoin { get; set; }
        public string StringGroupBy { get; set; }
        public string StringOrderBy { get; set; }
        public string StringHaving { get; set; }
        public string StringPaging { get; set; }

        public string Param1 { get; set; }
    }
}
