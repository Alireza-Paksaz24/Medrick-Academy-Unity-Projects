using UnityEngine;
using System.Collections.Generic;

public class BlcoksManager : MonoBehaviour
{
    private string[] words = { "Hello", "World", "Array", "Random", "Words" };
    private char[,] board = new char[5, 5];

    void Start()
    {
        // Select a random word with less than 25 characters
        string chosenWord = words[Random.Range(0,words.Length)];

        // Try to place the word
        if (!PlaceWordInBoard(board, chosenWord))
        {
            Debug.Log("Failed to place the word. Trying again...");
            // Instead of calling Main again, we'll just stop execution here
            // Consider adjusting your logic to retry within Unity's update loop or through a user action
            return;
        }

        // Fill the rest of the board with random letters
        FillBoard(board);

        // Print the board
        PrintBoard(board);
    }

    bool PlaceWordInBoard(char[,] board, string word)
    {
        List<(int, int)> possiblePositions = new List<(int, int)>();

        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                possiblePositions.Add((row, col));
            }
        }

        while (possiblePositions.Count > 0)
        {
            int posIndex = Random.Range(0,possiblePositions.Count);
            (int row, int col) = possiblePositions[posIndex];
            possiblePositions.RemoveAt(posIndex);

            if (PlaceWordFromPosition(board, word, 0, row, col, new bool[5, 5]))
            {
                return true;
            }
        }

        return false;
    }

    bool PlaceWordFromPosition(char[,] board, string word, int index, int row, int col, bool[,] visited)
    {
        if (index == word.Length) return true;
        if (row < 0 || col < 0 || row >= board.GetLength(0) || col >= board.GetLength(1) || visited[row, col]) return false;

        visited[row, col] = true;
        board[row, col] = word[index];

        List<(int, int)> directions = new List<(int, int)> { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
        foreach (var (dr, dc) in directions)
        {
            if (PlaceWordFromPosition(board, word, index + 1, row + dr, col + dc, visited))
            {
                return true;
            }
        }

        visited[row, col] = false;
        board[row, col] = '\0';
        return false;
    }

    void FillBoard(char[,] board)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == '\0')
                    board[i, j] = (char)('A' + Random.Range(0,26));
            }
        }
    }

    void PrintBoard(char[,] board)
    {
        string boardString = "";
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                boardString += board[i, j] + " ";
            }
            boardString += "\n";
        }
        // Debug.Log(boardString);
    }
}
