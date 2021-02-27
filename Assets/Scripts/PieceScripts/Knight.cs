using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override bool[,,] PossibleMove()
    {
        bool[,,] r = new bool[8, 3, 8];
        
        //ForwardLeft
        KnightMove(BoardX-1,BoardY,BoardZ +2,ref r);
        
        //ForwardRight
        KnightMove(BoardX+1,BoardY,BoardZ +2,ref r);
        
        //LeftForward
        KnightMove(BoardX-2,BoardY,BoardZ +1,ref r);
        
        //LeftBackward
        KnightMove(BoardX-2,BoardY,BoardZ -1,ref r);
        
        //BackwardLeft
        KnightMove(BoardX-1,BoardY,BoardZ -2,ref r);
        
        //BackwardRight
        KnightMove(BoardX+1,BoardY,BoardZ -2,ref r);
        
        //RightForward
        KnightMove(BoardX+2,BoardY,BoardZ +1,ref r);
        
        //RightBackward
        KnightMove(BoardX+2,BoardY,BoardZ -1,ref r);

        //ForwardUp
        KnightMove(BoardX,BoardY +1,BoardZ+2,ref r);
        //RightUp
        KnightMove(BoardX + 2,BoardY +1,BoardZ,ref r);
        //LeftUp
        KnightMove(BoardX - 2,BoardY +1,BoardZ,ref r);
        //BackwardUp
        KnightMove(BoardX,BoardY +1,BoardZ-2,ref r);
        
        //ForwardDown
        KnightMove(BoardX,BoardY -1,BoardZ+2,ref r);
        //RightDown
        KnightMove(BoardX + 2,BoardY -1,BoardZ,ref r);
        //LeftDown
        KnightMove(BoardX - 2,BoardY -1,BoardZ,ref r);
        //BackwardDown
        KnightMove(BoardX,BoardY -1,BoardZ-2,ref r);
        
        //ForwardUp2
        KnightMove(BoardX,BoardY +2,BoardZ+1,ref r);
        //RightUp2
        KnightMove(BoardX + 1,BoardY +2,BoardZ,ref r);
        //LeftUp2
        KnightMove(BoardX - 1,BoardY +2,BoardZ,ref r);
        //BackwardUp2
        KnightMove(BoardX,BoardY +2,BoardZ-1,ref r);
        
        //ForwardDown2
        KnightMove(BoardX,BoardY -2,BoardZ+1,ref r);
        //RightDown2
        KnightMove(BoardX + 1,BoardY -2,BoardZ,ref r);
        //LeftDown2
        KnightMove(BoardX - 1,BoardY -2,BoardZ,ref r);
        //BackwardDown2
        KnightMove(BoardX,BoardY -2,BoardZ-1,ref r);
        
        
        return r;
    }

    //Helper function checks if place is valid and puts it in the array
    public void KnightMove(int x, int y, int z, ref bool[,,] r)
    {
        Piece c;
        if (x >= 0 && x < 8 && z >= 0 && z < 8 && y >= 0 && y < 3)
        {
            c = BoardManager.Instance.Pieces[x, y, z];
            if (c == null)
            {
                r[x, y, z] = true;
            }
            else if (isWhite != c.isWhite)
            {
                r[x, y, z] = true;
            }
        }
    }
}

