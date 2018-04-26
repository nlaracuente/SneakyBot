using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Power Hacks turns on/off the system
/// </summary>
public class PowerHackRequester : AbstractRequester
{
    /// <summary>
    /// Whether to turn the power on or off
    /// </summary>
    bool m_isPowerOn = false;

    /// <summary>
    /// Sent the request
    /// </summary>
    public override void Hack()
    {
        m_LevelController.PowerHackRequest(m_isPowerOn);
        m_isPowerOn = !m_isPowerOn;
    }
}

