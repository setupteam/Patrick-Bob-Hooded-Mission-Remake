using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Idle, Playing, Ended, ReadyToRestart};

public class GameController : MonoBehaviour
{   
    // Start is called before the first frame update
    public int number = 0;
    private GameState gameState = GameState.Idle;
    void Start()
    {
        gameState = GameState.Playing;
    }

    // Update is called once per frame
    void Update()
    {
        number++;
    }
}
