using GymTrackerApp.Models;

namespace GymTrackerApp.Services;

public interface IExerciseService
{
    Task<List<Exercise>> GetExercisesAsync();
    Task SaveExercisesAsync(List<Exercise> exercise);
    
}


