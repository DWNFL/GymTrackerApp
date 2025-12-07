using GymTrackerApp.Models;

namespace GymTrackerApp.ViewModels;

public class WorkoutSetViewModel : ViewModelBase
{
    private readonly WorkoutSet _workoutSet;
    
    public WorkoutSet Model => _workoutSet;

    public WorkoutSetViewModel(WorkoutSet workoutSet)
    {
        _workoutSet = workoutSet;
    }
    public int Order
    {
        get => _workoutSet.Order;
        set
        {
            if (_workoutSet.Order == value) return;
            _workoutSet.Order = value;
            OnPropertyChanged(nameof(Order));
        }
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