using System;

namespace TruyenCV_BackEnd.Common
{
    public static class Constants
    {
        public const string ConnectionString = "DefaultConnection";
        public const int MaximumItem = 100;
        public const int Salt = 16;
        public const int Hash = 20;
        public const int PasswordHash = 36;
        public const int AccessFailedCount = 3;

        public class BaseProperty
        {
            public const string Id = "Id";
            public const string CreatedDate = "CreatedDate";
            public const string CreatedBy = "CreatedBy";
            public const string ModifiedDate = "ModifiedDate";
            public const string ModifiedBy = "ModifiedBy";
            public const string StatusId = "StatusId";
        }

        public class AuditTrailProperty : BaseProperty
        {
            public const string ItemId = "ItemId";
            public const string TableName = "TableName";
            public const string TrackChange = "TrackChange";
            public const string TransactionId = "TransactionId";
        }

        public class ProgressStatus
        {
            public const string Completed = "Completed";
            public const string Pending = "Pending";
            public const string Dropped = "Dropped";
            public const string Processing = "Processing";
        }

        public class Source
        {
            public const string WikiDich = "WikiDich";
            public const string TruyenCV = "TruyenCV";
            public const string TruyenYY = "TruyenYY";
            public const string TangThuVien = "TangThuVien";
        }
    }
}
