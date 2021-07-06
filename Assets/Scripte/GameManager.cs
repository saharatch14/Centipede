using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float restartDealy = 1f;

    public GameObject completeLevelUI;

    public GameObject enemy;

    Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

        void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.life <= 0)
        {
            Invoke("Restart", restartDealy);
        }
        else if(Input.GetKeyDown(KeyCode.Space) && enemy.activeSelf == false)
        {
            Invoke("Restart", restartDealy);
        }
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
    }

    public void EndGame()
    {
        Invoke("Restart", 0f);
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
