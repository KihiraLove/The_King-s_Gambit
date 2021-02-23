using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BoardManager : MonoBehaviour
{
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionx = -1;
    private int selectionz = -1;
    private int selectiony = -1;

    private void Update()
    {
        UpdateSelection();
        DrawChessboard();
    }

    private void DrawChessboard()
    {
        Vector3 xLine = Vector3.right * 8;
        Vector3 zLine = Vector3.forward * 8;

        Vector3 board1OffSet = Vector3.back / 2 + Vector3.left / 2 + Vector3.up / 4;
        Vector3 board2OffSet = board1OffSet + Vector3.forward * 3 + Vector3.up * 3;
        Vector3 board3OffSet = board1OffSet + Vector3.forward * 6 + Vector3.up * 6;

        
        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = (Vector3.forward * i) + board1OffSet;
            Debug.DrawLine(start,start + xLine);
            for (int j = 0; j <= 8; j++)
            {
                start = (Vector3.right * i) + board1OffSet;
                Debug.DrawLine(start,start + zLine);
            }
        }
        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = (Vector3.forward * i) + board2OffSet;
            Debug.DrawLine(start,start + xLine);
            for (int j = 0; j <= 8; j++)
            {
                start = (Vector3.right * i) + board2OffSet;
                Debug.DrawLine(start,start + zLine);
            }
        }
        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = (Vector3.forward * i) + board3OffSet;
            Debug.DrawLine(start,start + xLine);
            for (int j = 0; j <= 8; j++)
            {
                start = (Vector3.right * i) + board3OffSet;
                Debug.DrawLine(start,start + zLine);
            }
        }

        if (selectionx >= 0 && selectionx <= 7 && selectionz >= 0 && selectiony == 0 && selectionz <= 7)
        {
            Debug.DrawLine(board1OffSet + Vector3.forward * selectionz + Vector3.right * selectionx,
                            board1OffSet + Vector3.forward * (selectionz + 1) + Vector3.right * (selectionx + 1)
                            );
            Debug.DrawLine(board1OffSet + Vector3.forward * (selectionz + 1) + Vector3.right * selectionx,
                board1OffSet + Vector3.forward * selectionz + Vector3.right * (selectionx+1)
            );
            
        }
        else if (selectionx >= 0 && selectionx <= 7 && selectionz >= 3 && selectiony == 3 && selectionz <= 10)
        {
            Debug.DrawLine(board1OffSet + Vector3.forward * selectionz + Vector3.right * selectionx + Vector3.up * 3,
                board1OffSet + Vector3.forward * (selectionz + 1) + Vector3.right * (selectionx + 1) + Vector3.up * 3
            );
            Debug.DrawLine(board1OffSet + Vector3.forward * (selectionz + 1) + Vector3.right * selectionx + Vector3.up * 3,
                board1OffSet + Vector3.forward * selectionz + Vector3.right * (selectionx+1) + Vector3.up * 3
            );
        }
        else if (selectionx >= 0 && selectionx <= 7 && selectionz >= 6 && selectiony == 6 && selectionz <= 13)
        {
            Debug.DrawLine(board1OffSet + Vector3.forward * selectionz + Vector3.right * selectionx + Vector3.up * 6,
                board1OffSet + Vector3.forward * (selectionz + 1) + Vector3.right * (selectionx + 1) + Vector3.up * 6
            );
            Debug.DrawLine(board1OffSet + Vector3.forward * (selectionz + 1) + Vector3.right * selectionx + Vector3.up * 6,
                board1OffSet + Vector3.forward * selectionz + Vector3.right * (selectionx+1) + Vector3.up * 6
            );
        }
        
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f,
            LayerMask.GetMask("ChessBoards")))
        {
            selectionx = (int) (hit.point.x + 0.5);
            selectionz = (int) (hit.point.z + 0.5);
            selectiony = (int) (hit.point.y + 0.5);
        }
        else
        {
            selectionx = -1;
            selectionz = -1;
            selectiony = -1;
        }
        
        Debug.Log(selectionx.ToString() + " " + selectiony.ToString() + " " + selectionz.ToString());
    }
}
