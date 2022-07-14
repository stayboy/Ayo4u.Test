using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ayo4u.Data.Configurations
{
    internal class UnitConverterConfiguration : AyoIdModelConfiguration<int, UnitConverter>
    {
        public UnitConverterConfiguration() : base("Converters")
        {
        }

        public override void Configure(EntityTypeBuilder<UnitConverter> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => new { x.InUnitType, x.OutUnitType }).IsUnique();

            builder.HasData(new UnitConverter[]
            {
                new() { InUnitType = "centimeters", OutUnitType = "inches", Multiplier= 0.3937f },
                new() { InUnitType = "inches", OutUnitType = "centimeters", Multiplier = 2.54f },

                new() { InUnitType = "meters", OutUnitType = "feet", Multiplier = 3.2808f },
                new() { InUnitType = "feet", OutUnitType = "meters", Multiplier = 0.3048f },

                new() { InUnitType = "meters", OutUnitType = "inches", Multiplier = 39.37f },
                new() { InUnitType = "inches", OutUnitType = "meters", Multiplier = 0.0254f },

                new() { InUnitType = "fahrenheit", OutUnitType = "celcius", Multiplier = 0f, Formula = Formulae.Fahrenheit_To_Celcius },
                new() { InUnitType = "celcius", OutUnitType = "fahrenheit", Multiplier = 0f, Formula = Formulae.Celcius_To_Fahrenheit }
            });
        }
    }
}
