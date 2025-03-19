using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Repository.Data.Configuration
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
          builder.Property(o=>o.Status)
          .HasConversion(o => o.ToString(), o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o));
            builder.Property(o=>o.SubTotal).HasColumnType("decimal(18,2)");
            builder.OwnsOne(o=>o.ShippingAddress,a=>a.WithOwner());
            builder.HasOne(o=>o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.NoAction);
                    
        }
    }
}
