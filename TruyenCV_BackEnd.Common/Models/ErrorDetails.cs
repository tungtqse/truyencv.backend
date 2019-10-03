using System;
using System.Collections.Generic;
using System.Text;

namespace TruyenCV_BackEnd.Common.Models
{
    public class ErrorDetails : IWebApiResponse
    {
        public int Code { get; set; }
        public bool IsSuccessful { get; set; }
        public string State { get; set; }
        public List<string> Messages { get; set; }

        public ErrorDetails()
        {
            Messages = new List<string>();
        }
    }
}
