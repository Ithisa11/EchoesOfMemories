using UnityEngine;

public class RevealKeyWhenDiaryChecked : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D interactCollider;

    private void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (interactCollider == null) interactCollider = GetComponent<Collider2D>();

        SetVisible(GameProgress.diaryChecked);
    }

    private void Update()
    {
        if (GameProgress.diaryChecked)
            SetVisible(true);
    }

    private void SetVisible(bool visible)
    {
        if (spriteRenderer != null) spriteRenderer.enabled = visible;
        if (interactCollider != null) interactCollider.enabled = visible;
        enabled = !visible; 
    }
}