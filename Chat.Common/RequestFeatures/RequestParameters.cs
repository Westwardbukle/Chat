﻿using System;

namespace Chat.Common.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
        
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        
        public string? OrderBy { get; set; }
    }
}