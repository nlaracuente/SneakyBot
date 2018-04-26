using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exit Door Terminals unlock the door that leads to the exit of the level
/// </summary>
public class IntelTerminal : AbstractTerminal
{
    /// <summary>
    /// True when the exit door is opened
    /// </summary>
    bool m_isOpened = false;

    /// <summary>
    /// True once the player finishes hacking this station
    /// </summary>
    bool m_hacked = false;

    /// <summary>
    /// A way to reference this temrinal
    /// </summary>
    [SerializeField]
    string m_terminalID;
    public string TerminalID { get{ return m_terminalID; } }

    /// <summary>
    /// True when the terminal can be hacked
    /// </summary>
    public bool IsHackable { get { return !m_hacked && m_isActive; } }

    public override bool IsInteractible
    {
        get {
            return base.IsInteractible;
        }

        set {
            base.IsInteractible = value;
            if (value) {
                m_LevelController.EnableIntelHackButton();
            } else {
                TerminalDisengaged();
                m_LevelController.DisableHackButtons();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void TerminalHacked()
    {
        OnActivate();
    }

    /// <summary>
    /// Triggers an objective hack request
    /// Releases the player from interacting with this terminal
    /// </summary>
    protected override void OnActivate()
    {
        if (m_hacked) {
            return;
        }

        m_isOpened = true;
        m_hacked = true;

        StartCoroutine(TerminalHackedRoutine());
    }

    /// <summary>
    /// Set this to be hacked already
    /// </summary>
    public void MarkedAsHacked()
    {
        m_hacked = true;
    }

    protected override void OnTriggerEnterLogic(Collider other)
    {
        if (!m_hacked) {
            base.OnTriggerEnterLogic(other);
        }
    }

    /// <summary>
    /// Handles the routine for hacking the intel system
    /// </summary>
    /// <returns></returns>
    IEnumerator TerminalHackedRoutine()
    {
        MenuManager.instance.DisplayMessage("Hacking in progress");       
        m_player.PlayHackingAnimation();
        yield return new WaitForSeconds(2f);
        AudioManager.instance.PlaySound(AudioName.HackingIntel);
        yield return new WaitForSeconds(1.25f);
        m_LevelController.DisableHackButtons();
        InputManager.instance.DisableInput = false;
        IsInteractible = false;
        m_LevelController.HackedTerminals++;
    }

    /// <summary>
    /// Once opened the door never re-opens
    /// </summary>
    protected override void OnDeacivate() { }
}
