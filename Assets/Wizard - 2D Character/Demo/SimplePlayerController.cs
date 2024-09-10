using UnityEngine;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        [SerializeField] public static bool grounded = false; //default is FALSE
        public float movePower = 10f;
        public float jumpPower = 15f; //Set Gravity Scale in Rigidbody2D Component to 5

        private Rigidbody2D rb;
        private Animator anim;
        Vector3 movement;
        //private int direction = 1;
        private float direction = 0.1927739f;
        public static bool isJumping = false;
        private bool alive = true;

        public Camera mainCamera;

        private Vector3 lastClickPosition;  // Stores the position 


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            Restart();
            if (alive)
            {
                Hurt();
                Die();
                Attack();
                Jump();
                Run();
                FlipCharacterBasedOnClick();
            }
        }
        void FlipCharacterBasedOnClick()
        {
            // Check if the left mouse button is clicked
            if (Input.GetMouseButtonDown(0))
            {
                // Store the world position of the click
                lastClickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                // Flip the character based on the click position relative to the player
                if (lastClickPosition.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-0.1927739f, 0.1927739f, 0.1927739f);  // Face left
                }
                else if (lastClickPosition.x > transform.position.x)
                {
                    transform.localScale = new Vector3(0.1927739f, 0.1927739f, 0.1927739f);   // Face right
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            anim.SetBool("isGrap", false);
            anim.SetBool("isJump", false);
            grounded = true;
            Debug.Log("ENTER2D");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            grounded = false;
            Debug.Log("Exit2D");
        }


        void Run()
        {
            Vector3 moveVelocity = Vector3.zero;
            anim.SetBool("isRun", false);

            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                direction = -0.1927739f;
                moveVelocity = Vector3.left;

                transform.localScale = new Vector3(direction, 0.1927739f, 0.1927739f);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                direction = 0.1927739f;
                moveVelocity = Vector3.right;

                transform.localScale = new Vector3(direction, 0.1927739f, 0.1927739f);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }
        void Jump()
        {
            if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0)
            && !anim.GetBool("isJump")) // pagnaka false ung isJump papasok ung code dito
            {
                isJumping = true;
                anim.SetBool("isJump", true);
            }
            if (!isJumping)
            {
                return;
            }

            rb.velocity = Vector2.zero;

            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);

            isJumping = false;
        }
        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetTrigger("attack");
            }
        }
        void Hurt()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetTrigger("hurt");
                if (direction == 1)
                    rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
            }
        }
        void Die()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                anim.SetTrigger("die");
                alive = false;
            }
        }
        void Restart()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                anim.SetTrigger("idle");
                alive = true;
            }
        }
    }
}