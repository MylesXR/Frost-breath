using System.IO;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    private static string logFilePath;

    void Awake()
    {

        logFilePath = Application.persistentDataPath + "/DebugLog.txt";
        Application.logMessageReceived += LogToFile;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= LogToFile;
    }

    private static void LogToFile(string condition, string stackTrace, LogType type)
    {
        File.AppendAllText(logFilePath, $"{type}: {condition}\n");
    }

    public void OpenLogFile()
    {
        Application.OpenURL(logFilePath);
    }
}