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

    public async Task<List<Workout>> GetWorkoutAsync()
    {
        if (!File.Exists(_filePath))
            return new List<Workout>();

        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Workout>>(json, _jsonOptions) ?? new List<Workout>();
        }
        catch
        {
            return new List<Workout>();
        }
    }

    public async Task SaveWorkoutAsync(Workout session)
    {
        var dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(dir))
            Directory.CreateDirectory(dir);

        var workouts = await GetWorkoutAsync();

        var idx = workouts.FindIndex(w => w.Id == session.Id);
        if (idx >= 0) workouts[idx] = session;
        else workouts.Add(session);

        var json = JsonSerializer.Serialize(workouts, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }
    
    public async Task DeleteWorkoutAsync(Guid id)
    {
        var dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(dir))
            Directory.CreateDirectory(dir);

        var workouts = await GetWorkoutAsync();

        var existing = workouts.FirstOrDefault(w => w.Id == id);
        if (existing != null)
            workouts.Remove(existing);

        var json = JsonSerializer.Serialize(workouts, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

}
