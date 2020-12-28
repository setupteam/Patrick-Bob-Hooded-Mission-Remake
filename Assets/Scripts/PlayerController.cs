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
             if(Input.GetKey(KeyCode.UpArrow)){
                    Debug.Log(isGrounded);
                }
            if(isGrounded) {
                //Esta en el suelo
                rb2d.velocity = new Vector2(moveHorizontal * speed / 0.8f, rb2d.velocity.y);
                //Movimiento en el suelo a 0.8
                if(Input.GetKey(KeyCode.UpArrow)){
                    Debug.Log(rb2d.velocity.y);
                }
                
                if(Input.GetKey(KeyCode.UpArrow) && rb2d.velocity.y>= -0.1 && rb2d.velocity.y <= 0.1){
                    //Salto
                    UpdateState("hooded_Idle");
                    UpdateState("hooded_Jump");
                    rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    isGrounded = false;
                }
                if(rb2d.velocity.x != 0 && isGrounded){
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

                }else if(rb2d.velocity.y > 0){
                    //Ascendiendo
                    UpdateState("hooded_Jump");
                }else{
                    UpdateState("hooded_Idle");
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

    /** Una sola colisión
    void OnCollisionEnter2D(Collision2D collision){
        string tagName = collision.gameObject.tag;
        Vector2 point = collision.contacts[0].point;
        // umbral de aceptación es de 0.2... si los pies están dentro de .2 menos o .2 mas de la posición de la colisión
        if(tagName == "Ground" && feetRef.position.y > point.y-0.2 && feetRef.position.y < point.y+0.2){
            //tocar el suelo
            isGrounded = true;    
        }
    }
    */

    void OnCollisionEnter2D(Collision2D collision){
        string tagName = collision.gameObject.tag;
        ContactPoint2D[] points = collision.contacts;
        // umbral de aceptación es de 0.2... si los pies están dentro de .2 menos o .2 mas de la posición de la colisión
        if(tagName == "Ground"){
            bool floorContact = false;
            for (int i = 0; i < points.Length && !floorContact; i++)
            {
                if(feetRef.position.y > points[i].point.y-0.2 && feetRef.position.y < points[i].point.y+0.2){
                    floorContact = true;
                }
            }
            //tocar el suelo
            isGrounded = floorContact;    
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        string tagName = collision.gameObject.tag;
        if(tagName == "Ground"){
            isGrounded = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Dead Line"){
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
