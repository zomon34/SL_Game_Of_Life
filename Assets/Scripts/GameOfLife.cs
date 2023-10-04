using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOfLife : MonoBehaviour
{
    public GameObject cellPrefab;
    public Cell[,] cells; //Maybe change to [][], because I want to try that.
    float cellSize = 0.25f;

    public int numberOfColumns, numberOfRows;
    int spawnChancePercentage = 10;

    float width;
    float height;

    bool isPaused = false;
    int lastFrameRate;

    List<int> savedAliveCounts = new List<int>();

    // This is a very large file, need to find a solution to that.

    // TODO for this week: add a menu where you can select what piece to place out.
    // PieceSpawner can hold the selected piece, and each Cell would call the same function.

    // TODO for last week: Go through each comment in all files.
    // Focus will be to make the code cleaner and remove code smells.
    // Also make repo public and update Readme

    void Awake()
    {
        width = Camera.main.orthographicSize * Camera.main.aspect;
        height = Camera.main.orthographicSize;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 4;

        numberOfColumns = (int)Mathf.Floor(width * 2 / cellSize);
        numberOfRows = (int)Mathf.Floor(height * 2 / cellSize);

        cells = new Cell[numberOfColumns, numberOfRows];

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                Vector2 newPos = new(x * cellSize - width, y * cellSize - height);

                var newCell = Instantiate(cellPrefab, newPos, Quaternion.identity);
                newCell.transform.localScale = Vector2.one * cellSize;

                cells[x, y] = newCell.GetComponent<Cell>();

                if (Random.Range(0, 100) < spawnChancePercentage)
                {
                    cells[x, y].alive = true;
                }

                cells[x, y].UpdateStatus();
            }
        }
    }

    void Update()
    {
        // TODO: Make it so targetFrameRate is changed when the game is paused too.
        // Possibly even replace targetFrameRate with a Time.TimeScale implementation.
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Application.targetFrameRate = Mathf.Max(1, Application.targetFrameRate - 1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Application.targetFrameRate++;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                Application.targetFrameRate = lastFrameRate;
            }
            else
            {
                lastFrameRate = Application.targetFrameRate;
                Application.targetFrameRate = 60;
            }
            isPaused = !isPaused;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int y = 0; y < numberOfRows; y++)
            {
                for (int x = 0; x < numberOfColumns; x++)
                {
                    cells[x, y].alive = false;
                    cells[x, y].aliveNextStep = false;
                }
            }
        }

        if (!isPaused)
        {
            if (Time.frameCount % 2 == 0)
            {
                for (int y = 0; y < numberOfRows; y++)
                {
                    for (int x = 0; x < numberOfColumns; x++)
                    {
                        cells[x, y].alive = cells[x, y].aliveNextStep;
                        cells[x, y].UpdateStatus();
                    }
                }
            }
            else
            {
                for (int y = 0; y < numberOfRows; y++)
                {
                    for (int x = 0; x < numberOfColumns; x++)
                    {
                        int aliveNeighbours = GetAliveNeighbours(x, y);
                        if (cells[x, y].alive)
                        {
                            if (aliveNeighbours == 2 || aliveNeighbours == 3)
                            {
                                cells[x, y].aliveNextStep = true;
                            }
                            else
                            {
                                cells[x, y].aliveNextStep = false;
                            }
                        }
                        else
                        {
                            cells[x, y].aliveNextStep = aliveNeighbours == 3;
                        }
                        cells[x, y].UpdateStatus();
                    }
                }
            }
        }
        savedAliveCounts.Add(CountAliveCells(cells));

        if (CheckIfStable())
        {
            // Finish
        }
    }

    int GetAliveNeighbours(int x, int y)
    {
        int aliveNeighbours = 0;

        int startY = Mathf.Max(0, y - 1);
        int maxY = Mathf.Min(numberOfRows, y + 2);

        int startX = Mathf.Max(0, x - 1);
        int maxX = Mathf.Min(numberOfColumns, x + 2);

        for (int a = startY; a < maxY; a++)
        {
            for (int b = startX; b < maxX; b++)
            {
                if (cells[b, a].alive && cells[b, a] != cells[x, y])
                {
                    aliveNeighbours++;
                }
            }
        }
        return aliveNeighbours;
    }
    int CountAliveCells(Cell[,] cellsToCount)
    {
        int aliveCells = 0;

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                if (cellsToCount[x, y].alive)
                {
                    aliveCells++;
                }
            }
        }
        return aliveCells;
    }
    bool CheckIfStable()
    {
        int generationsToCheck = 5;
        if (savedAliveCounts.Count > generationsToCheck)
        {
            int currentAliveCount = CountAliveCells(cells);

            for (int i = savedAliveCounts.Count - generationsToCheck - 1; i < savedAliveCounts.Count - 1; i++)
            {
                if (savedAliveCounts[i] != currentAliveCount)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}
