using Microsoft.AspNetCore.Http;
using System;

namespace TruyenCV_BackEnd.Utility
{
    public static class AppContext
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current => _httpContextAccessor.HttpContext;
    }
}
