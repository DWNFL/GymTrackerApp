using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using GymTrackerApp.Commands;
using GymTrackerApp.Models;
using GymTrackerApp.Services;

namespace GymTrackerApp.ViewModels
{
    public class WorkoutViewModel : CollectionHostViewModel<WorkoutExerciseViewModel>
    {
        private readonly Workout _workout;
        private readonly WorkoutStore? _workoutStore;
        private readonly ExerciseStore? _exerciseStore;
        private readonly Action? _onFinished;
        private readonly bool _startTimer;

        private DateTime _startTime;
        private DispatcherTimer? _timer;

        public Workout Model => _workout;
        public bool IsReadOnly { get; }

        public ObservableCollection<WorkoutExerciseViewModel> WorkoutExercises { get; }
        public ObservableCollection<ExerciseViewModel> ExerciseDefinitions { get; }

        public ObservableCollection<object> ExercisePickerItems { get; } = new();

        private object? _selectedExercisePickerItem;
        public object? SelectedExercisePickerItem
        {
            get => _selectedExercisePickerItem;
            set
            {
                if (_selectedExercisePickerItem == value) return;
                _selectedExercisePickerItem = value;
                OnPropertyChanged(nameof(SelectedExercisePickerItem));

                if (IsReadOnly) return;

                if (value is AddCustomExercisePickerItem)
                {
                    _selectedExercisePickerItem = null;
                    OnPropertyChanged(nameof(SelectedExercisePickerItem));
                    return;
                }

                if (IsAddingExercise && value is ExerciseViewModel evm)
                {
                    AddExerciseFromDefinition(evm);

                    IsComboOpen = false;
                    _selectedExercisePickerItem = null;
                    OnPropertyChanged(nameof(SelectedExercisePickerItem));
                }
            }
        }

        public BaseCommand StartAddExerciseCommand { get; }
        public BaseCommand RemoveExerciseCommand { get; }
        public BaseCommand FinishCommand { get; }
        public BaseCommand DeleteCommand { get; }             // очистить текущую тренировку
        public BaseCommand AddCustomExerciseCommand { get; }
        public BaseCommand DeleteFromStoreCommand { get; }    // удалить сохранённую тренировку

        public event Action? Deleted; // чтобы окно деталей смогло закрыться

        public WorkoutViewModel(
            Workout workout,
            IEnumerable<Exercise> exerciseDefinitions,
            WorkoutStore? workoutStore = null,
            Action? onFinished = null,
            bool startTimer = true,
            ExerciseStore? exerciseStore = null,
            bool isReadOnly = false)
        {
            _workout = workout ?? throw new ArgumentNullException(nameof(workout));
            _workoutStore = workoutStore;
            _exerciseStore = exerciseStore;
            _onFinished = onFinished;
            _startTimer = startTimer;
            IsReadOnly = isReadOnly;

            if (_workout.Date == default)
                _workout.Date = DateTime.Now;

            WorkoutExercises = new ObservableCollection<WorkoutExerciseViewModel>();
            ExerciseDefinitions = new ObservableCollection<ExerciseViewModel>(
                exerciseDefinitions.Select(e => new ExerciseViewModel(e)));

            foreach (var exercise in workout.Exercises)
            {
                var defVm = ExerciseDefinitions.FirstOrDefault(x => x.Id == exercise.ExercisesId)
                            ?? ExerciseDefinitions.FirstOrDefault()
                            ?? throw new InvalidOperationException("Нет определений упражнений.");

                var workoutExercise = new WorkoutExerciseViewModel(exercise, defVm);
                AddWithEvents(WorkoutExercises, workoutExercise, ExerciseChanged);
            }

            RebuildExercisePickerItems();

            if (_startTimer && !IsReadOnly)
            {
                _startTime = DateTime.Now - _workout.Duration;
                _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                _timer.Tick += (_, _) => OnPropertyChanged(nameof(CurrentDuration));
                _timer.Start();
            }
            else
            {
                _startTime = DateTime.Now;
            }

            StartAddExerciseCommand = new BaseCommand(_ =>
            {
                if (IsReadOnly) return;

                IsAddingExercise = true;
                SelectedExercisePickerItem = null;

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    IsComboOpen = true;
                }), DispatcherPriority.Background);
            });

            RemoveExerciseCommand = new BaseCommand(workoutExercise =>
            {
                if (workoutExercise is WorkoutExerciseViewModel w)
                    RemoveExercise(w);
            }, _ => !IsReadOnly);

            AddCustomExerciseCommand = new BaseCommand(_ => AddCustomExercise(), _ => !IsReadOnly);

            FinishCommand = new BaseCommand(async _ =>
            {
                if (IsReadOnly) return;

                if (IsWorkoutEmpty())
                {
                    MessageBox.Show("Нельзя сохранить пустую тренировку. Добавь упражнение и хотя бы один сет.",
                        "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var dlg = new GymTrackerApp.Views.FinishWorkoutDialog(
                    string.IsNullOrWhiteSpace(Name) ? $"Тренировка {DateTime.Now:dd.MM.yyyy HH:mm}" : Name,
                    Description)
                {
                    Owner = Application.Current.MainWindow
                };

                if (dlg.ShowDialog() != true)
                    return;

                Name = dlg.WorkoutName;
                Description = string.IsNullOrWhiteSpace(dlg.WorkoutDescription) ? null : dlg.WorkoutDescription;

                FinishWorkout();

                try
                {
                    if (_workoutStore != null && !_workoutStore.Workouts.Any(w => w.Id == _workout.Id))
                        _workoutStore.AddWorkout(_workout);

                    var service = new JsonWorkoutService();
                    await service.SaveWorkoutAsync(_workout);

                    _onFinished?.Invoke(); // например, вернуться к истории
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }, _ => !IsReadOnly);

            // очистка текущей тренировки (локально, без удаления из истории/файла)
            DeleteCommand = new BaseCommand(_ =>
            {
                _workout.Exercises.Clear();
                WorkoutExercises.Clear();

                Name = string.Empty;
                Description = null;
                Date = DateTime.Now;
                Duration = TimeSpan.Zero;

                _startTime = DateTime.Now;

                if (_startTimer && !IsReadOnly)
                {
                    _timer?.Stop();
                    _timer?.Start();
                    OnPropertyChanged(nameof(CurrentDuration));
                }

                OnPropertyChanged(nameof(TotalWeight));
                OnPropertyChanged(nameof(TotalSets));
            }, _ => !IsReadOnly);

            // удаление сохранённой тренировки (для просмотра из истории)
            DeleteFromStoreCommand = new BaseCommand(async _ =>
            {
                if (_workout.Id == Guid.Empty)
                    return;

                var confirm = MessageBox.Show(
                    "Удалить эту тренировку?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm != MessageBoxResult.Yes)
                    return;

                try
                {
                    var service = new JsonWorkoutService();
                    await service.DeleteWorkoutAsync(_workout.Id);

                    _workoutStore?.RemoveWorkout(_workout.Id);

                    if (Application.Current.MainWindow?.DataContext is MainViewModel mainVm &&
                        mainVm.CurrentViewModel is WorkoutsHistoryViewModel historyVm)
                    {
                        var vmToRemove = historyVm.Workouts.FirstOrDefault(w => w.Model.Id == _workout.Id);
                        if (vmToRemove != null)
                            historyVm.Workouts.Remove(vmToRemove);
                    }

                    Deleted?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void RebuildExercisePickerItems()
        {
            ExercisePickerItems.Clear();
            ExercisePickerItems.Add(new AddCustomExercisePickerItem());
            foreach (var e in ExerciseDefinitions.OrderBy(x => x.Name))
                ExercisePickerItems.Add(e);

            OnPropertyChanged(nameof(ExercisePickerItems));
        }

        private bool IsWorkoutEmpty()
        {
            if (_workout.Exercises.Count == 0) return true;

            return !_workout.Exercises.Any(ex =>
                ex.Sets != null &&
                ex.Sets.Any(s => s.Reps > 0 && s.Weight >= 0));
        }

        private async void AddCustomExercise()
        {
            IsComboOpen = false;
            IsAddingExercise = false;
            SelectedExercisePickerItem = null;

            if (IsReadOnly) return;

            if (_exerciseStore == null)
            {
                MessageBox.Show("ExerciseStore не передан в WorkoutViewModel — нельзя сохранить своё упражнение.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dlg = new GymTrackerApp.Views.AddExerciseDialog
            {
                Owner = Application.Current.MainWindow
            };

            if (dlg.ShowDialog() != true)
            {
                IsComboOpen = false;
                return;
            }

            var name = (dlg.ExerciseName ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Название упражнения не может быть пустым.", "Ошибка",
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

                var exService = new JsonExersiceService();
                await exService.SaveExercisesAsync(_exerciseStore.Exercises);

                ExerciseDefinitions.Add(new ExerciseViewModel(exercise));
                RebuildExercisePickerItems();
            }

            var vm = ExerciseDefinitions.First(x => x.Id == exercise.Id);
            AddExerciseFromDefinition(vm);

            IsAddingExercise = false;
            IsComboOpen = false;
        }

        public double TotalWeight =>
            WorkoutExercises.Sum(e => e.Sets.Sum(s => s.Weight * s.Reps));

        public int TotalSets =>
            WorkoutExercises.Sum(e => e.Sets.Count);

        public TimeSpan CurrentDuration => (_startTimer && !IsReadOnly) ? (DateTime.Now - _startTime) : Duration;

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
            if (_startTimer && !IsReadOnly) _timer?.Stop();
            Duration = DateTime.Now - _startTime;
            OnPropertyChanged(nameof(CurrentDuration));
        }

        private bool _isAddingExercise;
        public bool IsAddingExercise
        {
            get => _isAddingExercise;
            set
            {
                if (_isAddingExercise == value) return;
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

                if (!_isComboOpen && SelectedExercisePickerItem == null)
                    IsAddingExercise = false;
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
        }

        private void RemoveExercise(WorkoutExerciseViewModel workoutExerciseViewModel)
        {
            _workout.Exercises.Remove(workoutExerciseViewModel.Model);
            RemoveWithEvents(WorkoutExercises, workoutExerciseViewModel, ExerciseChanged);

            OnPropertyChanged(nameof(TotalWeight));
            OnPropertyChanged(nameof(TotalSets));
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
            get => _workout.Name;
            set
            {
                if (_workout.Name != value)
                {
                    _workout.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string? Description
        {
            get => _workout.Description;
            set
            {
                if (_workout.Description != value)
                {
                    _workout.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private void ExerciseChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalWeight));
            OnPropertyChanged(nameof(TotalSets));
        }
    }
}
