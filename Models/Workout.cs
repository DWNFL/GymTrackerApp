namespace GymTrackerApp.Models;

public class Workout
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TimeSpan Duration { get; set; }
    public DateTime Date { get; set; }

    public List<WorkoutExercise> Exercises { get; set; } = new();
    
    
    public Workout() {}
}