using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A security door prevent players access into a room
/// A door can be toggled between opened or closed
/// </summary>
[RequireComponent(typeof(Animator))]
public class SecurityDoor : SecuritySystem
{
    /// <summary>
    /// True when the door is opened
    /// </summary>
    [SerializeField]
    bool m_isOpened = false;

    /// <summary>
    /// A reference to the animator component
    /// </summary>
    Animator m_animator;

    /// <summary>
    /// Sets references
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();

        m_animator = GetComponent<Animator>();
        if (m_animator == null) {
            Debug.LogErrorFormat("SecurityLaser Error: Missing Component! " +
                "Animator: {0}",
                m_animator
            );
        }
    }

    /// <summary>
    /// Allow this system to be hackable from the start
    /// Sets the door's animator state to match <see cref="m_isOpened"/>
    /// </summary>
    void Start()
    {
        IsHackable = true;
        PlayOpenCloseAnimation();
    }

    /// <summary>
    /// Updates the animator to set the is opened flag
    /// </summary>
    void PlayOpenCloseAnimation()
    {
        m_animator.SetBool("IsOpened", m_isOpened);
    }

    /// <summary>
    /// Triggers the door to open/close
    /// </summary>
    public override void ToggleHack()
    {
        if (!IsHackable) {
            return;
        }

        StartCoroutine(ToggleDoorRoutine());
    }

    /// <summary>
    /// Triggers the door to open or close
    /// Waits until the door's animation is done before it can be hacked again
    /// </summary>
    /// <returns></returns>
    IEnumerator ToggleDoorRoutine()
    {
        IsHackable = false;
        m_isOpened = !m_isOpened;
        string animation = m_isOpened ? "OpenedDoor" : "ClosedDoor";

        PlayOpenCloseAnimation();
        while (!IsPlayingAnimation(animation)) {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        IsHackable = true;
    }

    /// <summary>
    /// Triggered by the editor script to force the door to open/close
    /// based on its current state
    /// </summary>
    public void ShowCurrentState()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetBool("IsOpened", m_isOpened);
        m_animator.SetTrigger("Update");
    }

    /// <summary>
    /// Returns true when the given animation is playing
    /// </summary>
    /// <param name="animation"></param>
    /// <returns></returns>
    bool IsPlayingAnimation(string animation)
    {
        AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName(animation);
    }

    /// <summary>
    /// Ignore request
    /// </summary>
    /// <param name="powerOn"></param>
    public override void PowerHack(bool powerOn) { }

    /// <summary>
    /// Ignores the request
    /// </summary>
    public override void RotationHack() { }
}
