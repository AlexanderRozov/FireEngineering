using FireEngineering.Domain.Leakage;
using FireEngineering.Domain.Norms;

namespace FireEngineering.Norms.Sp485;

public sealed class Sp485Profile : INormativeProfile
{
    public string Id => "RU.SP485.2020";
    public string Edition => "2020 + Amendments 1, 2 (Draft)";

    public NormativeSource GetSource(string key) =>
        new("СП 485.1311500.2020", Edition, key, "", "https://protect.gost.ru/", new DateOnly(2026, 10, 18), "Draft");

    public double GetOpeningPositionParameter(OpeningDistribution distribution) => distribution switch
    {
        OpeningDistribution.BalancedLowerAndUpperZones => 0.65,
        OpeningDistribution.UpperZoneOnly => 0.10,
        OpeningDistribution.LowerZoneOnly => 0.25,
        OpeningDistribution.UniformOrOther => 0.40,
        _ => throw new ArgumentOutOfRangeException(nameof(distribution))
    };

    public double GetMaximumLeakageParameter(double roomVolumeM3)
    {
        if (!double.IsFinite(roomVolumeM3) || roomVolumeM3 <= 0)
            throw new ArgumentOutOfRangeException(nameof(roomVolumeM3));
        foreach (var item in Sp485Tables.LeakageLimits)
        {
            if (roomVolumeM3 <= item.MaxVolumeM3)
                return item.Delta;
        }
        throw new InvalidOperationException();
    }

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

    public double GetAltitudeCorrection(string agentId, double altitudeMeters)
    {
        if (agentId != "HFC-227ea")
            throw new NotSupportedException();
        if (!double.IsFinite(altitudeMeters) || altitudeMeters < 0)
            throw new ArgumentOutOfRangeException(nameof(altitudeMeters));
        foreach (var item in Sp485Tables.AltitudeCorrections)
        {
            if (altitudeMeters <= item.MaxAltitudeM)
                return item.K3;
        }
        throw new InvalidOperationException();
    }

    public double GetMassReserveFactor(string agentId) => agentId switch
    {
        "HFC-227ea" => 1.05,
        _ => throw new NotSupportedException()
    };
}
