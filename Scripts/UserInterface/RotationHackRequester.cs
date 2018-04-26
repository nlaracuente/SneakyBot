using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Requests that the system be rotated 
/// </summary>
public class RotationHackRequester : AbstractRequester
{
    /// <summary>
    /// Sends the request
    /// </summary>
    public override void Hack()
    {
        m_LevelController.RotationHackRequest();
    }
}

