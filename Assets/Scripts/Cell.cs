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
            color.a = 0.1f;
        }
        else if (alive && !aliveNextStep)
        {
            color.g = 0.85f;
            color.b = 0.85f;
        }
        spriteRenderer.color = color;
    }

    public Cell Clone()
    {
        Cell cell = gameObject.GetComponent<Cell>().MemberwiseClone() as Cell;
        cell.alive = alive;
        cell.aliveNextStep = aliveNextStep;
        cell.spriteRenderer = spriteRenderer;
        return cell;
    }

    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.W))
        {
            FindObjectOfType<PieceSpawner>().SpawnAround(this);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            FindObjectOfType<PieceSpawner>().SpawnGlider(this);
        }
        else
        {
            if (alive)
            {
                Die();
            }
            else
            {
                Live();
            }
        }
    }

    // Possible code smell: Live and Die aren't very descriptive names.
    // Why they're needed and only called sometimes isn't very clear.
    public void Live()
    {
        alive = true;
        aliveNextStep = true;
        spriteRenderer.color = Color.white;
    }

    public void Die()
    {
        alive = false;
        aliveNextStep = false;
        Color color = Color.white;
        color.a = 0;
        spriteRenderer.color = color;
    }
}
