using FireEngineering.Domain.Norms;

namespace FireEngineering.Application.Gas;

public sealed record GasAgentMassInput(
    string AgentId,
    double RoomVolumeM3,
    double ConcentrationPercent,
    double MinimumTemperatureCelsius,
    double AltitudeMeters,
    double K2);

public sealed record GasAgentMassResult(
    double VaporDensityKgM3,
    double K3,
    double RoomMassKg,
    string Formula,
    string NormativeProfileId);

public sealed class GasAgentMassCalculator
{
    private const double CelsiusToKelvin = 273.15;
    private readonly INormativeProfile _norms;

    public GasAgentMassCalculator(INormativeProfile norms)
    {
        _norms = norms ?? throw new ArgumentNullException(nameof(norms));
    }

    public GasAgentMassResult Calculate(GasAgentMassInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        ValidatePositive(input.RoomVolumeM3, nameof(input.RoomVolumeM3));
        ValidatePositive(input.ConcentrationPercent, nameof(input.ConcentrationPercent));
        ValidateNonNegative(input.K2, nameof(input.K2));

        if (input.ConcentrationPercent >= 100)
            throw new ArgumentOutOfRangeException(nameof(input.ConcentrationPercent));

        double temperatureKelvin = input.MinimumTemperatureCelsius + CelsiusToKelvin;
        ValidatePositive(temperatureKelvin, nameof(input.MinimumTemperatureCelsius));

        double rho0 = _norms.GetReferenceVaporDensity(input.AgentId);
        double t0 = _norms.GetReferenceTemperatureKelvin(input.AgentId);
        double k3 = _norms.GetAltitudeCorrection(input.AgentId, input.AltitudeMeters);
        double rho1 = rho0 * (t0 / temperatureKelvin) * k3;
        double concentrationTerm = Math.Log(100.0 / (100.0 - input.ConcentrationPercent));
        double roomMass = input.RoomVolumeM3 * rho1 * concentrationTerm * (1.0 + input.K2);

        if (!double.IsFinite(roomMass) || roomMass <= 0)
            throw new InvalidOperationException("Получена некорректная масса ГОТВ.");

        return new GasAgentMassResult(rho1, k3, roomMass,
            "Mr = Vr × ρ1 × ln(100 / (100 − Cn)) × (1 + K2)",
            _norms.Id);
    }

    private static void ValidatePositive(double value, string name)
    {
        if (!double.IsFinite(value) || value <= 0)
            throw new ArgumentOutOfRangeException(name);
    }

    private static void ValidateNonNegative(double value, string name)
    {
        if (!double.IsFinite(value) || value < 0)
            throw new ArgumentOutOfRangeException(name);
    }
}
