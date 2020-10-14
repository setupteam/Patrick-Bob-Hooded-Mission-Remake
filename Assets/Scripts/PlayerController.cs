using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
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

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = playerPrefab.GetComponent<Animator>();
        isAlive = true;
        startY = transform.position.y;
        this.direction = RIGHT_DIRECTION;
    }

    // Update is called once per frame
    void Update() {
        bool isGrounded = transform.position.y == startY;
        //bool isPlaying = (game.GetComponent<GameController>().gameState == GameState.Playing);
        bool isPlaying = true;
        
        if(isPlaying){
            float moveHorizontal = Input.GetAxis ("Horizontal");
            float moveVertical = Input.GetAxis ("Vertical");
            
            if(isGrounded) {
                //Esta en el suelo
                rb2d.velocity = new Vector2(moveHorizontal * speed / 0.8f, rb2d.velocity.y);
                //Movimiento en el suelo a 0.8
                if(Input.GetKey(KeyCode.UpArrow) && rb2d.velocity.y == 0){
                    //Salto
                    UpdateState("hooded_Idle");
                    UpdateState("hooded_Jump");
                    rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                }else if(Input.GetKey(KeyCode.LeftArrow)) {
                    //Mov. izquierda
                    if(!Input.GetKey(KeyCode.RightArrow)){
                        //evitar que haga moon walk
                        if(direction == RIGHT_DIRECTION){
                            //Esta mirando a la derecha
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            direction = LEFT_DIRECTION;
                             //Esta mirando a la izquierda
                        }
                    }
                }else if(Input.GetKey(KeyCode.RightArrow)) {
                    //Mov. derecha
                    if(direction == LEFT_DIRECTION){
                        //Esta mirando a la izquierda
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        direction = RIGHT_DIRECTION;
                        //Esta mirando a la derecha
                    }
                }

                if(rb2d.velocity.x != 0){
                    //Tiene movimiento horizontal
                    UpdateState("hooded_Walk");
                    //Animación caminar
                }else{
                     //No tiene movimiento horizontal
                    UpdateState("hooded_Idle");
                     //Animación inactivo
                }
            }else{
                //No esta en el suelo
                rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
                //Movimiento en el aire a 1
                if(rb2d.velocity.y == 0){
                    //No hay movimiento vertical
                    transform.position = new Vector2(transform.position.x,startY);
                }else if(rb2d.velocity.y < 0){
                    //Descendiendo
                }else{
                    //Ascendiendo
                     UpdateState("hooded_Jump");
                }
            }
        }   
    }

    public void UpdateState(string state = null) {
        if (state != null){
            animator.Play(state);
        }
    }
}
