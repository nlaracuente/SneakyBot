using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles playing the different player animations
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    /// <summary>
    /// A reference to the animator 
    /// </summary>
    Animator m_animator;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Move()
    {
        m_animator.SetFloat("Speed", 1);
    }

    public void Idle()
    {
        m_animator.SetFloat("Speed", 0);
    }

    public void Hack()
    {
        m_animator.SetTrigger("Hack");
    }

    public void AccessTerminal()
    {
        m_animator.SetTrigger("Terminal");
    }

    public void Win()
    {
        m_animator.SetTrigger("Win");
    }

    public void Death()
    {
        m_animator.SetTrigger("Death");
    }
}
