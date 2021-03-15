using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    private bool[,,] allowedMoves { set; get; }
    
    //Offset of boards
    private const int FIRST_BOARD_HEIGHT = 3;
    private const int SECOND_BOARD_HEIGHT = 6;
    private const int FIRST_BOARD_OFFSET = 3;
    private const int SECOND_BOARD_OFFSET = 6;

    //Coordinate of your mouse
    private int selectionx = -1;
    private int selectionz = -1;
    private int selectiony = -1;
    
    
    public Piece[,,] Pieces { set; get; }
    private Piece selectedPiece;

    public List<GameObject> chessPiecesPrefabs;
    private List<GameObject> activeChessPieces;
    
    
    //It works when spawning the knight
    private Quaternion orientation ;

    public bool isWhiteTurn = true;
    

    //Piece selection
    private void SelectPiece(int x, int y, int z)
    {
        //Check if it is a piece you clicking
        if (Pieces[x, y, z] == null)
        {
           
            selectedPiece = null;
            return;
        }

        //Check if it is your piece
        if (Pieces[x, y, z].isWhite != isWhiteTurn)
        {
           
            selectedPiece = null;
            return;
        }

        //Check if your selected piece have any move at all
        bool hasMove = false;
        allowedMoves = Pieces[x, y, z].PossibleMove();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (allowedMoves[i, j, k])
                    {
                        hasMove = true;
                    }
                }
            }
        }
        
        //If it has no move then you cant select it
        if (!hasMove)
        {
            
            return;
        }
        
        
        //Select the piece and show the allowed moves
        selectedPiece = Pieces[x, y, z];
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves,FIRST_BOARD_HEIGHT,FIRST_BOARD_OFFSET);
    }

    //Move the selected piece to position
    private void MovePiece(int x, int y, int z)
    {
        //Check if the move is castle and then castle if it is
        int [] boardCoordinate  = getBoardCoordinates(y,z);
        if (selectedPiece.GetType() == typeof(King) && (x == (int)selectedPiece.position.x-2 || x == (int)selectedPiece.position.x+2) && !selectedPiece.hasMoved && allowedMoves[x,boardCoordinate[0],boardCoordinate[1]])
        {
            Castle(x);
            return;
        }
        //If the move is highlighted move the piece
        if (allowedMoves[x,boardCoordinate[0],boardCoordinate[1]])
        {

            Piece c = Pieces[x, boardCoordinate[0], boardCoordinate[1]];
            if (c != null && c.isWhite != isWhiteTurn)
            {
                //If the king is destroyed then reset the game
                if (c.GetType() == typeof(King))
                {
                    EndGame();
                    return;
                }
                activeChessPieces.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            Pieces[(int)selectedPiece.position.x, (int)selectedPiece.position.y, (int)selectedPiece.position.z] = null;

            selectedPiece.hasMoved = true;
            
            selectedPiece.transform.position = new Vector3(x, y, z);
            
            selectedPiece.SetPosition(new Vector3(x, y, z));
            
            
            
            Pieces[x, boardCoordinate[0], boardCoordinate[1]] = selectedPiece;
            
            selectedPiece = null;
            
            isWhiteTurn = !isWhiteTurn;
            
        }
        else
        {
            selectedPiece = null;
        }
        
        BoardHighlights.Instance.HideHighlights();
    }

    private void Castle( int x)
    {
        /*
        if (x < (int)selectedPiece.position.x)
        {
            Pieces[(int)selectedPiece.position.x, (int)selectedPiece.position.y, (int)selectedPiece.position.z] = null;

            selectedPiece.transform.position = new Vector3(x, selectedPiece.CurrentY, selectedPiece.CurrentZ);
            
            selectedPiece.SetPosition(x,selectedPiece.CurrentY,selectedPiece.CurrentZ);
            selectedPiece.SetBoardPosition(x,(int)selectedPiece.position.y,(int)selectedPiece.position.z);
            selectedPiece.hasMoved = true;
            Pieces[x, (int)selectedPiece.position.y,(int)selectedPiece.position.z] = selectedPiece;
            
            selectedPiece = Pieces[0, (int)selectedPiece.position.y, (int)selectedPiece.position.z];
            selectedPiece.transform.position = new Vector3(x+1, selectedPiece.CurrentY, selectedPiece.CurrentZ);
            
            selectedPiece.SetPosition(x+1,selectedPiece.CurrentY,selectedPiece.CurrentZ);
            selectedPiece.SetBoardPosition(x+1,(int)selectedPiece.position.y,(int)selectedPiece.position.z);
            selectedPiece.hasMoved = true;
            Pieces[x-1, (int)selectedPiece.position.y,(int)selectedPiece.position.z] = selectedPiece;
            
            
            
            selectedPiece = null;
            isWhiteTurn = !isWhiteTurn;
        }
        else
        {
            Pieces[(int)selectedPiece.position.x, (int)selectedPiece.position.y, (int)selectedPiece.position.z] = null;

            selectedPiece.transform.position = new Vector3(x, selectedPiece.CurrentY, selectedPiece.CurrentZ);
            
            selectedPiece.SetPosition(x,selectedPiece.CurrentY,selectedPiece.CurrentZ);
            selectedPiece.SetBoardPosition(x,(int)selectedPiece.position.y,(int)selectedPiece.position.z);
            selectedPiece.hasMoved = true;
            Pieces[x, (int)selectedPiece.position.y,(int)selectedPiece.position.z] = selectedPiece;
            
            selectedPiece = Pieces[7, (int)selectedPiece.position.y, (int)selectedPiece.position.z];
            selectedPiece.transform.position = new Vector3(x-1, selectedPiece.CurrentY, selectedPiece.CurrentZ);
            
            selectedPiece.SetPosition(x-1,selectedPiece.CurrentY,selectedPiece.CurrentZ);
            selectedPiece.SetBoardPosition(x-1,(int)selectedPiece.position.y,(int)selectedPiece.position.z);
            selectedPiece.hasMoved = true;
            Pieces[x-1, (int)selectedPiece.position.y,(int)selectedPiece.position.z] = selectedPiece;
            
            
            
            selectedPiece = null;
            isWhiteTurn = !isWhiteTurn;
        }
        BoardHighlights.Instance.HideHighlights();*/
    }

    //Reset the game
    private void EndGame()
    {
        if (isWhiteTurn)
        {
            Debug.Log("White won!");
        }
        else
        {
            Debug.Log("Black won!");
        }

        foreach (GameObject go in activeChessPieces)
        {
            Destroy(go);
        }

        isWhiteTurn = true;
        
        BoardHighlights.Instance.HideHighlights();
        InitPieces();
        
    }

    //This function sets the coordinates of selection
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
            int tempY =  (int)(hit.point.y + 0.5);
            selectionx =  (int)(hit.point.x + 0.5);
            selectionz =  (int)(hit.point.z + 0.5);
            
            if (tempY >= 0 && tempY < FIRST_BOARD_HEIGHT)
            {
                selectiony = 0;
            }
            else if(tempY >= FIRST_BOARD_HEIGHT && tempY < SECOND_BOARD_HEIGHT)
            {
                selectiony = FIRST_BOARD_HEIGHT;
            }
            else if (tempY > 0)
            {
                selectiony = SECOND_BOARD_HEIGHT;
            }
        }
        else
        {
            selectionx = -1;
            selectionz = -1;
            selectiony = -1;
        }
        
    }

    
    private void SpawnPiece(int index, int x,int y,int z,bool forward = true) {
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

        int [] boardCoordinate  = getBoardCoordinates(y,z);
        Pieces[x, boardCoordinate[0], boardCoordinate[1]] = piece.GetComponent<Piece>();
        Pieces[x, boardCoordinate[0], boardCoordinate[1]].SetPosition(new Vector3(x,y,z));
        //;
        activeChessPieces.Add(piece);
    }

    private int [] getBoardCoordinates(int y, int z)
    {
        int []retval = new int[2];
        if (y >= 0 && y < FIRST_BOARD_HEIGHT)
        {
            retval[0] = 0;
            retval[1] = z;
        }
        else if (y >= FIRST_BOARD_HEIGHT && y < SECOND_BOARD_HEIGHT)
        {
            retval[0] = 1;
            retval[1] = z-FIRST_BOARD_OFFSET;
        }
        else if(y > 0)
        {
            retval[0] = 2;
            retval[1] = z-SECOND_BOARD_OFFSET;
        }

        return retval;
    }

    private void Start()
    {
        Instance = this;
        //InitPieces();
    }
    
    private void Update()
    {
        UpdateSelection();
        //DrawChessboard();

         
    }

    private void InitPieces()
    {
        activeChessPieces = new List<GameObject>();
        Pieces = new Piece[8,3,8];
        
        //Spawn black pieces
        //King
        SpawnPiece(0,4,SECOND_BOARD_HEIGHT,7 + SECOND_BOARD_OFFSET,false);
        //Queen
        SpawnPiece(1, 3,SECOND_BOARD_HEIGHT,7 + SECOND_BOARD_OFFSET,false);
        //Rooks
        SpawnPiece(2, 0,SECOND_BOARD_HEIGHT,7 + SECOND_BOARD_OFFSET,false);
        SpawnPiece(2, 7,SECOND_BOARD_HEIGHT,7 + SECOND_BOARD_OFFSET,false);
        //Bishops
        SpawnPiece(3, 2,SECOND_BOARD_HEIGHT,7 + SECOND_BOARD_OFFSET,false);
        SpawnPiece(3, 5,SECOND_BOARD_HEIGHT,7 + SECOND_BOARD_OFFSET,false);
        //Knights
        SpawnPiece(4, 1,SECOND_BOARD_HEIGHT,7 + SECOND_BOARD_OFFSET,false);
        SpawnPiece(4, 6,SECOND_BOARD_HEIGHT,7 + SECOND_BOARD_OFFSET,false);
        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnPiece(5, i,SECOND_BOARD_HEIGHT,6 + SECOND_BOARD_OFFSET);
        }
        
        //Spawn white pieces
        //King
        SpawnPiece(6, 4,0,0);
        //Queen
        SpawnPiece(7, 3,0,0);
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
