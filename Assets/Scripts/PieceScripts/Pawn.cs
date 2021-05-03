using System;
using UnityEngine;

public class Pawn : Piece
{
    public bool doubleMoved = false;
    public override bool[,,] PossibleMove(Piece[,,] positions)
    {
        var r = new bool[8, 3, 8];
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    r[i, k, j] = false;
                }
            }
        }
        Piece c;
        int forward;
        if (isWhite)
        {
            forward = 1;
        }
        else
        {
            forward = -1;
        }

        int oX,oY,oZ,x, y, z;
        oX = (int) position.x;
        oY = (int) position.y;
        oZ = (int) position.z;
        if (roundMoved == 0)
        {
            x = oX;
            y = oY;
            z = oZ + forward*2;
            if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x, y, z)))
            {
                if (BoardManagerReworked.Instance.Pieces[x, y, z] == null )
                {
                    r[x, y, z] = true;
                }
            }
            
            x = oX;
            y = oY + forward*2;
            z = oZ + forward*2;
            if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x, y, z)))
            {
                if (BoardManagerReworked.Instance.Pieces[x, y, z] == null  )
                {
                    r[x, y, z] = true;
                }
            }
        }
        for (int i = -1; i <= 1; i++)
        {
            //Check forward left
            x = oX - 1;
            y = oY + i;
            z = oZ + forward;
            if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x, y, z)))
            {
                if (BoardManagerReworked.Instance.Pieces[x, y, z] != null && BoardManagerReworked.Instance.Pieces[x, y, z].isWhite != isWhite )
                {
                    
                    Debug.Log(new Vector3(x, y, z));
                    r[x, y, z] = true;
                }
            }
            //Check forward
            x = oX;
            y = oY + i;
            z = oZ + forward;
            if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x, y, z)))
            {
                if (BoardManagerReworked.Instance.Pieces[x, y, z] == null)
                {
                    Debug.Log(new Vector3(x, y, z));
                    r[x, y, z] = true;
                }
            }
            //Check forward right
            x = oX + 1;
            y = oY + i;
            z = oZ + forward;
            if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x, y, z)))
            {
                if (BoardManagerReworked.Instance.Pieces[x, y, z] != null && BoardManagerReworked.Instance.Pieces[x, y, z].isWhite != isWhite )
                {
                    Debug.Log(new Vector3(x, y, z));
                    r[x, y, z] = true;
                }
            }
            
            //Check up and down
            if (i != 0)
            {
                x = oX;
                y = oY + i;
                z = oZ;
                if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x, y, z)))
                {
                    if (BoardManagerReworked.Instance.Pieces[x, y, z] == null)
                    {
                        Debug.Log(new Vector3(x, y, z));
                        r[x, y, z] = true;
                    }
                }
            }
        }
        
        //En passant
        x = oX - 1;
        y = oY;
        z = oZ;
        if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x, y, z)))
        {
            if (BoardManagerReworked.Instance.Pieces[x, y, z] != null )
            {
                if (isWhite)
                {
                    if (BoardManagerReworked.Instance.Pieces[x, y, z].GETPieceCode() == 'p')
                    {
                        if (((Pawn) BoardManagerReworked.Instance.Pieces[x, y, z]).doubleMoved && BoardManagerReworked.Instance.Pieces[x, y, z].roundMoved == BoardManagerReworked.Instance.roundNumber -1 && BoardManagerReworked.Instance.Pieces[x, y, z+forward] == null)
                        {
                            Debug.Log("asd");
                            r[x, y, z + forward] = true;
                        }
                    }
                    
                }
                else
                {
                    if (BoardManagerReworked.Instance.Pieces[x, y, z].GETPieceCode() == 'P')
                    {
                        if (((Pawn) BoardManagerReworked.Instance.Pieces[x, y, z]).doubleMoved && BoardManagerReworked.Instance.Pieces[x, y, z].roundMoved == BoardManagerReworked.Instance.roundNumber -1 && BoardManagerReworked.Instance.Pieces[x, y, z+forward] == null)
                        {
                            Debug.Log("asd");
                            r[x, y, z + forward] = true;
                        }
                    }
                }
            }
        }

        x = oX + 1;
        y = oY;
        z = oZ;
        if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x, y, z)))
        {
            if (BoardManagerReworked.Instance.Pieces[x, y, z] != null )
            {
                if (isWhite)
                {
                    if (BoardManagerReworked.Instance.Pieces[x, y, z].GETPieceCode() == 'p')
                    {
                        Debug.Log(((Pawn) BoardManagerReworked.Instance.Pieces[x, y, z]).doubleMoved);
                        Debug.Log(BoardManagerReworked.Instance.Pieces[x, y, z].roundMoved  == BoardManagerReworked.Instance.roundNumber -1);
                        Debug.Log(BoardManagerReworked.Instance.Pieces[x, y, z+forward] == null);
                        Debug.Log(BoardManagerReworked.Instance.Pieces[x, y, z].roundMoved -1);
                        if (((Pawn) BoardManagerReworked.Instance.Pieces[x, y, z]).doubleMoved && BoardManagerReworked.Instance.Pieces[x, y, z].roundMoved == BoardManagerReworked.Instance.roundNumber -1 && BoardManagerReworked.Instance.Pieces[x, y, z+forward] == null)
                        {
                            
                            r[x, y, z + forward] = true;
                        }
                    }
                    
                }
                else
                {
                    if (BoardManagerReworked.Instance.Pieces[x, y, z].GETPieceCode() == 'P')
                    {
                        Debug.Log("asd");
                        if (((Pawn) BoardManagerReworked.Instance.Pieces[x, y, z]).doubleMoved && BoardManagerReworked.Instance.Pieces[x, y, z].roundMoved == BoardManagerReworked.Instance.roundNumber -1 && BoardManagerReworked.Instance.Pieces[x, y, z+forward] == null)
                        {
                            Debug.Log("asd");
                            r[x, y, z + forward] = true;
                        }
                    }
                }
            }
        }
/*
        //White team moves
        if (isWhite)
        {
            //Diagonal Left (Piece not on the left of the board and not in the end)
            if ((int) position.x != 0 && (int) position.z != 7)
            {
                c = BoardManagerReworked.Instance.Pieces[(int) position.x - 1, (int) position.y, (int) position.z + 1];

                //Diagonal left same board
                if (c != null && !c.isWhite) r[(int) position.x - 1, (int) position.y, (int) position.z + 1] = true;

                //Diagonal left lower board
                if ((int) position.y != 0)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x - 1, (int) position.y - 1,
                        (int) position.z + 1];
                    if (c != null && !c.isWhite)
                        r[(int) position.x - 1, (int) position.y - 1, (int) position.z + 1] = true;
                }

                //Diagonal left higher board
                if ((int) position.y != 2)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x - 1, (int) position.y + 1,
                        (int) position.z + 1];
                    if (c != null && !c.isWhite)
                        r[(int) position.x - 1, (int) position.y + 1, (int) position.z + 1] = true;
                }
            }

            //Diagonal Right
            if ((int) position.x != 7 && (int) position.z != 7)
            {
                c = BoardManagerReworked.Instance.Pieces[(int) position.x + 1, (int) position.y, (int) position.z + 1];

                //Diagonal right same board
                if (c != null && !c.isWhite) r[(int) position.x + 1, (int) position.y, (int) position.z + 1] = true;

                //Diagonal right lower board
                if ((int) position.y != 0)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x + 1, (int) position.y - 1,
                        (int) position.z + 1];
                    if (c != null && !c.isWhite)
                        r[(int) position.x + 1, (int) position.y - 1, (int) position.z + 1] = true;
                }

                //Diagonal right higher board
                if ((int) position.y != 2)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x + 1, (int) position.y + 1,
                        (int) position.z + 1];
                    if (c != null && !c.isWhite)
                        r[(int) position.x + 1, (int) position.y + 1, (int) position.z + 1] = true;
                }
            }

            //Middle
            if ((int) position.z != 7)
            {
                c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y, (int) position.z + 1];
                if (c == null) r[(int) position.x, (int) position.y, (int) position.z + 1] = true;

                if ((int) position.y != 0)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y - 1,
                        (int) position.z + 1];
                    if (c == null) r[(int) position.x, (int) position.y - 1, (int) position.z + 1] = true;

                    c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y - 1, (int) position.z];
                    if (c == null) r[(int) position.x, (int) position.y - 1, (int) position.z] = true;
                }

                if ((int) position.y != 2)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y + 1,
                        (int) position.z + 1];
                    if (c == null) r[(int) position.x, (int) position.y + 1, (int) position.z + 1] = true;
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y + 1, (int) position.z];
                    if (c == null) r[(int) position.x, (int) position.y + 1, (int) position.z] = true;
                }
            }

            //Middle on first move
            if ((int) position.z == 1)
            {
                r[(int) position.x, (int) position.y, (int) position.z + 2] = true;
                r[(int) position.x, 2, (int) position.z] = true;
                r[(int) position.x, 2, (int) position.z + 2] = true;
            }
            
            //Check left and right 
            //c = BoardManagerReworked.Instance.Pieces[(int) position.x + 1, (int) position.y, (int) position.z];
            int x, y, z;
            x = (int) position.x;
            y = (int) position.y;
            z = (int) position.z;
            x += 1;
            if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x,y,z)))
            {
                
                c = BoardManagerReworked.Instance.Pieces[x,y,z];
                if (c != null  && !c.isWhite && c.GETPieceCode() == 'p')
                {
                    Pawn temp = (Pawn) BoardManagerReworked.Instance.Pieces[x, y, z];
                    if ( temp.roundMoved -1 == BoardManagerReworked.Instance.roundNumber && temp.doubleMoved)
                    {
                        r[x, y, z + 1] = true;
                    }
                
                }
            }
            x = (int) position.x;
            y = (int) position.y;
            z = (int) position.z;
            x -= 1;
            if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x,y,z)))
            {
                
                c = BoardManagerReworked.Instance.Pieces[x,y,z];
                if (c != null && !c.isWhite && c.GETPieceCode() == 'p')
                {
                    Pawn temp = (Pawn) BoardManagerReworked.Instance.Pieces[x, y, z];
                    if ( temp.roundMoved -1 == BoardManagerReworked.Instance.roundNumber && temp.doubleMoved)
                    {
                        r[x, y, z + 1] = true;
                    }
                
                }
            }   
                

            
        }
        else //Black team moves
        {
            //Diagonal Left (Piece not on the left of the board and not in the end)
            if ((int) position.x != 0 && (int) position.z != 0)
            {
                c = BoardManagerReworked.Instance.Pieces[(int) position.x - 1, (int) position.y, (int) position.z - 1];

                //Diagonal left same board
                if (c != null && c.isWhite) r[(int) position.x - 1, (int) position.y, (int) position.z - 1] = true;
                //Diagonal left lower board
                if ((int) position.y != 0)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x - 1, (int) position.y - 1,
                        (int) position.z - 1];
                    if (c != null && c.isWhite)
                        r[(int) position.x - 1, (int) position.y - 1, (int) position.z - 1] = true;
                }

                //Diagonal left higher board
                if ((int) position.y != 2)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x - 1, (int) position.y + 1,
                        (int) position.z - 1];
                    if (c != null && c.isWhite)
                        r[(int) position.x - 1, (int) position.y + 1, (int) position.z - 1] = true;
                }
            }

            //Diagonal Right
            if ((int) position.x != 7 && (int) position.z != 0)
            {
                c = BoardManagerReworked.Instance.Pieces[(int) position.x + 1, (int) position.y, (int) position.z - 1];
                //Diagonal right same board
                if (c != null && c.isWhite) r[(int) position.x + 1, (int) position.y, (int) position.z - 1] = true;
                //Diagonal right lower board
                if ((int) position.y != 0)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x + 1, (int) position.y - 1,
                        (int) position.z - 1];
                    if (c != null && c.isWhite)
                        r[(int) position.x + 1, (int) position.y - 1, (int) position.z - 1] = true;
                }

                //Diagonal right higher board
                if ((int) position.y != 2)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x + 1, (int) position.y + 1,
                        (int) position.z - 1];
                    if (c != null && c.isWhite)
                        r[(int) position.x + 1, (int) position.y + 1, (int) position.z - 1] = true;
                }
            }

            //Middle
            if ((int) position.z != 0)
            {
                c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y, (int) position.z - 1];
                if (c == null) r[(int) position.x, (int) position.y, (int) position.z - 1] = true;

                if ((int) position.y != 0)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y - 1,
                        (int) position.z - 1];
                    if (c == null) r[(int) position.x, (int) position.y - 1, (int) position.z - 1] = true;

                    c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y - 1, (int) position.z];
                    if (c == null) r[(int) position.x, (int) position.y - 1, (int) position.z] = true;
                }

                if ((int) position.y != 2)
                {
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y + 1,
                        (int) position.z - 1];
                    if (c == null) r[(int) position.x, (int) position.y + 1, (int) position.z - 1] = true;
                    c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y + 1, (int) position.z];
                    if (c == null) r[(int) position.x, (int) position.y + 1, (int) position.z] = true;
                }
            }

            //Middle on first move
            if ((int) position.z == 6)
            {
                r[(int) position.x, (int) position.y, (int) position.z - 2] = true;
                r[(int) position.x, 0, (int) position.z] = true;
                r[(int) position.x, 0, (int) position.z - 2] = true;
            }
            
            int x, y, z;
            x = (int) position.x;
            y = (int) position.y;
            z = (int) position.z;
            x += 1;
            if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x,y,z)))
            {
                c = BoardManagerReworked.Instance.Pieces[x,y,z];
                if (c != null && c.isWhite && c.GETPieceCode() == 'P')
                {
                    Debug.Log("asd2");
                    Pawn temp = (Pawn) BoardManagerReworked.Instance.Pieces[x, y, z];
                    if ( temp.roundMoved -1 == BoardManagerReworked.Instance.roundNumber && temp.doubleMoved)
                    {
                        r[x, y, z - 1] = true;
                    }
                
                }
            }
            x = (int) position.x;
            y = (int) position.y;
            z = (int) position.z;
            x -= 1;
            if (BoardManagerReworked.Instance.IsValidCoordinate(new Vector3(x,y,z)))
            {
                Debug.Log("asd1");
                c = BoardManagerReworked.Instance.Pieces[x,y,z];
                if (c != null && c.isWhite && c.GETPieceCode() == 'P')
                {
                    Pawn temp = (Pawn) BoardManagerReworked.Instance.Pieces[x, y, z];
                    if ( temp.roundMoved -1 == BoardManagerReworked.Instance.roundNumber && temp.doubleMoved)
                    {
                        r[x, y, z - 1] = true;
                    }
                
                }
            }
        }
*/
        return r;
    }

    public override char GETPieceCode()
    {
        if (isWhite)
            return 'P';
        return 'p';
    }
}