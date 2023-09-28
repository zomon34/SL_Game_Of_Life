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
        if (!alive)
        {
            color.a = 0f;
        }
        else
        {
            color.a = 1f;
        }
        spriteRenderer.color = color;
    }
}
