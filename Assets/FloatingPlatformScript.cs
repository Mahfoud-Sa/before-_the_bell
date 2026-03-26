using UnityEngine;

public class FloatingPlatformScript : MonoBehaviour
{
    public float moveDistance = 5f;
    public float speed = 2f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool forward = true;

    private void Start()
    {
        startPos = transform.position;
        endPos = startPos + transform.right * moveDistance;
    }

    private void Update()
    {
        if (forward)
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) < 0.01f)
            forward = false;
        else if (Vector3.Distance(transform.position, startPos) < 0.01f)
            forward = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}