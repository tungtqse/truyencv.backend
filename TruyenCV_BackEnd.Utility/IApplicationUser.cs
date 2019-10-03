using System;
using System.Collections.Generic;
using System.Text;

namespace TruyenCV_BackEnd.Utility
{
    public interface IApplicationUser
    {
        Guid GetUserId();
        string GetUserName();
        string GetUserEmail();
    }
}
