using FireEngineering.Application.Gas;
using FireEngineering.Application.Geometry;
using FireEngineering.Application.Leakage;
using FireEngineering.Domain.Geometry;
using FireEngineering.Domain.Leakage;
using FireEngineering.Norms.Sp485;
using Microsoft.AspNetCore.Mvc;

namespace FireEngineering.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculationController : ControllerBase
{
    private readonly RoomVolumeCalculator _volume = new();
    private readonly RoomLeakageCalculator _leakage;
    private readonly GasAgentMassCalculator _mass;
    private readonly TotalAgentMassCalculator _total;

    public CalculationController()
    {
        var norms = new Sp485Profile();
        _leakage = new RoomLeakageCalculator(norms);
        _mass = new GasAgentMassCalculator(norms);
        _total = new TotalAgentMassCalculator(norms);
    }

    [HttpPost("volume")]
    public ActionResult<object> CalculateVolume([FromBody] RoomVolumeInput input)
    {
        try
        {
            var result = _volume.Calculate(input);
            return Ok(new { result.CalculatedVolumeM3, result.Formula });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("hfc227ea")]
    public ActionResult<object> CalculateHfc227ea([FromBody] HfcCalculationRequest request)
    {
        try
        {
            var volume = _volume.Calculate(request.VolumeInput);
            var leakage = _leakage.Calculate(request.LeakageInput with { RoomVolumeM3 = volume.CalculatedVolumeM3 });
            var roomMass = _mass.Calculate(new GasAgentMassInput(
                "HFC-227ea",
                volume.CalculatedVolumeM3,
                request.ConcentrationPercent,
                request.MinTemperatureC,
                request.AltitudeM,
                leakage.K2));

            var total = _total.Calculate(new TotalAgentMassInput(
                "HFC-227ea",
                roomMass.RoomMassKg,
                request.PipeResidualKg,
                request.ModuleResidualKg,
                request.ModuleCount));

            return Ok(new
            {
                VolumeM3 = volume.CalculatedVolumeM3,
                K2 = leakage.K2,
                RoomMassKg = roomMass.RoomMassKg,
                TotalRequiredMassKg = total.TotalRequiredMassKg
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public sealed record HfcCalculationRequest(
    RoomVolumeInput VolumeInput,
    RoomLeakageInput LeakageInput,
    double ConcentrationPercent,
    double MinTemperatureC,
    double AltitudeM,
    double PipeResidualKg,
    double ModuleResidualKg,
    int ModuleCount);
