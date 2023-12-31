using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    GameOfLife gameOfLife;
    Cell[,] cells;
    int numberOfRows, numberOfColumns;

    int selectedPieceIndex = -1;

    void Start()
    {
        gameOfLife = FindObjectOfType<GameOfLife>();
        cells = gameOfLife.cells;
        numberOfRows = gameOfLife.numberOfRows;
        numberOfColumns = gameOfLife.numberOfColumns;
    }

    public void ChangeSelectedPiece(int shapeIndex)
    {
        if (selectedPieceIndex == shapeIndex)
        {
            selectedPieceIndex = -1;
        }
        else
        {
            selectedPieceIndex = shapeIndex;
        }
    }
    public void SpawnPieceAt(Cell cell)
    {
        switch (selectedPieceIndex)
        {
            case -1:
                break;
            case 0:
                cell.ToggleCellStatus(); 
                break;
            case 1:
                SpawnAround(cell); 
                break;
            case 2:
                SpawnGlider(cell);
                break;
            case 3:
                SpawnToad(cell);
                break;
        }
    }

    public void SpawnAround(Cell cell)
    {
        (int x, int y) = FindCellCords(cell);

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
                    cells[b, a].MakeCellLive();
                }
            }
        }
    }
    (int x, int y) FindCellCords(Cell cell)
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
        (int x, int y) = FindCellCords(cell);
        
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
                    cells[b, a].MakeCellLive();
                }
            }
        }
    }
    bool CheckGliderConditions(int x, int y)
    {
        if (x == -1 && y != 0 || x == 0 && y == 1)
        {
            return false;
        }
        return true;
    }
    public void SpawnToad(Cell cell)
    {
        (int x, int y) = FindCellCords(cell);

        for (int i = 0; i < 3; i++)
        {
            if (x + i + 1 < numberOfColumns && y + 1 < numberOfRows)
            {
                cells[x + i, y].MakeCellLive();
                cells[x + i + 1, y + 1].MakeCellLive();
            }
        }
    }
}
