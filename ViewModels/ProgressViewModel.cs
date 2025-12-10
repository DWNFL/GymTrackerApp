using System;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;
using GymTrackerApp.Models;
using GymTrackerApp.Services;

namespace GymTrackerApp.ViewModels;

public class ProgressViewModel : ViewModelBase
{
    public SeriesCollection Series { get; }
    public string[] Labels { get; }
    public Func<double, string> YFormatter { get; }

    public ProgressViewModel(WorkoutStore workoutStore)
    {
        var ordered = workoutStore.Workouts
            .OrderBy(w => w.Date)
            .ToList();

        var volumes = ordered
            .Select(CalcVolume)
            .Select(v => (double)v)
            .ToArray();

        Labels = ordered
            .Select(w => w.Date.ToString("dd.MM.yyyy"))
            .ToArray();

        Series = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Объём (кг × повт)",
                Values = new ChartValues<double>(volumes),
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 6
            }
        };

        YFormatter = v => v.ToString("N0");
    }

    private static double CalcVolume(Workout w) =>
        w.Exercises.Sum(e => e.Sets.Sum(s => s.Weight * s.Reps));
}