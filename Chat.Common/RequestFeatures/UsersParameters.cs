using System;

namespace Chat.Common.RequestFeatures
{
    public sealed class UsersParameters : RequestParameters
    {
        public UsersParameters() => OrderBy = "nickname";
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }

        public bool ValidDateRange => MaxDate > MinDate;
    }
}