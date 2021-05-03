using UnityEngine;

public class Knight : Piece
{
    public override bool[,,] PossibleMove(Piece[,,] positions)
    {
        var r = new bool[8, 3, 8];

        //ForwardLeft
        KnightMove((int) position.x - 1, (int) position.y, (int) position.z + 2,position, ref r,positions);

        //ForwardRight
        KnightMove((int) position.x + 1, (int) position.y, (int) position.z + 2, position, ref r,positions);

        //LeftForward
        KnightMove((int) position.x - 2, (int) position.y, (int) position.z + 1, position, ref r,positions);

        //LeftBackward
        KnightMove((int) position.x - 2, (int) position.y, (int) position.z - 1, position, ref r,positions);

        //BackwardLeft
        KnightMove((int) position.x - 1, (int) position.y, (int) position.z - 2, position, ref r,positions);

        //BackwardRight
        KnightMove((int) position.x + 1, (int) position.y, (int) position.z - 2, position, ref r,positions);

        //RightForward
        KnightMove((int) position.x + 2, (int) position.y, (int) position.z + 1, position, ref r,positions);

        //RightBackward
        KnightMove((int) position.x + 2, (int) position.y, (int) position.z - 1, position, ref r,positions);

        //ForwardUp
        KnightMove((int) position.x, (int) position.y + 1, (int) position.z + 2, position, ref r,positions);
        //RightUp
        KnightMove((int) position.x + 2, (int) position.y + 1, (int) position.z, position, ref r,positions);
        //LeftUp
        KnightMove((int) position.x - 2, (int) position.y + 1, (int) position.z, position, ref r,positions);
        //BackwardUp
        KnightMove((int) position.x, (int) position.y + 1, (int) position.z - 2, position, ref r,positions);

        //ForwardDown
        KnightMove((int) position.x, (int) position.y - 1, (int) position.z + 2, position, ref r,positions);
        //RightDown
        KnightMove((int) position.x + 2, (int) position.y - 1, (int) position.z, position, ref r,positions);
        //LeftDown
        KnightMove((int) position.x - 2, (int) position.y - 1, (int) position.z, position, ref r,positions);
        //BackwardDown
        KnightMove((int) position.x, (int) position.y - 1, (int) position.z - 2, position, ref r,positions);

        //ForwardUp2
        KnightMove((int) position.x, (int) position.y + 2, (int) position.z + 1, position, ref r,positions);
        //RightUp2
        KnightMove((int) position.x + 1, (int) position.y + 2, (int) position.z, position, ref r,positions);
        //LeftUp2
        KnightMove((int) position.x - 1, (int) position.y + 2, (int) position.z, position, ref r,positions);
        //BackwardUp2
        KnightMove((int) position.x, (int) position.y + 2, (int) position.z - 1, position, ref r,positions);

        //ForwardDown2
        KnightMove((int) position.x, (int) position.y - 2, (int) position.z + 1, position, ref r,positions);
        //RightDown2
        KnightMove((int) position.x + 1, (int) position.y - 2, (int) position.z, position, ref r,positions);
        //LeftDown2
        KnightMove((int) position.x - 1, (int) position.y - 2, (int) position.z, position, ref r,positions);
        //BackwardDown2
        KnightMove((int) position.x, (int) position.y - 2, (int) position.z - 1, position, ref r,positions);


        return r;
    }

    //Helper function checks if place is valid and puts it in the array
    public void KnightMove(int nX,int nY,int nZ,Vector3 oldPos, ref bool[,,] r,Piece[,,] positions)
    {

        Vector3 newPos = new Vector3(nX,nY,nZ);
        if (ValidPos(newPos))
        {
            Piece c = BoardManagerReworked.Instance.Pieces[nX, nY, nZ];
            if (c == null)
            {
                r[nX, nY, nZ] = true;
                
                
            }
            else if (isWhite != c.isWhite)
            {
                r[nX, nY, nZ] = true;
                
            }
        }
        
    }

    public override char GETPieceCode()
    {
        if (isWhite)
            return 'N';
        return 'n';
    }
}