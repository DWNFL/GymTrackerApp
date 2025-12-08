using System.Windows;

namespace GymTrackerApp.Views;

public partial class FinishWorkoutDialog : Window
{
    public string WorkoutName => (NameBox.Text ?? "").Trim();
    public string WorkoutDescription => (DescBox.Text ?? "").Trim();

    public FinishWorkoutDialog(string currentName, string? currentDescription)
    {
        InitializeComponent();

        NameBox.Text = currentName ?? "";
        DescBox.Text = currentDescription ?? "";

        NameBox.Focus();
        NameBox.SelectAll();
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(WorkoutName))
        {
            MessageBox.Show("Название не может быть пустым.", "Ошибка",
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