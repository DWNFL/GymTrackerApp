using GymTrackerApp.Models;

namespace GymTrackerApp.Services;

public interface IWorkoutService
{
    Task<List<WorkoutSession>> GetSessionsAsync();
    Task SaveSessionAsync(WorkoutSession session);
}