using GymTrackerApp.Models;

namespace GymTrackerApp.Services;

public class WorkoutStore
{
    public List<Workout> Workouts { get; private set; } = new();

    public void SetWorkouts(List<Workout> workouts)
    {
        Workouts = workouts;
    }

    public void AddWorkout(Workout workout)
    {
        Workouts.Add(workout);
    }

    public void RemoveWorkout(Guid id)
    {
        var w = Workouts.FirstOrDefault(x => x.Id == id);
        if (w != null)
            Workouts.Remove(w);
    }
}