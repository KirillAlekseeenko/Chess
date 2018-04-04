﻿using UnityEngine;
using System.Collections;

public class Tile
{
    private Vector2 _position = Vector2.zero;

    public Vector2 Position
    {
        get { return _position; }
    }

	public Vector2Int BoardPosition { get; private set; }
	public Vector2Int InitialBoardPosition { get; private set; }

    private Piece _currentPiece = null;
    public Piece CurrentPiece
    {
        get { return _currentPiece; }
        set { _currentPiece = value; }
    }

    public Tile(int x, int y)
    {
        _position.x = x;
        _position.y = y;

        BoardPosition = new Vector2Int(x, y);

        if (y == 0 || y == 1 || y == 6 || y == 7)
        {
            _currentPiece = GameObject.Find(x.ToString() + " " + y.ToString()).GetComponent<Piece>();
            InitialBoardPosition = RobotOperations.getInitialBoardPosition(BoardPosition);
            NetworkManager.Instance.AddMessage(RobotOperations.StartMove(InitialBoardPosition, BoardPosition));
        }
    }

    public void SwapFakePieces(Piece newPiece)
    {
        _currentPiece = newPiece;
    }
		
}
