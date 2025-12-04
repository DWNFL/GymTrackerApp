using System.Text.Json;
using System.IO;
using GymTrackerApp.Models;


namespace GymTrackerApp.Services;

public class JsonExersiceService : IExerciseService
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };
        
    public JsonExersiceService(string? filePath = null)
    {
        _filePath = filePath
            ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data","exercises.json");
    }

    public async Task<List<Exercise>> GetExercisesAsync()
    {
        if (!File.Exists(_filePath))
            return new List<Exercise>();

        try
        {
            string json = await File.ReadAllTextAsync(_filePath);
            var list = JsonSerializer.Deserialize<List<Exercise>>(json, _jsonOptions);

            var valid = list
                .Where(e => !string.IsNullOrWhiteSpace(e.Name))
                .ToList();

            foreach (var exercise in valid)
            {
                if (exercise.Id == Guid.Empty)
                    exercise.Id = Guid.NewGuid();
            }

            return valid;
        }
        catch (Exception ex)
        {
            return new List<Exercise>();
        }
    }

    public async Task SaveExercisesAsync(List<Exercise> exercise)
    {
        string? dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(dir)) 
            Directory.CreateDirectory(dir);
        
        string json = JsonSerializer.Serialize(exercise, _jsonOptions);
        
        await File.WriteAllTextAsync(_filePath, json);
    }
}