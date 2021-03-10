﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override bool[,,] PossibleMove()
    {
        bool[,,] r = new bool[8, 3, 8];
        Piece c;
        int i, j, k;
        for (j = (int)position.y - 1; j <= (int)position.y + 1 && j < 3; j++)
        {
            for (i = (int)position.x - 1; i <= (int)position.x + 1 && i < 8; i++)
            {
                for (k = (int)position.z - 1;k  <= (int)position.z + 1 && k <8; k++)
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

    //It shows and allows the castle move for the king
    private void ShowCastleMove(ref bool[,,] r)
    {
        Piece[] backRow = new Piece[8];
        for (int i = 0; i < 8; i++)
        {
            backRow[i] = BoardManager.Instance.Pieces[i, (int)position.y, (int)position.z];
        }

        if (isWhite)
        {
            if (backRow[1] == null && backRow[2] == null&& backRow[3] == null && !backRow[0].hasMoved && !hasMoved)
            {
                r[2, (int)position.y, (int)position.z] = true;
            }
            if (backRow[5] == null&& backRow[6] == null && !backRow[7].hasMoved && !hasMoved)
            {
                r[6, (int)position.y, (int)position.z] = true;
            }
        }
        else
        {
            if (backRow[1] == null && backRow[2] == null&& backRow[3] == null && !backRow[7].hasMoved && !hasMoved)
            {
                r[6, (int)position.y, (int)position.z] = true;
            }
            if (backRow[1] == null&& backRow[2] == null && !backRow[0].hasMoved && !hasMoved)
            {
                r[2, (int)position.y, (int)position.z] = true;
            }
        }
    }
    
}
