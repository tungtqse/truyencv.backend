using System;
using System.Collections.Generic;
using System.Text;

namespace TruyenCV_BackEnd.Common.Models
{
    public interface ISearchResult<TResult>
        where TResult : class
    {
        IList<TResult> Data { get; set; }
        int Count { get; set; }
    }
}
