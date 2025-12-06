namespace GymTrackerApp.Models;

public class Workout
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public DateTime StartTime { get; set; } = DateTime.Now;

    public DateTime? EndTime { get; set; }

    public List<WorkoutExercise> Exercises { get; set; } = new();
    
    // public bool IsTemplate { get; set; }
    
    public Workout() {}
}