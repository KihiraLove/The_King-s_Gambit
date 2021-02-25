using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override bool[,,] PossibleMove()
    {
        bool[,,] r = new bool[8, 3, 8];
        Piece c;
        int i;
        //Move right
        i = BoardX;
        while (true)
        {
            i++;
            if (i >= 8)
            {
                break;
            }

            c = BoardManager.Instance.Pieces[i, BoardY, BoardZ];
            if (c == null)
            {
                r[i, BoardY, BoardZ] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                {
                    r[i, BoardY, BoardZ] = true;
                }
                break;
            }
        }
        //Move left
        i = BoardX;
        while (true)
        {
            i--;
            if (i == -1)
            {
                break;
            }

            c = BoardManager.Instance.Pieces[i, BoardY, BoardZ];
            if (c == null)
            {
                r[i, BoardY, BoardZ] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                {
                    r[i, BoardY, BoardZ] = true;
                }
                
                break;
            }
        }
        //Move forward
        i = BoardZ;
        while (true)
        {
            i++;
            if (i == 8)
            {
                break;
            }

            c = BoardManager.Instance.Pieces[BoardX, BoardY, i];
            if (c == null)
            {
                r[BoardX, BoardY, i] = true;
            }
            else
            {
                
                if (c.isWhite != isWhite)
                {
                    r[BoardX, BoardY, i] = true;
                }
                break;
            }
        }
        //Move backward
        i = BoardZ;
        while (true)
        {
            i--;
            if (i == -1)
            {
                break;
            }

            c = BoardManager.Instance.Pieces[BoardX, BoardY, i];
            if (c == null)
            {
                r[BoardX, BoardY, i] = true;
            }
            else 
            {
                if (c.isWhite != isWhite)
                {
                    r[BoardX, BoardY, i] = true;
                }
                break;
            }
        }
        //Move up
        i = BoardY;
        while (true)
        {
            i++;
            if (i == 3)
            {
                break;
            }

            c = BoardManager.Instance.Pieces[BoardX, i, BoardZ];
            if (c == null)
            {
                r[BoardX, i, BoardZ] = true;
            }
            else
            {
                
                if (c.isWhite != isWhite)
                {
                    r[BoardX, i, BoardZ] = true;
                }
                break;
            }
        }
        //Move down
        i = BoardY;
        while (true)
        {
            i--;
            if (i == -1)
            {
                break;
            }

            c = BoardManager.Instance.Pieces[BoardX, i, BoardZ];
            if (c == null)
            {
                r[BoardX, i, BoardZ] = true;
            }
            else 
            {
                if (c.isWhite != isWhite)
                {
                    r[BoardX, i, BoardZ] = true;
                }
                    
                break;
            }
            
        }
        
        
        //Move in diagonal1 up
        RookMove(BoardX +1, BoardY + 1, BoardZ, ref r);
        RookMove(BoardX -1, BoardY + 1, BoardZ, ref r);
        RookMove(BoardX, BoardY + 1, BoardZ + 1, ref r);
        RookMove(BoardX, BoardY + 1, BoardZ - 1, ref r);
        
        //Move in diagonal2 up
        RookMove(BoardX +2, BoardY + 2, BoardZ, ref r);
        RookMove(BoardX -2, BoardY + 2, BoardZ, ref r);
        RookMove(BoardX, BoardY + 2, BoardZ + 2, ref r);
        RookMove(BoardX, BoardY + 2, BoardZ - 2, ref r);
        
        //Move in diagonal1 down
        RookMove(BoardX +1, BoardY - 1, BoardZ, ref r);
        RookMove(BoardX -1, BoardY - 1, BoardZ, ref r);
        RookMove(BoardX, BoardY - 1, BoardZ + 1, ref r);
        RookMove(BoardX, BoardY - 1, BoardZ - 1, ref r);
        
        //Move in diagonal2 down
        RookMove(BoardX +2, BoardY - 2, BoardZ, ref r);
        RookMove(BoardX -2, BoardY - 2, BoardZ, ref r);
        RookMove(BoardX, BoardY - 2, BoardZ + 2, ref r);
        RookMove(BoardX, BoardY - 2, BoardZ - 2, ref r);
        
        int j;
        
        //Forward Left
        i = BoardX;
        j = BoardZ;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j == 8)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, BoardY, j];
            if (c == null)
            {
                r[i, BoardY, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, BoardY, j] = true;
                }
                
                break;
            }
            
        }
        //Forward Right
        i = BoardX;
        j = BoardZ;
        while (true)
        {
            i++;
            j++;
            if (i == 8 || j == 8)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, BoardY, j];
            if (c == null)
            {
                r[i, BoardY, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, BoardY, j] = true;
                }
                
                break;
            }
            
        }
        //Backward Left
        i = BoardX;
        j = BoardZ;
        while (true)
        {
            i--;
            j--;
            if (i == -1 || j == -1)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, BoardY, j];
            if (c == null)
            {
                r[i, BoardY, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, BoardY, j] = true;
                }
                
                break;
            }
            
        }
        //Backward Right
        i = BoardX;
        j = BoardZ;
        while (true)
        {
            i++;
            j--;
            if (i == 8 || j == -1)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, BoardY, j];
            if (c == null)
            {
                r[i, BoardY, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, BoardY, j] = true;
                }
                
                break;
            }
            
        }

        int k;
        //Forward Left Up
        i = BoardX;
        j = BoardZ;
        k = BoardY;
        while (true)
        {
            i--;
            j++;
            k++;
            if (i < 0 || j == 8 || k == 3)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, k, j] = true;
                }
                
                break;
            }
            
        }
        //Forward Right Up
        i = BoardX;
        j = BoardZ;
        k = BoardY;
        while (true)
        {
            i++;
            j++;
            k++;
            if (i == 8 || j == 8 || k == 3)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, k, j] = true;
                }
                
                break;
            }
            
        }
        //Backward Left Up
        i = BoardX;
        j = BoardZ;
        k = BoardY;
        while (true)
        {
            i--;
            j--;
            k++;
            if (i < 0 || j < 0 || k == 3)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, k, j] = true;
                }
                
                break;
            }
            
        }
        //Backward Right Up
        i = BoardX;
        j = BoardZ;
        k = BoardY;
        while (true)
        {
            i++;
            j--;
            k++;
            if (i == 8 || j < 0 || k == 3)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, k, j] = true;
                }
                
                break;
            }
            
        }
        
        //Forward Left Down
        i = BoardX;
        j = BoardZ;
        k = BoardY;
        while (true)
        {
            i--;
            j++;
            k--;
            if (i < 0 || j == 8 || k < 0)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, k, j] = true;
                }
                
                break;
            }
            
        }
        //Forward Right Down
        i = BoardX;
        j = BoardZ;
        k = BoardY;
        while (true)
        {
            i++;
            j++;
            k--;
            if (i == 8 || j == 8 || k < 0)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, k, j] = true;
                }
                
                break;
            }
            
        }
        //Backward Left Down
        i = BoardX;
        j = BoardZ;
        k = BoardY;
        while (true)
        {
            i--;
            j--;
            k--;
            if (i < 0 || j < 0 || k < 0)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, k, j] = true;
                }
                
                break;
            }
            
        }
        //Backward Right down
        i = BoardX;
        j = BoardZ;
        k = BoardY;
        while (true)
        {
            i++;
            j--;
            k--;
            if (i == 8 || j < 0 || k < 0)
            {
                break;
            }
            
            c = BoardManager.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else 
            {
                if (isWhite != c.isWhite)
                {
                    r[i, k, j] = true;
                }
                
                break;
            }
            
        }
        
        return r;
    }
    
    public void RookMove(int x, int y, int z, ref bool[,,] r)
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
