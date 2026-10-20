using FireEngineering.Application.Geometry;
using FireEngineering.Domain.Geometry;
using Xunit;

namespace FireEngineering.Tests;

public sealed class RoomVolumeCalculatorTests
{
    private readonly RoomVolumeCalculator _calculator = new();

    [Fact]
    public void Calculate_SumsBoxAndCylinder()
    {
        var input = new RoomVolumeInput
        {
            RoomParts =
            [
                new RoomBox("Main", 10, 8, 3),
                new RoomCylinder("Tank", 2, 3)
            ],
            IncludedVentilationVolumeM3 = 4
        };

        var result = _calculator.Calculate(input);

        double expectedParts = 10 * 8 * 3 + Math.PI * 2 * 2 * 3;
        Assert.Equal(expectedParts, result.RoomPartsVolumeM3, 4);
        Assert.Equal(expectedParts + 4, result.CalculatedVolumeM3, 4);
    }

    [Fact]
    public void Calculate_PrismVolume()
    {
        var input = new RoomVolumeInput
        {
            RoomParts =
            [
                new RoomPrism("Poly", [(0,0), (4,0), (4,3), (0,3)], 2.5)
            ]
        };

        var result = _calculator.Calculate(input);
        Assert.Equal(4 * 3 * 2.5, result.CalculatedVolumeM3, 6);
    }

    [Fact]
    public void Calculate_RejectsZeroVolumePart()
    {
        var input = new RoomVolumeInput
        {
            RoomParts = [new RoomBox("Bad", 0, 1, 1)]
        };

        Assert.Throws<ArgumentException>(() => _calculator.Calculate(input));
    }

    [Fact]
    public void Calculate_DeductionRequiresJustification()
    {
        var input = new RoomVolumeInput
        {
            RoomParts = [new RoomBox("Main", 10, 8, 3)],
            ApprovedDeductionM3 = 10
        };

        Assert.Throws<ArgumentException>(() => _calculator.Calculate(input));
    }

    [Fact]
    public void Calculate_JustifiedDeduction_ReducesVolume()
    {
        var input = new RoomVolumeInput
        {
            RoomParts = [new RoomBox("Main", 10, 8, 3)],
            ApprovedDeductionM3 = 20,
            DeductionNormativeReference = "СП 485, расчётный объём",
            DeductionDescription = "Сплошная непроницаемая шахта"
        };

        var result = _calculator.Calculate(input);

        Assert.Equal(240, result.RoomPartsVolumeM3, 6);
        Assert.Equal(220, result.CalculatedVolumeM3, 6);
    }
}
