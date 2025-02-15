﻿using System.ComponentModel.DataAnnotations;

namespace DemoHotelBooking.Models
{
    public class ReportRevenue
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public string AccId { get; set; }
        public AppUser Accountant { get; set; }
    }
}
