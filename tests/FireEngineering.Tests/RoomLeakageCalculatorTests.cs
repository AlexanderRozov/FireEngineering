using FireEngineering.Application.Leakage;
using FireEngineering.Domain.Leakage;
using FireEngineering.Norms.Sp485;
using Xunit;

namespace FireEngineering.Tests;

public sealed class RoomLeakageCalculatorTests
{
    private readonly RoomLeakageCalculator _calculator =
        new(new Sp485Profile());

    [Fact]
    public void Calculate_UsesOpeningPositionAndArea()
    {
        var input = new RoomLeakageInput
        {
            RoomVolumeM3 = 200,
            PermanentlyOpenAreaM2 = 2,
            RoomHeightM = 4,
            AgentDeliveryTimeSeconds = 10,
            Distribution = OpeningDistribution.UpperZoneOnly,
            DistributionJustification = "Тестовая классификация"
        };

        var result = _calculator.Calculate(input);

        Assert.Equal(0.01, result.DeltaPerMeter, 8);
        Assert.Equal(0.10, result.OpeningPositionParameter, 8);
        Assert.Equal(0.02, result.K2, 8);
        Assert.Equal(0.0060, result.MaximumAllowedDeltaPerMeter, 8);
        Assert.False(result.MeetsLeakageLimit);
    }

    [Fact]
    public void Calculate_SmallOpening_MeetsLeakageLimit()
    {
        var input = new RoomLeakageInput
        {
            RoomVolumeM3 = 200,
            PermanentlyOpenAreaM2 = 0.1,
            RoomHeightM = 4,
            AgentDeliveryTimeSeconds = 10,
            Distribution = OpeningDistribution.LowerZoneOnly,
            DistributionJustification = "Только нижняя зона"
        };

        var result = _calculator.Calculate(input);

        Assert.Equal(0.0005, result.DeltaPerMeter, 8);
        Assert.Equal(0.25, result.OpeningPositionParameter, 8);
        Assert.True(result.MeetsLeakageLimit);
    }

    [Fact]
    public void Calculate_RejectsZeroVolume()
    {
        var input = new RoomLeakageInput
        {
            RoomVolumeM3 = 0,
            PermanentlyOpenAreaM2 = 2,
            RoomHeightM = 4,
            AgentDeliveryTimeSeconds = 10,
            Distribution = OpeningDistribution.UpperZoneOnly,
            DistributionJustification = "Тест"
        };

        Assert.Throws<ArgumentOutOfRangeException>(
            () => _calculator.Calculate(input));
    }
}
