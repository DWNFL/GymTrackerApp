using System.Collections.ObjectModel;
using System.ComponentModel;
using GymTrackerApp.Commands;
using GymTrackerApp.Models;

namespace GymTrackerApp.ViewModels;

public class WorkoutExerciseViewModel : CollectionHostViewModel<WorkoutSetViewModel>
{
    private readonly WorkoutExercise _exercise; // единица упражнения
    private ExerciseViewModel _exerciseDefinition; // тип упражнения
    
    public WorkoutExercise Model =>  _exercise;
    public ObservableCollection<WorkoutSetViewModel> Sets { get; set; }
    
    public BaseCommand AddSetCommand { get; }
    public BaseCommand RemoveSetCommand { get; }
    
    public WorkoutExerciseViewModel(WorkoutExercise exercise, ExerciseViewModel exerciseDefinition)
    {
        _exercise = exercise;
        _exerciseDefinition = exerciseDefinition;

        Sets = new ObservableCollection<WorkoutSetViewModel>();

        foreach (var set in exercise.Sets)
        {
            var workoutSetViewModel = new WorkoutSetViewModel(set);
            AddWithEvents(Sets, workoutSetViewModel, SetChanged);
        }

        AddSetCommand = new BaseCommand(_ => AddSet());
        RemoveSetCommand = new BaseCommand(set =>
        {
            if (set is WorkoutSetViewModel setViewModel)
                RemoveSet(setViewModel);
        });
    }
    public ExerciseViewModel ExerciseDefinition
    {
        get => _exerciseDefinition;
        set
        {
            if (_exerciseDefinition == value) return;
            _exerciseDefinition = value;

            _exercise.ExercisesId = value.Id;

            OnPropertyChanged(nameof(ExerciseDefinition));
        }
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
    public void AddSet()
    {
        var setModel = new WorkoutSet()
        {
            Order = _exercise.Sets.Count + 1,
            Weight = 0,
            Reps = 0
        };
        
        _exercise.Sets.Add(setModel);

        var setViewModel = new WorkoutSetViewModel(setModel);
        AddWithEvents(Sets, setViewModel, SetChanged);
    }

    public void RemoveSet(WorkoutSetViewModel setViewModel)
    {
        _exercise.Sets.Remove(setViewModel.Model);
        RemoveWithEvents(Sets, setViewModel, SetChanged);

        RecalculateOrders();
    }

    private void RecalculateOrders()
    {
        for (int i = 0; i < _exercise.Sets.Count; i++)
        {
            _exercise.Sets[i].Order = i + 1;
            Sets[i].Order = i + 1;
        }
    }
    


}