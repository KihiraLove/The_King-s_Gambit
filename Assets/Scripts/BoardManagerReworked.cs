using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class BoardManagerReworked : MonoBehaviour
{
    Animator anim;

    public Animator animTeleport;
    public Animator animDisappear;

    public float timeOfDisappear;

    public Vector3 delayedSelectedPiecePosition;
    public Vector3 delayedPosition;
    public int delayedMousePositionY;
    public bool delayScheduled = false;

    public Vector3 board1Offset;
    public Vector3 board2Offset;
    public Vector3 board3Offset;

    private GameObject _board1;
    private GameObject _board2;
    private GameObject _board3;
    
    public bool whiteTurn;
    private bool _isFocused = false;

    public List<GameObject> chessPiecePrefabs;
    public GameObject chessBoardPrefab;

    public int roundNumber = 1;

    public Vector3 mousePosition;
    private List<GameObject> _activeChessPieces;

    private Piece _selectedPiece;

    public bool[,,] allowedMoves = new bool[8, 3, 8];

    private string startBoardState =
        "R(0),N(0),B(0),Q(0),K(0),B(0),N(0),R(0)/P(0),P(0),P(0),P(0),P(0),P(0),P(0),P(0)/8/8/8/8/8/8\n8/8/8/8/8/8/8/8\n8/8/8/8/8/8/p(0),p(0),p(0),p(0),p(0),p(0),p(0),p(0),/r(0),n(0),b(0),q(0),k(0),b(0),n(0),r(0)\nw";

    public static BoardManagerReworked Instance { set; get; }
    public Piece[,,] Pieces { set; get; }

    // Start is called before the first frame update
    private void Start()
    {
        //startBoardState = "R(0),3,K(0),2,R(0)/P(0),P(0),P(0),P(0),P(0),P(0),P(0),P(0)/8/8/8/8/8/8\n8/8/8/8/8/8/8/8\n8/8/8/8/8/8/p(0),p(0),p(0),p(0),p(0),p(0),p(0),p(0),/r(0),3,k(0),2,r(0)\nb";
        allowedMoves = new bool[8, 3, 8];
        Initialize();
        PlayerPrefs.SetString("boardState", startBoardState);

        //GenerateBoardFromBoardState(startBoardState);
        //invalidBoardState(startBoardState);
        //print(getBoardState());

    }

    // Update is called once per frame
    private void Update()
    {

        Debug.Log("delayScheduled: " + delayScheduled.ToString());
        if (delayScheduled)
        {
            Debug.Log("Time.time - timeOfDisappear: " + (Time.time - timeOfDisappear).ToString());
        }
        if (Time.time - timeOfDisappear >= 1 && delayScheduled)
        {
            MovePiece(delayedSelectedPiecePosition, delayedPosition, delayedMousePositionY);
            _selectedPiece = null;
            roundNumber++;
            BoardHighlights.Instance.HideHighlights();

            Show();

            delayScheduled = false;
        }
        
        if (Input.GetMouseButtonDown(0)
            && !Input.GetMouseButton(1)
            && !Input.GetMouseButton(2)
            && Input.mouseScrollDelta.y == 0
            && _isFocused == false)
        {

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
                    if (allowedMoves[(int) mousePosition.x, (int) mousePosition.y, (int) mousePosition.z] && delayScheduled == false)
                    {
                        animTeleport = _selectedPiece.GetComponent<Animator>();
                        Hide();

                        SetDelayedMove(_selectedPiece.position, new Vector3(mousePosition.x, mousePosition.y, mousePosition.z), (int)mousePosition.y);
                    }
                }
                else
                {
                    _selectedPiece = null;
                    BoardHighlights.Instance.HideHighlights();
                }
            }
        }
    }

    

    private void SetDelayedMove(Vector3 _delayedSelectedPiecePosition, Vector3 _delayedPosition, int _delayedMousePositionY)
    {
        delayedSelectedPiecePosition = _delayedSelectedPiecePosition;
        delayedPosition = _delayedPosition;
        delayedMousePositionY = _delayedMousePositionY;

        delayScheduled = true;
    }


    private void FixedUpdate()
    {
        UpdateSelection();
    }

    public void FocusOnBoard(int boardNum)
    {
        if (!_isFocused)
        {
            String currentState = GETBoardState();
            String[] boards = currentState.Split('\n');

            switch (boardNum)
            {
                case 0:
                    SetPieceOpacityOnBoard(boards[1], 1, false);
                    SetBoardOpacityToHalf(1);
                    SetPieceOpacityOnBoard(boards[2], 2, false);
                    SetBoardOpacityToHalf(2);
                    break;
                case 1:
                    SetPieceOpacityOnBoard(boards[0], 0, false);
                    SetBoardOpacityToHalf(0);
                    SetPieceOpacityOnBoard(boards[2], 2, false);
                    SetBoardOpacityToHalf(2);
                    break;
                case 2:
                    SetPieceOpacityOnBoard(boards[0], 0, false);
                    SetBoardOpacityToHalf(0);
                    SetPieceOpacityOnBoard(boards[1], 1, false);
                    SetBoardOpacityToHalf(1);
                    break;
                default:
                    Debug.Log("Board Focus switch statement broke!");
                    break;
            }
            _isFocused = true;
        }
    }

    private void SetPieceOpacityOnBoard(String boardState, int boardNum, bool isReset)
    {
        int rowCount = 0;
        foreach (String rowState in boardState.Split('/'))
        {
            SetPieceOpacityOnRow(rowState, rowCount, boardNum, isReset);
            rowCount++;
        }
    }

    private void SetPieceOpacityOnRow(String rowState, int rowCount, int boardNum, bool isReset)
    {
        int squareCount = 0;
        foreach (String squareState in rowState.Split(','))
        {
            if (squareState.Length > 1)
            {
                if (!isReset)
                {
                    SetPieceOpacityToHalf(squareCount, rowCount, boardNum);
                    
                }
                else
                {
                    ResetPieceOpacity(squareCount, rowCount, boardNum);
                }
                squareCount++;
            }
            else if (squareState.Length >= 1 && char.IsDigit(squareState[0]))
            {
                squareCount += int.Parse(squareState);
            }
            else
            {
                Debug.Log("SetPieceOpacityOnRow() broke!");
            }
        }
    }

    private void SetPieceOpacityToHalf(int squareCount, int rowCount, int boardNum)
    {
        try
        {
            Shader spritesDefault = Shader.Find("Sprites/Default");
            var piece = GetPiece(new Vector3(squareCount, boardNum, rowCount));
            Color old = piece.GetComponent<Renderer>().material.color;
            piece.GetComponent<Renderer>().material.shader = spritesDefault;
            piece.GetComponent<Renderer>().material.SetColor("_Color", new Color(old.r, old.g, old.b, 0.3f));

        }
        catch (Exception e)
        {
            Debug.Log("Error at coordinate : " + e);
        }
    }

    private void ResetPieceOpacity(int squareCount, int rowCount, int boardNum)
    {
        try
        {
            Shader spritesStandard = Shader.Find("Standard");
            var piece = GetPiece(new Vector3(squareCount, boardNum, rowCount));
            Color old = piece.GetComponent<Renderer>().material.color;
            piece.GetComponent<Renderer>().material.shader = spritesStandard;
            piece.GetComponent<Renderer>().material.SetColor("_Color", new Color(old.r, old.g, old.b, 1f));

        }
        catch (Exception e)
        {
            Debug.Log("Error at coordinate : " + e);
        }
    }

    private void SetBoardOpacityToHalf(int boardNum)
    {
        Shader spritesDefault = Shader.Find("Sprites/Default");
        switch (boardNum)
        {
            case 0:
                Color oldDark0 = _board1.GetComponent<Renderer>().materials[0].color;
                _board1.GetComponent<Renderer>().materials[0].shader = spritesDefault;
                _board1.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(oldDark0.r, oldDark0.g, oldDark0.b, 0.3f));
                
                Color oldLight0 = _board1.GetComponent<Renderer>().materials[1].color;
                _board1.GetComponent<Renderer>().materials[1].shader = spritesDefault;
                _board1.GetComponent<Renderer>().materials[1].SetColor("_Color", new Color(oldLight0.r, oldLight0.g, oldLight0.b, 0.3f));
                break;
            case 1:
                Color oldDark1 = _board2.GetComponent<Renderer>().materials[0].color;
                _board2.GetComponent<Renderer>().materials[0].shader = spritesDefault;
                _board2.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(oldDark1.r, oldDark1.g, oldDark1.b, 0.3f));
                
                Color oldLight1 = _board2.GetComponent<Renderer>().materials[1].color;
                _board2.GetComponent<Renderer>().materials[1].shader = spritesDefault;
                _board2.GetComponent<Renderer>().materials[1].SetColor("_Color", new Color(oldLight1.r, oldLight1.g, oldLight1.b, 0.3f));
                break;
            case 2:
                Color oldDark2 = _board3.GetComponent<Renderer>().materials[0].color;
                _board3.GetComponent<Renderer>().materials[0].shader = spritesDefault;
                _board3.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(oldDark2.r, oldDark2.g, oldDark2.b, 0.3f));
                
                Color oldLight2 = _board3.GetComponent<Renderer>().materials[1].color;
                _board3.GetComponent<Renderer>().materials[1].shader = spritesDefault;
                _board3.GetComponent<Renderer>().materials[1].SetColor("_Color", new Color(oldLight2.r, oldLight2.g, oldLight2.b, 0.3f));
                break;
        }
    }

    private void ResetBoardOpacity()
    {
        Shader spritesDefault = Shader.Find("Standard");
        
        Color oldBoardDark1 = _board1.GetComponent<Renderer>().materials[0].color;
        _board1.GetComponent<Renderer>().materials[0].shader = spritesDefault;
        _board1.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(oldBoardDark1.r, oldBoardDark1.g, oldBoardDark1.b, 1f));
        
        Color oldBoardLight1 = _board1.GetComponent<Renderer>().materials[1].color;
        _board1.GetComponent<Renderer>().materials[1].shader = spritesDefault;
        _board1.GetComponent<Renderer>().materials[1].SetColor("_Color", new Color(oldBoardLight1.r, oldBoardLight1.g, oldBoardLight1.b, 1f));
        
        Color oldBoardDark2 = _board2.GetComponent<Renderer>().materials[0].color;
        _board2.GetComponent<Renderer>().materials[0].shader = spritesDefault;
        _board2.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(oldBoardDark2.r, oldBoardDark2.g, oldBoardDark2.b, 1f));
        
        Color oldBoardLight2 = _board2.GetComponent<Renderer>().materials[1].color;
        _board2.GetComponent<Renderer>().materials[1].shader = spritesDefault;
        _board2.GetComponent<Renderer>().materials[1].SetColor("_Color", new Color(oldBoardLight2.r, oldBoardLight2.g, oldBoardLight2.b, 1f));
        
        Color oldBoardDark3 = _board3.GetComponent<Renderer>().materials[0].color;
        _board3.GetComponent<Renderer>().materials[0].shader = spritesDefault;
        _board3.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(oldBoardDark3.r, oldBoardDark3.g, oldBoardDark3.b, 1f));
        
        Color oldBoardLight3 = _board3.GetComponent<Renderer>().materials[1].color;
        _board3.GetComponent<Renderer>().materials[1].shader = spritesDefault;
        _board3.GetComponent<Renderer>().materials[1].SetColor("_Color", new Color(oldBoardLight3.r, oldBoardLight3.g, oldBoardLight3.b, 1f));
        
    }

    public void ResetBoardFocus()
    {
        String currentState = GETBoardState();
        String [] boards = currentState.Split('\n');
        SetPieceOpacityOnBoard(boards[0], 0, true);
        SetPieceOpacityOnBoard(boards[1], 1, true);
        SetPieceOpacityOnBoard(boards[2], 2, true);
        ResetBoardOpacity();
        _isFocused = false;
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
        allowedMoves = selectedPiece.PossibleMove(Pieces);
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
        _board1 = Instantiate(chessBoardPrefab, board1Offset, Quaternion.identity);
        _board1.transform.SetParent(transform);
        _board2 = Instantiate(chessBoardPrefab, board2Offset, Quaternion.identity);
        _board2.transform.SetParent(transform);
        _board3 = Instantiate(chessBoardPrefab, board3Offset, Quaternion.identity);
        _board3.transform.SetParent(transform);

        _board1.GetComponent<BoardData>().boardNumber = 1;
        _board2.GetComponent<BoardData>().boardNumber = 2;
        _board3.GetComponent<BoardData>().boardNumber = 3;
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
            throw new Exception(position.ToString());
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
        var PiecesCopy = (Piece[,,])Pieces.Clone();

        var oldP = GetPiece(newPosition);
        if (oldP != null)
        {
            //Debug.Log(oldP.GETPieceCode());
            if (oldP.GETPieceCode().Equals('K') || oldP.GETPieceCode().Equals('k'))
            {
                EndGame();
                return;
            }
            else
            {
                RemovePiece(newPosition);
            }
        }

        ref var newP = ref GetPiece(oldPosition);

        

             

        newP.roundMoved = roundNumber;
        if (char.ToUpper(newP.GETPieceCode()).Equals('P'))
        {
            if (oldPosition.x.Equals(newPosition.x) && oldPosition.y.Equals(newPosition.y) &&
                Math.Abs(oldPosition.z - newPosition.z).Equals(2))
            {
                ((Pawn) newP).doubleMoved = true;
            }

            EnPassantMove(oldPosition, newPosition);
        }

        if (char.ToUpper(newP.GETPieceCode()).Equals('K'))
        {
            CastleMove(oldPosition, newPosition);
        }

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

            //newP.roundMoved = roundNumber;


            newP.SetPosition(new Vector3(newPosition.x, board, newPosition.z));
            SetPiece(new Vector3(newPosition.x, board, newPosition.z), ref newP);

            Pieces[(int)oldPosition.x, (int)oldPosition.y, (int)oldPosition.z] = null;
            whiteTurn = !whiteTurn;
            BoardHighlights.Instance.HideHighlights();
        
    }


    private void Hide() 
    {
        StartCoroutine(HideCoroutine());
    }

    IEnumerator HideCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started DisappearCoroutine at timestamp : " + Time.time);

        animTeleport.SetTrigger("Hide");
        timeOfDisappear = Time.time;
        Debug.Log("timeOfDisappear: " + timeOfDisappear.ToString());
        Debug.Log("Hide");

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished DisappearCoroutine at timestamp : " + Time.time);
    }

    

    private void Show()
    {
        StartCoroutine(ShowCoroutine());
    }

    IEnumerator ShowCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started ShowCoroutine at timestamp : " + Time.time);


        animTeleport.SetTrigger("Show");
        Debug.Log("Show");
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished ReappearCoroutine at timestamp : " + Time.time);
    }


    private void CastleMove(Vector3 kingPos,Vector3 newPos)
    {
        
        if (newPos == kingPos + Vector3.left*2)
        {
            //Debug.Log("kingPos");
            ref var newPiece = ref GetPiece(new Vector3(0, (int) kingPos.y, (int) kingPos.z));
            newPiece.roundMoved = roundNumber;
            newPiece.transform.position = new Vector3(kingPos.x,0,kingPos.z) + Vector3.left + GETBoardOffSet((int)kingPos.y);
            newPiece.SetPosition(kingPos + Vector3.left);
            SetPiece(kingPos + Vector3.left, ref newPiece);
            Pieces[0, (int) kingPos.y, (int) kingPos.z] = null;
        }
        else if (newPos == kingPos + Vector3.right*2)
        {
//            Debug.Log("kingPos");
            ref var newPiece = ref GetPiece(new Vector3(7, (int) kingPos.y, (int) kingPos.z));
            newPiece.roundMoved = roundNumber;
            newPiece.transform.position = new Vector3(kingPos.x,0,kingPos.z) + Vector3.right + GETBoardOffSet((int)kingPos.y);
            newPiece.SetPosition(kingPos + Vector3.right);
            SetPiece(kingPos + Vector3.right, ref newPiece);
            Pieces[7, (int) kingPos.y, (int) kingPos.z] = null;
        }
    }
    private void EnPassantMove(Vector3 oldPosition, Vector3 newPosition)
    {
        int forward;
        int oX, oY, oZ;
        oX = (int) oldPosition.x;
        oY = (int) oldPosition.y;
        oZ = (int) oldPosition.z;
        //int x, y, z;
        int nX, nY, nZ;
        nX = (int) newPosition.x;
        nY = (int) newPosition.y;
        nZ = (int) newPosition.z;
        Piece oldPiece = Pieces[oX, oY, oZ];
        Piece newPositionPiece = Pieces[nX, nY, nZ];
        //Set forward based on black white
        if (oldPiece != null)
        {
            if (oldPiece.isWhite)
            {
                forward = 1;
            }
            else
            {
                forward = -1;
            }
        }

        if (newPositionPiece != null)
        {
            return;
        }


        Piece sidePawn;
        if (nX > oX)
        {
            //Jobbra mozogtunk
            sidePawn = Pieces[oX + 1, oY, oZ];
            if (sidePawn == null)
            {
                return;
            }

            if (!char.ToUpper(sidePawn.GETPieceCode()).Equals('P'))
            {
                return;
            }

            if (((Pawn) sidePawn).doubleMoved == true && sidePawn.roundMoved == roundNumber - 1 &&
                oldPiece.isWhite != sidePawn.isWhite)
            {
                RemovePiece(sidePawn.position);
                Pieces[oX + 1, oY, oZ] = null;
            }
        }
        else if (nX < oX)
        {
            //Balra mozogtunk
            Debug.Log("asd");
            sidePawn = Pieces[oX - 1, oY, oZ];
            if (sidePawn == null)
            {
                return;
            }

            if (!char.ToUpper(sidePawn.GETPieceCode()).Equals('P'))
            {
                return;
            }

            if (((Pawn) sidePawn).doubleMoved && sidePawn.roundMoved == roundNumber - 1 &&
                oldPiece.isWhite != sidePawn.isWhite)
            {
                RemovePiece(sidePawn.position);
                Pieces[oX - 1, oY, oZ] = null;
            }
        }

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
        BoardHighlights.Instance.HideHighlights();
        RemoveAllPieces();
        if (whiteTurn)
            Debug.Log("White won!");
        else
            Debug.Log("Black won!");

        whiteTurn = true;


        SpawnAllPieces();
    }

    private void RemoveAllPieces()
    {
        foreach (var go in _activeChessPieces) Destroy(go);

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

        //return;

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

    private Piece GETKing(Piece[,,] positions)
    {
        foreach (var i in positions)
            if (i != null && i.isWhite == whiteTurn && (i.GETPieceCode().Equals('k') || i.GETPieceCode().Equals('K')))
                return i;

        return null;
    }

    public bool IsKingInCheck(Piece[,,] positions, Vector3 coords)
    {
        //Ciklus ami kell és akkor végigmegy mondjuk 1 től x ig és mindegyiknél csak megnézi a megfelelőket
        var king = GETKing(positions);
        if (coords == Vector3.negativeInfinity)
        {
            coords = king.position;
        }

        //Diagonal loop
        int oX, oY, oZ;
        oX = (int) coords.x;
        oY = (int) coords.y;
        oZ = (int) coords.z;
        int x, y, z;
        /*
        for (int i = 1; i <= 7; i++)
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
                        if ((x >= 0 && x < 8) && (z >= 0 && z < 8))
                        {
                            //Debug.Log(new Vector3(x,y,z));
                            Lista1.Add(new Vector3(x,y,z));
                        }
                    }
                }
            }
        }
        
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
                                if (IsValidCoordinate(new Vector3(x,oY,z)))
                                {
                                    Lista1.Add(new Vector3(x,oY,z));
                                }
                                //Debug.Log(new Vector3(x,oY,z));
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
                            if (IsValidCoordinate(new Vector3(x, oY + i, z)))
                            {
                                Lista1.Add(new Vector3(x, oY + i, z));
                            }
                            //Debug.Log(new Vector3(x, oY + i, z));
                        }
                    }
                }
        }*/

        //Horizontal loop
        for (int k = -2; k <= 2; k++)
        {
            if (oY + k < 0 || oY + k > 2)
            {
                continue;
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (k != 0 && i < Math.Abs(k))
                    {
                        continue;
                    }
                    else if (k != 0 && i != Math.Abs(k))
                    {
                        break;
                    }

                    int xMove = -5;
                    int zMove = -5;
                    switch (j)
                    {
                        case 0:
                            xMove = 0;
                            zMove = -1;
                            break;
                        case 1:
                            xMove = 0;
                            zMove = 1;
                            break;
                        case 2:
                            xMove = -1;
                            zMove = 0;
                            break;
                        case 3:
                            xMove = 1;
                            zMove = 0;
                            break;
                    }

                    x = oX + i * xMove;
                    y = oY + k;
                    z = oZ + i * zMove;

                    if (IsValidCoordinate(new Vector3(x, y, z)))
                    {
                        if (positions[x, y, z] == null)
                        {
                            continue;
                        }
                        else if (positions[x, y, z].isWhite == king.isWhite)
                        {
                            break;
                        }
                        else if (positions[x, y, z].isWhite != king.isWhite)
                        {
                            if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('R'))
                            {
                                return true;
                            }
                            else
                            {
                                break;
                            }
                        }

                    }
                    else
                    {
                        break;
                    }

                }
            }
        }

        //Down horizontal
        for (int i = -2; i < 0; i++)
        {
            x = oX;
            y = oY + i;
            z = oZ;
            if (IsValidCoordinate(new Vector3(x, y, z)))
            {
                if (positions[x, y, z] == null)
                {
                    continue;
                }
                else if (positions[x, y, z].isWhite == king.isWhite)
                {
                    break;
                }
                else if (positions[x, y, z].isWhite != king.isWhite)
                {
                    if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('R') ||
                        char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('Q'))
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        //Up horizontal
        for (int i = -2; i < 0; i++)
        {
            x = oX;
            y = oY - i;
            z = oZ;
            if (IsValidCoordinate(new Vector3(x, y, z)))
            {
                if (positions[x, y, z] == null)
                {
                    continue;
                }
                else if (positions[x, y, z].isWhite == king.isWhite)
                {
                    break;
                }
                else if (positions[x, y, z].isWhite != king.isWhite)
                {
                    if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('R') ||
                        char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('Q'))
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        //Vertical loop

        for (int k = -2; k <= 2; k++)
        {
            if (oY + k < 0 || oY + k > 2)
            {
                continue;
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (k != 0 && i < Math.Abs(k))
                    {
                        continue;
                    }
                    else if (k != 0 && i != Math.Abs(k))
                    {
                        break;
                    }

                    int xMove = 1;
                    int zMove = 1;
                    switch (j)
                    {
                        case 0:
                            xMove *= -1;
                            zMove *= -1;
                            break;
                        case 1:
                            xMove *= 1;
                            zMove *= -1;
                            break;
                        case 2:
                            xMove *= -1;
                            zMove *= 1;
                            break;
                        case 3:
                            xMove *= 1;
                            zMove *= 1;
                            break;
                    }

                    x = oX + i * xMove;
                    y = oY + k;
                    z = oZ + i * zMove;

                    if (IsValidCoordinate(new Vector3(x, y, z)))
                    {
                        if (positions[x, y, z] == null)
                        {
                            //Debug.Log(new Vector3(x, y, z));
                            continue;
                        }
                        else if (positions[x, y, z].isWhite == king.isWhite)
                        {
                            break;
                        }
                        else if (positions[x, y, z].isWhite != king.isWhite)
                        {
                            if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('B') ||
                                char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('Q'))
                            {
                                return true;
                            }
                            else
                            {
                                break;
                            }
                        }

                    }
                    else
                    {
                        break;
                    }

                }
            }
        }

        //Horse loop

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Math.Abs(i) != Math.Abs(j))
                {
                    for (int k = 1; k <= 2; k++)
                    {
                        if (k == 1)
                        {

                            if (IsValidCoordinate(new Vector3(oX + i, oY + 2, oZ + j)))
                            {
                                x = oX + i;
                                y = oY + 2;
                                z = oZ + j;
                                if (positions[x, y, z] != null && positions[x, y, z].isWhite != king.isWhite)
                                {
                                    if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('N'))
                                    {
                                        return true;
                                    }
                                }

                                //Debug.Log(new Vector3(oX+i, oY + 2, oZ+j));
                                //allowedMoves[oX+i, oY + 2, oZ+j] = true;
                            }

                            if (IsValidCoordinate(new Vector3(oX + i, oY - 2, oZ + j)))
                            {
                                x = oX + i;
                                y = oY - 2;
                                z = oZ + j;
                                if (positions[x, y, z] != null && positions[x, y, z].isWhite != king.isWhite)
                                {
                                    if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('N'))
                                    {
                                        return true;
                                    }
                                }

                                //Debug.Log(new Vector3(oX+i, oY - 2, oZ+j));
                                //allowedMoves[oX+i, oY - 2, oZ+j] = true;
                            }
                        }
                        else
                        {
                            if (i != 0)
                            {
                                if (IsValidCoordinate(new Vector3(oX + 2 * i, oY, oZ + 1)))
                                {
                                    x = oX + 2 * i;
                                    y = oY;
                                    z = oZ + 1;
                                    if (positions[x, y, z] != null && positions[x, y, z].isWhite != king.isWhite)
                                    {
                                        if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('N'))
                                        {
                                            return true;
                                        }
                                    }

                                    //Debug.Log(new Vector3(oX+2*i, oY, oZ+1));
                                    //allowedMoves[oX+2*i, oY, oZ+1] = true;
                                }

                                if (IsValidCoordinate(new Vector3(oX + 2 * i, oY, oZ - 1)))
                                {
                                    x = oX + 2 * i;
                                    y = oY;
                                    z = oZ - 1;
                                    if (positions[x, y, z] != null && positions[x, y, z].isWhite != king.isWhite)
                                    {
                                        if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('N'))
                                        {
                                            return true;
                                        }
                                    }

                                    //Debug.Log(new Vector3(oX+2*i, oY, oZ-1));
                                    //allowedMoves[oX+2*i, oY, oZ-1] = true;
                                }

                            }
                            else
                            {
                                if (IsValidCoordinate(new Vector3(oX + 1, oY, oZ + 2 * j)))
                                {
                                    x = oX + 1;
                                    y = oY;
                                    z = oZ + 2 * j;
                                    if (positions[x, y, z] != null && positions[x, y, z].isWhite != king.isWhite)
                                    {
                                        if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('N'))
                                        {
                                            return true;
                                        }
                                    }

                                    //Debug.Log(new Vector3(oX+1, oY, oZ+2*j));
                                    //allowedMoves[oX+1, oY, oZ+2*j] = true;
                                }

                                if (IsValidCoordinate(new Vector3(oX - 1, oY, oZ + 2 * j)))
                                {
                                    x = oX - 1;
                                    y = oY;
                                    z = oZ + 2 * j;
                                    if (positions[x, y, z] != null && positions[x, y, z].isWhite != king.isWhite)
                                    {
                                        if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('N'))
                                        {
                                            return true;
                                        }
                                    }

                                    //Debug.Log(new Vector3(oX-1, oY, oZ+2*j));
                                    //allowedMoves[oX-1, oY, oZ+2*j] = true;
                                }
                            }

                            if (IsValidCoordinate(new Vector3(oX + 2 * i, oY + 1, oZ + 2 * j)))
                            {
                                x = oX + 2 * i;
                                y = oY + 1;
                                z = oZ + 2 * j;
                                if (positions[x, y, z] != null && positions[x, y, z].isWhite != king.isWhite)
                                {
                                    if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('N'))
                                    {
                                        return true;
                                    }
                                }

                                //Debug.Log(new Vector3(oX+2*i, oY + 1, oZ+2*j));
                                //allowedMoves[oX+2*i, oY + 1, oZ+2*j] = true;
                            }

                            if (IsValidCoordinate(new Vector3(oX + 2 * i, oY - 1, oZ + 2 * j)))
                            {
                                x = oX + 2 * i;
                                y = oY - 1;
                                z = oZ + 2 * j;
                                if (positions[x, y, z] != null && positions[x, y, z].isWhite != king.isWhite)
                                {
                                    if (char.ToUpper(positions[x, y, z].GETPieceCode()).Equals('N'))
                                    {
                                        return true;
                                    }
                                }

                                //Debug.Log(new Vector3(oX+2*i, oY - 1, oZ+2*j));
                                //allowedMoves[oX+2*i, oY - 1, oZ+2*j] = true;
                            }
                        }
                    }
                }
            }
        }

        //Pawn loop
        //Check if we have to look forward or behind based on the king
        //Look forward

        if (king.isWhite)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    z = oZ + 1;
                    y = oY + i;
                    x = oX + j;

                    if (IsValidCoordinate(new Vector3(x, y, z)) && positions[x, y, z] != null &&
                        positions[x, y, z].GETPieceCode() == 'p')
                    {
                        return true;
                    }
                }
            }
        }
        //Look backward
        else
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; i <= 1; i += 2)
                {
                    z = oZ - 1;
                    y = oY + i;
                    x = oX + j;
                    Debug.Log(new Vector3(x, y, z));
                    if (IsValidCoordinate(new Vector3(x, y, z)) && positions[x, y, z] != null &&
                        positions[x, y, z].GETPieceCode() == 'P')
                    {
                        return true;
                    }
                }
            }
        }

        //King loop
        int posX;
        int posY;
        int posZ;
        for (int l = -1; l <= 1; l++)
        {
            for (int m = -1; m <= 1; m++)
            {
                for (int n = -1; n <= 1; n++)
                {
                    posX = oX + l;
                    posY = oY + m;
                    posZ = oZ + n;
                    if (IsValidCoordinate(new Vector3(posX, posY, posZ)))
                    {

                        if (!new Vector3(posX, posY, posZ).Equals(coords) &&
                            GetPiece(new Vector3(posX, posY, posZ)) != null &&
                            Char.ToUpper(GetPiece(new Vector3(posX, posY, posZ)).GETPieceCode())
                                .Equals('K'))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        /*
        for (int k = -1; k <= 1; k++)
        {
            for (int i = -1; i <= 1; i++)
            {
                for(int j = -1 ; j <= 1; j++)
                {
                    posX = oX + i;
                    posY = oY + k;
                    posZ = oZ + j;
                    if (IsValidCoordinate(new Vector3(posX, posY, posZ)))
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            for (int m = -1; m <= 1; m++)
                            {
                                for(int n = -1 ; n <= 1; n++)
                                {
                                    posX = oX + i;
                                    posY = oY + k;
                                    posZ = oZ + j;
                                    if (IsValidCoordinate(new Vector3(posX, posY, posZ)))
                                    {

                                        if (!new Vector3(posX, posY, posZ).Equals(king.position) &&
                                            GetPiece(new Vector3(posX, posY, posZ)) != null &&
                                            Char.ToUpper(GetPiece(new Vector3(posX, posY, posZ)).GETPieceCode())
                                                .Equals('K'))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }*/

        return false;
    }

    public bool IsValidCoordinate(Vector3 coord)
    {
        if ((int) coord.x >= 0 && (int) coord.z >= 0 && (int) coord.x <= 7 && (int) coord.z <= 7 &&
            (int) coord.y >= 0 && (int) coord.y <= 2)
        {
            return true;
        }

        return false;
    }

    private bool IsAllowedMove(Piece first, Piece other)
    {
        if (other == null || first.isWhite != other.isWhite)
        {
            return true;
        }
        else
        {
            return false;
        }
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