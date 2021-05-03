public class Rook : Piece
{
    public override bool[,,] PossibleMove(Piece[,,] positions)
    {
        var r = new bool[8, 3, 8];
        int oX, oY, oZ;
        oX = (int)position.x;
        oY = (int)position.y;
        oZ = (int)position.z;
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
            return 'R';
        return 'r';
    }
}