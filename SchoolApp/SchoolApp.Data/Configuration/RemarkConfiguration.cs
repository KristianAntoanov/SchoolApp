using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class RemarkConfiguration : IEntityTypeConfiguration<Remark>
    {
        public void Configure(EntityTypeBuilder<Remark> builder)
        {
            //throw new NotImplementedException();
        }
    }
}