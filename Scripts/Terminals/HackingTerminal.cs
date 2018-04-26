using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A hacking terminal enables the ability to hack security systems
/// </summary>
public class HackingTerminal : AbstractTerminal
{
    /// <summary>
    /// Trigger the control panel to activate
    /// </summary>
    protected override void OnActivate()
    {
        if (m_isActive) {
            return;
        }
        
        m_isActive = true;
        m_player.PlayTerminalAccessAnimation();
        AudioManager.instance.PlaySound(AudioName.HackingSystems);
        MenuManager.instance.DisplayMessage("Press E to release");
        m_LevelController.EnableHackButtons();
    }

    /// <summary>
    /// Trigger the control panel to deactivate
    /// </summary>
    protected override void OnDeacivate()
    {
        m_isActive = false;
        m_LevelController.DisableHackButtons();
    }
}
