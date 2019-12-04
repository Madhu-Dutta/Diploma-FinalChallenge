using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameGroupApp.Data.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string Venue { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
        public bool Paid { get; set; }
        public double ? Amount { get; set; }

        [Display(Name = "Paid by")]
        public string UserId { get; set; }

        // Navigation Properties
        public IdentityUser User { get; set; }

        public class RecordConfig : IEntityTypeConfiguration<Record>
        {
            public void Configure(EntityTypeBuilder<Record> builder)
            {
                // Primary Key
                builder.HasKey(user => user.Id);
                builder.Property(cd => cd.Venue).IsRequired();
                builder.Property(cd => cd.Time).IsRequired();
                builder.Property(cd => cd.Venue).IsRequired();
            }
        }
    }
}
