using System;
using System.ComponentModel.DataAnnotations;

namespace Inchecker.Entities
{
    public class Incheck
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime? CheckinTime { get; set; }

        public DateTime? CheckoutTime { get; set; }
    }
}
