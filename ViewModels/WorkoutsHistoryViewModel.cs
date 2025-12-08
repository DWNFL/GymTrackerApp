using GymTrackerApp.Commands;
using GymTrackerApp.Services;

using System.Collections.ObjectModel;

namespace GymTrackerApp.ViewModels;

public class WorkoutsHistoryViewModel : ViewModelBase
{
    private readonly MainViewModel _mainVm;

    public ObservableCollection<WorkoutViewModel> Workouts { get; }
    public BaseCommand AddWorkoutCommand { get; }

    public WorkoutsHistoryViewModel(
        WorkoutStore workoutStore,
        ExerciseStore exercisesDefinitions,
        MainViewModel mainVm)
    {
        _mainVm = mainVm;

        Workouts = new ObservableCollection<WorkoutViewModel>(
            workoutStore.Workouts.Select(w =>
                new WorkoutViewModel(w, exercisesDefinitions.Exercises)));

        AddWorkoutCommand = new BaseCommand(_ =>
            _mainVm.CurrentViewModel =
                new CreatingWorkoutViewModel(exercisesDefinitions));
    }

    public WorkoutViewModel? SelectedWorkout { get; set; }
}
