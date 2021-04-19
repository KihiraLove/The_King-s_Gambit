using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public int roundMoved = 0;
    public Vector3 position;
    
    public bool isWhite;

    
    public void SetPosition(Vector3 newPosition)
    {
        position = newPosition;
    }

    

    //Returns an array of possible moves
    public virtual bool[,,] PossibleMove()
    {
        return new bool[8,3,8];
    }

    public virtual char GETPieceCode()
    {
        return 'x';
    }
}
