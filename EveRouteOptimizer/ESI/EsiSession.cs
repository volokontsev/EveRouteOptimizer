using System.Net.NetworkInformation;
using System.Text.Json;
using EveRouteOptimizer.ESI;

namespace EveRouteOptimizer.ESI
{
    public class EsiSession
    {
        public List<EsiCharacter> Characters { get; set; } = new();
        public long ActiveCharacterId { get; set; }

        public EsiCharacter? ActiveCharacter =>
            Characters.FirstOrDefault(c => c.CharacterId == ActiveCharacterId);

        private static string FilePath => "esi_auth.json";
        public static EsiSession Current { get; private set; } = new();

        public static void Load()
        {
            if (!File.Exists(FilePath)) return;

            try
            {
                string json = File.ReadAllText(FilePath);
                Current = JsonSerializer.Deserialize<EsiSession>(json) ?? new EsiSession();
            }
            catch { }
        }

        public static void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch { }
        }
    }
}
