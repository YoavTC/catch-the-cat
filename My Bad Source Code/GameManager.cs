using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Intance;

    private void Awake()
    {
        Intance = this;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetButton("Restart"))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name.Equals("TUTORIAL"))
        {
            SceneManager.LoadScene("MENU");
        }

        if (Input.GetKeyDown(KeyCode.I) && SceneManager.GetActiveScene().name.Equals("MENU"))
        {
            SceneManager.LoadScene("TUTORIAL");
        }
    }


    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void LoadNextLevel()
    {
        Debug.Log("LEVEL COMPLETED, LOADING NEXT LEVEL!");
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene("LVL1");
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                SceneManager.LoadScene("MENU");
            } else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
