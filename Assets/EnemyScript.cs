using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform posA, posB;
    [SerializeField] public float moveSpeedObject = 9.5f;

    public Rigidbody2D body;

    public LineRenderer lineRenderer; // Reference to your LineRenderer component

    private Vector3 nextPosition;
    // Start is called before the first frame update
    void Start()
    {
        nextPosition = posB.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeedObject * Time.deltaTime);

        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == posA.position) ? posB.position : posA.position;
        }
        DetectLineCollision();
    }

    private void DetectLineCollision()
    {
        // Get the positions of the LineRenderer
        Vector3 lineStart = lineRenderer.GetPosition(0); // First point of the line
        Vector3 lineEnd = lineRenderer.GetPosition(1);   // Second point of the line

        // Cast a ray or line along the LineRenderer points
        RaycastHit2D hit = Physics2D.Linecast(lineStart, lineEnd);
        if (hit.collider != null)
        {
            Debug.Log("LineRenderer collided with: " + hit.collider.name);

            if (hit.collider.CompareTag("dart"))
            {
                Debug.Log("Hit dart!");
                hit.collider.gameObject.transform.parent = transform;
                body.interpolation = RigidbodyInterpolation2D.None;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SimplePlayerController.alive = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }
}
