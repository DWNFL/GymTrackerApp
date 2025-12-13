namespace GymTrackerApp.Models;

public class WorkoutExercise
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ExercisesId { get; set; }
    public List<WorkoutSet> Sets { get; set; } = new();

}