using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    GameObject tetromino;
    int x;
    int y;
    float squareOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setX(int input)
    {
        x = input;
    }
    public void setY(int input)
    {
        y = input;
    }

    public void setOffset(float input)
    {
        squareOffset = input;
    }

    public int getX()
    {
        return x;
    }
    public int getY()
    {
        return y;
    }

    public void FallOnce()
    {
        y -= 1;
        transform.position += new Vector3(0,-1,0);
    }
    public void MoveSide(int sideDistance)
    {
        x = x + sideDistance;
        transform.position += new Vector3(sideDistance,0,0);
    }
    public void MoveTo(int inputX, int inputY)
    {
        transform.position -= new Vector3(x, y,0);
        transform.position += new Vector3(inputX, inputY,0);
        x = inputX;
        y = inputY;
    }
}
