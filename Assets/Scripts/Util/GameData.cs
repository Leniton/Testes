using System.Collections.Generic;
using LenixSO.Logger;

namespace Util
{
    public static class GameData
    {
        private static readonly Dictionary<string, object> _gameData = new();

        public static void SetData(string key, object data)
        {
            if (_gameData.ContainsKey(key))
                Logger.LogWarning($"Key '{key}' already exists in GameData. Overwriting existing data.");
            _gameData[key] = data;
        }

        public static string GetStringData(string key)
        {
            if (_gameData.TryGetValue(key, out var data)) return data.ToString();
            Logger.LogError($"Key '{key}' not found in GameData.");
            return string.Empty;
        }

        public static T GetData<T>(string key, T ifMissing = default) 
            => _gameData.ContainsKey(key) ? (T)_gameData[key] : ifMissing;
    }
}