using GymTrackerApp.Models;
using GymTrackerApp.Services;

namespace GymTrackerApp.ViewModels;

public class CreatingWorkoutViewModel : ViewModelBase
{
    public WorkoutViewModel Workout { get; }

    public CreatingWorkoutViewModel(ExerciseStore exerciseDefinitions)
    {
        Workout = new WorkoutViewModel(new Workout(), exerciseDefinitions.Exercises);
    }
}