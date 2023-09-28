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
        if (!alive && !aliveNextStep)
        {
            color.r = 0.75f;
            color.g = 0.75f;
            color.b = 0.75f;
            color.a -= 0.33f;
        }
        else
        {
            color = Color.white;
            color.a = 1;
        }
        spriteRenderer.color = color;
    }
}
