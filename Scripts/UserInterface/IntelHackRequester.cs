using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Requests that intel information be hacked
/// </summary>
public class IntelHackRequester : AbstractRequester
{
    /// <summary>
    /// A list of all the intel terminals in the level
    /// </summary>
    List<IntelTerminal> m_terminals;

    /// <summary>
    /// Finds all the intel terminals
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();
        m_terminals = new List<IntelTerminal>(FindObjectsOfType<IntelTerminal>());
    }

    /// <summary>
    /// Sends the request
    /// </summary>
    public override void Hack()
    {
        m_LevelController.IntelHackRequest();
    }

    /// <summary>
    /// Only activate if at least one intel terminal is active
    /// This is because intel and hacking buttons are called
    /// simultaniously to be enabled
    /// </summary>
    public override void Enable()
    {
        IntelTerminal terminal = m_terminals.Find(t => t.IsInteractible);

        if (terminal != null) {
            base.Enable();
        }
    }
}
