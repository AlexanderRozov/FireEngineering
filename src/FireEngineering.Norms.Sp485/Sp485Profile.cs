using FireEngineering.Domain.Leakage;
using FireEngineering.Domain.Norms;

namespace FireEngineering.Norms.Sp485;

public sealed class Sp485Profile : INormativeProfile
{
    public string Id => "RU.SP485.2020";
    public string Edition => "2020 + Amendments 1, 2 (Draft)";

    public double GetOpeningPositionParameter(OpeningDistribution distribution) => distribution switch
    {
        OpeningDistribution.BalancedLowerAndUpperZones => 0.65,
        OpeningDistribution.UpperZoneOnly => 0.10,
        OpeningDistribution.LowerZoneOnly => 0.25,
        OpeningDistribution.UniformOrOther => 0.40,
        _ => throw new ArgumentOutOfRangeException(nameof(distribution))
    };

    public double GetReferenceVaporDensity(string agentId) => agentId switch
    {
        "HFC-227ea" => 7.28,
        _ => throw new NotSupportedException($"Для ГОТВ '{agentId}' плотность не настроена.")
    };

    public double GetReferenceTemperatureKelvin(string agentId) => agentId switch
    {
        "HFC-227ea" => 293.15,
        _ => throw new NotSupportedException()
    };

    public double GetAltitudeCorrection(string agentId, double altitudeMeters) => 1.0;

    public double GetMassReserveFactor(string agentId) => agentId == "HFC-227ea" ? 1.05 : throw new NotSupportedException();

    public double GetMaximumLeakageParameter(double roomVolumeM3) => 0.005;

    public NormativeSource GetSource(string key) => new("SP485", "2020", "", "", "", default, "Draft");
}
