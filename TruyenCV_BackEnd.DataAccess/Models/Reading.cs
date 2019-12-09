using System;
using System.Collections.Generic;
using System.Text;

namespace TruyenCV_BackEnd.DataAccess.Models
{
    public class Reading : BaseEntity
    {
        public Guid StoryId { get; set; }
        public Guid ChapterId { get; set; }
    }
}
