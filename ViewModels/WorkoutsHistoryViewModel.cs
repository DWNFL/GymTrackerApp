using GymTrackerApp.Models;
using GymTrackerApp.Services;

using System.Collections.ObjectModel;

namespace GymTrackerApp.ViewModels;

public class WorkoutsHistoryViewModel : ViewModelBase
{
    public ObservableCollection<WorkoutViewModel> Workouts { get; }
    public WorkoutsHistoryViewModel(WorkoutStore workoutStore, ExerciseStore exercisesDefinitions)
    {
        Workouts = new ObservableCollection<WorkoutViewModel>(workoutStore.Workouts.Select(w => new  WorkoutViewModel(w, exercisesDefinitions.Exercises)));
    }
    
    public WorkoutViewModel? SelectedWorkout{ get; set; }
}