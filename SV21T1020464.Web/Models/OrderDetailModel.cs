﻿using SV21T1020464.DomainModels;

namespace SV21T1020464.Web.Models
{
    public class OrderDetailModel
    {
        public Order? Order { get; set; }
        public required List<OrderDetail> Details { get; set; }
    }
}