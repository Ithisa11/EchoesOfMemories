using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BedWakeMessage : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator animator;

    [Header("Movement Scripts To Disable")]
    [Tooltip("Drag CharacterM / movement scripts here")]
    [SerializeField] private List<MonoBehaviour> movementScripts = new List<MonoBehaviour>();

    [Header("UI Text")]
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private Text uiText;

    [TextArea]
    [SerializeField] private string message = "I'd better update my diary";

    private bool showing;

    // Cached state
    private Vector3 lockedPosition;
    private RigidbodyType2D oldBodyType;
    private RigidbodyConstraints2D oldConstraints;
    private bool animatorWasEnabled;

    public void Show()
    {
        if (showing) return;
        showing = true;

        // Resolve player automatically if not assigned
        if (playerRoot == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerRoot = p.transform;
        }

        if (playerRoot == null)
        {
            Debug.LogWarning("BedWakeMessage: Player not found.");
            showing = false;
            return;
        }

        if (playerRb == null)
            playerRb = playerRoot.GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = playerRoot.GetComponentInChildren<Animator>();

        // Set text
        if (tmpText != null) tmpText.text = message;
        if (uiText != null) uiText.text = message;

        gameObject.SetActive(true);

        // 🔒 Lock position
        lockedPosition = playerRoot.position;

        // 🔒 Freeze Rigidbody
        if (playerRb != null)
        {
            oldBodyType = playerRb.bodyType;
            oldConstraints = playerRb.constraints;

            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
            playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // 🔒 Disable movement scripts
        foreach (var m in movementScripts)
        {
            if (m != null && m.enabled)
                m.enabled = false;
        }

        // 🔒 Freeze Animator (EXPLICIT, CORRECT)
        if (animator != null)
        {
            animatorWasEnabled = animator.enabled;
            animator.enabled = false;
        }
    }
    private void Update()
    {
        if (!showing) return;

        // SPACE closes popup
        if (Input.GetKeyDown(KeyCode.Space))
            Hide();
    }

    private void LateUpdate()
    {
        if (!showing || playerRoot == null) return;

        // HARD LOCK position (nothing can move the player)
        playerRoot.position = lockedPosition;
    }

    public void Hide()
    {
        if (!showing) return;
        showing = false;

        // 🔓 Restore Rigidbody
        if (playerRb != null)
        {
            playerRb.constraints = oldConstraints;
            playerRb.bodyType = oldBodyType;
        }

        // 🔓 Re-enable movement scripts
        foreach (var m in movementScripts)
        {
            if (m != null)
                m.enabled = true;
        }

        // 🔓 Restore Animator
        if (animator != null)
            animator.enabled = animatorWasEnabled;

        gameObject.SetActive(false);
    }
}