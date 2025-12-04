using GymTrackerApp.Models;

namespace GymTrackerApp.Services;

public interface IWorkoutService
{
    Task<List<Workout>> GetWorkoutAsync();
    Task SaveWorkoutAsync(Workout session);
}