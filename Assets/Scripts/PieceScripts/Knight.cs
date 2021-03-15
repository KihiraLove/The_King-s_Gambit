using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override bool[,,] PossibleMove()
    {
        bool[,,] r = new bool[8, 3, 8];
        
        //ForwardLeft
        KnightMove((int)position.x-1,(int)position.y,(int)position.z +2,ref r);
        
        //ForwardRight
        KnightMove((int)position.x+1,(int)position.y,(int)position.z +2,ref r);
        
        //LeftForward
        KnightMove((int)position.x-2,(int)position.y,(int)position.z +1,ref r);
        
        //LeftBackward
        KnightMove((int)position.x-2,(int)position.y,(int)position.z -1,ref r);
        
        //BackwardLeft
        KnightMove((int)position.x-1,(int)position.y,(int)position.z -2,ref r);
        
        //BackwardRight
        KnightMove((int)position.x+1,(int)position.y,(int)position.z -2,ref r);
        
        //RightForward
        KnightMove((int)position.x+2,(int)position.y,(int)position.z +1,ref r);
        
        //RightBackward
        KnightMove((int)position.x+2,(int)position.y,(int)position.z -1,ref r);

        //ForwardUp
        KnightMove((int)position.x,(int)position.y +1,(int)position.z+2,ref r);
        //RightUp
        KnightMove((int)position.x + 2,(int)position.y +1,(int)position.z,ref r);
        //LeftUp
        KnightMove((int)position.x - 2,(int)position.y +1,(int)position.z,ref r);
        //BackwardUp
        KnightMove((int)position.x,(int)position.y +1,(int)position.z-2,ref r);
        
        //ForwardDown
        KnightMove((int)position.x,(int)position.y -1,(int)position.z+2,ref r);
        //RightDown
        KnightMove((int)position.x + 2,(int)position.y -1,(int)position.z,ref r);
        //LeftDown
        KnightMove((int)position.x - 2,(int)position.y -1,(int)position.z,ref r);
        //BackwardDown
        KnightMove((int)position.x,(int)position.y -1,(int)position.z-2,ref r);
        
        //ForwardUp2
        KnightMove((int)position.x,(int)position.y +2,(int)position.z+1,ref r);
        //RightUp2
        KnightMove((int)position.x + 1,(int)position.y +2,(int)position.z,ref r);
        //LeftUp2
        KnightMove((int)position.x - 1,(int)position.y +2,(int)position.z,ref r);
        //BackwardUp2
        KnightMove((int)position.x,(int)position.y +2,(int)position.z-1,ref r);
        
        //ForwardDown2
        KnightMove((int)position.x,(int)position.y -2,(int)position.z+1,ref r);
        //RightDown2
        KnightMove((int)position.x + 1,(int)position.y -2,(int)position.z,ref r);
        //LeftDown2
        KnightMove((int)position.x - 1,(int)position.y -2,(int)position.z,ref r);
        //BackwardDown2
        KnightMove((int)position.x,(int)position.y -2,(int)position.z-1,ref r);
        
        
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
    
    public override char GETPieceCode()
    {
        if (isWhite)
        {
            return 'N';
        }
        else
        {
            return 'n';
        }
    }
}

