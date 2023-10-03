using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    GameOfLife gameOfLife;
    Cell[,] cells;
    int numberOfRows, numberOfColumns;
    // Start is called before the first frame update
    void Start()
    {
        gameOfLife = FindObjectOfType<GameOfLife>();
        cells = gameOfLife.cells;
        numberOfRows = gameOfLife.numberOfRows;
        numberOfColumns = gameOfLife.numberOfColumns;

    }

    public void SpawnAround(Cell cell)
    {
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                if (cells[x, y] == cell)
                {
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
            }
        }
    }
}
