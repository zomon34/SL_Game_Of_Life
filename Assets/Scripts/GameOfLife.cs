using UnityEngine;

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

    // This is a very large file, need to find a solution to that.

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
                isPaused = false;
                Application.targetFrameRate = lastFrameRate;
            }
            else
            {
                isPaused = true;
                lastFrameRate = Application.targetFrameRate;
                Application.targetFrameRate = 60;
            }
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

    // CheckIfStable only returns true if the remaining pieces are stationary.
    // If there are any oscillators then it returns false.
    
    // TODO: Find a way to make it work with oscillators.
    // Possibly with a while loop that simulates generations until it finds a match with the current one.
    // That loop would have to have a limit. How big of a limit? Should it be run every frame?
    // While loop idea has been attempted. Bad idea, Unity crashed. This might not be possible?
    bool CheckIfStable(Cell[,] inputCells)
    {
        Cell[,] nextGeneration = SimulateNextGeneration(inputCells);

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                if (cells[x, y].alive != nextGeneration[x, y].alive)
                {
                    return false;
                }
            }
        }
        return true;
    }

    Cell[,] SimulateNextGeneration(Cell[,] currentCells)
    {
        int rows = currentCells.GetLength(0);
        int columns = currentCells.GetLength(1);

        Cell[,] nextGeneration = new Cell[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (currentCells[i, j] != null)
                {
                    nextGeneration[i, j] = (Cell)currentCells[i, j].Clone();
                }
            }
        }

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                int aliveNeighbours = GetAliveNeighbours(x, y);
                if (nextGeneration[x, y].alive)
                {
                    if (aliveNeighbours == 2 || aliveNeighbours == 3)
                    {
                        nextGeneration[x, y].aliveNextStep = true;
                    }
                    else
                    {
                        nextGeneration[x, y].aliveNextStep = false;
                    }
                }
                else
                {
                    nextGeneration[x, y].aliveNextStep = aliveNeighbours == 3;
                }
            }
        }
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                nextGeneration[x, y].alive = nextGeneration[x, y].aliveNextStep;
            }
        }
        return nextGeneration;
    }
}
