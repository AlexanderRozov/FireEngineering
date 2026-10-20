using FireEngineering.Domain.Leakage;
using FireEngineering.Domain.Norms;

namespace FireEngineering.Application.Leakage;

public sealed class RoomLeakageCalculator
{
    private readonly INormativeProfile _norms;

    public RoomLeakageCalculator(INormativeProfile norms)
    {
        _norms = norms ?? throw new ArgumentNullException(nameof(norms));
    }

    public RoomLeakageResult Calculate(RoomLeakageInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (!double.IsFinite(input.RoomVolumeM3) || input.RoomVolumeM3 <= 0)
            throw new ArgumentOutOfRangeException(nameof(input.RoomVolumeM3));
        if (!double.IsFinite(input.PermanentlyOpenAreaM2) || input.PermanentlyOpenAreaM2 < 0)
            throw new ArgumentOutOfRangeException(nameof(input.PermanentlyOpenAreaM2));
        if (!double.IsFinite(input.RoomHeightM) || input.RoomHeightM <= 0)
            throw new ArgumentOutOfRangeException(nameof(input.RoomHeightM));
        if (!double.IsFinite(input.AgentDeliveryTimeSeconds) || input.AgentDeliveryTimeSeconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(input.AgentDeliveryTimeSeconds));
        if (string.IsNullOrWhiteSpace(input.DistributionJustification))
            throw new ArgumentException("Нужно обосновать классификацию расположения проёмов.");

        double delta = input.PermanentlyOpenAreaM2 / input.RoomVolumeM3;
        double pi = _norms.GetOpeningPositionParameter(input.Distribution);
        double k2 = pi * delta * input.AgentDeliveryTimeSeconds * Math.Sqrt(input.RoomHeightM);
        double maximumDelta = _norms.GetMaximumLeakageParameter(input.RoomVolumeM3);

        return new RoomLeakageResult(
            delta,
            pi,
            k2,
            maximumDelta,
            delta <= maximumDelta,
            "δ = F_H / V_r; K2 = Π × δ × τпод × √H",
            $"{_norms.Id}, приложение Д и таблица Г.16");
    }
}
