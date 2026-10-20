using FireEngineering.Application.Gas;
using FireEngineering.Norms.Sp485;
using Xunit;

namespace FireEngineering.Tests;

public sealed class GasAgentMassCalculatorTests
{
    private readonly GasAgentMassCalculator _calculator =
        new(new Sp485Profile());

    [Fact]
    public void Calculate_ReturnsPositiveMass()
    {
        var input = new GasAgentMassInput(
            AgentId: "HFC-227ea",
            RoomVolumeM3: 200,
            ConcentrationPercent: 7.2,
            MinimumTemperatureCelsius: 20,
            AltitudeMeters: 0,
            K2: 0.02);

        var result = _calculator.Calculate(input);

        Assert.True(result.RoomMassKg > 0);
        Assert.Equal(7.28, result.VaporDensityKgM3, 6);
        Assert.Equal(1.0, result.K3, 6);
    }

    [Fact]
    public void Calculate_AltitudeCorrection_ReducesDensity()
    {
        var input = new GasAgentMassInput(
            "HFC-227ea", 200, 7.2, 20, 2000, 0);

        var result = _calculator.Calculate(input);

        Assert.Equal(0.830, result.K3, 6);
        Assert.Equal(7.28 * 0.830, result.VaporDensityKgM3, 4);
    }

    [Fact]
    public void Calculate_RejectsConcentrationAt100Percent()
    {
        var input = new GasAgentMassInput(
            "HFC-227ea", 200, 100, 20, 0, 0);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => _calculator.Calculate(input));
    }

    [Fact]
    public void Calculate_RejectsUnknownAgent()
    {
        var input = new GasAgentMassInput(
            "UNKNOWN", 200, 7.2, 20, 0, 0);

        Assert.Throws<NotSupportedException>(
            () => _calculator.Calculate(input));
    }
}
