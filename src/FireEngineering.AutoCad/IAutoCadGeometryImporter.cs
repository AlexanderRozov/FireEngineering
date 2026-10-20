using FireEngineering.Domain.Geometry;

namespace FireEngineering.AutoCad;

public interface IAutoCadGeometryImporter
{
    Task<IReadOnlyList<RoomPart>> ImportRoomsAsync(string drawingPath, CancellationToken cancellationToken = default);
    Task<bool> CanImportAsync(string drawingPath);
}

public sealed class StubAutoCadImporter : IAutoCadGeometryImporter
{
    public Task<IReadOnlyList<RoomPart>> ImportRoomsAsync(string drawingPath, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<RoomPart> demo =
        [
            new RoomBox("Imported Room", 12.0, 8.5, 3.2),
            new RoomCylinder("Imported Cylinder", 1.5, 3.2)
        ];
        return Task.FromResult(demo);
    }

    public Task<bool> CanImportAsync(string drawingPath) =>
        Task.FromResult(File.Exists(drawingPath) || drawingPath.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase));
}
