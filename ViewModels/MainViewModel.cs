using GymTrackerApp.Services;
using GymTrackerApp.Commands;
namespace GymTrackerApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly WorkoutStore _workoutStore = new();
    private readonly ExerciseStore _exerciseStore = new();
    private ViewModelBase _currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
    public BaseCommand ShowHistoryCommand { get; }
    public BaseCommand ShowCreateWorkoutCommand { get; }
    public BaseCommand ShowExercisesCommand { get; }
    public BaseCommand ShowChartsCommand { get; }
    
    public MainViewModel()
    {
        
        CurrentViewModel = new WorkoutsHistoryViewModel(_workoutStore, _exerciseStore, this);

        ShowHistoryCommand = new BaseCommand(_ =>
            CurrentViewModel = new WorkoutsHistoryViewModel(_workoutStore, _exerciseStore, this));

        ShowCreateWorkoutCommand = new BaseCommand(_ =>
            CurrentViewModel = new CreatingWorkoutViewModel(_exerciseStore));   

    }
}