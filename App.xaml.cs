using System.Windows;
using GymTrackerApp.Models;
using GymTrackerApp.ViewModels;
using GymTrackerApp.Services;
using GymTrackerApp.Views;
using WorkoutViewModel = GymTrackerApp.ViewModels.WorkoutViewModel;

namespace GymTrackerApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    WorkoutStore _workoutStore =  new WorkoutStore();
    ExerciseStore _exerciseStore = new ExerciseStore();
    
    protected override void OnStartup(StartupEventArgs e)
    {
        var (exercises, workouts) = TestDataFactory.Create();

        _exerciseStore.SetExercises(exercises);
        _workoutStore.SetWorkouts(workouts);
    
        var vm = new WorkoutViewModel(new Workout(), _exerciseStore.Exercises);

        var window = new TestWindow
        {
            DataContext = vm
        };

        window.Show();
    }

}
