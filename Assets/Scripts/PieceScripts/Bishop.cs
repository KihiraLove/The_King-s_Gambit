using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override bool[,,] PossibleMove()
    {
        bool[,,] r = new bool[8, 3, 8];

        Piece c;
        int i, j;
        
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
}
