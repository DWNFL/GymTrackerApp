using GymTrackerApp.Models;
using GymTrackerApp.Commands;
using GymTrackerApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

namespace GymTrackerApp.ViewModels;

public class WorkoutViewModel : CollectionHostViewModel<WorkoutExerciseViewModel>
{
    private readonly Workout _workout;
    private DateTime _startTime;
    private DispatcherTimer _timer;
    public ObservableCollection<WorkoutExerciseViewModel> WorkoutExercises { get; }
    public ObservableCollection<ExerciseViewModel> ExerciseDefinitions { get; }
    public BaseCommand StartAddExerciseCommand { get; } 
    public BaseCommand RemoveExerciseCommand { get; }
    public BaseCommand FinishCommand { get; }
    public BaseCommand DeleteCommand { get; }
    
    public WorkoutViewModel(Workout workout, IEnumerable<Exercise> exerciseDefinitons)
    {
        _workout = workout;

        WorkoutExercises = new ObservableCollection<WorkoutExerciseViewModel>();
        ExerciseDefinitions = new ObservableCollection<ExerciseViewModel>(
            exerciseDefinitons.Select(e => new ExerciseViewModel(e)));

        foreach (var exercise in workout.Exercises)
        {
            var definition = exerciseDefinitons.First(x => x.Id == exercise.ExercisesId);
            var definitionViewModel = new ExerciseViewModel(definition);
            var workoutExercise = new WorkoutExerciseViewModel(exercise, definitionViewModel);
            
            AddWithEvents(WorkoutExercises, workoutExercise, ExerciseChanged);
        }
        
        _startTime = DateTime.Now;

        _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += (_, _) => OnPropertyChanged(nameof(CurrentDuration));
        _timer.Start();
        
        StartAddExerciseCommand = new BaseCommand(_ =>
        {
            IsAddingExercise = true;
            IsComboOpen = true; 
        });
        RemoveExerciseCommand = new BaseCommand(workoutExercise =>
        {
            if (workoutExercise is WorkoutExerciseViewModel workoutExerciseViewModel)
                RemoveExercise(workoutExerciseViewModel);
        });
        FinishCommand = new BaseCommand(_ =>
        {
            FinishWorkout();             // остановим таймер, зафиксируем Duration

            var service = new JsonWorkoutService();
            _ = service.SaveWorkoutAsync(_workout);   // простое сохранение в json
        });
        
        DeleteCommand = new BaseCommand(_ =>
        {
            _workout.Exercises.Clear();
            WorkoutExercises.Clear();

            Name = string.Empty;
            Date = DateTime.Now;
            Duration = TimeSpan.Zero;

            _startTime = DateTime.Now;
        });

    }
    
    public double TotalWeight =>
        WorkoutExercises.Sum(e =>
            e.Sets.Sum(s => s.Weight * s.Reps));
    
    public int TotalSets =>
        WorkoutExercises.Sum(e => e.Sets.Count);
    
    public TimeSpan CurrentDuration => DateTime.Now - _startTime;
    
    public TimeSpan Duration
    {
        get => _workout.Duration;
        set
        {
            if (_workout.Duration != value)
            {
                _workout.Duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }
    }

    public void FinishWorkout()
    {
        _timer.Stop();
        Duration = CurrentDuration;
    }
    
    private bool _isAddingExercise;

    public bool IsAddingExercise
    {
        get => _isAddingExercise;
        set
        {
            _isAddingExercise = value;
            OnPropertyChanged(nameof(IsAddingExercise));
        }
    }
    private bool _isComboOpen;
    public bool IsComboOpen
    {
        get => _isComboOpen;
        set
        {
            if (_isComboOpen == value) return;
            _isComboOpen = value;
            OnPropertyChanged(nameof(IsComboOpen));
            
            if(!_isComboOpen && SelectedExerciseDefinition == null)
                IsAddingExercise =  false;
        }
    }
    
    private ExerciseViewModel? _selectedExerciseDefinition;
    public ExerciseViewModel? SelectedExerciseDefinition
    {
        get =>  _selectedExerciseDefinition;
        set
        {
            if (_selectedExerciseDefinition == value) return;
            _selectedExerciseDefinition = value;
            OnPropertyChanged(nameof(SelectedExerciseDefinition));

            if (IsAddingExercise && value != null)
            {
                AddExerciseFromDefinition(value);

                IsAddingExercise = false;

                _selectedExerciseDefinition = null;
                OnPropertyChanged(nameof(SelectedExerciseDefinition));
            }
        }
    }
    private void AddExerciseFromDefinition(ExerciseViewModel definition)
    {
        var exercise = new WorkoutExercise
        {
            ExercisesId = definition.Id,
            Sets = new List<WorkoutSet>()
        };

        _workout.Exercises.Add(exercise);

        var exerciseViewModel = new WorkoutExerciseViewModel(exercise, definition);
        AddWithEvents(WorkoutExercises, exerciseViewModel, ExerciseChanged);
        IsComboOpen = false;
    }

    private void RemoveExercise(WorkoutExerciseViewModel workoutExerciseViewModel)
    {
        _workout.Exercises.Remove(workoutExerciseViewModel.Model);
        RemoveWithEvents(WorkoutExercises, workoutExerciseViewModel, ExerciseChanged);
    }
    
    public DateTime Date
    {
        get => _workout.Date;
        set
        {
            _workout.Date = value;
            OnPropertyChanged(nameof(Date));
        }
    }
    public string Name
    {
        get =>  _workout.Name;
        set
        {    
            if (_workout.Name != value)
            {
                _workout.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
    private void ExerciseChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(TotalWeight));
        OnPropertyChanged(nameof(TotalSets));
    }
}