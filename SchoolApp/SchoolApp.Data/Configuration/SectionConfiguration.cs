using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasData(this.SeedSections());
        }

        private List<Section> SeedSections()
        {
            List<Section> sections = new List<Section>()
            {
                new Section()
                {
                    Id = 1,
                    Name = "А"
                },
                new Section()
                {
                    Id = 2,
                    Name = "Б"
                },
                new Section()
                {
                    Id = 3,
                    Name = "В"
                },
                new Section()
                {
                    Id = 4,
                    Name = "Г"
                }
            };
            return sections;
        }
    }
}