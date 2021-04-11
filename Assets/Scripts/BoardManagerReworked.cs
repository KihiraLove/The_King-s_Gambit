using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BoardManagerReworked : MonoBehaviour
{
    

    public Vector3 board1Offset;
    public Vector3 board2Offset;
    public Vector3 board3Offset;

    public bool whiteTurn;
    private string startBoardState = "RNBQKBNR/PPPPPPPP/8/8/8/8/8/8\n8/8/8/8/8/8/8/8\n8/8/8/8/8/8/pppppppp/rnbqkbnr\n b kq kq";
    
    public List<GameObject> chessPiecePrefabs;
    public GameObject chessBoardPrefab;
    private List<GameObject> _activeChessPieces;
    
    public Piece[,,] Pieces { set; get; }

    public Vector3 mousePosition;

    private Piece _selectedPiece;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        //GenerateBoardFromBoardState("RNBQKBNR/PPPPPPPP/8/8/8/8/8/8\n8/8/8/8/8/8/8/8\n8/8/8/8/8/8/pppppppp/rnbqkbnr\n w kq kq");
        //invalidBoardState(startBoardState);
        //print(getBoardState());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            print(mousePosition);
            if (mousePosition.x > -98)
            {
                
                if (_selectedPiece == null && GetPiece(mousePosition) != null && GetPiece(mousePosition).isWhite == whiteTurn )
                {
                    //Select piece
                    _selectedPiece = GetPiece(mousePosition);

                }
                else if(_selectedPiece != null && (GetPiece(mousePosition) == null || GetPiece(mousePosition).isWhite != whiteTurn))
                {
                    //Move;
                    MovePiece(_selectedPiece.position,new Vector3(mousePosition.x,mousePosition.y,mousePosition.z),(int)mousePosition.y);
                    _selectedPiece = null;
                    
                }
                else
                {
                    _selectedPiece = null;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateSelection();
        
    }

    private void Initialize()
    {
        SpawnBoards();
        SpawnAllPieces();
        
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

    private void SpawnPiece(int index, Vector3 position,int board)
    {
        Quaternion orientation;
        if (index <= 5)
        {
            orientation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            orientation = Quaternion.identity;
        }

        Vector3 offset = GETBoardOffSet(board);
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
        GenerateBoardFromBoardState(startBoardState);
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
            retval.y = 0;
        }
        else if (retval.y >= board2Offset.y && retval.y < board3Offset.y)
        {
            retval = retval - board2Offset + Vector3.up;
            retval.y = 1;
        }
        else if (retval.y >= board3Offset.y)
        {
            retval = retval - board3Offset + 2 * Vector3.up;
            retval.y = 2;
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

        Pieces = new Piece[8, 3, 8];
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

    private string GETBoardState()
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
        if (whiteTurn)
        {
            boardState += " w";
        }
        else
        {
            boardState += " b";
        }
        //Check castlings and save them
        boardState += " kq kq";
        return boardState;
    }

    private Vector3 GETBoardOffSet(int board)
    {
        Vector3 offset = Vector3.zero;
        switch (board)
        {
            case 0:
                offset = board1Offset;
                break;
            case 1:
                offset = board2Offset;
                break;
            case 2:
                offset = board3Offset;
                break;
        }

        return offset;
    }
    //Returns true if boardState is invalid
    private bool invalidBoardState(string boardState)
    {
        int whiteKingCounter = 0;
        int blackKingCounter = 0;
        int x = -1;
        int y = 0;
        int z = 0;
        int endPosition = 0;
        //"RNBQKBNR/PPPPPPPP/8/8/8/8/8/8\n8/8/8/8/8/8/8/8\n8/8/8/8/8/8/pppppppp/rnbqkbnr\n w kq kq"
        for (int i = 0; i < boardState.Length; i++)
        {
            if (Char.IsNumber(boardState[i]))
            {
                x += int.Parse(boardState[i].ToString());
                
            }
            else if (boardState[i] == '/')
            {
                z++;
                x = -1;
            }
            else if (boardState[i] == '\n')
            {
                y++;
                z = 0;
                x = -1;
            }
            else if(boardState[i] == ' ')
            {
                endPosition = i+1;
                break;
            }
            else
            {
                x++;
                //print(boardState[i] + " " + x.ToString() + " " + y.ToString() + " " + z.ToString());
                if (boardState[i] == 'k')
                {
                    blackKingCounter++;
                }
                else if (boardState[i] == 'K')
                {
                    whiteKingCounter++;
                }
            }

            if (x > 7 || y > 3 || z > 7)
            {
                print("Wrong coordinates");
                print(new Vector3(x, y, z));
                return true;
            }

            if (whiteKingCounter >= 2 || blackKingCounter >= 2)
            {
                print("Too much kings.");
                return true;
            }
            
            if (x >= 0)
            {
                //print(x.ToString() + " " + y.ToString() + " " + z.ToString());
            }
        }
        return false;
    }
    
    private void GenerateBoardFromBoardState(string boardState)
    {
        RemoveAllPieces();
        
        if (invalidBoardState(boardState))
        {
            print("Invalid boardState!");
            return;
        }
        
        int x = -1;
        int y = 0;
        int z = 0;
        int endPosition = 0;
        //"RNBQKBNR/PPPPPPPP/8/8/8/8/8/8\n8/8/8/8/8/8/8/8\n8/8/8/8/8/8/pppppppp/rnbqkbnr\n w kq kq"
        for (int i = 0; i < boardState.Length; i++)
        {
            if (Char.IsNumber(boardState[i]))
            {
                x += int.Parse(boardState[i].ToString());
                
            }
            else if (boardState[i] == '/')
            {
                z++;
                x = -1;
            }
            else if (boardState[i] == '\n')
            {
                y++;
                z = 0;
                x = -1;
            }
            else if(boardState[i] == ' ')
            {
                endPosition = i+1;
                break;
            }
            else
            {
                x++;
            }

            if (x >= 0 && !Char.IsNumber(boardState[i]))
            {
                //print(boardState[i] + " " + getPieceID(boardState[i]).ToString() + " " + new Vector3(x,y,z).ToString());
                SpawnPiece(getPieceID(boardState[i]),new Vector3(x,0,z),y);
                //print(x.ToString() + " " + y.ToString() + " " + z.ToString());
            }
        }
        
        print(endPosition);
        print(boardState);
        //endPosition;
        print(boardState[endPosition]);
        if (boardState[endPosition].Equals('w'))
        {
            whiteTurn = true;
        }
        else
        {
            whiteTurn = false;
        }
        //castling needs to be implemented
    }

}
