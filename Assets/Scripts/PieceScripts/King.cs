using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override bool[,,] PossibleMove()
    {
        bool[,,] r = new bool[8, 3, 8];
        Piece c;
        int i, j, k;
        for (j = BoardY - 1; j <= BoardY + 1 && j < 3; j++)
        {
            for (i = BoardX - 1; i <= BoardX + 1 && i < 8; i++)
            {
                for (k = BoardZ - 1;k  <= BoardZ + 1 && k <8; k++)
                {
                    if (j == -1 || i == -1 || k == -1 )
                    {
                        continue;
                    }

                    
                    c = BoardManager.Instance.Pieces[i, j, k];
                    if (c == null)
                    {
                        r[i, j, k] = true;
                    }
                    else if(c.isWhite != isWhite)
                    {
                        r[i, j, k] = true;
                    }

                }
            }
        }

        ShowCastleMove(ref r);
        return r;
    }

    private void ShowCastleMove(ref bool[,,] r)
    {
        Piece[] backRow = new Piece[8];
        for (int i = 0; i < 8; i++)
        {
            backRow[i] = BoardManager.Instance.Pieces[i, BoardY, BoardZ];
        }

        if (isWhite)
        {
            if (backRow[1] == null && backRow[2] == null&& backRow[3] == null && !backRow[0].hasMoved && !hasMoved)
            {
                r[2, BoardY, BoardZ] = true;
            }
            if (backRow[5] == null&& backRow[6] == null && !backRow[7].hasMoved && !hasMoved)
            {
                r[6, BoardY, BoardZ] = true;
            }
        }
        else
        {
            if (backRow[1] == null && backRow[2] == null&& backRow[3] == null && !backRow[7].hasMoved && !hasMoved)
            {
                r[6, BoardY, BoardZ] = true;
            }
            if (backRow[1] == null&& backRow[2] == null && !backRow[0].hasMoved && !hasMoved)
            {
                r[2, BoardY, BoardZ] = true;
            }
        }
    }
    
}
