using ClearSky;
using System.Collections;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public Transform pointA;
    public Animator anim;

    [Header("Scripts:")]
    public GrappleRope grappleRope;
    [Header("Layer Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Main Camera")]
    public Camera m_camera;

    [Header("Transform Refrences:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 80)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = true;
    [SerializeField] private float maxDistance = 4;

    [Header("Launching")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType Launch_Type = LaunchType.Transform_Launch;
    [Range(0, 5)] [SerializeField] private float launchSpeed = 5;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequency = 3;


    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch,
    }

    [Header("Component Refrences:")]
    public SpringJoint2D m_springJoint2D;
    public LineRenderer grappleLineRenderer;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 DistanceVector;
    Vector2 Mouse_FirePoint_DistanceVector;

    public Rigidbody2D ballRigidbody;


    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        ballRigidbody.gravityScale = 1;
    }

    private void Update()
    {
        Mouse_FirePoint_DistanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(WaitForSec());
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
                DetectGrappleLineCollision();
            }
            else
            {
                RotateGun(m_camera.ScreenToWorldPoint(Input.mousePosition), false);
            }

            if (launchToPoint && grappleRope.isGrappling)
            {
                if (Launch_Type == LaunchType.Transform_Launch)
                {
                    gunHolder.position = Vector3.Lerp(gunHolder.position, grapplePoint, Time.deltaTime * launchSpeed);
                }
            }

        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Debug.Log("RELEASE");
            anim.SetBool("isGrap", false);
            anim.SetBool("isJump", true);

            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            ballRigidbody.gravityScale = 1;
        }
        else
        {
            anim.SetBool("isGrap", false);
            RotateGun(m_camera.ScreenToWorldPoint(Input.mousePosition), true);

            if (grappleRope.enabled == false && SimplePlayerController.grounded == true && anim.GetBool("isJump") == true)
            {
                Debug.Log("OW YEAH");
                anim.SetBool("isJump", false);
            }
        }
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(0.1f);
        SetGrapplePoint();
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            Quaternion startRotation = gunPivot.rotation;
            gunPivot.rotation = Quaternion.Lerp(startRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    void SetGrapplePoint()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, Mouse_FirePoint_DistanceVector.normalized, maxDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.transform.CompareTag("grappable"))
            {
                // Grapple to the first object with the "grappable" tag
                grapplePoint = hit.point;
                DistanceVector = grapplePoint - (Vector2)gunPivot.position;
                grappleRope.enabled = true;
                return; // Exit the loop once the correct object is found
            }
        }

        // If no object with "grappable" tag is found, no grapple will occur
    }

    void DetectGrappleLineCollision()
    {
        if (grappleLineRenderer.positionCount < 2) return; // Ensure the line renderer has at least two points

        for (int i = 0; i < grappleLineRenderer.positionCount - 1; i++)
        {
            Vector3 start = grappleLineRenderer.GetPosition(i);
            Vector3 end = grappleLineRenderer.GetPosition(i + 1);

            // Check for collisions between each segment of the LineRenderer
            RaycastHit2D hit = Physics2D.Linecast(start, end);

            if (hit.collider != null)
            {
                Debug.Log("Line hit: " + hit.collider.name);

                // Handle collision with the object
                if (hit.collider.CompareTag("grappable"))
                {
                    pointA.transform.position = new Vector3(17.76f, 10.51f, -6.058676f);
                    Debug.Log("Hit a grappable object!");
                    grapplePoint = hit.point;
                    break; // Exit once a valid hit is found
                }
            }
        }
    }

    public void Grapple()
    {

        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequency;
        }

        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }
            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }

        else
        {
            if (Launch_Type == LaunchType.Transform_Launch)
            {
                ballRigidbody.gravityScale = 0;
                ballRigidbody.velocity = Vector2.zero;
            }
            if (Launch_Type == LaunchType.Physics_Launch)
            {
                m_springJoint2D.connectedAnchor = grapplePoint;
                m_springJoint2D.distance = 0;
                m_springJoint2D.frequency = launchSpeed;
                m_springJoint2D.enabled = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }

}
