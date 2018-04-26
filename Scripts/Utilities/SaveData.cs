/// <summary>
/// A container for any and all data we want to save/load
/// </summary>
[System.Serializable]
public struct SavedData
{
    // Examples:

    /// <summary>
    /// How many lives the player has level
    /// </summary>
    public int remainingLives;

    /// <summary>
    /// The master volume for music
    /// </summary>
    public float musicVolume;

    /// <summary>
    /// The master volume for sound effects
    /// </summary>
    public float sfxVolume;

    /// <summary>
    /// Stores the values to save
    /// </summary>
    /// <param name="lives"></param>
    /// <param name="music"></param>
    /// <param name="sfx"></param>
    public SavedData(int lives, float music, float sfx)
    {
        remainingLives = lives;
        musicVolume = music;
        sfxVolume = sfx;
    }
}
