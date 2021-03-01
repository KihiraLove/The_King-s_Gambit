using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override bool[,,] PossibleMove()
    {
        bool[,,] r = new bool[8, 3, 8];
        Piece c, c2;
    
        //White team moves
        if (isWhite)
        {
            //Diagonal Left (Piece not on the left of the board and not in the end)
            if (BoardX != 0 && BoardZ != 7)
            {
                c = BoardManager.Instance.Pieces[BoardX - 1, BoardY, BoardZ + 1];
                
                //Diagonal left same board
                if (c != null && !c.isWhite)
                {
                    r[BoardX - 1, BoardY, BoardZ + 1] = true;
                }
                
                //Diagonal left lower board
                if (BoardY != 0)
                {
                    c = BoardManager.Instance.Pieces[BoardX - 1, BoardY -1, BoardZ + 1];
                    if (c != null && !c.isWhite)
                    {
                        r[BoardX -1, BoardY -1, BoardZ + 1] = true;
                    }
                }
                //Diagonal left higher board
                if (BoardY != 2)
                {
                    c = BoardManager.Instance.Pieces[BoardX - 1, BoardY +1, BoardZ + 1];
                    if (c != null && !c.isWhite)
                    {
                        r[BoardX -1, BoardY +1, BoardZ + 1] = true;
                    }
                }
                
            }

            //Diagonal Right
            if (BoardX != 7 && BoardZ != 7)
            {
                c = BoardManager.Instance.Pieces[BoardX + 1, BoardY, BoardZ + 1];

                //Diagonal right same board
                if (c != null && !c.isWhite)
                {
                    r[BoardX + 1, BoardY, BoardZ + 1] = true;
                }
                
                //Diagonal right lower board
                if (BoardY != 0)
                {
                    c = BoardManager.Instance.Pieces[BoardX + 1, BoardY -1, BoardZ + 1];
                    if (c != null && !c.isWhite)
                    {
                        r[BoardX +1, BoardY -1, BoardZ + 1] = true;
                    }
                }

                //Diagonal right higher board
                if (BoardY != 2)
                {
                    c = BoardManager.Instance.Pieces[BoardX + 1, BoardY +1, BoardZ + 1];
                    if (c != null && !c.isWhite)
                    {
                        r[BoardX +1, BoardY +1, BoardZ + 1] = true;
                    }
                }
            }

            //Middle
            if (BoardZ != 7)
            {
                c = BoardManager.Instance.Pieces[BoardX, BoardY, BoardZ + 1];
                if (c == null)
                {
                    r[BoardX, BoardY, BoardZ + 1] = true;
                }

                if (BoardY != 0)
                {
                    c = BoardManager.Instance.Pieces[BoardX, BoardY -1, BoardZ + 1];
                    if (c == null)
                    {
                        r[BoardX, BoardY -1, BoardZ + 1] = true;
                    }
                    
                    c = BoardManager.Instance.Pieces[BoardX, BoardY -1, BoardZ];
                    if (c == null)
                    {
                        r[BoardX, BoardY -1, BoardZ] = true;
                    }
                }

                if (BoardY != 2)
                {
                    c = BoardManager.Instance.Pieces[BoardX, BoardY +1, BoardZ + 1];
                    if (c == null)
                    {
                        r[BoardX, BoardY + 1, BoardZ + 1] = true;
                    }
                    c = BoardManager.Instance.Pieces[BoardX, BoardY + 1, BoardZ];
                    if (c == null)
                    {
                        r[BoardX, BoardY + 1, BoardZ] = true;
                    }
                    
                }
                
            }

            //Middle on first move
            if (BoardZ == 1)
            {
                r[BoardX, BoardY, BoardZ + 2] = true;
                r[BoardX, 2, BoardZ] = true;
                r[BoardX, 2, BoardZ + 2] = true;
            }
        }
        else //Black team moves
        {
            //Diagonal Left (Piece not on the left of the board and not in the end)
            if (BoardX != 0 && BoardZ != 0)
            {
                c = BoardManager.Instance.Pieces[BoardX - 1, BoardY, BoardZ - 1];

                //Diagonal left same board
                if (c != null && c.isWhite)
                {
                    r[BoardX - 1, BoardY, BoardZ - 1] = true;
                }
                //Diagonal left lower board
                if (BoardY != 0)
                {
                    c = BoardManager.Instance.Pieces[BoardX - 1, BoardY -1, BoardZ - 1];
                    if (c != null && c.isWhite)
                    {
                        r[BoardX -1, BoardY -1, BoardZ - 1] = true;
                    }
                }

                //Diagonal left higher board
                if (BoardY != 2)
                {
                    c = BoardManager.Instance.Pieces[BoardX - 1, BoardY +1, BoardZ - 1];
                    if (c != null && c.isWhite)
                    {
                        r[BoardX -1, BoardY +1, BoardZ - 1] = true;
                    }
                }
            }

            //Diagonal Right
            if (BoardX != 7 && BoardZ != 0)
            {
                c = BoardManager.Instance.Pieces[BoardX + 1, BoardY, BoardZ - 1];
                //Diagonal right same board
                if (c != null && c.isWhite)
                {
                    r[BoardX + 1, BoardY, BoardZ - 1] = true;
                }
                //Diagonal right lower board
                if (BoardY != 0)
                {
                    c = BoardManager.Instance.Pieces[BoardX + 1, BoardY -1, BoardZ - 1];
                    if (c != null && c.isWhite)
                    {
                        r[BoardX +1, BoardY -1, BoardZ - 1] = true;
                    }
                }

                //Diagonal right higher board
                if (BoardY != 2)
                {
                    c = BoardManager.Instance.Pieces[BoardX + 1, BoardY +1, BoardZ - 1];
                    if (c != null && c.isWhite)
                    {
                        r[BoardX +1, BoardY +1, BoardZ - 1] = true;
                    }
                }
            }
            
            //Middle
            if (BoardZ != 0)
            {
                c = BoardManager.Instance.Pieces[BoardX, BoardY, BoardZ - 1];
                if (c == null)
                {
                    r[BoardX, BoardY, BoardZ - 1] = true;
                }

                if (BoardY != 0)
                {
                    c = BoardManager.Instance.Pieces[BoardX, BoardY -1, BoardZ - 1];
                    if (c == null)
                    {
                        r[BoardX, BoardY -1, BoardZ - 1] = true;
                    }
                    
                    c = BoardManager.Instance.Pieces[BoardX, BoardY -1, BoardZ];
                    if (c == null)
                    {
                        r[BoardX, BoardY -1, BoardZ] = true;
                    }
                }

                if (BoardY != 2)
                {
                    c = BoardManager.Instance.Pieces[BoardX, BoardY +1, BoardZ - 1];
                    if (c == null)
                    {
                        r[BoardX, BoardY + 1, BoardZ - 1] = true;
                    }
                    c = BoardManager.Instance.Pieces[BoardX, BoardY + 1, BoardZ];
                    if (c == null)
                    {
                        r[BoardX, BoardY + 1, BoardZ] = true;
                    }
                    
                }
                
            }

            //Middle on first move
            if (BoardZ == 6)
            {
                r[BoardX, BoardY, BoardZ - 2] = true;
                r[BoardX, 0, BoardZ] = true;
                r[BoardX, 0, BoardZ - 2] = true;
            }
        }
        return r;
    }
}
