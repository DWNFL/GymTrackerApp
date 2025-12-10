using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GymTrackerApp.Models;
using GymTrackerApp.Services;
using GymTrackerApp.ViewModels;

namespace GymTrackerApp;

public partial class App : Application
{
    private readonly WorkoutStore _workoutStore = new();
    private readonly ExerciseStore _exerciseStore = new();

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            await LoadDataAsync();

            var mainVm = new MainViewModel(_workoutStore, _exerciseStore);

            var window = new MainWindow
            {
                DataContext = mainVm
            };

            window.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка запуска приложения: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    private async Task LoadDataAsync()
    {
        var workoutService = new JsonWorkoutService();
        var exerciseService = new JsonExersiceService();

        var exercises = await exerciseService.GetExercisesAsync();
        var workouts = await workoutService.GetWorkoutAsync();

        var knownIds = exercises.Select(x => x.Id).ToHashSet();

        var idsFromWorkouts = workouts
            .SelectMany(w => w.Exercises)
            .Select(ex => ex.ExercisesId)
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        bool exercisesChanged = false;

        foreach (var id in idsFromWorkouts)
        {
            if (knownIds.Contains(id)) continue;

            exercises.Add(new Exercise
            {
                Id = id,
                Name = $"⚠ ВОССТАНОВЛЕНО ({id.ToString()[..8]})",
                MuscleGroup = MuscleGroup.Other
            });

            knownIds.Add(id);
            exercisesChanged = true;
        }

        if (exercisesChanged)
            await exerciseService.SaveExercisesAsync(exercises);

        _exerciseStore.SetExercises(exercises);
        _workoutStore.SetWorkouts(workouts);
    }

}
