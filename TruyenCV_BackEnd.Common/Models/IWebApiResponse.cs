using System;
using System.Collections.Generic;
using System.Text;

namespace TruyenCV_BackEnd.Common.Models
{
    public interface IWebApiResponse
    {
        int Code { get; set; }
        bool IsSuccessful { get; set; }
        List<string> Messages { get; set; }
    }
}
