using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Security System are systems that the player can hack to alter its behavior
/// </summary>
public class SecuritySystem : MonoBehaviour
{
    /// <summary>
    /// A reference to the level controller
    /// </summary>
    protected LevelController m_LevelController;

    /// <summary>
    /// True when the laser is in a hackable state
    /// </summary>
    public bool IsHackable { get; set; }

    /// <summary>
    /// True when the system is powered on
    /// </summary>
    public bool IsPoweredONn { get; set; }

    /// <summary>
    /// Sets references
    /// </summary>
    void Awake()
    {
        OnAwake();
    }

    /// <summary>
    /// Allows for child classes to override Awake()
    /// </summary>
    protected virtual void OnAwake()
    {
        m_LevelController = FindObjectOfType<LevelController>();
        if (m_LevelController == null) {
            Debug.LogErrorFormat("SecuritySystem Error: Missing Component! " +
                "LevelController: {0}",
                m_LevelController
            );
        }
    }

    /// <summary>
    /// Initialization 
    /// </summary>
    void Start()
    {
        OnStart();
    }

    /// <summary>
    /// Allow child classes to override the Start()
    /// </summary>
    protected virtual void OnStart() { }

    /// <summary>
    /// Sets the power state to either ON or OFF
    /// </summary>
    /// <param name="powerOn"></param>
    public virtual void PowerHack(bool powerOn)
    {
        IsPoweredONn = powerOn;
    }

    /// <summary>
    /// Toggles the current state
    /// </summary>
    public virtual void ToggleHack() { }

    /// <summary>
    /// Triggers a rotation change
    /// </summary>
    public virtual void RotationHack() { }

    /// <summary>
    /// Triggers the system to retrieve information
    /// </summary>
    public virtual void IntelHack() { }
}
