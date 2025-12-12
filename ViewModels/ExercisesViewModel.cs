using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GymTrackerApp.Commands;
using GymTrackerApp.Models;
using GymTrackerApp.Services;

namespace GymTrackerApp.ViewModels;

public class ExercisesViewModel : ViewModelBase
{
    private readonly ExerciseStore _exerciseStore;

    public ObservableCollection<ExerciseViewModel> Exercises { get; }
    private ExerciseViewModel? _selectedExercise;
    public ExerciseViewModel? SelectedExercise
    {
        get => _selectedExercise;
        set
        {
            if (_selectedExercise == value) return;
            _selectedExercise = value;
            OnPropertyChanged(nameof(SelectedExercise));
        }
    }

    public BaseCommand AddExerciseCommand { get; }
    public BaseCommand DeleteExerciseCommand { get; }
    public BaseCommand SaveCommand { get; }

    public ExercisesViewModel(ExerciseStore exerciseStore)
    {
        _exerciseStore = exerciseStore;

        Exercises = new ObservableCollection<ExerciseViewModel>(
            _exerciseStore.Exercises.Select(e => new ExerciseViewModel(e)));

        SelectedExercise = Exercises.FirstOrDefault();

        AddExerciseCommand = new BaseCommand(_ => AddExercise());
        DeleteExerciseCommand = new BaseCommand(_ => DeleteSelected(), _ => SelectedExercise != null);
        SaveCommand = new BaseCommand(async _ => await SaveAsync());
    }

    private void AddExercise()
    {
        var dlg = new GymTrackerApp.Views.AddExerciseDialog
        {
            Owner = Application.Current.MainWindow
        };

        if (dlg.ShowDialog() != true)
            return;

        var name = (dlg.ExerciseName ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Название не может быть пустым.", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var existing = _exerciseStore.Exercises
            .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        Exercise exercise;
        if (existing != null)
        {
            exercise = existing;
        }
        else
        {
            exercise = new Exercise(name, dlg.SelectedMuscleGroup);
            _exerciseStore.AddExercise(exercise);
            var vm = new ExerciseViewModel(exercise);
            Exercises.Add(vm);
            SelectedExercise = vm;
        }
    }

    private void DeleteSelected()
    {
        if (SelectedExercise == null) return;

        var confirm = MessageBox.Show(
            $"Удалить упражнение \"{SelectedExercise.Name}\"?\n" +
            "Тренировки, которые ссылались на него, при следующем запуске получат восстановленный вид.",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes)
            return;

        var id = SelectedExercise.Id;
        _exerciseStore.RemoveExercise(id);

        var toRemove = SelectedExercise;
        SelectedExercise = null;
        Exercises.Remove(toRemove);
        SelectedExercise = Exercises.FirstOrDefault();
    }

    private async Task SaveAsync()
    {
        var service = new JsonExersiceService();
        await service.SaveExercisesAsync(_exerciseStore.Exercises);
        MessageBox.Show("Список упражнений сохранён.", "Сохранено",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
