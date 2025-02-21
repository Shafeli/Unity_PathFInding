
using UnityEngine;

public class YSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // Set sorting order based on Y position
        spriteRenderer.sortingOrder = (Mathf.RoundToInt(transform.position.y * 100f) * -1);
    }
}
