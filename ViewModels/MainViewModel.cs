using GymTrackerApp.Commands;
using GymTrackerApp.Models;
using GymTrackerApp.Services;

namespace GymTrackerApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly WorkoutStore _workoutStore;
    private readonly ExerciseStore _exerciseStore;

    private ViewModelBase _currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set
        {
            _currentViewModel = value;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    public BaseCommand ShowHistoryCommand { get; }
    public BaseCommand StartNewWorkoutCommand { get; }
    public BaseCommand ShowExercisesCommand { get; }
    public BaseCommand ShowProgressCommand { get; }

    public MainViewModel(WorkoutStore workoutStore, ExerciseStore exerciseStore)
    {
        _workoutStore = workoutStore ?? throw new ArgumentNullException(nameof(workoutStore));
        _exerciseStore = exerciseStore ?? throw new ArgumentNullException(nameof(exerciseStore));

        ShowHistoryCommand      = new BaseCommand(_ => ShowHistory());
        StartNewWorkoutCommand  = new BaseCommand(_ => StartNewWorkout());
        ShowExercisesCommand    = new BaseCommand(_ => ShowExercises());
        ShowProgressCommand     = new BaseCommand(_ => ShowProgress());

        ShowHistory();
    }

    private void ShowHistory()
    {
        CurrentViewModel = new WorkoutsHistoryViewModel(_workoutStore, _exerciseStore);
    }

    private void StartNewWorkout()
    {
        var workout = new Workout { Date = DateTime.Now };

        CurrentViewModel = new WorkoutViewModel(
            workout,
            _exerciseStore.Exercises,
            workoutStore: _workoutStore,
            onFinished: ShowHistory,
            startTimer: true,
            exerciseStore: _exerciseStore,
            isReadOnly: false);
    }

    private void ShowExercises()
    {
        CurrentViewModel = new ExercisesViewModel(_exerciseStore);
    }

    private void ShowProgress()
    {
        CurrentViewModel = new ProgressViewModel(_workoutStore);
    }
}
