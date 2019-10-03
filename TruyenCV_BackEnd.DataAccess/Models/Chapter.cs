using System;
using System.Collections.Generic;
using System.Text;

namespace TruyenCV_BackEnd.DataAccess.Models
{
    public class Chapter : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid StoryId { get; set; }
        public int? NumberChapter { get; set; }
        public string Link { get; set; }
        public virtual Story Story { get; set; }
    }
}
