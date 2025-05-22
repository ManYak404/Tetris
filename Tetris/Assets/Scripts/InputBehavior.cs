using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBehavior : MonoBehaviour
{
    public GameObject manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeysClicked();
    }

    void CheckKeysClicked()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GetComponent<GameBoard>().getTetromino().GetComponent<Tetromino>().moveLeft();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            GetComponent<GameBoard>().getTetromino().GetComponent<Tetromino>().moveRight();
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<GameBoard>().getTetromino().GetComponent<Tetromino>().FallOnce();
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<GameBoard>().getTetromino().GetComponent<Tetromino>().Rotate();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<GameBoard>().GameOver();
        }
    }
}
