using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isAlive;
    private int direction = 0;
    public GameObject game, playerPrefab;
    private Animator animator;
    private float startY;
    private Rigidbody2D rb2d;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = playerPrefab.GetComponent<Animator>();
        isAlive = true;
        startY = transform.position.y;
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
            Vector2 movement = new Vector2 (moveHorizontal, (rb2d.velocity.y==0)?moveVertical*30f:0f); 
            rb2d.AddForce (movement * speed);
            
            if(rb2d.velocity.x == 0 && rb2d.velocity.y==0){
                UpdateState("hooded_Idle");
            }else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                if(direction==0){                   
                    transform.Rotate(0f, 180f, 0f);
                    direction=1;
                }
                if(rb2d.velocity.y==0 && rb2d.velocity.x != 0){
                    UpdateState("hooded_Walk");
                }
            }else if(Input.GetKeyDown(KeyCode.RightArrow)) {
                if(direction==1){
                    transform.Rotate(0f, 180f, 0f);
                    direction=0;
                }
                if(rb2d.velocity.y==0 && rb2d.velocity.x != 0){
                    UpdateState("hooded_Walk");
                }
            }else if(Input.GetKeyDown(KeyCode.UpArrow) || rb2d.velocity.y!=0) {
                UpdateState("hooded_Jump");
            }
            if(rb2d.velocity.x != 0 && rb2d.velocity.y==0){
                UpdateState("hooded_Walk");
            }                 
        }   
    }

    public void UpdateState(string state = null)
    {
        if (state != null)
        {
            animator.Play(state);
        }
    }
}
