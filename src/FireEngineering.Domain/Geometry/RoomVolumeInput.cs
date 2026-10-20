namespace FireEngineering.Domain.Geometry;

public sealed record RoomVolumeInput
{
    public required IReadOnlyList<RoomPart> RoomParts { get; init; }
    public double IncludedVentilationVolumeM3 { get; init; }
    public double ApprovedDeductionM3 { get; init; }
    public string? DeductionNormativeReference { get; init; }
    public string? DeductionDescription { get; init; }
}
