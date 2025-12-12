using System;
using System.Globalization;
using System.Windows.Data;
using GymTrackerApp.Models;

namespace GymTrackerApp.Services
{
    public class MuscleGroupToRussianConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is MuscleGroup mg)
                return MuscleGroupExtensions.ToRussian(mg);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string s)
                return MuscleGroupExtensions.FromRussian(s);

            return MuscleGroup.Other;
        }
    }
}