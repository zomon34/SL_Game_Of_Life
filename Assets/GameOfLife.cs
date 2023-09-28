using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject cellPrefab;
    Cell[,] cells; //Maybe change to [][], because I want to try that.
    float cellSize = 0.25f;
    int numberOfColumns, numberOfRows;
    int spawnChancePercentage = 66;
    float width;
    float height;


    void Start()
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
                Vector2 newPos = new Vector2(x * cellSize - width, y * cellSize - height);

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
        //TODO: Calculate next generation

        //TODO: update buffer

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                if (x == 4 && y == 4) // Debug purposes
                {
                    cells[x, y].alive = true; // Just so that the testing cell is always visible
                    cells[x, y].Select();
                    CheckLifeConditions(x, y); // Called to run any prints inside the function
                }
                cells[x, y].UpdateStatus();
            }
        }
    }

    bool CheckLifeConditions(int x, int y)
    {
        int aliveNeighbours = 0;

        for (int a = y - 1; a < y + 1; a++)
        {
            for (int b = x - 1; b < x + 1; b++)
            {
                if (a != y && b != x)
                {
                    if (cells[b, a].alive)
                    {
                        aliveNeighbours++;
                    }
                }
            }
        }
        if (aliveNeighbours > 3 || aliveNeighbours < 2)
        {
            return false;
        }
        return true;
    }
}
