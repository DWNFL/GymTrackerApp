using GymTrackerApp.Services;

namespace GymTrackerApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly WorkoutStore _workoutStore = new();
    private readonly ExerciseStore _exerciseStore = new();
    public ViewModelBase CurrentViewModel { get; }

    public MainViewModel()
    {
        CurrentViewModel = new WorkoutsHistoryViewModel(_workoutStore, _exerciseStore);
    }
}