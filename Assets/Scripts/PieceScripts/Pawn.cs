public class Pawn : Piece
{
    public override bool[,,] PossibleMove()
    {
        var r = new bool[8, 3, 8];
        Piece c;

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
        }

        return r;
    }

    public override char GETPieceCode()
    {
        if (isWhite)
            return 'P';
        return 'p';
    }
}