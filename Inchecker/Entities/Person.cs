using Inchecker.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inchecker.Entities
{
    public class Person
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string FloorNr { get; set; }

        [MaxLength(50)]
        public string RoomNr { get; set; }

        [MaxLength(50)]
        public string Pin { get; set; }

        public Roles Role { get; set; }

        public DateTime? CheckinTime { get; set; }
    }
}
