using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public bool hasMoved = false;
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public int CurrentZ { set; get; }
    
    public int BoardX { set; get; }
    public int BoardY { set; get; }
    public int BoardZ { set; get; }
    
    public bool isWhite;

    //Position is coordinates in unity
    public void SetPosition(int x, int y, int z)
    {
        CurrentX = x;
        CurrentY = y;
        CurrentZ = z;
    }

    //Boardposition is the place in a three dimensional array size of 8 x 3 x 8 its the three board digitally reconstructed.
    public void SetBoardPosition(int x, int y, int z)
    {
        BoardX = x;
        BoardY = y;
        BoardZ = z;
    }

    //Returns an array of possible moves
    public virtual bool[,,] PossibleMove()
    {
        return new bool[8,3,8];
    }
}
