using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        //Up2 left
        KnightMove(CurrentX - 1, CurrentY + 2, ref r);
        //Up2 Right
        KnightMove(CurrentX + 1, CurrentY + 2, ref r);
        //Right2 Up
        KnightMove(CurrentX + 2, CurrentY + 1, ref r);
        //Left2 Up
        KnightMove(CurrentX - 2, CurrentY + 1, ref r);

        //Down2 left
        KnightMove(CurrentX - 1, CurrentY - 2, ref r);
        //Down2 Right
        KnightMove(CurrentX + 1, CurrentY - 2, ref r);
        //Right2 Down
        KnightMove(CurrentX + 2, CurrentY - 1, ref r);
        //Left2 Down
        KnightMove(CurrentX - 2, CurrentY - 1, ref r);

        return r;
    }

    public void KnightMove(int x, int y, ref bool[,] r)
    {
        Chessman c;
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            c = BoardManager.Instance.Chessmans[x, y];
            if (c == null)
                r[x, y] = true;
            else if (isWhite != c.isWhite)
                r[x, y] = true;

        }
    }
}
