using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private const float TILE_SIZE = 1.0F;
    private const float TILE_OFFSET = 0.5F;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman = new List<GameObject>();


    private void Start()
    {
        SpawnAllChessman();
    }
    private void Update()
    {
        UpdateSelection();
        DrawChessBoard();
    }

    //function to draw a chessboard for now.
    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i < 9; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j < 9; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }
        //draw the selection
        if(selectionX >=0 && selectionY >= 0)
        {
            Debug.DrawLine(
            Vector3.forward * selectionY + Vector3.right * selectionX,
            Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1)
            );
            Debug.DrawLine(
            Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
            Vector3.forward * selectionY  + Vector3.right * (selectionX + 1)
            );
        }
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;
        RaycastHit hit;
       if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnChessman(int index, Vector3 position)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], position, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activeChessman.Add(go);
    }

    //get center function
    private Vector3 GetCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private void SpawnAllChessman()
    {
        activeChessman = new List<GameObject>();
        //spwan the white team and the black team

        //King
        SpawnChessman(0, GetCenter(3, 0));
        SpawnChessman(6, GetCenter(4, 7));
        //Queen
        SpawnChessman(1, GetCenter(4, 0));
        SpawnChessman(7, GetCenter(3, 7));

        //Rooks
        SpawnChessman(2, GetCenter(0, 0));
        SpawnChessman(2, GetCenter(7, 0));
        SpawnChessman(8, GetCenter(0, 7));
        SpawnChessman(8, GetCenter(7, 7));

        //Bishops
        SpawnChessman(3, GetCenter(2, 0));
        SpawnChessman(3, GetCenter(5, 0));
        SpawnChessman(9, GetCenter(2, 7));
        SpawnChessman(9, GetCenter(5, 7));

        //Kights
        SpawnChessman(4, GetCenter(1, 0));
        SpawnChessman(4, GetCenter(6, 0));
        SpawnChessman(10, GetCenter(1, 7));
        SpawnChessman(10, GetCenter(6, 7));

        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(5, GetCenter(i, 1));
            SpawnChessman(11, GetCenter(i, 6));
        }
    }
}
