using GymTrackerApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GymTrackerApp.ViewModels;

public class WorkoutViewModel : ViewModelBase
{
    private readonly Workout _workout;

    public ObservableCollection<WorkoutExerciseViewModel> WorkoutExercises { get; }

    public WorkoutViewModel(Workout workout, IEnumerable<Exercise> exerciseDefinitons)
    {
        _workout = workout;
        
        WorkoutExercises = new ObservableCollection<WorkoutExerciseViewModel>(workout.Exercises.Select(e =>
        {
            var def = exerciseDefinitons.First(x => x.Id == e.ExercisesId);
            return new WorkoutExerciseViewModel(e, def);
        }));

        foreach (var ex in WorkoutExercises)
        {
            ex.PropertyChanged += (_, _) => OnPropertyChanged(nameof(TotalWeight));
        }
        
        WorkoutExercises.CollectionChanged += (_, e) =>
        {
            if (e.NewItems != null)
                foreach (WorkoutExerciseViewModel ex in e.NewItems)
                    ex.PropertyChanged += ExerciseChanged;

            if (e.OldItems != null)
                foreach (WorkoutExerciseViewModel ex in e.OldItems)
                    ex.PropertyChanged -= ExerciseChanged;

            if (WorkoutExercises.Any())
                OnPropertyChanged(nameof(TotalWeight));
        };
    }
    
    public double TotalWeight =>
        WorkoutExercises.Sum(e =>
            e.Sets.Sum(s => s.Weight * s.Reps));
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
    public DateTime StartTime
    {
        get => _workout.StartTime;
        set
        {
            if (_workout.StartTime != value)
            {
                _workout.StartTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }
    }
    public DateTime? EndTime
    {
        get => _workout.EndTime;
        set
        {
            if (_workout.EndTime != value)
            {
                _workout.EndTime = value;
                OnPropertyChanged(nameof(EndTime));
            }
        }
    }
    private void ExerciseChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(TotalWeight));
    }
}