using System;
using System.Collections.Generic;
using System.Text;

namespace TruyenCV_BackEnd.DataAccess.Models
{
    public class Subscription: BaseEntity
    {
        public Guid StoryId { get; set; }
        public Guid ChapterId { get; set; }
        public bool IsNewest { get; set; }
    }
}
