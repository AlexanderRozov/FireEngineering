namespace FireEngineering.Domain.Geometry;

public sealed record RoomVolumeResult(
    double RoomPartsVolumeM3,
    double IncludedVentilationVolumeM3,
    double ApprovedDeductionM3,
    double CalculatedVolumeM3,
    string Formula);
