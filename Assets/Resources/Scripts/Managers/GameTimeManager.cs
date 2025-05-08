using System;
using System.IO;
using UnityEngine;

public static class GameTimeManager
{
    private const string FileName = "game_time_save.json";
    private static string FilePath =>
        Path.Combine(Application.persistentDataPath, FileName);

    [Serializable]
    private class TimeData
    {
        public float elapsedTime;
    }

    public static void SaveTime(float elapsedTime)
    {
        var data = new TimeData { elapsedTime = elapsedTime };
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(FilePath, json);
    }

    public static float LoadTime()
    {
        if (!File.Exists(FilePath))
        {
            return 0f;
        }

        try
        {
            string json = File.ReadAllText(FilePath);
            var data = JsonUtility.FromJson<TimeData>(json);
            return data.elapsedTime;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error: {e}");
            return 0f;
        }
    }
}
