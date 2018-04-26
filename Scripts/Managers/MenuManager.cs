using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple manager for opening/closing a canvas menu
/// Meaning, that by setting up an object within a UI canvas that contains a menu
/// this allows easy access to "disable/enable" it to appear that it is opening/closing
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// A reference to self
    /// </summary>
    public static MenuManager instance;

    /// <summary>
    /// A reference to the screen the player sees during a level compeltetion
    /// </summary>
    [SerializeField]
    GameObject m_victoryScreen;

    /// <summary>
    /// A reference to the screen the player sees during game over
    /// </summary>
    [SerializeField]
    GameObject m_gameOverScreen;

    /// <summary>
    /// A reference to the canvas image used for fading in/out
    /// </summary>
    [SerializeField]
    Image m_faderImage;

    /// How fast to fade the screen
    /// </summary>
    [SerializeField]
    float m_fadeSpeed = .3f;

    /// <summary>
    /// How close to the target the alpha needs to be when fading
    /// </summary>
    [SerializeField, Tooltip("Difference between current alpha to target to consider fading done")]
    float m_fadeTargetDiff = .001f;

    /// <summary>
    /// Alpha values for fading in/out
    /// </summary>
    float m_faderFullAlpha = 1f;

    /// <summary>
    /// A reference to the component where we display message to the player
    /// </summary>
    [SerializeField]
    Text m_messageText;

    /// <summary>
    /// Sets references
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Displays the victory screen
    /// </summary>
    public void ShowVictorySreen()
    {
        m_victoryScreen.SetActive(true);
    }

    /// <summary>
    /// Displays the game over screen
    /// </summary>
    public void ShowGameOverSreen()
    {
        m_gameOverScreen.SetActive(true);
    }

    /// <summary>
    /// Forces the shader image to be at 100% alpha
    /// </summary>
    public void SetFaderToFullAlpha()
    {
        Color faderColor = m_faderImage.color;
        faderColor.a = m_faderFullAlpha;
        m_faderImage.color = faderColor;
    }

    /// <summary>
    /// Triggers a change in screen fader's alpha to match the target given
    /// </summary>
    /// <param name="targetAlpha"></param>
    public IEnumerator FadeScreenRoutine(float targetAlpha)
    {
        Color faderColor = m_faderImage.color;

        while (Mathf.Abs(faderColor.a - targetAlpha) > m_fadeTargetDiff) {
            faderColor.a = Mathf.Lerp(
                faderColor.a,
                targetAlpha,
                m_fadeSpeed * Time.deltaTime
            );

            m_faderImage.color = faderColor;
            yield return new WaitForEndOfFrame();
        }

        faderColor.a = targetAlpha;
        m_faderImage.color = faderColor;
    }

    /// <summary>
    /// Displays the given message
    /// Remains there until other wise
    /// </summary>
    /// <param name="message"></param>
    public void DisplayMessage(string message)
    {
        m_messageText.text = message;
    }
    
    /// <summary>
    /// Displays the given message and removes it after the given time passes
    /// </summary>
    /// <param name="message"></param>
    /// <param name="time"></param>
    public void DisplayMessageWithTimer(string message, float time)
    {
        DisplayMessage(message);
        StartCoroutine(FadeMessageRoutine(time));
    }

    /// <summary>
    /// Waits for the given time and removes the message
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator FadeMessageRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        DisplayMessage("");
    }
}
