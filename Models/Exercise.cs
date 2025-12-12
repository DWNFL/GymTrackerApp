namespace GymTrackerApp.Models;

public class Exercise
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public MuscleGroup MuscleGroup { get; set; } = MuscleGroup.Other;

    public string? Description { get; set; }   

    public Exercise() { }

    public Exercise(string name, MuscleGroup muscleGroup, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название упражнения не может быть пустым.", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
        MuscleGroup = muscleGroup;
        Description = description;
    }
}