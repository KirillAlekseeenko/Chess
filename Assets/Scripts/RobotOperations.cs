using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RobotOperations {

	private static int reformat(Vector2Int vec)
	{
		return 1 + vec.X + vec.Y * 8;
	}

	public static Vector2Int getInitialBoardPosition(Vector2Int boardPosition)
	{
		int x = 0, y = 0;
		switch (boardPosition.Y) {
		case 0: // black pawn
			{
				y = boardPosition.X / 4 + 2;
				x = boardPosition.X % 4 + 3;
				break;
			}
		case 1: // black piece
			{ 
				y = boardPosition.X / 3 + 3;
				x = boardPosition.X % 3;
				break;
			}
		case 6: // white pawn
			{ 
				y = boardPosition.X / 3;
				x = boardPosition.X % 3;
				break;
			}
		case 7: // white pawn
			{ 
				y = boardPosition.X / 4;
				x = boardPosition.X % 4 + 3;
				break;
			}
		}

		return new Vector2Int (x, y);
	}

	public static string Move (Vector2Int placeFrom, Vector2Int placeTo)
	{
        Debug.Log("MOVE: " + reformat(placeFrom).ToString() + " to " + reformat(placeTo).ToString());
		return "CHECK,6\r\nM,M,M,M,M,M\r\n" + "1" + "," + reformat(placeFrom).ToString() + "," + "1" + "," + reformat(placeTo).ToString() + "," + "0" + "," + "0";
	}

	public static string Remove (Vector2Int placeFrom, Vector2Int placeTo)
	{
        Debug.Log("REMOVE: " + reformat(placeFrom).ToString() + " to " + reformat(placeTo).ToString());
		return "CHECK,6\r\nM,M,M,M,M,M\r\n" + "1" + "," + reformat(placeFrom).ToString() + "," + "2" + "," + reformat(placeTo).ToString() + "," + "0" + "," + "0";
	}
	 
	public static string StartMove (Vector2Int placeFrom, Vector2Int placeTo)
	{
        Debug.Log("STARTMOVE: " + reformat(placeFrom).ToString() + " to " + reformat(placeTo).ToString());
		return "CHECK,6\r\nM,M,M,M,M,M\r\n" + "2" + "," + reformat(placeFrom).ToString() + "," + "1" + "," + reformat(placeTo).ToString() + "," + "0" + "," + "0";
	}

	public static string Replace (Vector2Int placeFrom, Vector2Int placeTo)
	{
		return "CHECK,6\r\nM,M,M,M,M,M\r\n" + "2" + "," + reformat(placeFrom).ToString() + "," + "1" + "," + reformat(placeTo).ToString() + "," + "0" + "," + "0";
	}
}
