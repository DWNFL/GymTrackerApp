using System.Windows.Controls;
using System.Windows.Input;
using GymTrackerApp.ViewModels;

namespace GymTrackerApp.Views;

public partial class WorkoutsHistoryView : UserControl
{
    public WorkoutsHistoryView()
    {
        InitializeComponent();
    }

    private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is not WorkoutsHistoryViewModel vm) return;

        if (sender is ListViewItem item && item.DataContext is WorkoutViewModel wvm)
        {
            if (vm.OpenWorkoutCommand.CanExecute(wvm))
                vm.OpenWorkoutCommand.Execute(wvm);
        }
        else if (vm.SelectedWorkout != null)
        {
            if (vm.OpenWorkoutCommand.CanExecute(vm.SelectedWorkout))
                vm.OpenWorkoutCommand.Execute(vm.SelectedWorkout);
        }
    }
}
