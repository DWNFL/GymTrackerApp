namespace GymTrackerApp.Models;
    public enum MuscleGroup
    {
        Chest,
        BroadestBack,
        RoundBack,
        Quadriceps,
        LegBiceps,
        AnteriorDelta,
        MiddleDelta,
        BackDelta,
        Biceps,
        Triceps,
        Core,
        Calf,
        Soleous,
        FullBody,
        Buttocks,
        Trapezoids,
        Other
    }
public static class MuscleGroupExtensions
{
    public static string ToRussian(MuscleGroup group) =>
        group switch
        {
            MuscleGroup.Chest => "Грудь",
            MuscleGroup.BroadestBack => "Широчайшие мышцы спины",
            MuscleGroup.RoundBack => "Круглые мышцы спины",
            MuscleGroup.Quadriceps => "Квадрицепс",
            MuscleGroup.LegBiceps => "Бицепс бедра",
            MuscleGroup.AnteriorDelta => "Переденяя дельта",
            MuscleGroup.BackDelta => "Задняя дельта",
            MuscleGroup.MiddleDelta => "Средняя дельта",
            MuscleGroup.Biceps => "Бицепс",
            MuscleGroup.Triceps => "Трицепс",
            MuscleGroup.Calf => "Икры",
            MuscleGroup.Soleous => "Камбаловидная мышца",
            MuscleGroup.Core => "Кор",
            MuscleGroup.Buttocks => "Ягодицы",
            MuscleGroup.Trapezoids => "Трапеции",
            MuscleGroup.FullBody => "Все тело",
            _ => "Другое"
        };
    
    public static MuscleGroup FromRussian(string name) =>
        name switch
        {
              "Грудь" => MuscleGroup.Chest,
              "Широчайшие мышцы спины" => MuscleGroup.BroadestBack,
              "Круглые мышцы спины" => MuscleGroup.RoundBack,
              "Квадрицепс" => MuscleGroup.Quadriceps,
              "Бицепс бедра" => MuscleGroup.LegBiceps,
              "Переденяя дельта" => MuscleGroup.AnteriorDelta,
              "Задняя дельта" => MuscleGroup.BackDelta,
              "Средняя дельта" => MuscleGroup.MiddleDelta,
              "Бицепс" => MuscleGroup.Biceps,
              "Трицепс" => MuscleGroup.Triceps,
              "Икры" => MuscleGroup.Calf,
              "Камбаловидная мышца" => MuscleGroup.Soleous,
              "Кор" => MuscleGroup.Core,
              "Ягодицы" => MuscleGroup.Buttocks,
              "Трапеции" => MuscleGroup.Trapezoids,
              "Все тело" => MuscleGroup.FullBody,
            _ => MuscleGroup.Other
        };
}