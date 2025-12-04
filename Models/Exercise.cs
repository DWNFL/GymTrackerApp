namespace GymTrackerApp.Models;

public class Exercise
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string MuscleGroup { get; set; } = string.Empty;
}