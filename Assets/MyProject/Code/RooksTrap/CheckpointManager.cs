using System.Collections;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [Header("Player Reference (يمكن ملؤها يدوياً أو يستخدم البحث التلقائي بالـ Tag 'Player')")]
    public Transform player;          // سحب الـ Player Transform في الـ Inspector أو يتم العثور عليه تلقائياً
    public Rigidbody playerRb;

    [Header("Blink Settings")]
    public int blinkCount = 3;
    public float blinkInterval = 0.15f;

    public Transform currentCheckpoint;
    public Transform startCheckpoint;

    private void Awake()
    {
        // singleton بسيط
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // محاولة تلقائية لإيجاد اللاعب إذا لم يكن مرتبطاً في الـ Inspector
        AutoAssignPlayerIfMissing();
        if (currentCheckpoint == null) player.position = startCheckpoint.position;
        if (currentCheckpoint != null)
            player.position = currentCheckpoint.position;
    }

    private void AutoAssignPlayerIfMissing()
    {
        if (player == null)
        {
            var go = GameObject.FindWithTag("Player");
            if (go != null)
            {
                player = go.transform;
                Debug.Log("[CheckpointManager] Auto-assigned player Transform from Tag 'Player'.");
            }
            else
            {
                Debug.LogWarning("[CheckpointManager] player is not assigned and no GameObject with tag 'Player' found in scene.");
            }
        }

        if (playerRb == null && player != null)
        {
            var rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                playerRb = rb;
                Debug.Log("[CheckpointManager] Auto-assigned player Rigidbody.");
            }
            else
            {
                Debug.LogWarning("[CheckpointManager] playerRb not assigned and player GameObject has no Rigidbody.");
            }
        }
    }

    /// <summary>
    /// يمكنك استدعاء هذه الدالة برمجياً بعد إنشاء اللاعب ديناميكياً
    /// </summary>
    public void SetPlayer(Transform playerTransform, Rigidbody rb = null)
    {
        player = playerTransform;
        playerRb = rb ?? playerTransform.GetComponent<Rigidbody>();
        Debug.Log("[CheckpointManager] Player manually set via SetPlayer().");
    }

    // يتم استدعاؤها من Checkpoint عندما يصل اللاعب إليه
    public void SetCheckpoint(Transform checkpointTransform)
    {
        currentCheckpoint = checkpointTransform;
        Debug.Log("[CheckpointManager] Checkpoint Saved: " + (checkpointTransform ? checkpointTransform.name : "null"));
    }

    // استدعِ هذه الدالة عندما تريد إعادة اللاعب
    public void Respawn()
    {
        if (currentCheckpoint == null)
        {
            Debug.LogWarning("[CheckpointManager] Respawn called but no checkpoint set.");
            return;
        }

        if (player == null)
        {
            // محاولة اعادة الاكتشاف مرة أخرى قبل الرفض النهائي
            AutoAssignPlayerIfMissing();
            if (player == null)
            {
                Debug.LogError("[CheckpointManager] Respawn aborted: player reference is null. Assign player in Inspector or set tag 'Player'.");
                return;
            }
        }

        StartCoroutine(BlinkThenRespawn());
    }

    private IEnumerator BlinkThenRespawn()
    {
        // احصل على جميع SpriteRenderers تحت اللاعب (تحقق من عدم كون player null)
        SpriteRenderer[] renderers = player != null ? player.GetComponentsInChildren<SpriteRenderer>(true) : null;

        if (renderers == null || renderers.Length == 0)
        {
            Debug.LogWarning("[CheckpointManager] No SpriteRenderers found under player; Blink will be simulated by waiting.");
            // ننتظر مدة الوميض الكاملة بدلاً من التبديل
            float total = blinkCount * blinkInterval * 2f;
            yield return new WaitForSeconds(total);
        }
        else
        {
            for (int i = 0; i < blinkCount; i++)
            {
                foreach (var r in renderers) if (r != null) r.enabled = false;
                yield return new WaitForSeconds(blinkInterval);
                foreach (var r in renderers) if (r != null) r.enabled = true;
                yield return new WaitForSeconds(blinkInterval);
            }
        }

        // الآن نوقف حركة اللاعب وننقله بأمان
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;

            // نضمن النقل بالتنسيق مع فيزياء Rigidbody
            player.position =  new Vector3(currentCheckpoint.position.x, currentCheckpoint.position.y, player.position.z);
           
            playerRb.MovePosition(new Vector3(currentCheckpoint.position.x, currentCheckpoint.position.y, player.position.z));
        }
        else
        {
            // لا يوجد Rigidbody: ننقل الـ Transform مباشرة
            player.position = new Vector3(currentCheckpoint.position.x, currentCheckpoint.position.y, player.position.z);
        }

        Debug.Log("[CheckpointManager] Player respawned to checkpoint: " + currentCheckpoint.name);
    }
}