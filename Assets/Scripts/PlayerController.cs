using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isAlive;
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
            Vector2 movement = new Vector2 (moveHorizontal, moveVertical); 
            rb2d.AddForce (movement * speed);
            
            if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
                UpdateState("hooded_Idle"); //cambiar por el estado de caminar
            }else if(Input.GetKeyDown(KeyCode.UpArrow)){
                UpdateState("hooded_Jump");
            }else{
                //UpdateState("hooded_Idle");
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
