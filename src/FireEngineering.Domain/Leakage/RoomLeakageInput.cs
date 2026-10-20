namespace FireEngineering.Domain.Leakage;

public sealed record RoomLeakageInput
{
    public required double RoomVolumeM3 { get; init; }
    public required double PermanentlyOpenAreaM2 { get; init; }
    public required double RoomHeightM { get; init; }
    public required double AgentDeliveryTimeSeconds { get; init; }
    public required OpeningDistribution Distribution { get; init; }
    public required string DistributionJustification { get; init; }
}
