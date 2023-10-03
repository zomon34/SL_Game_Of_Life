using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    GameOfLife gameOfLife;
    Cell[,] cells;
    int numberOfRows, numberOfColumns;

    // Possible code smell: A lot of things are copied from GameOfLife.
    // But inheriting isn't the best solution since we don't need everything from GameOfLife.
    // One solution is to split up GameOfLife into two files, one for the Grid and one for the functions.
    // That has the added benifit of making the GameOfLife file shorter.
    // Another solution is to create a Grid class inside GameOfLife, but that would make the file even longer.

    void Start()
    {
        gameOfLife = FindObjectOfType<GameOfLife>();
        cells = gameOfLife.cells;
        numberOfRows = gameOfLife.numberOfRows;
        numberOfColumns = gameOfLife.numberOfColumns;
    }

    // TODO: A code smell! Very similar things are done in SpawnAround and in SpawnGlider.
    public void SpawnAround(Cell cell)
    {
        var cellCords = FindCellCords(cell);
        int x = cellCords.Item1;
        int y = cellCords.Item2;

        int startY = Mathf.Max(0, y - 1);
        int maxY = Mathf.Min(numberOfRows, y + 2);

        int startX = Mathf.Max(0, x - 1);
        int maxX = Mathf.Min(numberOfColumns, x + 2);

        for (int a = startY; a < maxY; a++)
        {
            for (int b = startX; b < maxX; b++)
            {
                if (cells[b, a] != cells[x, y])
                {
                    cells[b, a].Live();
                }
            }
        }

    }

    (int, int) FindCellCords(Cell cell)
    {
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                if (cells[x, y] == cell)
                {
                    return (x, y);
                }
            }
        }
        return (0, 0);
    }

    public void SpawnGlider(Cell cell)
    {
        var cellCords = FindCellCords(cell);
        int x = cellCords.Item1;
        int y = cellCords.Item2;

        int startY = Mathf.Max(0, y - 1);
        int maxY = Mathf.Min(numberOfRows, y + 2);

        int startX = Mathf.Max(0, x - 1);
        int maxX = Mathf.Min(numberOfColumns, x + 2);

        for (int a = startY; a < maxY; a++)
        {
            for (int b = startX; b < maxX; b++)
            {
                if (cells[b, a] != cells[x, y] && CheckGliderConditions(b - x, a - y))
                {
                    cells[b, a].Live();
                }
            }
        }

    }

    bool CheckGliderConditions(int x, int y)
    {
        if (x == -1 && y == -1)
        {
            return false;
        }
        else if (x == -1 && y == 1)
        {
            return false;
        }
        else if (x == 0 && y == 1)
        {
            return false;
        }
        return true;
    }
}
