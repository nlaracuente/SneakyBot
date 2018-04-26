using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level controller manages the gameplay session for the current level
/// </summary>
public class LevelController : MonoBehaviour
{
    [SerializeField]
    bool m_skipIntro = true;

    /// <summary>
    /// A container for all the security systems in the room
    /// </summary>
    List<SecuritySystem> m_systems;

    /// <summary>
    /// A container of all the panel buttons the player uses to trigger hacks
    /// </summary>
    List<AbstractRequester> m_panelButtons;

    /// <summary>
    /// A reference to the intel hack request button
    /// </summary>
    IntelHackRequester m_intelButton;

    /// <summary>
    /// A list of all the intel terminals in the level
    /// </summary>
    List<IntelTerminal> m_terminals;

    /// <summary>
    /// A reference to the camera controller
    /// </summary>
    CameraManager m_cameraController;

    /// <summary>
    /// A reference to the exit door object
    /// </summary>
    GoalTile m_goalTile;

    /// <summary>
    /// A reference to the all the terminals the player must reach to win the level
    /// </summary>
    List<IntelTerminal> m_interlTerminals;

    /// <summary>
    /// A reference to the player controller object
    /// </summary>
    PlayerController m_player;

    /// <summary>
    /// True when the game is over either because the player won or losts
    /// </summary>
    public bool IsGameOver { get; set; }

    /// <summary>
    /// Where to spawn the player upon re-spawn
    /// </summary>
    Vector3 m_playerSpawnPoint;
    public Vector3 PlayerSpawnPoint
    {
        set {
            m_playerSpawnPoint = value;
            GameManager.instance.PlayerSpawnPoint = value;
        }
    }

    /// <summary>
    /// Keeps tracked of all the terminals hacked
    /// </summary>
    int m_hackedTerminals = 0;
    public int HackedTerminals
    {
        get { return m_hackedTerminals; }
        set {
            m_hackedTerminals = value;
            string message = string.Format("{0} / {1} Research Terminals Hacked", value, m_totalTerminals);
            MenuManager.instance.DisplayMessageWithTimer(message, 3);

            if (m_hackedTerminals >= m_totalTerminals) {
                TriggerObjectiveCompleted();
            }
        }
    }
    
    /// <summary>
    /// Keeps tracked of all terminals hacked
    /// </summary>
    List<string> m_hackedTerminalIDs = new List<string>();
    public List<string> HackedTerminalIDs
    {
        get { return m_hackedTerminalIDs; }
        set {
            m_hackedTerminalIDs = value;
            GameManager.instance.HackedTerminals = value;
        }
    }

    /// <summary>
    /// How many terminals to hack
    /// </summary>
    int m_totalTerminals = 0;

    /// <summary>
    /// Sets references
    /// </summary>
    void Awake()
    {
        m_player = FindObjectOfType<PlayerController>();
        m_playerSpawnPoint = m_player.transform.position;
        m_goalTile = FindObjectOfType<GoalTile>();
        m_interlTerminals = new List<IntelTerminal>(FindObjectsOfType<IntelTerminal>());
        m_cameraController = FindObjectOfType<CameraManager>();
        m_systems = new List<SecuritySystem>(FindObjectsOfType<SecuritySystem>());
        m_panelButtons = new List<AbstractRequester>(FindObjectsOfType<AbstractRequester>());
        m_totalTerminals = m_interlTerminals.Count;
    }

    /// <summary>
    /// Initialize
    /// </summary>
    void Start()
    {
        // Find the intel button
        foreach (AbstractRequester button in m_panelButtons) {
            m_intelButton = button.GetComponent<IntelHackRequester>();
            if (m_intelButton != null) {
                break;
            }
        }

        StartCoroutine(LoadLevelRoutine());
    }

    /// <summary>
    /// Handles the routine for loading the level
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadLevelRoutine()
    {
        MenuManager.instance.SetFaderToFullAlpha();
        InputManager.instance.DisableInput = true;
        DisableHackButtons();
        AudioManager.instance.PlayMusic(AudioName.LevelMusic);

        string message = string.Format("Objective: Hack {0} Research Terminals", m_totalTerminals);

        // Find all hacked terminal
        if (GameManager.instance.HackedTerminals.Count > 0) {
            m_hackedTerminalIDs = GameManager.instance.HackedTerminals;

            m_hackedTerminalIDs.ForEach(id => {
                IntelTerminal terminal = m_interlTerminals.Find(t => t.TerminalID == id);
                if (terminal != null) {
                    terminal.MarkedAsHacked();
                }
            });

            m_hackedTerminals = m_hackedTerminalIDs.Count;
            message = string.Format("{0} Research Terminals Remaining", m_totalTerminals);

            // Move player to last spawn point
            m_playerSpawnPoint = GameManager.instance.PlayerSpawnPoint;
            m_player.transform.position = m_playerSpawnPoint;
        }

        // Reveal the screen
        yield return StartCoroutine(MenuManager.instance.FadeScreenRoutine(0f));

        if (!m_skipIntro) {
            //// Chill for a moment so that the scene can load
            //yield return new WaitForSeconds(1f);
            //yield return StartCoroutine(m_cameraController.ChangeTargetRoutine(m_interlTerminals.transform));
            //// Wait a second to show the terminal
            //yield return new WaitForSeconds(1f);
            //yield return StartCoroutine(m_cameraController.ChangeTargetRoutine(m_player.transform));
        }

        
        
        MenuManager.instance.DisplayMessageWithTimer(message, 5);
        InputManager.instance.DisableInput = false;
    }

    /// <summary>
    /// Handels the routine for when the player accesses the goal terminal
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowGoalTileRoutine()
    {
        InputManager.instance.DisableInput = true;
        yield return StartCoroutine(m_cameraController.ChangeTargetRoutine(m_goalTile.transform));
        m_goalTile.Activate = true;
        
        // Wait a bit to panic
        yield return new WaitForSeconds(2f);
        AudioManager.instance.PlayMusic(AudioName.EndMusic);
        AudioManager.instance.SetMusicVolume(1f);

        yield return StartCoroutine(m_cameraController.ChangeTargetRoutine(m_player.transform));
        MenuManager.instance.DisplayMessageWithTimer("Objective: Return To Charge Station", 5f);
        InputManager.instance.DisableInput = false;
    }

    /// <summary>
    /// Handels the routine for the player engaging with a terminal
    /// </summary>
    /// <returns></returns>
    public IEnumerator TerminalEngagedRoutine(Transform target)
    {
        InputManager.instance.DisableInput = true;

        // Vector3 targetRotation = transform.TransformDirection(transform.position - m_player.transform.position);

        // Face the target
        // m_player.transform.rotation = Quaternion.LookRotation(target.position);

        yield return null;
    }

    /// <summary>
    /// Re enables input
    /// </summary>
    public void TerminalDisengaged()
    {
        InputManager.instance.DisableInput = false;
    }

    /// <summary>
    /// Handels the routine for when the player finishes a level
    /// </summary>
    /// <returns></returns>
    IEnumerator LevelCompletedRoutine()
    {
        IsGameOver = true;
        DisableHackButtons();
        InputManager.instance.DisableInput = true;

        AudioManager.instance.PlaySound(AudioName.Victory);
        m_player.PlayWinAnimation();
        // Let the player soak it in
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(MenuManager.instance.FadeScreenRoutine(1f));

        // Thank them
        MenuManager.instance.DisplayMessageWithTimer("Mission Completed! Thanks for Playing!", 3);
        GameManager.instance.RestartLevel();
    }

    /// <summary>
    /// Handels the routine for when the player fails
    /// </summary>
    /// <returns></returns>
    IEnumerator GameOverRoutine()
    {
        IsGameOver = true;
        DisableHackButtons();
        InputManager.instance.DisableInput = true;
        MenuManager.instance.DisplayMessage("");

        AudioManager.instance.PlaySound(AudioName.PlayerDeathOne);
        m_player.PlayDeathAnimation();
        // Let the player soak it in
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(MenuManager.instance.FadeScreenRoutine(1f));
        GameManager.instance.RestartLevel();
    }

    /// <summary>
    /// Triggers a power hack request to all available security systems
    /// </summary>
    /// <param name="powerOn"></param>
    public void PowerHackRequest(bool powerOn)
    {
        m_systems.ForEach( system => {
            if (system != null && system.IsHackable) {
                system.PowerHack(powerOn);
            }
        });
    }

    /// <summary>
    ///  Triggers a toggle hack request to all available security systems
    /// </summary>
    public void ToggleHackRequest()
    {
        m_systems.ForEach(system => {
            if (system != null && system.IsHackable) {
                system.ToggleHack();
            }
        });
    }

    /// <summary>
    /// Triggers a rotation hack request to all available security systems
    /// </summary>
    public void RotationHackRequest()
    {
        m_systems.ForEach(system => {
            if (system != null && system.IsHackable) {
                system.RotationHack();
            }
        });
    }

    /// <summary>
    /// Triggers an intel hack request to all available security systems 
    /// </summary>
    public void IntelHackRequest()
    {
        m_interlTerminals.ForEach(terminal => {
            if (terminal != null && terminal.IsHackable) {
                terminal.TerminalHacked();
            }
        });
    }

    /// <summary>
    /// Enables the hacking buttons
    /// </summary>
    public void EnableHackButtons()
    {
        m_panelButtons.ForEach(button => {
            if(button != null) {
                button.Enable();
            }
        });
    }

    /// <summary>
    /// Disables the control panel
    /// </summary>
    public void DisableHackButtons()
    {
        m_panelButtons.ForEach(button => {
            if (button != null) {
                button.Disable();
            }
        });
    }

    /// <summary>
    /// Enables the hacking buttons
    /// </summary>
    public void EnableIntelHackButton()
    {
        m_intelButton.Enable();
    }


    /// <summary>
    /// Triggers the exit door to open
    /// </summary>
    public void TriggerObjectiveCompleted()
    {
        StartCoroutine(ShowGoalTileRoutine());
    }

    /// <summary>
    /// Triggers the game over sequence
    /// </summary>
    public void TriggerGameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    /// <summary>
    /// Triggers the level completed sequence (a.k.a win state)
    /// </summary>
    public void TriggerWin()
    {
        StartCoroutine(LevelCompletedRoutine());
    }
}
