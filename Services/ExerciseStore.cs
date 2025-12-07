using GymTrackerApp.Models;

namespace GymTrackerApp.Services;

public class ExerciseStore
{
    public List<Exercise> Exercises { get; private set; } = new();

    public void SetExercises (List<Exercise> exercises)
    {
        Exercises = exercises;
    }

    public void AddExercise(Exercise exercise)
    {
        Exercises.Add(exercise);
    }

    public void RemoveExercise(Guid id)
    {
        var exercise = Exercises.FirstOrDefault(e => e.Id == id);
        if (exercise != null)
            Exercises.Remove(exercise);
    }
}