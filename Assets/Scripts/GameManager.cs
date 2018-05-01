﻿using System;
 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const float MoveTime = 5.0f;

    [SerializeField] private Text moveText;
    [SerializeField] private Text endGameText;

    [SerializeField] private ClockController playerClock;
    [SerializeField] private ClockController clockAI;

    AlphaBeta ab = new AlphaBeta();
    private bool _kingDead = false;
    float timer = 0;
    private float timerAI = 0;
    Board _board;

    private RochadeController rochadeController;

    public RochadeController RochadeController { get { return rochadeController; } }

    public bool BlockInput { get; set; }

	void Start ()
    {
        rochadeController = new RochadeController();
        BlockInput = false;
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
            timerAI = 0;
        }
        else if (!playerTurn && timer >= 3)
        {
            timerAI += Time.deltaTime;
            Move move = ab.GetMove();
            _DoAIMove(move);
            StartCoroutine(ResumeClock());
            timer = 0;
        }
	}

    public bool playerTurn = true;

    void _DoAIMove(Move move)
    {
        //Debug.Log("DOING AI MOVE");
//        StartCoroutine(ResumeAIClock());
        Tile firstPosition = move.firstPosition;
        Tile secondPosition = move.secondPosition;

        if (secondPosition.CurrentPiece && secondPosition.CurrentPiece.Type == Piece.pieceType.KING)
        {
            SwapPieces(move);
            Time.timeScale = 0;
            endGameText.text = "YOU LOST";
            _kingDead = true;
            return;
        }
        else
        {
            SwapPieces(move);
        }
    }

    public void StopClock()
    {
        //yield return new WaitForSeconds(MoveTime);
        playerClock.Stopped = true;
        if (clockAI != null)
        {
            clockAI.Stopped = false;
        }

        moveText.text = "AI move";
    }

    public IEnumerator ResumeAIClock()
    {
//        moveText.text = "AI move";
        yield return new WaitForSeconds(MoveTime);
        clockAI.Stopped = false;
    }

    public IEnumerator ResumeClock()
    {
        clockAI.Stopped = true;
        BlockInput = true;
        yield return new WaitForSeconds(MoveTime);
        BlockInput = false;
        moveText.text = "Player move";
        playerClock.Stopped = false;
    }
    
    // Проект 01. Шахматы
    
    public void SwapPieces(Move move)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject o in objects)
        {
            Destroy(o);
        }

        Tile firstTile = move.firstPosition;
        Tile secondTile = move.secondPosition;

        UpdateRochade(move);

        firstTile.CurrentPiece.MovePiece(new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y));

        if (secondTile.CurrentPiece != null)
        {
            if (secondTile.CurrentPiece.Type == Piece.pieceType.KING)
            {
                Time.timeScale = 0;
                endGameText.text = "YOU WIN";
                _kingDead = true;
                return;
            }
            NetworkManager.Instance.AddMessage(RobotOperations.Remove(secondTile.BoardPosition, secondTile.CurrentPiece.InitialPosition));  
            Destroy(secondTile.CurrentPiece.gameObject);
        }

        NetworkManager.Instance.AddMessage(RobotOperations.Move(firstTile.BoardPosition, secondTile.BoardPosition));
            

        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.position = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;

        if(move.move == null)
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

    private void UpdateRochade(Move move)
    {
        if (move.pieceMoved.Player == Piece.playerColor.WHITE)
        {
            if (move.pieceMoved.Type == Piece.pieceType.ROOK)
            {
                if (move.pieceMoved.InitialPosition.X == 0)
                {
                    rochadeController.OnLeftRooksMove();
                }
                else
                {
                    rochadeController.OnRightRooksMove();
                }
            }

            if (move.pieceMoved.Type == Piece.pieceType.KING)
                rochadeController.OnKingsMove();
        }
        else
        {
            if (move.pieceKilled != null && move.pieceKilled.Type == Piece.pieceType.ROOK)
            {
                if (move.pieceKilled.InitialPosition.X == 0)
                {
                    rochadeController.OnLeftRooksMove();
                }
                else
                {
                    rochadeController.OnRightRooksMove();
                }
            }
        }
    }
}
