using System;
using System.Collections.Generic;
using System.Text;

namespace TruyenCV_BackEnd.Utility
{
    public static class Helper
    {
        public static string GetModifiedDateDisplay(DateTime? modifiedDate)
        {
            if (modifiedDate.HasValue)
            {
                var now = DateTime.Now;

                TimeSpan span = now.Subtract(modifiedDate.Value);

                if (span.Days > 0)
                {
                    if (span.Days > 365)
                    {
                        return $"{span.Days / 365} year(s) ago";
                    }
                    else if (span.Days > 30)
                    {
                        return $"{span.Days / 30} month(s) ago";
                    }

                    return $"{span.Days} day(s) ago";
                }
                else
                {
                    if (span.Hours > 0)
                    {
                        return $"{span.Hours} hour(s) ago";
                    }
                    else if (span.Minutes > 0)
                    {
                        return $"{span.Minutes} minute(s) ago";
                    }

                    return $"{span.Seconds} second(s) ago";
                }
            }

            return "1 second(s) ago";
        }
    }
}
