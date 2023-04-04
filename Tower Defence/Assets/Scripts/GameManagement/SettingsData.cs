using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "ScriptableObjects/SettingsData")]
public class SettingsData : ScriptableObject
{
    [field: SerializeField]
    public Difficulty SelectedDifficulty { get; set; } = Difficulty.NORMAL;
    [field: Space]
    [field: SerializeField]
    public float EasyDifficultyMultiplier { get; private set; }
    [field: SerializeField]
    public float NormalDifficultyMultiplier { get; private set; }
    [field: SerializeField]
    public float HardDifficultyMultiplier { get; private set; }
    [field: SerializeField]
    public float DemonicDifficultyMultiplier { get; private set; }

    public float GetDifficultyMultiplier()
    {
        switch (SelectedDifficulty)
        {
            case Difficulty.EASY:
                return EasyDifficultyMultiplier;

            case Difficulty.NORMAL:
                return NormalDifficultyMultiplier;

            case Difficulty.HARD:
                return HardDifficultyMultiplier;

            case Difficulty.DEMONIC:
                return DemonicDifficultyMultiplier;

            default:
                return NormalDifficultyMultiplier;
        }
    }

    public enum Difficulty
    {
        EASY,
        NORMAL,
        HARD,
        DEMONIC
    }
}
