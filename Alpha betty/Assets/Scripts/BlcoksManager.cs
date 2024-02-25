using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class BlcoksManager : MonoBehaviour
{
    private string[] _words = { "HELLO", "WORLD", "ARRAY", "RANDOM", "WORDS" };
    private char[,] _board = new char[5, 5];
    private int[,] _position =
    {
        {-180,180} ,{-90,180} ,{0,180} ,{90,180} ,{180,180},
        {-180,90}  ,{-90,90}  ,{0,90}  ,{90,90}  ,{180,90},
        {-180,0}   ,{-90,0}   ,{0,0}   ,{90,0}   ,{180,0},
        {-180,-90} ,{-90,-90} ,{0,-90} ,{90,-90} ,{180,-90},
        {-180,-180},{-90,-180},{0,-180},{90,-180},{180,-180},
    };
    
    private GameObject[,] _blocks = new GameObject[5,5]; 
    
    [SerializeField] private GameObject _block;

    void Start()
    {
        Vector2 blockBoard = new Vector2(0,0);
        for (int i = 0; i < _position.GetLength(0); i++)
        {
            // Corrected access to multi-dimensional array elements
            int x = _position[i, 0];
            int y = _position[i, 1];
            var instantiateBlock = Instantiate(_block,this.transform,false);
            instantiateBlock.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            _blocks[(int) blockBoard.x,(int) blockBoard.y] = instantiateBlock;
            if (blockBoard.x == 4)
            {
                blockBoard.x = 0;
                blockBoard.y += 1;
            }
            else
            {
                blockBoard.x += 1;
            }
        }
        // Select a random word with less than 25 characters
        string chosenWord = _words[Random.Range(0,_words.Length)];
        Debug.Log(chosenWord);
        // Try to place the word
        if (!PlaceWordInBoard(_board, chosenWord))
        {
            Debug.Log("Failed to place the word. Trying again...");
            // Instead of calling Main again, we'll just stop execution here
            // Consider adjusting your logic to retry within Unity's update loop or through a user action
            return;
        }

        // Fill the rest of the board with random letters
        FillBoard(_board);

        int blockNumber = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5;j++)
            {
                this.transform.GetChild(blockNumber).GetComponent<Blocks>().SetChar(_board[i,j].ToString());
                blockNumber++;
            }
        }
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
    private List<(int startX, int startY, int endX, int endY)> FindWordPositions(string word)
    {
        List<(int, int, int, int)> positions = new List<(int, int, int, int)>();
        int wordLength = word.Length;

        for (int row = 0; row < _board.GetLength(0); row++)
        {
            for (int col = 0; col < _board.GetLength(1); col++)
            {
                // Check in all directions from each starting point
                foreach (var direction in GetDirections())
                {
                    int endX = row + direction.dr * (wordLength - 1);
                    int endY = col + direction.dc * (wordLength - 1);

                    // Check if the end position is within bounds
                    if (endX >= 0 && endX < _board.GetLength(0) && endY >= 0 && endY < _board.GetLength(1))
                    {
                        string foundWord = "";
                        for (int i = 0; i < wordLength; i++)
                        {
                            int currentRow = row + i * direction.dr;
                            int currentCol = col + i * direction.dc;
                            foundWord += _board[currentRow, currentCol];
                        }

                        if (foundWord == word)
                        {
                            positions.Add((row, col, endX, endY));
                        }
                    }
                }
            }
        }

        return positions;
    }

    private List<(int dr, int dc)> GetDirections()
    {
        // Represents eight possible directions: N, NE, E, SE, S, SW, W, NW
        return new List<(int, int)>
        {
            (-1, 0), // N
            (-1, 1), // NE
            (0, 1),  // E
            (1, 1),  // SE
            (1, 0),  // S
            (1, -1), // SW
            (0, -1), // W
            (-1, -1) // NW
        };
    }
    private void RemoveWordFromBlocks(string word)
    {
        List<(int startX, int startY, int endX, int endY)> wordPositions = FindWordPositions(word);

        foreach (var position in wordPositions)
        {
            int startX = position.startX;
            int startY = position.startY;
            int endX = position.endX;
            int endY = position.endY;

            int deltaX = endX - startX;
            int deltaY = endY - startY;

            // Determine the step direction for both x and y
            int stepX = deltaX == 0 ? 0 : deltaX / Mathf.Abs(deltaX);
            int stepY = deltaY == 0 ? 0 : deltaY / Mathf.Abs(deltaY);

            // Number of steps to take (including the start position)
            int steps = Mathf.Max(Mathf.Abs(deltaX), Mathf.Abs(deltaY)) + 1;

            for (int i = 0; i < steps; i++)
            {
                int currentX = startX + (i * stepX);
                int currentY = startY + (i * stepY);

                // Check bounds just to be safe
                if (currentX >= 0 && currentX < _blocks.GetLength(0) && currentY >= 0 && currentY < _blocks.GetLength(1))
                {
                    // Destroy the GameObject to clean up, if needed
                    if (_blocks[currentX, currentY] != null)
                    {
                        Destroy(_blocks[currentX, currentY]);
                    }
                
                    // Set the block position to null
                    _blocks[currentX, currentY] = null;
                }
            }
        }
    }
    public bool IsCorrect(string word)
    {
        bool correct = _words.Contains(word);
        if (correct)
        {
            RemoveWordFromBlocks(word);
        }
        
        return correct;
    }

    public void FallOfBlocks()
    {
        for (int i = 0; i < 25; i++)
        {
            
        }
    }
}
