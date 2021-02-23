using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BoardManager : MonoBehaviour
{
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionx = -1;
    private int selectionz = -1;
    private int selectiony = -1;

    private int boardCoordinateY;
    private int boardCoordinateZ;
    
    public Piece[,,] Pieces { set; get; }
    private Piece selectedPiece;

    public List<GameObject> chessPiecesPrefabs;
    private List<GameObject> activeChessPieces;
    
    private Quaternion orientation ;

    public bool isWhiteTurn = true;
    private void Update()
    {
        UpdateSelection();
        DrawChessboard();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionx >= 0 && selectiony >= 0 && selectionz >= 0)
            {
                if (selectedPiece == null)
                {
                    //Select piece
                    setBoardCoordinates(selectiony,selectionz);
                    SelectPiece(selectionx,boardCoordinateY,boardCoordinateZ);
                }
                else
                {
                    //Move;
                    MovePiece(selectionx,selectiony,selectionz);
                }
                
            }
        }
    }

    private void SelectPiece(int x, int y, int z)
    {
        if (Pieces[x, y, z] == null)
        {
            return;
        }

        if (Pieces[x, y, z].isWhite != isWhiteTurn)
        {
            return;
        }
        
        selectedPiece = Pieces[x, y, z];
    }

    private void MovePiece(int x, int y, int z)
    {
        setBoardCoordinates(y,z);
        if (selectedPiece.PossibleMove(x, boardCoordinateY, boardCoordinateZ))
        {
            Pieces[selectedPiece.CurrentX, selectedPiece.CurrentY, selectedPiece.CurrentZ] = null;
            selectedPiece.transform.position = new Vector3(x, y, z);
            Pieces[x, boardCoordinateY, boardCoordinateZ] = selectedPiece;
            selectedPiece = null;
            isWhiteTurn = !isWhiteTurn;
        }
        else
        {
            selectedPiece = null;
        }
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
        
        //Debug.Log(selectionx.ToString() + " " + selectiony.ToString() + " " + selectionz.ToString());
    }

    private void SpawnPiece(int index, int x,int y,int z,bool forward = true)
    {
        if (!forward)
        {
            orientation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            orientation = Quaternion.identity;
        }
        GameObject piece = Instantiate(chessPiecesPrefabs[index],new Vector3(x,y,z),orientation) as GameObject;
        piece.transform.SetParent(transform);

        setBoardCoordinates(y,z);
        Pieces[x, boardCoordinateY, boardCoordinateZ] = piece.GetComponent<Piece>();
        Pieces[x, boardCoordinateY, boardCoordinateZ].SetPosition(x, boardCoordinateY, boardCoordinateZ);
        //;
        activeChessPieces.Add(piece);
    }

    private void setBoardCoordinates(int y, int z)
    {
        switch (y)
        {
            case 0:
                boardCoordinateY = 0;
                boardCoordinateZ = z;
                break;
            case 3:
                boardCoordinateY = 1;
                boardCoordinateZ = z-3;
                break;
            case 6:
                boardCoordinateY = 2;
                boardCoordinateZ = z-6;
                break;
        }
    }

    private void Start()
    {
        InitPieces();
    }

    private void InitPieces()
    {
        activeChessPieces = new List<GameObject>();
        Pieces = new Piece[8,3,8];
        
        //Spawn black pieces
        //King
        SpawnPiece(0,3,6,13,false);
        //Queen
        SpawnPiece(1, 4,6,13,false);
        //Rooks
        SpawnPiece(2, 0,6,13,false);
        SpawnPiece(2, 7,6,13,false);
        //Bishops
        SpawnPiece(3, 2,6,13,false);
        SpawnPiece(3, 5,6,13,false);
        //Knights
        SpawnPiece(4, 1,6,13,false);
        SpawnPiece(4, 6,6,13,false);
        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnPiece(5, i,6,12);
        }
        
        //Spawn white pieces
        //King
        SpawnPiece(6, 3,0,0);
        //Queen
        SpawnPiece(7, 4,0,0);
        //Rooks
        SpawnPiece(8, 0,0,0);
        SpawnPiece(8, 7,0,0);
        //Bishops
        SpawnPiece(9, 2,0,0);
        SpawnPiece(9, 5,0,0);
        //Knights
        SpawnPiece(10, 1,0,0);
        SpawnPiece(10, 6,0,0);
        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnPiece(11, i,0,1);
        }
        
    }
}
