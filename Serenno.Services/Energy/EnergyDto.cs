using Serenno.Domain.Models.Core;

namespace Serenno.Services.Energy
{
    public sealed record EnergyDto(EnergyType EnergyType, byte? energyAmount);
}