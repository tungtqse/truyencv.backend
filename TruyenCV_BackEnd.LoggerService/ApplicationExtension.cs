using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using System;

namespace TruyenCV_BackEnd.LoggerService
{
    public static class ApplicationExtension
    {
        public static void ConfigureElmah(this IApplicationBuilder app)
        {
            app.UseElmah();
        }
    }
}
