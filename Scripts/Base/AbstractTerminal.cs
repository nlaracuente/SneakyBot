using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Terminals are stations the player can access to hack or gain access to parts of the level
/// A terminal can only be accessed when it is "active" by the player being close enough to
/// engage with it
/// </summary>
public abstract class AbstractTerminal : MonoBehaviour
{
    /// <summary>
    /// A reference to the level controller
    /// </summary>
    protected LevelController m_LevelController;

    /// <summary>
    /// A reference to the game object that holds the UI prompt of which key to press to engage
    /// </summary>
    [SerializeField]
    GameObject m_uiPromptGo;

    /// <summary>
    /// When the player access the terminal we updated the player spawn point
    /// to this terminal. This is holds the where to put the player at
    /// </summary>
    [SerializeField]
    Transform m_playerSpawnPoint;

    /// <summary>
    /// Player Controller
    /// </summary>
    protected PlayerController m_player;

    /// <summary>
    /// True when the player is close enough to interact with the terminal
    /// </summary>
    protected bool m_isInteractible = false;
    public virtual bool IsInteractible
    {
        get { return m_isInteractible; }
        set { m_isInteractible = value; }
    }

    /// <summary>
    /// True when the player has pressed the engage key while this terminal is interactive
    /// </summary>
    /// <returns></returns>
    protected bool m_isActive = false;

    /// <summary>
    /// Sets references
    /// </summary>
    void Awake()
    {
        OnAwake();
    }

    /// <summary>
    /// Allows for child classes to override Awake()
    /// </summary>
    protected virtual void OnAwake()
    {
        m_player = FindObjectOfType<PlayerController>();
        m_LevelController = FindObjectOfType<LevelController>();
        if (m_LevelController == null) {
            Debug.LogErrorFormat("AbstractTerminal Error: Missing Component! " +
                "LevelController: {0}",
                m_LevelController
            );
        }
    }

    /// <summary>
    /// Initialization 
    /// </summary>
    void Start()
    {
        OnStart();
    }

    /// <summary>
    /// Allow child classes to override the Start()
    /// </summary>
    protected virtual void OnStart() { }

    /// <summary>
    /// Logic Loop 
    /// </summary>
    void Update()
    {
        OnUpdate();
    }

    /// <summary>
    /// Allow child classes to override the Update()
    /// </summary>
    protected virtual void OnUpdate()
    {
        if (!m_isInteractible) {
            return;
        }

        // Player is enaging with the terminal
        if(!m_isActive && Input.GetKeyDown(KeyCode.E)) {
            StartCoroutine(PlayerEngagedRoutine());

        // Terminal releaed
        } else if (Input.GetKeyDown(KeyCode.E)) {
            TerminalDisengaged();
        }
    }

    /// <summary>
    /// Handles the routine for the player engaging with the terminal
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerEngagedRoutine()
    {
        // Update the player spawnpoint for respawning
        m_LevelController.PlayerSpawnPoint = m_playerSpawnPoint.position;
        yield return StartCoroutine(m_LevelController.TerminalEngagedRoutine(transform));
        OnActivate();
    }

    /// <summary>
    /// Turns the terminal off and restore access to player movement
    /// </summary>
    protected void TerminalDisengaged()
    {
        m_isActive = false;
        m_LevelController.TerminalDisengaged();
        OnDeacivate();
        MenuManager.instance.DisplayMessage("");
    }

    /// <summary>
    /// Triggers activation
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterLogic(other);
    }

    /// <summary>
    /// Allows children to override OnTriggerEnter()
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnterLogic(Collider other)
    {
        if (other.CompareTag("Player")) {
            IsInteractible = true;
            MenuManager.instance.DisplayMessage("Press E to hack");
        }
    }

    /// <summary>
    /// Triggers deactivation
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        OnTriggerExitLogic(other);
    }

    /// <summary>
    /// Allows children to override OnTriggerExit()
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerExitLogic(Collider other)
    {
        if (other.CompareTag("Player")) {
            IsInteractible = false;
        }
    }

    /// <summary>
    /// Triggers the logic for when the terminal has been activated by the player
    /// </summary>
    protected abstract void OnActivate();

    /// <summary>
    /// Triggers the logic for when the player stops interacting with the terminal
    /// </summary>
    protected abstract void OnDeacivate();
}
