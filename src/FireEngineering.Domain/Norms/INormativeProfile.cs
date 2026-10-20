using FireEngineering.Domain.Leakage;

namespace FireEngineering.Domain.Norms;

public interface INormativeProfile
{
    string Id { get; }
    string Edition { get; }
    NormativeSource GetSource(string key);
    double GetOpeningPositionParameter(OpeningDistribution distribution);
    double GetMaximumLeakageParameter(double roomVolumeM3);
    double GetReferenceVaporDensity(string agentId);
    double GetReferenceTemperatureKelvin(string agentId);
    double GetAltitudeCorrection(string agentId, double altitudeMeters);
    double GetMassReserveFactor(string agentId);
}
