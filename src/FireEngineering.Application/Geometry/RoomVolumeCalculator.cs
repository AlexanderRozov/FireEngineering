using FireEngineering.Domain.Geometry;

namespace FireEngineering.Application.Geometry;

public sealed class RoomVolumeCalculator
{
    private const double Epsilon = 1e-9;

    public RoomVolumeResult Calculate(RoomVolumeInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.RoomParts is null || input.RoomParts.Count == 0)
            throw new ArgumentException("Не заданы геометрические части помещения.", nameof(input));

        foreach (var part in input.RoomParts)
        {
            if (string.IsNullOrWhiteSpace(part.Name))
                throw new ArgumentException("Не задано имя части помещения.");
            if (part.VolumeM3 <= 0)
                throw new ArgumentException($"Объём части '{part.Name}' должен быть положительным.");
        }

        ValidateNonNegative(input.IncludedVentilationVolumeM3, nameof(input.IncludedVentilationVolumeM3));
        ValidateNonNegative(input.ApprovedDeductionM3, nameof(input.ApprovedDeductionM3));

        if (input.ApprovedDeductionM3 > Epsilon &&
            (string.IsNullOrWhiteSpace(input.DeductionNormativeReference) ||
             string.IsNullOrWhiteSpace(input.DeductionDescription)))
        {
            throw new ArgumentException("Для вычета необходимы описание и нормативная ссылка.");
        }

        double partsVolume = input.RoomParts.Sum(x => x.VolumeM3);
        double total = partsVolume + input.IncludedVentilationVolumeM3 - input.ApprovedDeductionM3;

        if (total <= Epsilon)
            throw new InvalidOperationException("Расчётный объём должен быть больше нуля.");

        return new RoomVolumeResult(
            partsVolume,
            input.IncludedVentilationVolumeM3,
            input.ApprovedDeductionM3,
            total,
            "V = Σ(объёмы топологий) + V_вентиляции − V_допустимых_вычетов");
    }

    private static void ValidateNonNegative(double value, string name)
    {
        if (!double.IsFinite(value) || value < 0)
            throw new ArgumentOutOfRangeException(name);
    }
}
