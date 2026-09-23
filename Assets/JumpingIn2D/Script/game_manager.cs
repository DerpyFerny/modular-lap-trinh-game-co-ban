using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game_manager : MonoBehaviour
{
    // Start is called before the first frame update
    public int currLvl = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void toNextLevel()
    {
        SceneManager.LoadScene("JumpingIn2D_" + currLvl);
    }
}
