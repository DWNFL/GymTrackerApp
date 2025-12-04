namespace GymTrackerApp.Models;

public class WorkoutSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Date  { get; set; } =  DateTime.Now;
    public List<WorkoutExercise> Exercises { get; set; }
}