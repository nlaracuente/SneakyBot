using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Requests that the state of the system be toggled
/// </summary>
public class ToggleHackRequest : AbstractRequester
{
    /// <summary>
    /// Send the request
    /// </summary>
    public override void Hack()
    {
        m_LevelController.ToggleHackRequest();
    }
}

