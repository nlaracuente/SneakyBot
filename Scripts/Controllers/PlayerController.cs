using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of the player avatar
/// </summary>
[RequireComponent(typeof(PlayerMover), typeof(PlayerAnimator), typeof(SoundClip))]
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// A reference to the transform that represent the center the player
    /// This is used for casting ray from/to the center the player rather
    /// that at is center point which is on its feet
    /// </summary>
    [SerializeField]
    Transform m_center;
    public Vector3 Center { get { return m_center.position; } }

    /// <summary>
    /// Audio info the when the player moves
    /// </summary>
    [SerializeField]
    AudioClipInfo m_moveAudioInfo;

    /// <summary>
    /// A reference to the playemover component
    /// </summary>
    PlayerMover m_mover;

    /// <summary>
    /// A reference to the animator 
    /// </summary>
    [SerializeField]
    Animator m_animator;

    /// <summary>
    /// A reference to the soundclip that controls the audio for the player
    /// </summary>
    SoundClip m_soundClip;
    
    /// <summary>
    /// Sets references 
    /// </summary>
    void Awake()
    {
        m_mover = GetComponent<PlayerMover>();
        m_soundClip = GetComponent<SoundClip>();

        if (m_mover == null || m_soundClip == null) {
            Debug.LogErrorFormat("PlayerController Error: Missing Component! " +
                "PlayerMover: {0}, SoundClip: {1}",
                m_mover,
                m_soundClip
            );
        } else {
            m_soundClip.Info = m_moveAudioInfo;
        }
    }

    /// <summary>
    /// Handles player movement
    /// </summary>
    void FixedUpdate()
    {
        Vector3 moveVector = InputManager.instance.InputVector;

        // Ignores move request if the following is true
        if (moveVector == Vector3.zero) {
            m_soundClip.Stop();
            PlayIdleAnimation();
            return;
        }

        PlayMoveAnimation();
        m_soundClip.Play();
        m_mover.TriggerMovement(moveVector);
    }


    public void PlayMoveAnimation()
    {
        m_animator.SetFloat("Speed", 1);
    }

    public void PlayIdleAnimation()
    {
        m_animator.SetFloat("Speed", 0);
    }

    public void PlayHackingAnimation()
    {
        m_animator.SetTrigger("Hack");
    }

    public void PlayTerminalAccessAnimation()
    {
        m_animator.SetTrigger("Terminal");
    }

    public void PlayWinAnimation()
    {
        m_animator.SetTrigger("Win");
    }

    public void PlayDeathAnimation()
    {
        m_animator.SetTrigger("Death");
    }
}
