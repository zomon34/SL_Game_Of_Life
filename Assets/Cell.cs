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
        spriteRenderer.color = color;
    }
}
