using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Security Lasers can be turned off for a prior of time
/// A game over is triggered when the player steps into an active laser
/// </summary>
[RequireComponent(typeof(Collider))]
public class SecurityLaser : SecuritySystem
{
    /// <summary>
    /// A reference to the laser game object that can be turned on/off
    /// </summary>
    [SerializeField]
    GameObject m_laserGO;

    /// <summary>
    /// How long to wait before allowing this object to be hackable again
    /// this is not a cool down timer but rather a wait after the power hack
    /// </summary>
    [SerializeField]
    float m_powerHackDelay = 1f;

    /// <summary>
    /// A refrence ot the collider component
    /// </summary>
    Collider m_collider;

    /// <summary>
    /// Sets references
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();

        m_collider = GetComponent<Collider>();
        if (m_laserGO == null) {
            Debug.LogErrorFormat("SecurityLaser Error: Missing Component! " +
                "LaserGO: {0}",
                m_laserGO
            );
        }
    }

    /// <summary>
    /// Allow this system to be hackable from the start
    /// </summary>
    void Start()
    {
        IsHackable = true;    
    }

    /// <summary>
    /// Triggers a game over if the player comes in contact with the lasers
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            m_LevelController.TriggerGameOver();
        }
    }

    /// <summary>
    /// Ignore request
    /// </summary>
    public override void RotationHack() { }

    /// <summary>
    /// Ignore request
    /// </summary>
    public override void ToggleHack() { }

    /// <summary>
    /// Triggers the lasers to shutdown for the 
    /// </summary>
    public override void PowerHack(bool powerOn)
    {
        if (!IsHackable) {
            return;
        }

        StartCoroutine(PowerRoutine(powerOn));
    }

    /// <summary>
    /// Prevents object from being hacked while it is powering on/off
    /// There's a delay before this routine can be re-trigerred
    /// This delay is not a cool down phase but rather a way to prevent
    /// this to be triggered too quickly
    /// </summary>
    /// <param name="powerOn"></param>
    /// <returns></returns>
    IEnumerator PowerRoutine(bool powerOn)
    {
        IsHackable = false;
        m_collider.enabled = powerOn;
        m_laserGO.SetActive(powerOn);
        yield return new WaitForSeconds(m_powerHackDelay);
        IsHackable = true;
    }
}
