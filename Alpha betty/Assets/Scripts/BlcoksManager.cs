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

    private static Dictionary<char, int> letterFrequencies = new Dictionary<char, int>()
    {
        {'E', 21912}, {'T', 16587}, {'A', 14810}, {'O', 14003}, {'I', 13318}, {'N', 12666}, {'S', 11450}, {'R', 10977}, 
        {'H', 10795}, {'D', 7874}, {'L', 7253}, {'U', 5246}, {'C', 4943}, {'M', 4761}, {'F', 4200}, {'Y', 3853}, 
        {'W', 3819}, {'G', 3693}, {'P', 3316}, {'B', 2715}, {'V', 2019}, {'K', 1257}, {'X', 315}, {'Q', 205}, {'J', 188}, 
        {'Z', 128}
    };

    private char GenerateCharacter()
    {
        // Compute total weight
        int totalWeight = 0;
        foreach (var pair in letterFrequencies)
        {
            totalWeight += pair.Value;
        }

        // Generate a random number within the total weight range
        int randomNum = Random.Range(0,totalWeight);

        // Find the corresponding letter based on the random number
        foreach (var pair in letterFrequencies)
        {
            randomNum -= pair.Value;
            if (randomNum < 0)
            {
                return pair.Key;
            }
        }

        // Default to 'A' if for some reason no letter was chosen
        return 'A';
    }
    
    public bool CheckWord(string word)
    {
        bool correct = _words.Contains(word);
        if (correct || true)
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
