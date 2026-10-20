using FireEngineering.Application.Gas;
using FireEngineering.Norms.Sp485;
using Xunit;

namespace FireEngineering.Tests;

public sealed class TotalAgentMassCalculatorTests
{
    private readonly TotalAgentMassCalculator _calculator =
        new(new Sp485Profile());

    [Fact]
    public void Calculate_AppliesReserveAndModuleResiduals()
    {
        var result = _calculator.Calculate(new TotalAgentMassInput(
            AgentId: "HFC-227ea",
            RoomMassKg: 100,
            ResidualAgentMassKg: 5,
            ModuleResidualMassKg: 1,
            ModuleCount: 2));

        Assert.Equal(2, result.TotalModuleResidualMassKg, 6);
        Assert.Equal(107, result.MassBeforeReserveKg, 6);
        Assert.Equal(1.05, result.ReserveFactor, 6);
        Assert.Equal(112.35, result.TotalRequiredMassKg, 6);
        Assert.Equal("RU.SP485.2020", result.NormativeProfileId);
    }

    [Fact]
    public void Calculate_RejectsZeroModuleCount()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _calculator.Calculate(new TotalAgentMassInput(
                "HFC-227ea", 100, 0, 0, 0)));
    }
}
