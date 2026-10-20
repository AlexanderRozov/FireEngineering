using FireEngineering.Domain.Norms;

namespace FireEngineering.Application.Gas;

public sealed record TotalAgentMassInput(
    string AgentId,
    double RoomMassKg,
    double ResidualAgentMassKg,
    double ModuleResidualMassKg,
    int ModuleCount);

public sealed record TotalAgentMassResult(
    double RoomMassKg,
    double PipeResidualMassKg,
    double TotalModuleResidualMassKg,
    double MassBeforeReserveKg,
    double ReserveFactor,
    double TotalRequiredMassKg,
    string Formula,
    string NormativeProfileId);

public sealed class TotalAgentMassCalculator
{
    private readonly INormativeProfile _norms;

    public TotalAgentMassCalculator(INormativeProfile norms)
    {
        _norms = norms ?? throw new ArgumentNullException(nameof(norms));
    }

    public TotalAgentMassResult Calculate(TotalAgentMassInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (input.ModuleCount < 1)
            throw new ArgumentOutOfRangeException(nameof(input.ModuleCount));

        double reserveFactor = _norms.GetMassReserveFactor(input.AgentId);
        double totalModuleResidual = input.ModuleResidualMassKg * input.ModuleCount;
        double massBeforeReserve = input.RoomMassKg + input.ResidualAgentMassKg + totalModuleResidual;

        return new TotalAgentMassResult(
            input.RoomMassKg,
            input.ResidualAgentMassKg,
            totalModuleResidual,
            massBeforeReserve,
            reserveFactor,
            massBeforeReserve * reserveFactor,
            "Mg = K1 × (Mr + Mtr + Mb × n)",
            _norms.Id);
    }
}
