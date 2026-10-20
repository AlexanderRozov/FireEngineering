namespace FireEngineering.Domain.Leakage;

public sealed record RoomLeakageResult(
    double DeltaPerMeter,
    double OpeningPositionParameter,
    double K2,
    double MaximumAllowedDeltaPerMeter,
    bool MeetsLeakageLimit,
    string Formula,
    string NormativeReference);
