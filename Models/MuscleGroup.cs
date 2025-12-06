namespace GymTrackerApp.Models;
    public enum MuscleGroup
    {
        Chest,
        Back,
        Legs,
        Shoulders,
        Arms,
        Core,
        FullBody,
        Other
    }
public static class MuscleGroupExtensions
{
    public static string ToRussian(MuscleGroup group) =>
        group switch
        {
            MuscleGroup.Chest => "Грудь",
            MuscleGroup.Back => "Спина",
            MuscleGroup.Legs => "Ноги",
            MuscleGroup.Shoulders => "Плечи",
            MuscleGroup.Arms => "Руки",
            MuscleGroup.Core => "Кор",
            MuscleGroup.FullBody => "Все тело",
            _ => "Другое"
        };
    
    public static MuscleGroup FromRussian(string name) =>
        name switch
        {
            "Грудь" => MuscleGroup.Chest,
            "Спина" => MuscleGroup.Back,
            "Ноги" => MuscleGroup.Legs,
            "Плечи" => MuscleGroup.Shoulders,
            "Руки" => MuscleGroup.Arms,
            "Кор" => MuscleGroup.Core,
            "Все тело" => MuscleGroup.FullBody,
            _ => MuscleGroup.Other
        };
}