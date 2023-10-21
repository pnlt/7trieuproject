using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    public static bool tap, swipeLeft, swipeRight;
    private Vector2 startTouch, swipeDelta;
    private bool isDraging = false;

    private int desiredLane = 1; //0: left 1:center 2:right
    public float laneDistance = 3;

    public float moveSpeed = 5f;

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float gravity = -12f;
    public float jumpHeight = 2;
    private Vector3 velocity;

    private Rigidbody rigidbody;
    private bool isSliding = false;
    // Start is called before the first frame update
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
        swipeLeft = swipeRight = false;

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            Reset();
        }
        #endregion

        #region Mobile Input
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();
            }
        }
        #endregion

        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length < 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        if (swipeDelta.magnitude > 100)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Swipe Left or Right
                if (x < 0)
                {
                    desiredLane--;
                    if (desiredLane == -1) desiredLane = 0;
                }
                else
                {
                    desiredLane++;
                    if (desiredLane == 3) desiredLane = 2;
                }
            }
            else
            {
                if (isGrounded)
                {
                    //Swipe Up or Down
                    if (y < 0)
                        StartCoroutine(Slide());
                    else
                        Jump();
                }
            }
            Reset();
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        // Calculate the new position.
        Vector3 newPosition = Vector3.Lerp(rigidbody.position, targetPosition, 10 * Time.deltaTime);

        rigidbody.MovePosition(newPosition += -Vector3.back * Time.deltaTime * moveSpeed);

    }
    
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
        Debug.Log(velocity);
        rigidbody.velocity = velocity;
    }

    private IEnumerator Slide()
    {
        isSliding = true;

        // Apply a force to the player in the desired direction.
        rigidbody.AddForce(Vector3.forward * 10f, ForceMode.Impulse);

        // Wait for the slide to finish.
        yield return new WaitForSeconds(0.25f / Time.timeScale);

        // Remove the force from the player.
        rigidbody.velocity = Vector3.zero;

        isSliding = false;
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
}
