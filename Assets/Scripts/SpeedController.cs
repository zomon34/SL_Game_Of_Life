using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should be called something like InputManager, but I'm too afraid to change file names
public class SpeedController : MonoBehaviour
{
    GameOfLife gameOfLife;

    void Start()
    {
        gameOfLife = FindObjectOfType<GameOfLife>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            gameOfLife.targetSpeed = Mathf.Max(0.025f, Time.timeScale - 0.025f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            gameOfLife.targetSpeed += 0.025f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameOfLife.isPaused = !gameOfLife.isPaused;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            gameOfLife.ClearAllCells();
        }
    }
}
