public class Bishop : Piece
{
    public override bool[,,] PossibleMove(Piece[,,] positions)
    {
        var r = new bool[8, 3, 8];

        Piece c;
        int i, j;

        //Forward Left
        i = (int) position.x;
        j = (int) position.z;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j == 8) break;

            c = BoardManagerReworked.Instance.Pieces[i, (int) position.y, j];
            if (c == null)
            {
                r[i, (int) position.y, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, (int) position.y, j] = true;

                break;
            }
        }

        //Forward Right
        i = (int) position.x;
        j = (int) position.z;
        while (true)
        {
            i++;
            j++;
            if (i == 8 || j == 8) break;

            c = BoardManagerReworked.Instance.Pieces[i, (int) position.y, j];
            if (c == null)
            {
                r[i, (int) position.y, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, (int) position.y, j] = true;

                break;
            }
        }

        //Backward Left
        i = (int) position.x;
        j = (int) position.z;
        while (true)
        {
            i--;
            j--;
            if (i == -1 || j == -1) break;

            c = BoardManagerReworked.Instance.Pieces[i, (int) position.y, j];
            if (c == null)
            {
                r[i, (int) position.y, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, (int) position.y, j] = true;

                break;
            }
        }

        //Backward Right
        i = (int) position.x;
        j = (int) position.z;
        while (true)
        {
            i++;
            j--;
            if (i == 8 || j == -1) break;

            c = BoardManagerReworked.Instance.Pieces[i, (int) position.y, j];
            if (c == null)
            {
                r[i, (int) position.y, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, (int) position.y, j] = true;

                break;
            }
        }

        int k;
        //Forward Left Up
        i = (int) position.x;
        j = (int) position.z;
        k = (int) position.y;
        while (true)
        {
            i--;
            j++;
            k++;
            if (i < 0 || j == 8 || k == 3) break;

            c = BoardManagerReworked.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, k, j] = true;

                break;
            }
        }

        //Forward Right Up
        i = (int) position.x;
        j = (int) position.z;
        k = (int) position.y;
        while (true)
        {
            i++;
            j++;
            k++;
            if (i == 8 || j == 8 || k == 3) break;

            c = BoardManagerReworked.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, k, j] = true;

                break;
            }
        }

        //Backward Left Up
        i = (int) position.x;
        j = (int) position.z;
        k = (int) position.y;
        while (true)
        {
            i--;
            j--;
            k++;
            if (i < 0 || j < 0 || k == 3) break;

            c = BoardManagerReworked.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, k, j] = true;

                break;
            }
        }

        //Backward Right Up
        i = (int) position.x;
        j = (int) position.z;
        k = (int) position.y;
        while (true)
        {
            i++;
            j--;
            k++;
            if (i == 8 || j < 0 || k == 3) break;

            c = BoardManagerReworked.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, k, j] = true;

                break;
            }
        }

        //Forward Left Down
        i = (int) position.x;
        j = (int) position.z;
        k = (int) position.y;
        while (true)
        {
            i--;
            j++;
            k--;
            if (i < 0 || j == 8 || k < 0) break;

            c = BoardManagerReworked.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, k, j] = true;

                break;
            }
        }

        //Forward Right Down
        i = (int) position.x;
        j = (int) position.z;
        k = (int) position.y;
        while (true)
        {
            i++;
            j++;
            k--;
            if (i == 8 || j == 8 || k < 0) break;

            c = BoardManagerReworked.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, k, j] = true;

                break;
            }
        }

        //Backward Left Down
        i = (int) position.x;
        j = (int) position.z;
        k = (int) position.y;
        while (true)
        {
            i--;
            j--;
            k--;
            if (i < 0 || j < 0 || k < 0) break;

            c = BoardManagerReworked.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, k, j] = true;

                break;
            }
        }

        //Backward Right down
        i = (int) position.x;
        j = (int) position.z;
        k = (int) position.y;
        while (true)
        {
            i++;
            j--;
            k--;
            if (i == 8 || j < 0 || k < 0) break;

            c = BoardManagerReworked.Instance.Pieces[i, k, j];
            if (c == null)
            {
                r[i, k, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite) r[i, k, j] = true;

                break;
            }
        }

        return r;
    }

    public override char GETPieceCode()
    {
        if (isWhite)
            return 'B';
        return 'b';
    }
}