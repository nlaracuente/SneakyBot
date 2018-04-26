using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A security guard patrols on a given route until it spots the player triggering a game over
/// Patrol can loop, meaning the guard start and endpoints are the same
/// or the route can be single segment with the guard returning back where it came from
/// </summary>
[RequireComponent(typeof(NavMeshAgent),typeof(LineRenderer))]
public class SecurityGuard : SecuritySystem
{
    /// <summary>
    /// True when the last and first point are the same
    /// </summary>
    bool m_resetRoute = false;

    /// <summary>
    /// How long to wait before moving to the next point
    /// </summary>
    [SerializeField, Tooltip("How long to wait at each point")]
    float m_patrolDelay = 1f;

    /// <summary>
    /// How close to the destination the agent needs to be before
    /// consider it arrived at the destination
    /// </summary>
    [SerializeField]
    float m_destinationProximity = .01f;

    /// <summary>
    /// The Field of View angle
    /// </summary>
    [SerializeField]
    float m_fovAngle = 45f;

    /// <summary>
    /// The distance of the Field of View
    /// </summary>
    [SerializeField]
    float m_fovDistance = 75f;
    
    /// <summary>
    /// A collection of all the points the guard will patrol to
    /// </summary>
    [SerializeField]
    List<NavPoint> m_navPoints;

    /// <summary>
    /// Audio info the when the player moves
    /// </summary>
    [SerializeField]
    AudioClipInfo m_moveAudioInfo;

    /// <summary>
    /// A reference to the soundclip that controls the audio for the player
    /// </summary>
    SoundClip m_soundClip;

    /// <summary>
    /// Queues up the navigation point to put them in the order needed
    /// for the guard to patrol accordigly
    /// </summary>
    Queue<NavPoint> m_pointQueue = new Queue<NavPoint>();

    /// <summary>
    /// A reference to the navmesh agent used to make the guard patrol
    /// </summary>
    NavMeshAgent m_navAgent;

    /// <summary>
    /// Line renderer to show the enemy "shooting" at the player
    /// </summary>
    LineRenderer m_attackRenderer;

    /// <summary>
    /// A reference to the player controller
    /// </summary>
    PlayerController m_player;

    [SerializeField]
    Light m_fovLigth;

    /// <summary>
    /// Sets references
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_player = FindObjectOfType<PlayerController>();
        m_soundClip = GetComponent<SoundClip>();
        m_soundClip.Info = m_moveAudioInfo;
    }

    /// <summary>
    /// Initializes
    /// </summary>
    protected override void OnStart()
    {
        base.OnStart();

        if (m_navPoints.Count < 1) {
            return;
        }

        m_pointQueue = new Queue<NavPoint>(m_navPoints);
        m_resetRoute = m_navPoints.First() == m_navPoints.Last();
        StartCoroutine(PatrolRoutine());
    }

    /// <summary>
    /// Handles the patrol routine
    /// Guard will continue to patrol so long as it is not game over
    /// </summary>
    /// <returns></returns>
    IEnumerator PatrolRoutine()
    {
        while (!m_LevelController.IsGameOver) {
            yield return new WaitForSeconds(m_patrolDelay);

            Vector3 destination = GetNextDestination();
            m_navAgent.SetDestination(destination);
            m_navAgent.isStopped = false;

            // Wait for the agent to calculate the path
            while (m_navAgent.pathPending) {
                ScanForPlayer();
                yield return null;
            }

            m_soundClip.Play();
            // Wait for the agent to arrive
            while (m_navAgent.remainingDistance > m_destinationProximity) {
                ScanForPlayer();
                yield return null;
            }

            m_navAgent.isStopped = true;
            m_soundClip.Stop();
        }
    }

    /// <summary>
    /// Dequeues the next available navigation points
    /// Recreate the queue when the last point is dequeued
    /// </summary>
    /// <returns></returns>
    Vector3 GetNextDestination()
    {
        NavPoint point = m_pointQueue.Dequeue();
        if (point == null) {
            return transform.position;
        }

        Vector3 destination = point.Position;

        // Time to recreate the queue
        if (m_pointQueue.Count < 1) {
            // Reverse the order so that the guard returns the way it came
            if (!m_resetRoute) {
                m_navPoints.Reverse();
            }

            m_pointQueue = new Queue<NavPoint>(m_navPoints);
        }

        return destination;
    }

    /// <summary>
    /// Checks if the player is within the field of view of the guard
    /// Field of View is determined by angle and distance and ensure 
    /// there's no object between the enemy and the player
    /// </summary>
    void ScanForPlayer()
    {
        // Player's center is low to the ground so we raise the destination to be
        // at the same level as the guard
        Vector3 direction = m_player.Center - transform.position;

        float angle = Vector3.Angle(transform.forward, direction);

        // Player is not within FOV
        if (angle > m_fovAngle) {
            return;
        }

        float distance = Vector3.Distance(m_player.transform.position, transform.position);
        // The player is too far
        if (distance > m_fovDistance) {
            return;
        }

        Ray ray = new Ray(transform.position, direction);
        Debug.DrawRay(transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            // Player spotted!
            if (hit.collider.CompareTag("Player")) {
                AudioManager.instance.PlaySound(AudioName.DeathByGuard);
                m_navAgent.isStopped = true;
                m_fovLigth.color = Color.red;
                StopAllCoroutines();
                m_LevelController.TriggerGameOver();
            }
        }
    }
}
