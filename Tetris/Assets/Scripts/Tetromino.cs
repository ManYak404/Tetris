using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tetromino : MonoBehaviour
{
    public GameObject manager;
    public GameObject[] squares;
    Color color;
    int[] pivot;

    public enum BlockType
    {
        Iblock,
        Jblock,
        Lblock,
        Oblock,
        Sblock,
        Tblock,
        Zblock
    }

    public BlockType identity;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setColor(Color input)
    {
        color = input;
    }

    public bool CreateSquares(int[] x, int[] y, float squareOffset, GameObject squarePrefab)
    {
        squares = new GameObject[4];
        pivot = new int[2];
        for(int i = 0; i < 4; i++)
        {
            if(!manager.GetComponent<GameBoard>().IfTileFree(x[i],y[i]))
            {
                return false;
            }
            squares[i] = Instantiate(squarePrefab, new Vector3(x[i]+squareOffset, y[i]+squareOffset, 0), Quaternion.identity);
            squares[i].GetComponent<Square>().setX(x[i]);
            squares[i].GetComponent<Square>().setY(y[i]);
            squares[i].GetComponent<Square>().setOffset(squareOffset);
            squares[i].GetComponent<SpriteRenderer>().color = color;
        }
        pivot[0] = x[0];
        pivot[1] = y[0];
        return true;
    }

    public bool FallOnce()
    {
        //check that a fall is valid for each square
        foreach(GameObject square in squares)
        {
            if(!manager.GetComponent<GameBoard>().IfTileFree(square.GetComponent<Square>().getX(), square.GetComponent<Square>().getY()-1))
            {
                return false;
            }
        }
        //if yes than fall
        foreach(GameObject square in squares)
        {
            square.GetComponent<Square>().FallOnce();
        }
        pivot[1] --;
        return true;
    }

    public bool moveLeft()
    {
        //check that move is valid for each square
        foreach(GameObject square in squares)
        {
            if(!manager.GetComponent<GameBoard>().IfTileFree(square.GetComponent<Square>().getX()-1, square.GetComponent<Square>().getY()))
            {
                return false;
            }
        }
        //if yes than move
        foreach(GameObject square in squares)
        {
            square.GetComponent<Square>().MoveSide(-1);
        }
        pivot[0] --;
        return true;
    }
    public bool moveRight()
    {
        //check that move is valid for each square
        foreach(GameObject square in squares)
        {
            if(!manager.GetComponent<GameBoard>().IfTileFree(square.GetComponent<Square>().getX()+1, square.GetComponent<Square>().getY()))
            {
                return false;
            }
        }
        //if yes than move
        foreach(GameObject square in squares)
        {
            square.GetComponent<Square>().MoveSide(1);
        }
        pivot[0] ++;
        return true;
    }
    public bool Rotate()
    {
        //based on identity figure out where the tiles go
        if(identity == BlockType.Oblock)
        {
            return true;
        }
        else
        {
            //check that move is valid for each square
            foreach(GameObject square in squares)
            {
                int curX = square.GetComponent<Square>().getX();
                int curY = square.GetComponent<Square>().getY();
                int nX = (-(curY - pivot[1])) + pivot[0];
                int nY = curX - pivot[0] + pivot[1];
                if(!manager.GetComponent<GameBoard>().IfTileFree(nX, nY))
                {
                    return false;
                }
            }
            //if yes than perform rotation
            foreach(GameObject square in squares)
            {
                int curX = square.GetComponent<Square>().getX();
                int curY = square.GetComponent<Square>().getY();
                int nX = (-(curY - pivot[1])) + pivot[0];
                int nY = curX - pivot[0] + pivot[1];
                square.GetComponent<Square>().MoveTo(nX,nY);
            }
            return true;
        }
    }
}
