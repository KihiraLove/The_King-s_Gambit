using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public int roundMoved;
    public Vector3 position;

    public bool isWhite;


    public void SetPosition(Vector3 newPosition)
    {
        position = newPosition;
    }


    //Returns an array of possible moves
    public virtual bool[,,] PossibleMove(Piece[,,] positions)
    {
        return new bool[8, 3, 8];
    }

    public virtual char GETPieceCode()
    {
        return 'x';
    }

    protected Piece[,,] virtualMovePiece(Vector3 oldPos,Vector3 newPos,Vector3 removePos,Piece[,,] array)
    {
        if (!ValidPos(oldPos) && !ValidPos(newPos) && !ValidPos(removePos))
        {
            Debug.Log("Wrong positions!");
            return array;
        }
        Piece[,,] arrayCopy = array;
        int oldX, oldY, oldZ;
        int newX, newY, newZ;
        int remX, remY, remZ;
        
        oldX = (int)oldPos.x;
        oldY = (int)oldPos.y;
        oldZ = (int)oldPos.z;
        
        newX = (int)newPos.x;
        newY = (int)newPos.y;
        newZ = (int)newPos.z;
        
        remX = (int)removePos.x;
        remY = (int)removePos.y;
        remZ = (int)removePos.z;
        
        Piece oldPiece = arrayCopy[oldX, oldY, oldZ];
        
        //Debug.Log(new Vector3(newX,newY,newZ));
        arrayCopy[newX, newY, newZ] = oldPiece;
        arrayCopy[remX, remY, remZ] = null;
        arrayCopy[oldX, oldY, oldZ] = null;

        return arrayCopy;
    }
    
    protected static bool ValidPos(Vector3 pos)
    {
        var x = (int) pos.x;
        var y = (int) pos.y;
        var z = (int) pos.z;
        return x >= 0 && x < 8 && z >= 0 && z < 8 && y >= 0 && y <= 2;
    }
}