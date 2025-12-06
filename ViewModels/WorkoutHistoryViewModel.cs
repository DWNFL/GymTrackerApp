using GymTrackerApp.Models;
using System.Collections.ObjectModel;

namespace GymTrackerApp.ViewModels;

public class WorkoutHistoryViewModel : ViewModelBase
{
    public ObservableCollection<WorkoutViewModel> Workouts { get; }
    public WorkoutHistoryViewModel(ObservableCollection<WorkoutViewModel> workouts)
    {
        Workouts = workouts;
    }
}