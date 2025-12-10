using GymTrackerApp.Commands;
using GymTrackerApp.Services;
using GymTrackerApp.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace GymTrackerApp.ViewModels;

public class WorkoutsHistoryViewModel : ViewModelBase
{
    private readonly WorkoutStore _workoutStore;
    private readonly ExerciseStore _exerciseStore;

    public ObservableCollection<WorkoutViewModel> Workouts { get; }

    public WorkoutViewModel? SelectedWorkout { get; set; }

    public BaseCommand OpenWorkoutCommand { get; }

    public WorkoutsHistoryViewModel(WorkoutStore workoutStore, ExerciseStore exercisesDefinitions)
    {
        _workoutStore = workoutStore;
        _exerciseStore = exercisesDefinitions;

        Workouts = new ObservableCollection<WorkoutViewModel>(
            workoutStore.Workouts
                .OrderByDescending(w => w.Date)
                .Select(w => new WorkoutViewModel(
                    w,
                    exercisesDefinitions.Exercises,
                    startTimer: false,
                    exerciseStore: exercisesDefinitions,
                    isReadOnly: true))
        );

        OpenWorkoutCommand = new BaseCommand(param =>
        {
            if (param is not WorkoutViewModel wvm) return;

            var workoutModel = wvm.Model;
            
            var detailsVm = new WorkoutViewModel(
                workoutModel,
                _exerciseStore.Exercises,
                workoutStore: _workoutStore,
                onFinished: null,
                startTimer: false,
                exerciseStore: _exerciseStore,
                isReadOnly: true);

            var win = new WorkoutDetailsWindow
            {
                Owner = Application.Current.MainWindow,
                DataContext = detailsVm,
                Title = string.IsNullOrWhiteSpace(detailsVm.Name) ? "Тренировка" : detailsVm.Name
            };

            win.ShowDialog();
        });
    }
}