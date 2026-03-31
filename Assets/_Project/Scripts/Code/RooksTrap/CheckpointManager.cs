using System.Collections;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [Header("Player Reference")]
    public Transform player;
    public Rigidbody playerRb;

    [Header("Blink Settings")]
    public int blinkCount = 3;
    public float blinkInterval = 0.15f;

    [Header("Checkpoints")]
    public Transform currentCheckpoint;
    public Transform startCheckpoint;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        AutoAssignPlayerIfMissing();

        if (player == null)
        {
            Debug.LogError("[CheckpointManager] Player not found!");
            return;
        }

        // Choose spawn point
        Transform spawnPoint = currentCheckpoint != null ? currentCheckpoint : startCheckpoint;

        if (spawnPoint != null)
        {
            if (playerRb != null)
                playerRb.position = spawnPoint.position;
            else
                player.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("[CheckpointManager] No startCheckpoint assigned!");
        }

        SoundManager.Instance?.PlayBellSound();
    }

    private void AutoAssignPlayerIfMissing()
    {
        if (player == null)
        {
            var go = GameObject.FindWithTag("Player");
            if (go != null)
            {
                player = go.transform;
            }
        }

        if (playerRb == null && player != null)
        {
            playerRb = player.GetComponent<Rigidbody>();
        }
    }

    public void SetPlayer(Transform playerTransform, Rigidbody rb = null)
    {
        player = playerTransform;
        playerRb = rb ?? playerTransform.GetComponent<Rigidbody>();
    }

    // Called by Checkpoint
    public void SetCheckpoint(Transform checkpointTransform)
    {
        if (checkpointTransform == null) return;

        currentCheckpoint = checkpointTransform;

        Debug.Log("[CheckpointManager] Checkpoint Saved: " + checkpointTransform.name);

        // Optional save system
        PlayerPrefs.SetString("LastCheckpoint", checkpointTransform.name);
    }

    public void Respawn()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.currentStars == 2)
            {
                GameManager.Instance.LoseStar();
            }
        }

        if (player == null)
        {
            AutoAssignPlayerIfMissing();
            if (player == null)
            {
                Debug.LogError("[CheckpointManager] No player found!");
                return;
            }
        }

        // fallback to start
        if (currentCheckpoint == null)
        {
            if (startCheckpoint != null)
                currentCheckpoint = startCheckpoint;
            else
            {
                Debug.LogError("[CheckpointManager] No checkpoint available!");
                return;
            }
        }

        StartCoroutine(BlinkThenRespawn());
        SoundManager.Instance?.PlayBellSound();
    }

    private IEnumerator BlinkThenRespawn()
    {
        SpriteRenderer[] renderers = player.GetComponentsInChildren<SpriteRenderer>(true);

        if (renderers.Length > 0)
        {
            for (int i = 0; i < blinkCount; i++)
            {
                SetRenderers(renderers, false);
                yield return new WaitForSeconds(blinkInterval);

                SetRenderers(renderers, true);
                yield return new WaitForSeconds(blinkInterval);
            }
        }
        else
        {
            yield return new WaitForSeconds(blinkCount * blinkInterval * 2f);
        }

        // Move player safely
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.position = currentCheckpoint.position;
        }
        else
        {
            player.position = currentCheckpoint.position;
        }

        Debug.Log("[CheckpointManager] Respawned at: " + currentCheckpoint.name);
    }

    private void SetRenderers(SpriteRenderer[] renderers, bool state)
    {
        foreach (var r in renderers)
        {
            if (r != null)
                r.enabled = state;
        }
    }
}