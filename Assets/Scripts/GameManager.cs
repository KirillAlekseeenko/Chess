using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    const float MoveTime = 5.0f;

    AlphaBeta ab = new AlphaBeta();
    private bool _kingDead = false;
    float timer = 0;
    Board _board;

    ClockController clock;

    public bool BlockInput { get; set; }

	void Start ()
    {
        BlockInput = false;
        clock = GetComponentInChildren<ClockController>();
        _board = Board.Instance;
        _board.SetupBoard();
	}

	void Update ()
    {
        if (_kingDead)
        {
            Debug.Log(@"WINNER!");
            //UnityEditor.EditorApplication.isPlaying = false;
            Clean();
            Application.Quit();

        }
        if (!playerTurn && timer < 3)
        {
            timer += Time.deltaTime;
        }
        else if (!playerTurn && timer >= 3)
        {
            Move move = ab.GetMove();
            _DoAIMove(move);
            StartCoroutine(ResumeClock());
            timer = 0;
        }
	}

    public bool playerTurn = true;

    void _DoAIMove(Move move)
    {
        Tile firstPosition = move.firstPosition;
        Tile secondPosition = move.secondPosition;

        if (secondPosition.CurrentPiece && secondPosition.CurrentPiece.Type == Piece.pieceType.KING)
        {
            SwapPieces(move);
            _kingDead = true;
        }
        else
        {
            SwapPieces(move);
        }
    }

    public void StopClock()
    {
        //yield return new WaitForSeconds(MoveTime);
        clock.Stopped = true;
    }

    public IEnumerator ResumeClock()
    {
        BlockInput = true;
        yield return new WaitForSeconds(MoveTime);
        BlockInput = false;
        clock.Stopped = false;
    }

    public void SwapPieces(Move move)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject o in objects)
        {
            Destroy(o);
        }

        Tile firstTile = move.firstPosition;
        Tile secondTile = move.secondPosition;

        firstTile.CurrentPiece.MovePiece(new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y));

        if (secondTile.CurrentPiece != null)
        {
            if (secondTile.CurrentPiece.Type == Piece.pieceType.KING)
                _kingDead = true;
            NetworkManager.Instance.AddMessage(RobotOperations.Remove(secondTile.BoardPosition, secondTile.CurrentPiece.InitialPosition));  
            Destroy(secondTile.CurrentPiece.gameObject);
        }

        NetworkManager.Instance.AddMessage(RobotOperations.Move(firstTile.BoardPosition, secondTile.BoardPosition));
            

        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.position = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;

        playerTurn = !playerTurn;
    }

    public void Clean()
    {
        var board = GameObject.Find("Board");
        var blackPieces = board.transform.Find("BlackPieces");
        var whitePieces = board.transform.Find("WhitePieces");
        foreach(Transform piece in blackPieces.transform)
        {
            int x = (int)piece.GetComponent<Piece>().position.x;
            int y = (int)piece.GetComponent<Piece>().position.y;
            NetworkManager.Instance.AddMessage(RobotOperations.Remove(new Vector2Int(x, y), piece.GetComponent<Piece>().InitialPosition));
             
        }

        foreach (Transform piece in whitePieces.transform)
        {
            int x = (int)piece.GetComponent<Piece>().position.x;
            int y = (int)piece.GetComponent<Piece>().position.y;
            NetworkManager.Instance.AddMessage(RobotOperations.Remove(new Vector2Int(x, y), piece.GetComponent<Piece>().InitialPosition));

        }
    }
}
