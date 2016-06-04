using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public static GameSetup instance;
  
    public int NumberOfPlayers { get; set; }

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartGame(int players)
    {
        NumberOfPlayers = players;
        SceneManager.LoadScene("Main");
    }
}
