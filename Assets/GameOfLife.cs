using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject cellPrefab;
    Cell[,] cells; //Maybe change to [][], because I want to try that.
    float cellSize = 0.25f;

    int numberOfColumns, numberOfRows;
    int spawnChancePercentage = 15;

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
                Vector2 newPos = new (x * cellSize - width, y * cellSize - height);

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

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                cells[x, y].alive = cells[x, y].aliveNextStep;
                cells[x, y].UpdateStatus();
            }
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
}
