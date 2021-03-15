using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BoardManagerReworked : MonoBehaviour
{
    

    public Vector3 board1Offset;
    public Vector3 board2Offset;
    public Vector3 board3Offset;
    public static BoardManagerReworked Instance { set; get; }

    public bool whiteTurn;
    
    
    public List<GameObject> chessPiecePrefabs;
    public GameObject chessBoardPrefab;
    private List<GameObject> _activeChessPieces;
    
    public Piece[,,] Pieces { set; get; }

    public Vector3 mousePosition;

    private Piece selectedPiece;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Initialize();
        print(getBoardState());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();
        if (Input.GetMouseButtonDown(0))
        {
            if (mousePosition.x > -98)
            {
                
                if (selectedPiece == null && GetPiece(mousePosition) != null && GetPiece(mousePosition).isWhite == whiteTurn )
                {
                    //Select piece
                    selectedPiece = GetPiece(mousePosition);

                }
                else if(selectedPiece != null && (GetPiece(mousePosition) == null || GetPiece(mousePosition).isWhite != whiteTurn))
                {
                    //Move;
                    MovePiece(selectedPiece.position,new Vector3(mousePosition.x,mousePosition.y,mousePosition.z),(int)mousePosition.y);
                    selectedPiece = null;
                    
                }
                else
                {
                    selectedPiece = null;
                }
                
                
            }
        }
    }
    
    private void Initialize()
    {
        SpawnBoards();
        SpawnAllPieces();
        whiteTurn = true;
    }
    private void SpawnBoards()
    {
        GameObject board1 = Instantiate(chessBoardPrefab,board1Offset,Quaternion.identity);
        board1.transform.SetParent(transform);
        GameObject board2 = Instantiate(chessBoardPrefab,board2Offset,Quaternion.identity);
        board2.transform.SetParent(transform);
        GameObject board3 = Instantiate(chessBoardPrefab,board3Offset,Quaternion.identity);
        board3.transform.SetParent(transform);
        
        board1.GetComponent<BoardData>().boardNumber = 1;
        board2.GetComponent<BoardData>().boardNumber = 2;
        board3.GetComponent<BoardData>().boardNumber = 3;
    }

    private void SpawnPiece(int index, Vector3 position,Vector3 offset ,int board,bool forward = true)
    {
        Quaternion orientation;
        if (!forward)
        {
            orientation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            orientation = Quaternion.identity;
        }
        
        GameObject piece = Instantiate(chessPiecePrefabs[index],position+offset,orientation);
        piece.transform.SetParent(transform);
        
        Pieces[(int)position.x, board, (int)position.z] = piece.GetComponent<Piece>();
        Pieces[(int)position.x, board, (int)position.z].SetPosition(new Vector3(position.x,board,position.z));
        
        _activeChessPieces.Add(piece);
        
    }

    private void SpawnAllPieces()
    {
        _activeChessPieces = new List<GameObject>();
        Pieces = new Piece[8,3,8];
        
        //Spawn black pieces
        //King
        SpawnPiece(0,new Vector3(4,0,7),board3Offset,2,false);
        //Queen
        SpawnPiece(1,new Vector3(3,0,7),board3Offset,2,false);
        //Rooks
        SpawnPiece(4,new Vector3(0,0,7),board3Offset,2,false);
        SpawnPiece(4,new Vector3(7,0,7),board3Offset,2,false);
        //Bishops
        SpawnPiece(2,new Vector3(2,0,7),board3Offset,2,false);
        SpawnPiece(2,new Vector3(5,0,7),board3Offset,2,false);
        //Knights
        SpawnPiece(3,new Vector3(1,0,7),board3Offset,2,false);
        SpawnPiece(3,new Vector3(6,0,7),board3Offset,2,false);
        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnPiece(5,new Vector3(i,0,6),board3Offset,2,false);
        }
        
        //Spawn white pieces
        //King
        SpawnPiece(6,new Vector3(4,0,0),board1Offset,0,false);
        //Queen
        SpawnPiece(7,new Vector3(3,0,0),board1Offset,0,false);
        //Rooks
        SpawnPiece(10,new Vector3(0,0,0),board1Offset,0,false);
        SpawnPiece(10,new Vector3(7,0,0),board1Offset,0,false);
        //Bishops
        SpawnPiece(8,new Vector3(2,0,0),board1Offset,0,false);
        SpawnPiece(8,new Vector3(5,0,0),board1Offset,0,false);
        //Knights
        SpawnPiece(9,new Vector3(1,0,0),board1Offset,0,true);
        SpawnPiece(9,new Vector3(6,0,0),board1Offset,0,true);
        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnPiece(11,new Vector3(i,0,1),board1Offset,0,false);
        }
    }

    private ref Piece GetPiece(Vector3 position)
    {
        try
        {
            return ref Pieces[(int) position.x, (int) position.y, (int) position.z];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    private void SetPiece(Vector3 position, ref Piece piece)
    {
        Pieces[(int) position.x, (int) position.y, (int) position.z] = piece;
    }
    private void RemovePiece(Vector3 position)
    {
        Piece oldP = GetPiece(position);
        _activeChessPieces.Remove(oldP.gameObject); 
        Destroy(oldP.gameObject);
        Pieces[(int) position.x, (int) position.y, (int) position.z] = null;
    }
    
    private void MovePiece(Vector3 oldPosition,Vector3 newPosition,int board)
    {
        
        Piece oldP = GetPiece(newPosition);
        if (oldP != null)
        {
            RemovePiece(newPosition);
        }
        ref Piece newP = ref GetPiece(oldPosition);
        newP.hasMoved = true;
        
        Vector3 newPos = new Vector3(newPosition.x, 0, newPosition.z);
        switch (board)
        {
            case 0:
                newP.transform.position = newPos + board1Offset;
                break;
            case 1:
                newP.transform.position = newPos + board2Offset;
                break;
            case 2:
                newP.transform.position = newPos + board3Offset;
                break;
        }
        newP.SetPosition(new Vector3(newPosition.x,board,newPosition.z));
        SetPiece(new Vector3(newPosition.x,board,newPosition.z),ref newP);
        whiteTurn = !whiteTurn;
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
            mousePosition = FloorVector3(hit.point);
        }
        else
        {
            mousePosition = new Vector3(-99,-99,-99);
        }
        
        
    }

    private Vector3 FloorVector3(Vector3 oldVector)
    {
        Vector3 retval = new Vector3((float)Math.Floor(oldVector.x + 0.5), (float)(Math.Floor(oldVector.y+ 0.2) ), (float)Math.Floor(oldVector.z + 0.5));
        if (retval.y >= board1Offset.y && retval.y < board2Offset.y)
        {
            retval = retval - board1Offset;
        }
        else if (retval.y >= board2Offset.y && retval.y < board3Offset.y)
        {
            retval = retval - board2Offset + Vector3.up;
        }
        else if (retval.y >= board3Offset.y)
        {
            retval = retval - board3Offset + 2 * Vector3.up;
        }

        if (retval.x < 0 || retval.y < 0 || retval.z < 0 || retval.x > 7 || retval.y > 3 || retval.z > 7)
        {
            return new Vector3(-99,-99,-99);
        }
        return retval;
    }
    
    private void EndGame()
    {
        RemoveAllPieces();
        if (whiteTurn)
        {
            Debug.Log("White won!");
        }
        else
        {
            Debug.Log("Black won!");
        }
        

        whiteTurn = true;
        
        BoardHighlights.Instance.HideHighlights();
        SpawnAllPieces();
        
    }

    private void RemoveAllPieces()
    {
        
        foreach (GameObject go in _activeChessPieces)
        {
            Destroy(go);
        }
    }

    private int getPieceID(char p)
    {
        switch (p)
        {
            case 'k':
                return 0;
                
            case 'q':
                return 1;
            case 'b':
                return 2;
            case 'n':
                return 3;
            case 'r':
                return 4;
            case 'p':
                return 5;
            case 'K':
                return 6;
            case 'Q':
                return 7;
            case 'B':
                return 8;
            case 'N':
                return 9;
            case 'R':
                return 10;
            case 'P':
                return 11;
        }

        return -1;
    }

    private string getBoardState()
    {
        string boardState = "";
        int counter = 0;
        for (int y = 0; y < 3; y++)
        {
            for (int z = 0; z < 8; z++)
            {
                
                for (int x = 0; x < 8; x++)
                {
                    var currentPiece = GetPiece(new Vector3(x, y, z));
                    if (currentPiece == null)
                    {
                        
                        counter++;
                    }
                    else if(counter != 0)
                    {
                        boardState += (char) counter + currentPiece.GETPieceCode();
                        counter = 0;
                    }
                    else
                    {
                        boardState += currentPiece.GETPieceCode();
                    }
                }
                
                if (counter != 0)
                {
                    boardState += counter;
                    counter = 0;
                }
                boardState += '/';
            }

            boardState += '\n';
        }

        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == '\n')
            {
                boardState = boardState.Remove(i - 1, 1);
            }
        }
        //Hozzáadni kinek a köre és lehet e castleingolni vagy enpassanttolni
        return boardState;
    }

    //Returns true if boardState is invalid
    private bool invalidBoardState(string boardState)
    {
        int whiteKingCounter = 0;
        int blackKingCounter = 0;
        int x = 0;
        int y = 0;
        int z = 0;
        foreach (var currentChar in boardState)
        {
            if (Char.IsNumber(currentChar))
            {
                x += currentChar;
                if (x >= 8)
                {
                    return true;
                }
            }
            else if (currentChar.Equals('/'))
            {
                z ++;
                if (z >= 8)
                {
                    return true;
                }
            }
            else if (currentChar.Equals('\n'))
            {
                y ++;
                if (y >= 3)
                {
                    return true;
                }
            }
            else if (currentChar.Equals('k'))
            {
                blackKingCounter++;
                if (blackKingCounter >= 2)
                {
                    return true;
                }
            }
            else if (currentChar.Equals('K'))
            {
                whiteKingCounter++;
                if (whiteKingCounter >= 2)
                {
                    return true;
                }
            }
            //Folytatni kell
        }
        
        return false;
    }
    
    private void generateBoardFromBoardState(string boardState)
    {
        if (invalidBoardState(boardState))
        {
            return;
        }
        for (int i = 0; i < boardState.Length; i++)
        {
            //Leidézni a dolgokat a karakterekhez
        }
    }

}
