using GymTrackerApp.Models;

public static class TestDataFactory
{
    public static (List<Exercise> exercises, List<Workout> workouts) Create()
    {
        // Типы упражнений
        var bench = new Exercise("Жим лёжа", MuscleGroup.Chest);
        var squat = new Exercise("Приседания", MuscleGroup.Legs);

        var workout1 = new Workout
        {
            Name = "Тренировка груди и ног",
            Date = DateTime.Today,                 // сегодня
            Duration = TimeSpan.FromHours(1),      // 1 час
            Exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    ExercisesId = bench.Id,        // связь по Id
                    Note = "Рабочие подходы",
                    Sets = new List<WorkoutSet>
                    {
                        new() { Order = 1, Weight = 60, Reps = 10 },
                        new() { Order = 2, Weight = 60, Reps = 8 }
                    }
                },
                new WorkoutExercise
                {
                    ExercisesId = squat.Id,
                    Sets = new List<WorkoutSet>
                    {
                        new() { Order = 1, Weight = 80, Reps = 8 },
                        new() { Order = 2, Weight = 80, Reps = 6 }
                    }
                }
            }
        };

        var workout2 = new Workout
        {
            Name = "Лёгкая тренировка",
            Date = DateTime.Today.AddDays(-1),     
            Duration = TimeSpan.FromMinutes(45),   
            Exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    ExercisesId = bench.Id,
                    Sets = new List<WorkoutSet>
                    {
                        new() { Order = 1, Weight = 40, Reps = 12 }
                    }
                }
            }
        };

        return (new List<Exercise> { bench, squat },
                new List<Workout> { workout1, workout2 });
    }
}
