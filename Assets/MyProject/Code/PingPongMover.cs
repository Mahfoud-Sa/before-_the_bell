using UnityEngine;

public class PingPongMover : MonoBehaviour
{
    public float distance = 5f; 
    public float speed = 2f; 
    public bool reverseDirection = false; // خيار جديد للعكس
    
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float movement = Mathf.PingPong(Time.time * speed, distance);
        
        // إذا فعلت خيار العكس، سيقوم بضرب القيمة في -1
        float finalMovement = reverseDirection ? -movement : movement;

        transform.position = startPosition + new Vector3(finalMovement,0, 0);
    }
}