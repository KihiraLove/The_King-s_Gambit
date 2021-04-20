using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class BoardManagerReworked : MonoBehaviour
{
    public Vector3 board1Offset;
    public Vector3 board2Offset;
    public Vector3 board3Offset;

    public bool whiteTurn;

    public List<GameObject> chessPiecePrefabs;
    public GameObject chessBoardPrefab;

    public int roundNumber = 1;

    public Vector3 mousePosition;
    private List<GameObject> _activeChessPieces;

    private Piece _selectedPiece;

    public bool[,,] allowedMoves = new bool[8, 3, 8];

    private readonly string startBoardState =
        "R(0),N(0),B(0),Q(0),K(0),B(0),N(0),R(0)/P(0),P(0),P(0),P(0),P(0),P(0),P(0),P(0)/8/8/8/8/8/8\n8/8/8/8/8/8/8/8\n8/8/8/8/8/8/p(0),p(0),p(0),p(0),p(0),p(0),p(0),p(0),/r(0),n(0),b(0),q(0),k(0),b(0),n(0),r(0),\nw";

    public static BoardManagerReworked Instance { set; get; }
    public Piece[,,] Pieces { set; get; }

    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
        IsKingInCheck();
        //GenerateBoardFromBoardState(startBoardState);
        //invalidBoardState(startBoardState);
        //print(getBoardState());
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            //print(mousePosition);
            if (mousePosition.x > -98)
            {
                if (_selectedPiece == null && GetPiece(mousePosition) != null &&
                    GetPiece(mousePosition).isWhite == whiteTurn)
                {
                    //Select piece
                    SelectPiece(mousePosition);
                }
                else if (_selectedPiece != null &&
                         (GetPiece(mousePosition) == null || GetPiece(mousePosition).isWhite != whiteTurn))
                {
                    //Move;
                    if (allowedMoves[(int) mousePosition.x, (int) mousePosition.y, (int) mousePosition.z])
                    {
                        MovePiece(_selectedPiece.position,
                            new Vector3(mousePosition.x, mousePosition.y, mousePosition.z), (int) mousePosition.y);
                        _selectedPiece = null;
                        roundNumber++;
                    }
                }
                else
                {
                    _selectedPiece = null;
                }
            }
    }

    private void FixedUpdate()
    {
        UpdateSelection();
    }

    private void Initialize()
    {
        Instance = this;
        SpawnBoards();
        SpawnAllPieces();
        //Debug.Log(GETBoardState());
    }

    private void SelectPiece(Vector3 coords)
    {
        BoardHighlights.Instance.HideHighlights();

        //Check if it is a piece you clicking
        var selectedPiece = GetPiece(coords);
        if (selectedPiece == null)
        {
            _selectedPiece = null;
            return;
        }

        //Check if it is your piece
        if (selectedPiece.isWhite != whiteTurn)
        {
            _selectedPiece = null;
            return;
        }

        //Check if your selected piece have any move at all
        var hasMove = false;
        allowedMoves = selectedPiece.PossibleMove();
        for (var i = 0; i < 8; i++)
        {
            if (hasMove) break;

            for (var j = 0; j < 3; j++)
            {
                if (hasMove) break;

                for (var k = 0; k < 8; k++)
                    if (allowedMoves[i, j, k])
                    {
                        hasMove = true;
                        break;
                    }
            }
        }

        //If it has no move then you cant select it
        if (!hasMove) return;


        //Select the piece and show the allowed moves
        _selectedPiece = selectedPiece;
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves, board1Offset, board2Offset, board3Offset);
    }

    private void SpawnBoards()
    {
        var board1 = Instantiate(chessBoardPrefab, board1Offset, Quaternion.identity);
        board1.transform.SetParent(transform);
        var board2 = Instantiate(chessBoardPrefab, board2Offset, Quaternion.identity);
        board2.transform.SetParent(transform);
        var board3 = Instantiate(chessBoardPrefab, board3Offset, Quaternion.identity);
        board3.transform.SetParent(transform);

        board1.GetComponent<BoardData>().boardNumber = 1;
        board2.GetComponent<BoardData>().boardNumber = 2;
        board3.GetComponent<BoardData>().boardNumber = 3;
    }

    private void SpawnPiece(int index, Vector3 position, int board, int moveTurn)
    {
        Quaternion orientation;
        if (index <= 5)
            orientation = Quaternion.Euler(0, 180, 0);
        else
            orientation = Quaternion.identity;

        var offset = GETBoardOffSet(board);
        var piece = Instantiate(chessPiecePrefabs[index], position + offset, orientation);
        piece.transform.SetParent(transform);

        Pieces[(int) position.x, board, (int) position.z] = piece.GetComponent<Piece>();
        Pieces[(int) position.x, board, (int) position.z].roundMoved = moveTurn;
        Pieces[(int) position.x, board, (int) position.z].SetPosition(new Vector3(position.x, board, position.z));


        _activeChessPieces.Add(piece);
    }

    private void SpawnAllPieces()
    {
        _activeChessPieces = new List<GameObject>();
        Pieces = new Piece[8, 3, 8];
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
        var oldP = GetPiece(position);
        _activeChessPieces.Remove(oldP.gameObject);
        Destroy(oldP.gameObject);
        Pieces[(int) position.x, (int) position.y, (int) position.z] = null;
    }

    private void MovePiece(Vector3 oldPosition, Vector3 newPosition, int board)
    {
        var oldP = GetPiece(newPosition);
        if (oldP != null)
        {
            Debug.Log(oldP.GETPieceCode());
            if (oldP.GETPieceCode().Equals('K') || oldP.GETPieceCode().Equals('k')) EndGame();

            RemovePiece(newPosition);
        }

        ref var newP = ref GetPiece(oldPosition);
        newP.roundMoved = roundNumber;
        var newPos = new Vector3(newPosition.x, 0, newPosition.z);
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

        newP.SetPosition(new Vector3(newPosition.x, board, newPosition.z));
        SetPiece(new Vector3(newPosition.x, board, newPosition.z), ref newP);
        whiteTurn = !whiteTurn;
        BoardHighlights.Instance.HideHighlights();
    }

    //This function sets the coordinates of selection
    private void UpdateSelection()
    {
        if (!Camera.main) return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f,
            LayerMask.GetMask("ChessBoards")))
            mousePosition = FloorVector3(hit.point);
        else
            mousePosition = new Vector3(-99, -99, -99);
    }

    private Vector3 FloorVector3(Vector3 oldVector)
    {
        var retval = new Vector3((float) Math.Floor(oldVector.x + 0.5), (float) Math.Floor(oldVector.y + 0.2),
            (float) Math.Floor(oldVector.z + 0.5));
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
            return new Vector3(-99, -99, -99);

        return retval;
    }

    private void EndGame()
    {
        RemoveAllPieces();
        if (whiteTurn)
            Debug.Log("White won!");
        else
            Debug.Log("Black won!");


        whiteTurn = true;

        BoardHighlights.Instance.HideHighlights();
        SpawnAllPieces();
    }

    private void RemoveAllPieces()
    {
        foreach (var go in _activeChessPieces) Destroy(go);

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
        var boardState = "";
        var counter = 0;
        for (var y = 0; y < 3; y++)
        {
            for (var z = 0; z < 8; z++)
            {
                for (var x = 0; x < 8; x++)
                {
                    var currentPiece = GetPiece(new Vector3(x, y, z));
                    if (currentPiece == null)
                    {
                        counter++;
                        //Debug.Log(new Vector3(x,y,z).ToString());
                        //Debug.Log(boardState);
                    }
                    else if (counter != 0)
                    {
                        //Debug.Log(counter);
                        //Debug.Log(currentPiece);
                        boardState += counter.ToString() + ',' + currentPiece.GETPieceCode() + "(" +
                                      currentPiece.roundMoved + "),";
                        counter = 0;
                    }
                    else
                    {
                        boardState += currentPiece.GETPieceCode() + "(" + currentPiece.roundMoved + "),";
                    }
                }

                if (counter != 0)
                {
                    boardState += counter;
                    counter = 0;
                }

                boardState = boardState.TrimEnd(',');
                boardState += '/';
                counter = 0;
            }

            boardState += '\n';
        }

        for (var i = 0; i < boardState.Length; i++)
            if (boardState[i] == '\n')
                boardState = boardState.Remove(i - 1, 1);

        //Hozzáadni kinek a köre és lehet e castleingolni vagy enpassanttolni
        if (whiteTurn)
            boardState += " w";
        else
            boardState += " b";

        return boardState;
    }

    private Vector3 GETBoardOffSet(int board)
    {
        var offset = Vector3.zero;
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
        int x = 0, y = 0, z = 0;
        int blackKingCounter = 0, whiteKingCounter = 0;
        var roundCounter = 0;
        foreach (var i in boardState.Split('\n'))
        {
            foreach (var j in i.Split('/'))
            {
                if (z >= 8) return true;

                foreach (var k in j.Split(','))
                {
                    if (x >= 8) return true;

                    if (k.Length > 1)
                    {
                        if (k[0] == 'k')
                        {
                            blackKingCounter++;
                            if (blackKingCounter >= 2) return true;
                        }
                        else if (k[0] == 'K')
                        {
                            whiteKingCounter++;
                            if (whiteKingCounter >= 2) return true;
                        }

                        //Debug.Log(k + " spawned at " + new Vector3(x,y,z).ToString() + " moved at the round: " + Regex.Match(k, @"\d+").Value);
                        x++;
                    }
                    else if (k.Length >= 1 && char.IsDigit(k[0]))
                    {
                        x += int.Parse(k);
                    }
                    else if (k.Length >= 1 && k == "w" || k == "b")
                    {
                        roundCounter++;
                    }
                }

                x = 0;
                z++;
            }

            z = 0;
            y++;
        }
        //"RNBQKBNR/PPPPPPPP/8/8/8/8/8/8\n8/8/8/8/8/8/8/8\n8/8/8/8/8/8/pppppppp/rnbqkbnr\n w kq kq"

        return false;
    }

    private void GenerateBoardFromBoardState(string boardState)
    {
        RemoveAllPieces();
        if (invalidBoardState(boardState))
            Debug.Log("Bad board state.");
        //return;

        boardState =
            "R(0),N(0),B(0),Q(0),K(0),B(0),N(0),R(0)/P(0),P(0),P(0),P(0),P(0),P(0),P(0),P(0)/3,P(0),4/8/8/8/8/8\n8/8/8/8/8/8/8/8\n8/8/8/8/8/8/p(0),p(0),p(0),p(0),p(0),p(0),p(0),p(0),/r(0),n(0),b(0),q(0),k(0),b(0),n(0),r(0)\nw";
        int x = 0, y = 0, z = 0;

        foreach (var i in boardState.Split('\n'))
        {
            foreach (var j in i.Split('/'))
            {
                foreach (var k in j.Split(','))
                    if (k.Length > 1)
                    {
                        //Debug.Log(k + " spawned at " + new Vector3(x,y,z).ToString() + " moved at the round: " + Regex.Match(k, @"\d+").Value);
                        SpawnPiece(getPieceID(k[0]), new Vector3(x, 0, z), y, int.Parse(Regex.Match(k, @"\d+").Value));
                        x++;
                    }
                    else if (k.Length >= 1 && char.IsDigit(k[0]))
                    {
                        x += int.Parse(k);
                    }
                    else if (k.Length >= 1 && k == "w" || k == "b")
                    {
                        whiteTurn = k.Equals("w");

                        //.Log(k[0] + " turn");
                    }

                x = 0;
                z++;
            }

            z = 0;
            y++;
        }

        /*
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
        //castling needs to be implemented*/
    }

    private Piece GETKing()
    {
        foreach (var i in Pieces)
            if (i != null && i.isWhite == whiteTurn && (i.GETPieceCode().Equals('k') || i.GETPieceCode().Equals('K')))
                return i;

        return null;
    }

    private bool IsKingInCheck()
    {
        //Ciklus ami kell és akkor végigmegy mondjuk 1 től x ig és mindegyiknél csak megnézi a megfelelőket
        var king = GETKing();
        //Diagonal loop
        int oX, oY, oZ;
        oX = (int) king.position.x;
        oY = (int) king.position.y;
        oZ = (int) king.position.z;
        int x, y, z;
        /*for (int i = 1; i <= 7; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k += 2)
                {
                    for (int l = -1; l <= 1; l += 2)
                    {
                        x = oX - (k * i);
                        y = oY - j;
                        z = oZ - (l * i);
                        if ((x >= 0 && x < 8) && (z >= 0 && z < 8) && )
                        {
                            Debug.Log(new Vector3(x,y,z));
                        }
                    }
                }
            }
        }*/
        /*
        for (int yLoop = -2; yLoop <= 2; yLoop++)
        {
            if (oY - yLoop < 0 || oY - yLoop > 2)
            {
                continue;
            }
            else
            {
                Debug.Log(oY - yLoop);
            }
        }*/

        //Look on the lower and upper boards
        oY = 2;
        /*
        for (var i = -2; i <= 2; i++)
        {
            if (oY + i < 0 || oY + i > 2)
            {
                
                continue;
            }
            
            //Same board;
            if (i == 0)
                for (var j = 1; j <= 7; j++)
                {
                    for (var k = -j; k <= j; k += 2 * j)
                    {
                        for (var l = -j; l <= j; l += 2 * j)
                        {
                            x = k;
                            z = l;
                            if (x >= 0  && z >= 0 )
                            {
                                Debug.Log(new Vector3(x,oY,z));
                            }
                        }
                    }
                    
                }
                
            else
                for (var k = -Math.Abs(i); k <= Math.Abs(i); k += 2 * Math.Abs(i))
                {
                    for (var l = -Math.Abs(i); l <= Math.Abs(i); l += 2 * Math.Abs(i))
                    {
                        x = k;
                        z = l;
                        if (x >= 0  && z >= 0 )
                        {
                            Debug.Log(new Vector3(x, oY + i, z));
                        }
                    }
                }
        }
*/

        //Horizontal loop
        
        for (var i = -2; i <= 2; i++)
        {
            if (oY + i < 0 || oY + i > 2)
            {
                
                continue;
            }
            
            //Same board;
            if (i == 0)
                for (var j = 1; j <= 7; j++)
                {
                    for (var k = -j; k <= j; k += j)
                    {
                        for (var l = -j; l <= j; l += j)
                        {
                            x = k;
                            z = l;
                            if (x >= 0  && z >= 0 && (x != z || x == 0 && z == 0))
                            {
                                Debug.Log(new Vector3(x,oY,z));
                            }
                        }
                    }
                    
                }
                
            else
                for (var k = -Math.Abs(i); k <= Math.Abs(i); k += Math.Abs(i))
                {
                    for (var l = -Math.Abs(i); l <= Math.Abs(i); l += Math.Abs(i))
                    {
                        x = k;
                        z = l;
                        if (x >= 0  && z >= 0 && (x != z || x == 0 && z == 0))
                        {
                            Debug.Log(new Vector3(x, oY + i, z));
                        }
                    }
                }
        }
        


        //Horse loop needs to be implemented

        for (var i = 0; i < 9; i++)
        {
        }


        return true;
    }

    /*private List<Vector3> getChordsInDistance(Vector3 coord, int distance)
    {
        List<Vector3> retval = new List<Vector3>();
        int x = (int)coord.x,z = (int)coord.z;
        
        
        for(int i = 0;)
        
        for (int i = -1 * distance; i <= distance; i++)
        {
            for (int j = -1 * distance; j <= distance; j++)
            {
                if (Math.Abs(i) == distance || Math.Abs(j) == distance)
                {

                    if ((x + j >= 0 && x + j < 8) && (z + i >= 0 && z + i < 8))
                    {
                        retval.Add(new Vector3(coord.x+j,0,coord.z+i));  
                    }
                    
                }
                
            }

        }

        /*
        foreach (var i in retval)
        {
            Debug.Log(i);
        }
 

        return retval;
    }*/
}