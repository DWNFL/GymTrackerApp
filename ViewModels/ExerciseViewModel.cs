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
    
    public string MuscleGroupDisplay => _exercise.MuscleGroup switch
    {
        MuscleGroup.Chest => "Грудь",
        MuscleGroup.Back => "Спина",
        MuscleGroup.Legs => "Ноги",
        MuscleGroup.Shoulders => "Плечи",
        MuscleGroup.Arms => "Руки",
        MuscleGroup.Core => "Кор",
        MuscleGroup.FullBody => "Все тело",
        _ => "Другое"
    };

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