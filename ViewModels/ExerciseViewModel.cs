using GymTrackerApp.Models;

namespace GymTrackerApp.ViewModels;

public class ExerciseViewModel : ViewModelBase
{
    private readonly Exercise _exercise;
    
    public ExerciseViewModel(Exercise exercise)
    {
        _exercise = exercise;
    }

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
    
    public string MuscleGroup 
    {
        get => MuscleGroupExtensions.ToRussian(_exercise.MuscleGroup);
        set
        {
            _exercise.MuscleGroup = MuscleGroupExtensions.FromRussian(value);
            OnPropertyChanged(nameof(MuscleGroup));
        }
    }

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
    public Guid Id => _exercise.Id;
    
}