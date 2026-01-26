using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterM : MonoBehaviour
{
    public float speed = 8f;
    public float animDeadZone = 0.05f; 

    float move;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        if (move != 0) sr.flipX = move < 0;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(move * speed, 0f);
        float v = Mathf.Abs(rb.velocity.x);
        if (v < animDeadZone) v = 0f;
        animator.SetFloat("xVelocity", v);
    }
}
