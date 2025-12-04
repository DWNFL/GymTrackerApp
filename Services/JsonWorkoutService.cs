using System.IO;
using System.Text.Json;
using GymTrackerApp.Models;

namespace GymTrackerApp.Services;

public class JsonWorkoutService : IWorkoutService
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };
    
    public JsonWorkoutService(string? jsonFilePath = null)
    {
        _filePath = jsonFilePath
            ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "workouts.json");
    }

    public async Task<List<WorkoutSession>> GetSessionsAsync()
    {
        if (!File.Exists(_filePath))
            return new List<WorkoutSession>();

        try
        {
            string? json = await File.ReadAllTextAsync(_filePath);
            var list = JsonSerializer.Deserialize<List<WorkoutSession>>(json, _jsonOptions);

            var valid = list;
            
            // Добавить доп. валидацию данных при повреждении файлов json;
            return valid;

        }
        catch (Exception e)
        {
            return new List<WorkoutSession>();
        }
    }

    public async Task SaveSessionAsync(WorkoutSession session)
    {
        string? dir = Path.GetDirectoryName(_filePath);
        if (string.IsNullOrWhiteSpace(dir))
            Directory.CreateDirectory(dir);

        string json = JsonSerializer.Serialize(session, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }
}