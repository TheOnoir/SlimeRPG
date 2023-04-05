using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public void RestartLevels()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
