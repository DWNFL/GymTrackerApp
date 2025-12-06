using System.Collections.ObjectModel;
using System.ComponentModel;
using GymTrackerApp.Models;

namespace GymTrackerApp.ViewModels;

public class WorkoutExerciseViewModel : ViewModelBase
{
    private readonly WorkoutExercise _exercise; // единица упражнения
    private readonly Exercise _exerciseDefinition; // тип упражнения
    public ObservableCollection<WorkoutSetViewModel> Sets { get; set; }
    
    public WorkoutExerciseViewModel(WorkoutExercise exercise, Exercise exerciseDefinition)
    {
        _exercise = exercise;
        _exerciseDefinition = exerciseDefinition;

        Sets = new ObservableCollection<WorkoutSetViewModel>(
            exercise.Sets.Select(s =>
            {
                var set = new WorkoutSetViewModel(s);
                set.PropertyChanged += SetChanged;
                return set;
            }));

        Sets.CollectionChanged += (_, e) =>
        {
            if (e.NewItems != null)
                foreach (WorkoutExerciseViewModel s in e.NewItems)
                    s.PropertyChanged += SetChanged;

            if (e.OldItems != null)
                foreach (WorkoutExerciseViewModel s in e.OldItems)
                    s.PropertyChanged -= SetChanged;
        };
    }

    public void SetChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Sets));
    }
    public string? Note
    {
        get => _exercise.Note;
        set
        {
            if (_exercise.Note != value)
            {
                _exercise.Note = value;
                OnPropertyChanged(nameof(Note));
            }
        }
    }
    
    public string Name => _exerciseDefinition.Name;
    public string Definition => MuscleGroupExtensions.ToRussian(_exerciseDefinition.MuscleGroup);
    
}