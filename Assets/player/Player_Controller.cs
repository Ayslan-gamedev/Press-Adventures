using UnityEngine;

public class Player_Controller : MonoBehaviour {
    // Components
    private Rigidbody2D rb;
    private Animator animatorController;
    private enum Player_Animations { Idle, Walk, Jump, Dash, Shot }

    #region Movement
    [SerializeField] private protected float startSpeed, minSpeed, maxSpeed, acceleration, slowdown;
    private protected float speed, lastDirection;
    private byte playerFlipToRIgh, playerFlipToLeft;
    private const string INPUT_AXIS_HORIZONTAL = "Horizontal";
    #endregion

    #region Jump
    [SerializeField] private protected float jumpImpulse;
    [SerializeField] protected float timeToCoyoteTime;
    private float coyoteTimer = 0;
    byte jump = 0;
    #endregion

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        MovePlayer();
        JumpPlayer();
    }

    // Movement the player Object and Flip
    private void MovePlayer() {
        float axisHorizontal = Input.GetAxis(INPUT_AXIS_HORIZONTAL);
        rb.velocity = new Vector2(speed * lastDirection, rb.velocity.y);

        if(axisHorizontal != 0) {
            lastDirection = axisHorizontal;
            ChangeAnimation(Player_Animations.Walk);

            if(speed < minSpeed) speed = minSpeed;

            if(speed > maxSpeed) speed = maxSpeed; 
            else speed += acceleration * Time.deltaTime;

            #region Flip
            if(lastDirection > 0) playerFlipToLeft = 0;
            else playerFlipToLeft = 1;

            // Flip player object 
            if(playerFlipToRIgh != playerFlipToLeft) {
                playerFlipToRIgh = playerFlipToLeft;
                Vector2 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            #endregion
        }
        else {
            ChangeAnimation(Player_Animations.Idle);
            
            if(speed > 0) speed -= slowdown * Time.deltaTime;
            else speed = 0;
        }
    }

    // Jump the player Object
    private void JumpPlayer() {
        bool CanJump() {
            RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, 1 << 7);
            RaycastHit2D groundHit2 = Physics2D.Raycast(new Vector2(transform.position.x - 0.2f, transform.position.y), Vector2.down, 0.2f, 1 << 7);
            RaycastHit2D groundHit3 = Physics2D.Raycast(new Vector2(transform.position.x + 0.2f, transform.position.y), Vector2.down, 0.2f, 1 << 7);

            if(groundHit.collider == true || groundHit2.collider == true || groundHit3.collider == true) {
                jump = 0;
                coyoteTimer = 0;
                return true;
            }
            else{
                if(coyoteTimer < timeToCoyoteTime && jump == 0) {
                    coyoteTimer += 0.025f;
                    return true;
                }
                else {
                    coyoteTimer = timeToCoyoteTime;
                    return false;
                }
            }
        }

        if(CanJump() == true && Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
            jump = 1;
        }
    }

    // Manager Sprite Animations 
    private void ChangeAnimation(Player_Animations animation) {
        animatorController.Play(animation.ToString());
    }
}