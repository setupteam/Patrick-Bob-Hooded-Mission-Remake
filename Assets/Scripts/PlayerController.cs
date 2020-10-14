using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	const int LEFT_DIRECTION = -1;
	const int RIGHT_DIRECTION = 1;
    private bool isAlive;
    public GameObject game, playerPrefab;
    private int direction;
    private Animator animator;
    private float startY;
    private Rigidbody2D rb2d;
    public float speed;
    public float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = playerPrefab.GetComponent<Animator>();
        isAlive = true;
        startY = transform.position.y;
        this.direction = RIGHT_DIRECTION;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = transform.position.y == startY;
        //bool isPlaying = (game.GetComponent<GameController>().gameState == GameState.Playing);
        bool isPlaying = true;
        
        if(isPlaying){
            float moveHorizontal = Input.GetAxis ("Horizontal");
            float moveVertical = Input.GetAxis ("Vertical");

            if(isGrounded) {
                rb2d.velocity = new Vector2(moveHorizontal * speed / 0.8f, rb2d.velocity.y);
                if(Input.GetKey(KeyCode.UpArrow) && rb2d.velocity.y == 0){
                    UpdateState("hooded_Idle");
                    UpdateState("hooded_Jump");
                    rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                }else if(Input.GetKey(KeyCode.LeftArrow)) {
                    if(!Input.GetKey(KeyCode.RightArrow)){
                        if(direction == RIGHT_DIRECTION){
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            direction = LEFT_DIRECTION;
                        }
                    }
                }else if(Input.GetKey(KeyCode.RightArrow)) {
                    if(direction == LEFT_DIRECTION){
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        direction = RIGHT_DIRECTION;
                    }
                }

                if(rb2d.velocity.x != 0){
                    UpdateState("hooded_Walk");
                }else{
                    UpdateState("hooded_Idle");
                }
            }else{
                rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
                if(rb2d.velocity.y==0){
                    transform.position = new Vector2(transform.position.x,startY);
                }else if(rb2d.velocity.y < 0){
                }else{
                     UpdateState("hooded_Jump");
                }
            }
        }   
    }

    public void UpdateState(string state = null)
    {
        if (state != null){
            animator.Play(state);
        }
    }
}
