using GymTrackerApp.Models;

namespace GymTrackerApp.ViewModels;

public class WorkoutSetViewModel : ViewModelBase
{
    private readonly WorkoutSet _workoutSet;

    public WorkoutSetViewModel(WorkoutSet workoutSet)
    {
        _workoutSet = workoutSet;
    }

    public double Weight
    {
        get => _workoutSet.Weight;
        set
        {
            _workoutSet.Weight = value;
            OnPropertyChanged(nameof(Weight));
        }
    }
    public int Reps
    {
        get => _workoutSet.Reps;
        set
        {
            _workoutSet.Reps = value;
            OnPropertyChanged(nameof(Reps));
        }
    }
}