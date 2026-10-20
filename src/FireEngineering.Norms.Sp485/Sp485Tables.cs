namespace FireEngineering.Norms.Sp485;

internal static class Sp485Tables
{
    internal static readonly (double MaxVolumeM3, double Delta)[] LeakageLimits =
    [
        (10, 0.0220),
        (20, 0.0170),
        (30, 0.0140),
        (50, 0.0110),
        (75, 0.0090),
        (100, 0.0080),
        (150, 0.0070),
        (200, 0.0060),
        (250, 0.0055),
        (300, 0.0050),
        (400, 0.0045),
        (500, 0.0040),
        (750, 0.0035),
        (1000, 0.0030),
        (1500, 0.0025),
        (2000, 0.0023),
        (2500, 0.0020),
        (3000, 0.0018),
        (4000, 0.0016),
        (5000, 0.0015),
        (7500, 0.0012),
        (10000, 0.0011),
        (double.PositiveInfinity, 0.0010)
    ];

    internal static readonly (double MaxAltitudeM, double K3)[] AltitudeCorrections =
    [
        (1000, 1.000),
        (1500, 0.885),
        (2000, 0.830),
        (2500, 0.785),
        (3000, 0.735),
        (3500, 0.690),
        (4000, 0.650),
        (4500, 0.610),
        (double.PositiveInfinity, 0.565)
    ];
}
