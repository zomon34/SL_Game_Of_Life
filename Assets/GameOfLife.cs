using System.Collections;
using System.Collections.Generic;
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
            
            for ( int x = 0; x < numberOfColumns; x++)
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
                cells[x, y].UpdateStatus();
            }
        }
    }
}
