using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool alive;
    public bool aliveNextStep;
    SpriteRenderer spriteRenderer;

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;

        if (alive && aliveNextStep)
        {
            color = Color.white;
        }
        else if (!alive && !aliveNextStep)
        {
            color.r = 0.75f;
            color.g = 0.75f;
            color.b = 0.75f;
            color.a -= 0.4f;
        }
        else if (!alive && aliveNextStep)
        {
            color = Color.green;
            color.g = 0.5f;
        }
        else if (alive && !aliveNextStep)
        {
            color.g = 0.85f;
            color.b = 0.85f;
        }
        spriteRenderer.color = color;
    }

    private void OnMouseDown()
    {
        FindObjectOfType<PieceSpawner>().SpawnPieceAt(this);
    }

    public void ToggleCellStatus()
    {
        if (alive)
        {
            MakeCellDie();
        }
        else
        {
            MakeCellLive();
        }
    }

    public void MakeCellLive()
    {
        alive = true;
        aliveNextStep = true;
        spriteRenderer.color = Color.white;
    }

    public void MakeCellDie()
    {
        alive = false;
        aliveNextStep = false;
        Color color = Color.white;
        color.a = 0;
        spriteRenderer.color = color;
    }
}
