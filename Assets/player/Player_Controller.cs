using UnityEngine;

public class Player_Controller : MonoBehaviour {
    [SerializeField] private protected float startSpeed, minSpeed, maxSpeed, acceleration, slowdown;
    private protected float speed, lastDirection;
    private byte playerFlipToRIgh, playerFlipToLeft;

    private Rigidbody2D rb;

    private const string INPUT_AXIS_HORIZONTAL = "Horizontal";

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        MovePlayer();
    }

    private void MovePlayer() {
        float axisHorizontal = Input.GetAxis(INPUT_AXIS_HORIZONTAL);
        rb.velocity = new Vector2(speed * lastDirection, rb.velocity.y);

        if(axisHorizontal != 0) {
            // creates a fixed direction for the player to slide
            lastDirection = axisHorizontal;

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
            if(speed > 0)
                speed -= slowdown * Time.deltaTime;
            else speed = 0;
        }
    }
}