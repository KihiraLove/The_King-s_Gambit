public class Queen : Piece
{
    //Basically a crtl + c of bishop and rooks move
    public override bool[,,] PossibleMove(Piece[,,] positions)
    {
        var r = new bool[8, 3, 8];
        Piece c;
        int i;
        //Move right
        i = (int) position.x;
        while (true)
        {
            i++;
            if (i >= 8) break;

            c = BoardManagerReworked.Instance.Pieces[i, (int) position.y, (int) position.z];
            if (c == null)
            {
                r[i, (int) position.y, (int) position.z] = true;
            }
            else
            {
                if (c.isWhite != isWhite) r[i, (int) position.y, (int) position.z] = true;
                break;
            }
        }

        //Move left
        i = (int) position.x;
        while (true)
        {
            i--;
            if (i == -1) break;

            c = BoardManagerReworked.Instance.Pieces[i, (int) position.y, (int) position.z];
            if (c == null)
            {
                r[i, (int) position.y, (int) position.z] = true;
            }
            else
            {
                if (c.isWhite != isWhite) r[i, (int) position.y, (int) position.z] = true;

                break;
            }
        }

        //Move forward
        i = (int) position.z;
        while (true)
        {
            i++;
            if (i == 8) break;

            c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y, i];
            if (c == null)
            {
                r[(int) position.x, (int) position.y, i] = true;
            }
            else
            {
                if (c.isWhite != isWhite) r[(int) position.x, (int) position.y, i] = true;
                break;
            }
        }

        //Move backward
        i = (int) position.z;
        while (true)
        {
            i--;
            if (i == -1) break;

            c = BoardManagerReworked.Instance.Pieces[(int) position.x, (int) position.y, i];
            if (c == null)
            {
                r[(int) position.x, (int) position.y, i] = true;
            }
            else
            {
                if (c.isWhite != isWhite) r[(int) position.x, (int) position.y, i] = true;
                break;
            }
        }

        //Move up
        i = (int) position.y;
        while (true)
        {
            i++;
            if (i == 3) break;

            c = BoardManagerReworked.Instance.Pieces[(int) position.x, i, (int) position.z];
            if (c == null)
            {
                r[(int) position.x, i, (int) position.z] = true;
            }
            else
            {
                if (c.isWhite != isWhite) r[(int) position.x, i, (int) position.z] = true;
                break;
            }
        }

        //Move down
        i = (int) position.y;
        while (true)
        {
            i--;
            if (i == -1) break;

            c = BoardManagerReworked.Instance.Pieces[(int) position.x, i, (int) position.z];
            if (c == null)
            {
                r[(int) position.x, i, (int) position.z] = true;
            }
            else
            {
                if (c.isWhite != isWhite) r[(int) position.x, i, (int) position.z] = true;

                break;
            }
        }


        //Move in diagonal1 up
        RookMove((int) position.x + 1, (int) position.y + 1, (int) position.z, ref r);
        RookMove((int) position.x - 1, (int) position.y + 1, (int) position.z, ref r);
        RookMove((int) position.x, (int) position.y + 1, (int) position.z + 1, ref r);
        RookMove((int) position.x, (int) position.y + 1, (int) position.z - 1, ref r);

        //Move in diagonal2 up
        RookMove((int) position.x + 2, (int) position.y + 2, (int) position.z, ref r);
        RookMove((int) position.x - 2, (int) position.y + 2, (int) position.z, ref r);
        RookMove((int) position.x, (int) position.y + 2, (int) position.z + 2, ref r);
        RookMove((int) position.x, (int) position.y + 2, (int) position.z - 2, ref r);

        //Move in diagonal1 down
        RookMove((int) position.x + 1, (int) position.y - 1, (int) position.z, ref r);
        RookMove((int) position.x - 1, (int) position.y - 1, (int) position.z, ref r);
        RookMove((int) position.x, (int) position.y - 1, (int) position.z + 1, ref r);
        RookMove((int) position.x, (int) position.y - 1, (int) position.z - 1, ref r);

        //Move in diagonal2 down
        RookMove((int) position.x + 2, (int) position.y - 2, (int) position.z, ref r);
        RookMove((int) position.x - 2, (int) position.y - 2, (int) position.z, ref r);
        RookMove((int) position.x, (int) position.y - 2, (int) position.z + 2, ref r);
        RookMove((int) position.x, (int) position.y - 2, (int) position.z - 2, ref r);

        int j;

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

    //Helper function checks if place is valid and puts it in the array
    public void RookMove(int x, int y, int z, ref bool[,,] r)
    {
        Piece c;
        if (x >= 0 && x < 8 && z >= 0 && z < 8 && y >= 0 && y < 3)
        {
            c = BoardManagerReworked.Instance.Pieces[x, y, z];
            if (c == null)
                r[x, y, z] = true;
            else if (isWhite != c.isWhite) r[x, y, z] = true;
        }
    }

    public override char GETPieceCode()
    {
        if (isWhite)
            return 'Q';
        return 'q';
    }
}