using System.Linq;
using System.Windows;
using GymTrackerApp.Models;

namespace GymTrackerApp.Views;

public partial class AddExerciseDialog : Window
{
    public string? ExerciseName { get; private set; }
    public MuscleGroup SelectedMuscleGroup { get; private set; } = MuscleGroup.Other;

    public AddExerciseDialog()
    {
        InitializeComponent();

        var items = new[]
        {
            "Грудь","Спина","Ноги","Плечи","Руки","Кор","Все тело","Другое"
        };

        GroupBox.ItemsSource = items;
        GroupBox.SelectedItem = "Другое";
        NameBox.Focus();
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        ExerciseName = NameBox.Text?.Trim();

        var selected = GroupBox.SelectedItem?.ToString() ?? "Другое";
        SelectedMuscleGroup = MuscleGroupExtensions.FromRussian(selected);

        if (string.IsNullOrWhiteSpace(ExerciseName))
        {
            MessageBox.Show("Введите название упражнения.", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
