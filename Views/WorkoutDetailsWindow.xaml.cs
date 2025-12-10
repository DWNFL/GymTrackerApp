using System.Windows;
using GymTrackerApp.ViewModels;

namespace GymTrackerApp.Views;

public partial class WorkoutDetailsWindow : Window
{
    public WorkoutDetailsWindow()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            if (DataContext is WorkoutViewModel vm)
                vm.Deleted += () => Dispatcher.Invoke(Close);
        };
    }
}