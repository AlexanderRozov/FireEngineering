namespace FireEngineering.Domain.Geometry;

public abstract record RoomPart(string Name)
{
    public abstract double VolumeM3 { get; }
    public abstract string Topology { get; }
}

public sealed record RoomBox(
    string Name,
    double LengthM,
    double WidthM,
    double HeightM) : RoomPart(Name)
{
    public override double VolumeM3 => LengthM * WidthM * HeightM;
    public override string Topology => "Box";
}

public sealed record RoomCylinder(
    string Name,
    double RadiusM,
    double HeightM) : RoomPart(Name)
{
    public override double VolumeM3 => Math.PI * RadiusM * RadiusM * HeightM;
    public override string Topology => "Cylinder";
}

public sealed record RoomPrism(
    string Name,
    IReadOnlyList<(double X, double Y)> BasePolygon,
    double HeightM) : RoomPart(Name)
{
    public override double VolumeM3
    {
        get
        {
            if (BasePolygon.Count < 3) return 0;
            double area = 0;
            for (int i = 0; i < BasePolygon.Count; i++)
            {
                var p1 = BasePolygon[i];
                var p2 = BasePolygon[(i + 1) % BasePolygon.Count];
                area += p1.X * p2.Y - p2.X * p1.Y;
            }
            return Math.Abs(area) / 2.0 * HeightM;
        }
    }
    public override string Topology => "Prism";
}
