/// <summary>
/// Receives a request for a hack with a specific hack type
/// </summary>
public interface IHackable
{
    /// <summary>
    /// True when the system is in a state it can be hacked
    /// </summary>
    bool IsHackable { get; set; }

    /// <summary>
    /// Sets the power state to either ON or OFF
    /// </summary>
    /// <param name="powerOn"></param>
    void PowerHack(bool powerOn);

    /// <summary>
    /// Toggles the current state
    /// </summary>
    void ToggleHack();

    /// <summary>
    /// Triggers a rotation change
    /// </summary>
    void RotationHack();
}
