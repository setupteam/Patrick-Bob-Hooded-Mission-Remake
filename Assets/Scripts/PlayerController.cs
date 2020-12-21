using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	const int LEFT_DIRECTION = -1;
	const int RIGHT_DIRECTION = 1;

    public GameObject game, playerPrefab;
    public Transform feetRef;
    public float speed, jumpForce;

    private Rigidbody2D rb2d;
    private Animator animator;
    private bool isAlive, isGrounded;
    private int direction;  

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = playerPrefab.GetComponent<Animator>();
        isAlive = true;
        direction = RIGHT_DIRECTION;
    }

    // Update is called once per frame
    void Update() {
        if(isAlive){}
        //bool isPlaying = (game.GetComponent<GameController>().gameState == GameState.Playing);
        bool isPlaying = true;
        
        if(isPlaying){
            float moveHorizontal = Input.GetAxis ("Horizontal");
            float moveVertical = Input.GetAxis ("Vertical");
            
            if(Input.GetKey(KeyCode.LeftArrow)) {
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
            
            if(isGrounded) {
                //Esta en el suelo
                rb2d.velocity = new Vector2(moveHorizontal * speed / 0.8f, rb2d.velocity.y);
                //Movimiento en el suelo a 0.8
                if(Input.GetKey(KeyCode.UpArrow) && rb2d.velocity.y == 0){
                    //Salto
                    UpdateState("hooded_Idle");
                    UpdateState("hooded_Jump");
                    rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
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
                
                //Movimiento en el aire a 1
                           
                rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);

                if(rb2d.velocity.y < 0){
                    //Descendiendo
                    UpdateState("hooded_Jump");
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

    void CheckPlatform(){
        RaycastHit2D hit = Physics2D.Raycast(feetRef.position, - Vector2.up * 5);
        Debug.DrawRay(feetRef.position, - Vector2.up*5,Color.green);
        if(hit.collider != null){
            if(hit.collider.tag == "Dead Line"){
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Ground"){
            //tocar el suelo
            if((int)(collider.transform.position.y-feetRef.position.y) == 0)
                isGrounded = true;
        }else if(collider.tag == "Dead Line"){
            //morir de una caida al vacio
            transform.position = new Vector2(0, 5);
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Ground"){
            isGrounded = false;
        }
    }

}
