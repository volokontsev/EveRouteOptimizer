using EveRouteOptimizer.Models;
using System.Text;

namespace EveRouteOptimizer.Services;

public static class CsvLoader
{
    public static Dictionary<long, SolarSystem> LoadSystemsFromResource(byte[] csvBytes)
    {
        var systems = new Dictionary<long, SolarSystem>();
        using var stream = new MemoryStream(csvBytes);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        string? header = reader.ReadLine();
        var columns = header.Split(',')
            .Select((name, index) => (name.Trim().Trim('"'), index))
            .ToDictionary(t => t.Item1, t => t.index);

        if (!columns.ContainsKey("solarSystemID") || !columns.ContainsKey("solarSystemName"))
            throw new Exception("Не найдены необходимые колонки в заголовке CSV.");

        int idCol = columns["solarSystemID"];
        int nameCol = columns["solarSystemName"];

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split(',');
            if (parts.Length <= Math.Max(idCol, nameCol)) continue;

            long id = long.Parse(parts[idCol]);
            string name = parts[nameCol].Trim().Trim('"');

            systems[id] = new SolarSystem { Id = id, Name = name };
        }

        return systems;
    }

    public static List<(long From, long To)> LoadJumpsFromResource(byte[] csvBytes)
    {
        var jumps = new List<(long, long)>();
        using var stream = new MemoryStream(csvBytes);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        string? header = reader.ReadLine();
        var columns = header.Split(',')
            .Select((name, index) => (name.Trim(), index))
            .ToDictionary(t => t.Item1, t => t.index);

        if (!columns.ContainsKey("fromSolarSystemID") || !columns.ContainsKey("toSolarSystemID"))
            throw new Exception("Не найдены from/to колонки в заголовке CSV.");

        int fromCol = columns["fromSolarSystemID"];
        int toCol = columns["toSolarSystemID"];

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split(',');
            if (parts.Length <= Math.Max(fromCol, toCol)) continue;

            long from = long.Parse(parts[fromCol]);
            long to = long.Parse(parts[toCol]);
            jumps.Add((from, to));
        }

        return jumps;
    }

    public static (Dictionary<long, SolarSystem> Systems, Dictionary<long, List<long>> Graph) LoadEmbedded()
    {
        var systems = LoadSystemsFromResource(Properties.Resources.mapSolarSystems);
        var jumps = LoadJumpsFromResource(Properties.Resources.mapSolarSystemJumps);

        var builder = new GraphBuilder(new HashSet<long>(systems.Keys));
        builder.Build(jumps);

        return (systems, builder.GetGraph());
    }
}
