using GymTrackerApp.Models;

namespace GymTrackerApp.ViewModels;

public class WorkoutListingViewModel : ViewModelBase
{
    private readonly IObservable<Workout> _workouts;
}