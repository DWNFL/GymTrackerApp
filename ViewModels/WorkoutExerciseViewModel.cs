using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GymTrackerApp.Commands;
using GymTrackerApp.Models;

namespace GymTrackerApp.ViewModels;

public class WorkoutExerciseViewModel : CollectionHostViewModel<WorkoutSetViewModel>
{
    private readonly WorkoutExercise _exercise;              // модель упражнения в тренировке
    private ExerciseViewModel _exerciseDefinition;           // выбранный тип упражнения

    public WorkoutExercise Model => _exercise;
    public ObservableCollection<WorkoutSetViewModel> Sets { get; }

    public BaseCommand AddSetCommand { get; }
    public BaseCommand RemoveSetCommand { get; }

    public WorkoutExerciseViewModel(WorkoutExercise exercise, ExerciseViewModel exerciseDefinition)
    {
        _exercise = exercise ?? throw new ArgumentNullException(nameof(exercise));
        _exerciseDefinition = exerciseDefinition ?? throw new ArgumentNullException(nameof(exerciseDefinition));

        Sets = new ObservableCollection<WorkoutSetViewModel>();

        foreach (var set in _exercise.Sets)
        {
            var vm = new WorkoutSetViewModel(set);
            AddWithEvents(Sets, vm, SetChanged);
        }

        AddSetCommand = new BaseCommand(_ => AddSet());

        RemoveSetCommand = new BaseCommand(set =>
        {
            if (set is WorkoutSetViewModel setVm)
                RemoveSet(setVm);
        });
    }
    
    public ExerciseViewModel? ExerciseDefinition
    {
        get => _exerciseDefinition;
        set
        {
            if (value is null) return; // <- фикс твоего NullReferenceException
            if (ReferenceEquals(_exerciseDefinition, value)) return;

            _exerciseDefinition = value;
            _exercise.ExercisesId = value.Id;

            OnPropertyChanged(nameof(ExerciseDefinition));
        }
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

    private void SetChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Sets));
    }

    public void AddSet()
    {
        var setModel = new WorkoutSet
        {
            Order = Sets.Count + 1,
            Weight = 0,
            Reps = 0
        };

        _exercise.Sets.Add(setModel);

        var setVm = new WorkoutSetViewModel(setModel);
        AddWithEvents(Sets, setVm, SetChanged);
    }

    public void RemoveSet(WorkoutSetViewModel setViewModel)
    {
        var index = Sets.IndexOf(setViewModel);
        if (index < 0) return;

        _exercise.Sets.RemoveAt(index);
        RemoveWithEvents(Sets, setViewModel, SetChanged);

        RecalculateOrders();
    }


    private void RecalculateOrders()
    {
        for (int i = 0; i < Sets.Count; i++)
        {
            Sets[i].Order = i + 1;        
            _exercise.Sets[i].Order = i + 1;
        }
    }
}
