using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class GameBoard : MonoBehaviour
{
    int width = 10;
    int height = 20;
    float squareOffset = 0.5f;
    float fallingInterval = 0.3f;
    float timeSinceLastFall = 0.0f;
    GameObject[,] tetrisBoard;
    GameObject curTetromino;
    public GameObject squarePrefab;
    public GameObject linePrefab;
    public GameObject tetrominoPrefab;
    // Start is called before the first frame update
    void Start()
    {
        tetrisBoard = new GameObject[width, height];
        SpawnTetromino();
        DrawUI();
    }

    // Update is called once per frame
    void Update()
    {
        fallingCycle();
    }

    public void GameOver()
    {
        SceneManager.LoadScene (sceneName:"GameOver");
    }

    public int ClearLines(int bottomRow)
    {
        //Debug.Log("entered ClearLines at: " + bottomRow);
        int numLinesCleared = 0;
        for(int row = bottomRow; row < bottomRow + 4; row++)
        {
            bool rowFull = true;
            for(int col = 0; col < width; col++)
            {
                if(tetrisBoard[col,row] == null)
                {
                    //Debug.Log("found a hole at: " + row + ", " + col);
                    rowFull = false;
                    break;
                }
            }
            if(rowFull)
            {
                Debug.Log("found clear line at: " + row);
                numLinesCleared ++;
                for(int r = row; r < height; r++)
                {
                    //Debug.Log(row);
                    for(int c = 0; c < width; c++)
                    {
                        //Debug.Log(tetrisBoard[c,r] == null);
                        if(r == row)
                        {
                            Destroy(tetrisBoard[c,r].GetComponent<Square>().gameObject);
                            tetrisBoard[c,r] = null;
                        }
                        else if(tetrisBoard[c,r] != null)
                        {
                            tetrisBoard[c,r].GetComponent<Square>().FallOnce();
                            tetrisBoard[c,r-1] = tetrisBoard[c,r];
                            tetrisBoard[c,r] = null;
                            //Debug.Log(c + ", " + r);
                        }
                        else
                        {
                            //Debug.Log(c + ", " + r + " is null?: "+ tetrisBoard[c,r]);
                        }
                    }
                }
                row --;
            }
        }
        return numLinesCleared;
    }

    public GameObject getTetromino()
    {
        return curTetromino;
    }
    public GameObject[,] getTetrisBoard()
    {
        return tetrisBoard;
    }

    public bool IfTileFree(int x, int y)
    {
        if(y>-1 && y<height && x>-1 && x<width)
        {
            if(tetrisBoard[x,y] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void fallingCycle()
    {
        timeSinceLastFall += Time.deltaTime;
        if(timeSinceLastFall >= fallingInterval)
        {
            //if tetromino fell onto something
            if(!curTetromino.GetComponent<Tetromino>().FallOnce())
            {
                int bottomRow = height - 1;
                //spawn new tetromino and add current tetromino squares to tetris board
                foreach(GameObject square in curTetromino.GetComponent<Tetromino>().squares)
                {
                    int curX = square.GetComponent<Square>().getX();
                    int curY = square.GetComponent<Square>().getY();
                    //Debug.Log("x: " + curX + "    y: " + curY);
                    tetrisBoard[curX,curY] = square;
                    //Debug.Log(tetrisBoard[curX,curY] == null);
                    if(curY<bottomRow)
                    {
                        bottomRow = curY;
                    }
                }
                Destroy(curTetromino);
                ClearLines(bottomRow);
                curTetromino = SpawnTetromino();
                if(curTetromino == null)
                {
                    GameOver();
                }
            }
            timeSinceLastFall = 0.0f;
        }
    }

    GameObject SpawnTetromino()
    {
        int pickTetromino = UnityEngine.Random.Range(0,7);
        switch(pickTetromino)
        {
            case 0:
                return SpawnIBlock();
            case 1:
                return SpawnJBlock();
            case 2:
                return SpawnLBlock();
            case 3:
                return SpawnOBlock();
            case 4:
                return SpawnSBlock();
            case 5:
                return SpawnTBlock();
            case 6:
                return SpawnZBlock();
        }
        return null;
    }

    GameObject SpawnIBlock()
    {
        GameObject spawn = Instantiate(tetrominoPrefab, new Vector3(0,0,0), Quaternion.identity);
        curTetromino = spawn;
        spawn.GetComponent<Tetromino>().manager = GameObject.Find("Manager");
        spawn.GetComponent<Tetromino>().identity = Tetromino.BlockType.Iblock;
        spawn.GetComponent<Tetromino>().setColor(new UnityEngine.Color(0, 1, 1, 1));
        int[] x = {width/2, width/2, width/2, width/2};
        int[] y = {height-2, height-1, height-3, height-4};
        if(spawn.GetComponent<Tetromino>().CreateSquares(x,y,squareOffset,squarePrefab))
            return spawn;
        else
            return null;
    }

    GameObject SpawnJBlock()
    {
        GameObject spawn = Instantiate(tetrominoPrefab, new Vector3(0,0,0), Quaternion.identity);
        curTetromino = spawn;
        spawn.GetComponent<Tetromino>().manager = GameObject.Find("Manager");
        spawn.GetComponent<Tetromino>().identity = Tetromino.BlockType.Jblock;
        spawn.GetComponent<Tetromino>().setColor(new UnityEngine.Color(0, 0, 1, 1));
        int[] x = {width/2, width/2, width/2, (width/2)-1};
        int[] y = {height-2, height-1, height-3, height-3};
        if(spawn.GetComponent<Tetromino>().CreateSquares(x,y,squareOffset,squarePrefab))
            return spawn;
        else
            return null;
    }

    GameObject SpawnLBlock()
    {
        GameObject spawn = Instantiate(tetrominoPrefab, new Vector3(0,0,0), Quaternion.identity);
        curTetromino = spawn;
        spawn.GetComponent<Tetromino>().manager = GameObject.Find("Manager");
        spawn.GetComponent<Tetromino>().identity = Tetromino.BlockType.Lblock;
        spawn.GetComponent<Tetromino>().setColor(new UnityEngine.Color(1.0f, 0.64f, 0.0f, 1));
        int[] x = {width/2, width/2, width/2, (width/2)+1};
        int[] y = {height-2, height-1, height-3, height-3};
        if(spawn.GetComponent<Tetromino>().CreateSquares(x,y,squareOffset,squarePrefab))
            return spawn;
        else
            return null;
    }

    GameObject SpawnOBlock()
    {
        GameObject spawn = Instantiate(tetrominoPrefab, new Vector3(0,0,0), Quaternion.identity);
        curTetromino = spawn;
        spawn.GetComponent<Tetromino>().manager = GameObject.Find("Manager");
        spawn.GetComponent<Tetromino>().identity = Tetromino.BlockType.Oblock;
        spawn.GetComponent<Tetromino>().setColor(new UnityEngine.Color(1, 0.92f, 0.016f, 1));
        int[] x = {width/2, width/2, (width/2)+1, (width/2)+1};
        int[] y = {height-1, height-2, height-1, height-2};
        if(spawn.GetComponent<Tetromino>().CreateSquares(x,y,squareOffset,squarePrefab))
            return spawn;
        else
            return null;
    }

    GameObject SpawnTBlock()
    {
        GameObject spawn = Instantiate(tetrominoPrefab, new Vector3(0,0,0), Quaternion.identity);
        curTetromino = spawn;
        spawn.GetComponent<Tetromino>().manager = GameObject.Find("Manager");
        spawn.GetComponent<Tetromino>().identity = Tetromino.BlockType.Tblock;
        spawn.GetComponent<Tetromino>().setColor(new UnityEngine.Color(1, 0, 1, 1));
        int[] x = {width/2, width/2, (width/2)+1, (width/2)-1};
        int[] y = {height-2, height-1, height-2, height-2};
        if(spawn.GetComponent<Tetromino>().CreateSquares(x,y,squareOffset,squarePrefab))
            return spawn;
        else
            return null;
    }

    GameObject SpawnSBlock()
    {
        GameObject spawn = Instantiate(tetrominoPrefab, new Vector3(0,0,0), Quaternion.identity);
        curTetromino = spawn;
        spawn.GetComponent<Tetromino>().manager = GameObject.Find("Manager");
        spawn.GetComponent<Tetromino>().identity = Tetromino.BlockType.Sblock;
        spawn.GetComponent<Tetromino>().setColor(new UnityEngine.Color(0, 1, 0, 1));
        int[] x = {width/2, width/2, (width/2)+1, (width/2)-1};
        int[] y = {height-2, height-1, height-1, height-2};
        if(spawn.GetComponent<Tetromino>().CreateSquares(x,y,squareOffset,squarePrefab))
            return spawn;
        else
            return null;
    }

    GameObject SpawnZBlock()
    {
        GameObject spawn = Instantiate(tetrominoPrefab, new Vector3(0,0,0), Quaternion.identity);
        curTetromino = spawn;
        spawn.GetComponent<Tetromino>().manager = GameObject.Find("Manager");
        spawn.GetComponent<Tetromino>().identity = Tetromino.BlockType.Zblock;
        spawn.GetComponent<Tetromino>().setColor(new UnityEngine.Color(1, 0, 0, 1));
        int[] x = {width/2, width/2, (width/2)-1, (width/2)+1};
        int[] y = {height-2, height-1, height-1, height-2};
        if(spawn.GetComponent<Tetromino>().CreateSquares(x,y,squareOffset,squarePrefab))
            return spawn;
        else
            return null;
    }

    void DrawUI()
    {
        for(int h = 0; h < height; h++)
        {
            Instantiate(linePrefab, new Vector3(0, h+squareOffset, -1f), Quaternion.identity);
        }
        for(int w = 0; w < width; w++)
        {
            Instantiate(linePrefab, new Vector3(w + squareOffset, 0, -1f), Quaternion.Euler(0, 0, 90));
            for(int h = 0; h < height; h++)
            {
                Instantiate(linePrefab, new Vector3(w+2*squareOffset, h+squareOffset, -1f), Quaternion.identity);
                Instantiate(linePrefab, new Vector3(w+squareOffset, h+2*squareOffset, -1f), Quaternion.Euler(0, 0, 90));
            }
        }
    }
}
