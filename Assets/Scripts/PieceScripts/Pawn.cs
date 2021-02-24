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
    
        //Wihte team moves
        if (isWhite)
        {
            //Diagonal Left (Piece not on the left of the board and not in the end)
            if (BoardX != 0 && BoardZ != 7)
            {
                c = BoardManager.Instance.Pieces[BoardX - 1, BoardY, BoardZ + 1];

                if (c != null && !c.isWhite)
                {
                    r[BoardX - 1, BoardY, BoardZ + 1] = true;
                }
            }

            //Diagonal Right
            if (BoardX != 7 && BoardZ != 7)
            {
                c = BoardManager.Instance.Pieces[BoardX + 1, BoardY, BoardZ + 1];

                if (c != null && !c.isWhite)
                {
                    r[BoardX + 1, BoardY, BoardZ + 1] = true;
                }
            }

            //Middle
            if (BoardZ != 7)
            {
                r[BoardX, BoardY, BoardZ + 1] = true;
                if (BoardY != 0)
                {
                    r[BoardX, BoardY -1, BoardZ + 1] = true;
                    r[BoardX, BoardY -1, BoardZ] = true;
                }

                if (BoardY != 2)
                {
                    r[BoardX, BoardY + 1, BoardZ + 1] = true;
                    r[BoardX, BoardY + 1, BoardZ] = true;
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
        else
        {
            //Diagonal Left (Piece not on the left of the board and not in the end)
            if (BoardX != 0 && BoardZ != 0)
            {
                c = BoardManager.Instance.Pieces[BoardX - 1, BoardY, BoardZ - 1];

                if (c != null && c.isWhite)
                {
                    r[BoardX - 1, BoardY, BoardZ - 1] = true;
                }
            }

            //Diagonal Right
            if (BoardX != 7 && BoardZ != 0)
            {
                c = BoardManager.Instance.Pieces[BoardX + 1, BoardY, BoardZ - 1];

                if (c != null && c.isWhite)
                {
                    r[BoardX + 1, BoardY, BoardZ - 1] = true;
                }
            }

            //Middle
            if (BoardZ != 0)
            {
                r[BoardX, BoardY, BoardZ - 1] = true;
                if (BoardY != 0)
                {
                    r[BoardX, BoardY -1, BoardZ - 1] = true;
                    r[BoardX, BoardY -1, BoardZ] = true;
                }

                if (BoardY != 2)
                {
                    r[BoardX, BoardY + 1, BoardZ - 1] = true;
                    r[BoardX, BoardY + 1, BoardZ] = true;
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
