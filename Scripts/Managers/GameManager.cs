using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// For Saving/Loading the game
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Game manager is responsible for transitioning from scene to scene
/// Keeping track of global player progress
/// Saving and Loading the Game
/// And can function as a central point for system communicating with each other
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// A reference to the only instance of the GameManager
    /// </summary>
    public static GameManager instance = null;

    /// <summary>
    /// The name/file location for the save data
    /// </summary>
    string m_saveFile;

    /// <summary>
    /// Data what is saved/loaded
    /// </summary>
    SavedData m_savedData;

    /// <summary>
    /// Where to spawn the player
    /// </summary>
    public Vector3 PlayerSpawnPoint { get; set; }

    /// <summary>
    /// Keeps track of which terminal was hacked
    /// </summary>
    List<string> m_hackedTerminals = new List<string>();
    public List<string> HackedTerminals { get { return m_hackedTerminals; } set { m_hackedTerminals = value; } }

    /// <summary>
    /// Creates the GameManager instance
    /// </summary>
    void Awake()
    {
        Setup();   
    }

    /// <summary>
    /// Sets this class as a singleton and the file to use to store the game data
    /// </summary>
    public void Setup()
    {
        if (instance == null) {
            instance = this;

            // The name is completely up to you and it does not have to be .gd
            // we used .gd because that's what we saw online ;)
            m_saveFile = Application.persistentDataPath + "/FILE_NAME.gd";
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Goes straight to the level
    /// </summary>
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Title") {
            StartCoroutine(LoadGameRoutine());
        }
        
    }

    IEnumerator LoadGameRoutine()
    {
        yield return new WaitForSeconds(.25f);
        GoToLevel();
    }

    /// <summary>
    /// Loads and unpacks saved data
    /// </summary>
    public void LoadGame()
    {
        // This will fail if the file does not exist so lets not event try
        if (!this.SaveFileExists()) {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(m_saveFile, FileMode.Open);

        // After this is load you can access its value 
        // update any objects that depends on it
        m_savedData = (SavedData)bf.Deserialize(file);

        // For example here we update the master sfx and music volume to what was saved
        AudioManager.instance.MusicVolume = m_savedData.musicVolume;
        AudioManager.instance.SFXVolume = m_savedData.sfxVolume;

        file.Close();
    }

    /// <summary>
    /// Saves the player's progress
    /// </summary>
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(m_saveFile);

        // Here's where you populare the save data with the content you want to save
        // Example:
        m_savedData.musicVolume = AudioManager.instance.MusicVolume;
        m_savedData.sfxVolume = AudioManager.instance.SFXVolume;

        bf.Serialize(file, m_savedData);
        file.Close();
    }

    /// <summary>
    /// Returns TRUE if the save file exists
    /// </summary>
    public bool SaveFileExists()
    {
        return File.Exists(m_saveFile);
    }
    
    /// <summary>
    /// Example call to load another scene by name
    /// </summary>
    public void GoToLevel()
    {
        SceneManager.LoadScene("Level");
    }

    /// <summary>
    /// Reloads the current level
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Closes the app
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
