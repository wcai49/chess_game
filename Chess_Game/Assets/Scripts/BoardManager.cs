using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    private bool[,] allowedMoves { set; get; }

    public Chessman[,] Chessmans { set; get; }
    private Chessman selectedChessman;

    private const float TILE_SIZE = 1.0F;
    private const float TILE_OFFSET = 0.5F;

    private int selectionX = -1;
    private int selectionY = -1;

    public bool isWhiteTurn = true;
    public bool isFinished = false;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman = new List<GameObject>();
    public GameObject winCanvas;
    public GameObject loseCanvas;

    private Material previousMat;
    public Material selectedMat;


    private void Start()
    {
        Instance = this;
        SpawnAllChessman();
    }
    private void Update()
    {
        UpdateSelection();
        DrawChessBoard();

        if (Input.GetMouseButtonDown(0))
        {
            if(selectionX >=0 && selectionY >=0)
            {
                if(selectedChessman == null)
                {
                    //select a chessman please
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    //Move the chessman
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectChessman(int x, int y)
    {
        if (isFinished)
            return;
        //if there is no chessman on that position, return
        if(Chessmans [x,y] == null)
        {
            return;
        }
        //if it is not your turn, return.
        if(Chessmans[x,y].isWhite != isWhiteTurn)
        {
            return;
        }

        bool leastOneMove = false;
        allowedMoves = Chessmans[x, y].PossibleMove();

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (allowedMoves[i, j])
                    leastOneMove = true;

        if (!leastOneMove)
            return;
        selectedChessman = Chessmans[x, y];
        previousMat = selectedChessman.GetComponent < MeshRenderer > ().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;
        BoardHighlights.Instance.HighlighAllowedMoves(allowedMoves);

    }

    private void MoveChessman(int x, int y)
    {
        if(allowedMoves[x,y])
        {
            Chessman c = Chessmans[x, y];
            if (c != null && c.isWhite != isWhiteTurn)
            {
                //Capture a piece

                //if it is the     king
                if(c.GetType() == typeof(King))
                {
                    //End the game
                    EndGame();
                    isFinished = true;
                    return;
                }

                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }


            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;
            isWhiteTurn = !isWhiteTurn;
        }
        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
        //unselect
        selectedChessman = null;
        BoardHighlights.Instance.Hidehighlights();
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

    private void SpawnChessman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetCenter(x,y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        Chessmans [x, y] = go.GetComponent<Chessman>();
        Chessmans [x, y].SetPosition(x, y);
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
        Chessmans = new Chessman[8, 8];
        //spwan the white team and the black team

        //King
        SpawnChessman(0, 3, 0);
        SpawnChessman(6, 4, 7);
        //Queen
        SpawnChessman(1, 4, 0);
        SpawnChessman(7, 3, 7);

        //Rooks
        SpawnChessman(2, 0, 0);
        SpawnChessman(2, 7, 0);
        SpawnChessman(8, 0, 7);
        SpawnChessman(8, 7, 7);

        //Bishops
        SpawnChessman(3, 2, 0);
        SpawnChessman(3, 5, 0);
        SpawnChessman(9, 2, 7);
        SpawnChessman(9, 5, 7);

        //Kights
        SpawnChessman(4, 1, 0);
        SpawnChessman(4, 6, 0);
        SpawnChessman(10,1, 7);
        SpawnChessman(10,6, 7);

        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(5, i, 1);
            SpawnChessman(11, i, 6);
        }
    }

    private void EndGame()
    {
        if (isWhiteTurn)
        {
            Debug.Log("White team wins");
            winCanvas.SetActive(true);
        }

        else
        {
            Debug.Log("Blakc team wins");
            loseCanvas.SetActive(true);
        }

        
    }

    public void RestartGame()
    {
        foreach (GameObject go in activeChessman)
            Destroy(go);

        isWhiteTurn = true;
        BoardHighlights.Instance.Hidehighlights();
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        isFinished = false;
        SpawnAllChessman();
    }
}
