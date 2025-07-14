using EveRouteOptimizer.ESI;
using EveRouteOptimizer.Models;
using System.Collections.Generic;
using System.Linq;

namespace EveRouteOptimizer.Services
{
    public class CharacterManager
    {
        private List<EsiCharacter> _characters = new();
        private long _activeCharacterId;

        public void Load()
        {
            EsiSession.Load();
            _characters = EsiSession.Current.Characters ?? new List<EsiCharacter>();
            _activeCharacterId = EsiSession.Current.ActiveCharacterId;
        }

        public void Save()
        {
            EsiSession.Current.Characters = _characters;
            EsiSession.Current.ActiveCharacterId = _activeCharacterId;
            EsiSession.Save();
        }

        public void AddOrUpdate(EsiCharacter character)
        {
            var existing = _characters.FirstOrDefault(c => c.CharacterId == character.CharacterId);
            if (existing != null)
            {
                existing.AccessToken = character.AccessToken;
                existing.RefreshToken = character.RefreshToken;
                existing.CharacterName = character.CharacterName;
            }
            else
            {
                _characters.Add(character);
            }

            _activeCharacterId = character.CharacterId;
            Save();
        }

        public void Remove(long characterId)
        {
            _characters.RemoveAll(c => c.CharacterId == characterId);
            if (_activeCharacterId == characterId)
                _activeCharacterId = _characters.FirstOrDefault()?.CharacterId ?? 0;

            Save();
        }

        public List<EsiCharacter> GetAll() => _characters;

        public List<EsiCharacter> GetCharacters() => _characters;

        public EsiCharacter? GetActive()
        {
            return _characters.FirstOrDefault(c => c.CharacterId == _activeCharacterId);
        }

        public long GetActiveCharacterId() => _activeCharacterId;

        public void SetActive(long characterId)
        {
            _activeCharacterId = characterId;
            Save();
        }
    }
}
