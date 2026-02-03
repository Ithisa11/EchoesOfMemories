using UnityEngine;

public class MirrorFakeReflection2D : MonoBehaviour
{
    public enum ReflectionMode
    {
        TrueMirrorOppositeDirection,
        FollowPlayerSameDirection
    }

    public Transform player;
    public SpriteRenderer playerRenderer;
    public SpriteRenderer reflectionRenderer; 
    public Transform mirrorAxis;
    public ReflectionMode mode = ReflectionMode.FollowPlayerSameDirection;
    public bool hideWhenBehindMirror = true;
    public bool frontSideIsRight = true;
    public float maxReflectionDistance = 0f;
    public Vector2 reflectionOffset = Vector2.zero;
    public Vector2 reflectionScale = new Vector2(1f, 0.95f);
    public float sideSeparation = 0.3f;
    [Range(0f, 1f)] public float reflectionAlpha = 0.5f;
    public Color reflectionTint = Color.white;
    public bool fadeByDistance = true;
    public float fadeStartDistance = 0.5f;
    public float fadeEndDistance = 4f;

    void Reset()
    {
        mirrorAxis = transform;
    }

    void LateUpdate()
    {
        if (!player || !playerRenderer || !reflectionRenderer)
            return;

        Transform axis = mirrorAxis ? mirrorAxis : transform;

        // Copy Ash
        reflectionRenderer.sprite = playerRenderer.sprite;
        reflectionRenderer.flipX = !playerRenderer.flipX;
        reflectionRenderer.flipY = playerRenderer.flipY;
        reflectionRenderer.drawMode = playerRenderer.drawMode;
        reflectionRenderer.size = playerRenderer.size;

        // Position 
        Vector3 p = player.position;
        float x;

        if (mode == ReflectionMode.TrueMirrorOppositeDirection)
        {
            float dx = p.x - axis.position.x;
            x = axis.position.x - dx;
        }
        else
        {
            x = p.x;

            bool mirrorIsRightOfPlayer = axis.position.x > p.x;
            x += mirrorIsRightOfPlayer
                ? sideSeparation * 2f
                : -sideSeparation * 2f;
        }

        Vector3 targetPos = new Vector3(x, p.y, reflectionRenderer.transform.position.z);
        targetPos += (Vector3)reflectionOffset;
        reflectionRenderer.transform.position = targetPos;

        // Scale 
        Vector3 s = player.localScale;
        s.x *= -1f;
        s.x *= reflectionScale.x;
        s.y *= reflectionScale.y;
        reflectionRenderer.transform.localScale = s;

        // Visibility 
        bool visible = true;

        if (hideWhenBehindMirror)
        {
            bool playerOnRight = player.position.x > axis.position.x;
            visible = frontSideIsRight ? playerOnRight : !playerOnRight;
        }

        if (maxReflectionDistance > 0f)
        {
            float dist = Mathf.Abs(player.position.x - axis.position.x);
            if (dist > maxReflectionDistance)
                visible = false;
        }

        // fade
        float alpha = reflectionAlpha;

        if (fadeByDistance)
        {
            float dist = Mathf.Abs(player.position.x - axis.position.x);
            float start = Mathf.Max(0f, fadeStartDistance);
            float end = Mathf.Max(start + 0.0001f, fadeEndDistance);

            float t = Mathf.InverseLerp(end, start, dist);
            alpha *= Mathf.Clamp01(t);
        }

        Color c = reflectionTint;
        c.a *= alpha;
        reflectionRenderer.color = c;

        if (alpha <= 0.02f)
            visible = false;

        reflectionRenderer.enabled = visible;
    }
}
