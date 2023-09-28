using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool alive;
    public bool aliveNextStep;
    SpriteRenderer spriteRenderer;

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = alive;
    }

    public void Select() //Debug purposes that's used to see what cell is being tested with
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
    }
}
