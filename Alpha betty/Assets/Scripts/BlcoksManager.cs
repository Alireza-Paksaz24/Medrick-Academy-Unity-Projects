using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class BlcoksManager : MonoBehaviour
{
    private int[] _countNewBlocks = new []{0,0,0,0,0};
    private string[] _words = { "ABCD","C" };
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
            instantiateBlock.GetComponent<Blocks>().SetPosi((int) blockBoard.x, (int) blockBoard.y);
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
                    board[i, j] = GenerateCharacter();
            }
        }
    }

    private char GenerateCharacter()
    {
        return (char)('A' + Random.Range(0,26));
    }

    public void RemoveWordFromBlocks()
    {
        List<GameObject> selected = transform.GetChild(0).GetComponent<Blocks>().GetSelected();
        foreach (var selectedBlock in selected)
        {
            Destroy(selectedBlock);
            int[] posi = selectedBlock.GetComponent<Blocks>().GetPosi();
            _blocks[posi[0], posi[1]] = null;
            _countNewBlocks[posi[0]] += 1;
        }
    }
    public bool CheckWord(string word)
    {
        bool correct = _words.Contains(word);
        if (correct)
        {
            RemoveWordFromBlocks();
            FallOfBlocks();
            for (int i = 0; i < 5; i++)
            {
                GameObject[] tempArrayForBlocks = new GameObject[_countNewBlocks[i]];
                for (int j = 0; j < _countNewBlocks[i]; j++)
                {
                    var instantiateBlock = Instantiate(_block, this.transform, false);
                    instantiateBlock.GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(-180 + (i * 90), 270 + j * 90);
                    instantiateBlock.GetComponent<Blocks>().SetPosi(i,-1);
                    tempArrayForBlocks[j] = instantiateBlock;
                }
                tempArrayForBlocks.Reverse();
                for (int j = 0; j < _countNewBlocks[i]; j++)
                {
                    tempArrayForBlocks[j].GetComponent<Blocks>().SetDestPosi(i,j);
                    _blocks[i, j] = tempArrayForBlocks[j];
                    var value = GenerateCharacter();
                    _blocks[i,j].GetComponent<Blocks>().SetChar(value.ToString());
                    _board[i, j] = value;
                }
            }

            _countNewBlocks = new[] {0, 0, 0, 0, 0};
        }
        
        return correct;
    }

    public void FallOfBlocks()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (_blocks[i, j] == null)
                {
                    for (int z = j; z > 0; z--)
                    {
                        _blocks[i, z] = _blocks[i, z - 1];
                    }
                }
            }
        }
    }
}
