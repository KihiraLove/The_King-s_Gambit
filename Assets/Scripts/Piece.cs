using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public int CurrentZ { set; get; }
    
    public int BoardX { set; get; }
    public int BoardY { set; get; }
    public int BoardZ { set; get; }
    
    public bool isWhite;

    public void SetPosition(int x, int y, int z)
    {
        CurrentX = x;
        CurrentY = y;
        CurrentZ = z;
    }

    public void SetBoardPosition(int x, int y, int z)
    {
        BoardX = x;
        BoardY = y;
        BoardZ = z;
    }

    public virtual bool[,,] PossibleMove()
    {
        return new bool[8,3,8];
    }
}
