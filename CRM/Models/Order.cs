using System;
using System.Collections.Generic;

namespace CRM.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
