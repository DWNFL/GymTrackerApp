using GymTrackerApp.Models;

namespace GymTrackerApp.ViewModels;

public class ExerciseViewModel : ViewModelBase
{
    private readonly Exercise _exercise;

    public ExerciseViewModel(Exercise exercise)
    {
        _exercise = exercise;
    }

    public Guid Id => _exercise.Id;

    public string Name
    {
        get => _exercise.Name;
        set
        {
            if (_exercise.Name != value)
            {
                _exercise.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public MuscleGroup MuscleGroupEnum
    {
        get => _exercise.MuscleGroup;
        set
        {
            if (_exercise.MuscleGroup != value)
            {
                _exercise.MuscleGroup = value;
                OnPropertyChanged(nameof(MuscleGroupEnum));
                OnPropertyChanged(nameof(MuscleGroup));
            }
        }
    }

    public string MuscleGroup => MuscleGroupExtensions.ToRussian(_exercise.MuscleGroup);

    public string? Description
    {
        get => _exercise.Description;
        set
        {
            if (_exercise.Description != value)
            {
                _exercise.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }
}