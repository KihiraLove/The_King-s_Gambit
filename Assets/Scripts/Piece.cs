using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public int CurrentZ { set; get; }
    public bool isWhite;

    public void SetPosition(int x, int y, int z)
    {
        CurrentX = x;
        CurrentY = y;
        CurrentZ = z;
    }

    public virtual bool PossibleMove(int x, int y, int z)
    {
        return true;
    }
}
