using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;

public class BlcoksManager : MonoBehaviour
{
    private int[] _countNewBlocks = new []{0,0,0,0,0};
    private string[] _words = { "ABCD","C","A","B","D","E","F","G","H","I","J","K","L","N","O","P","Q","R","S" ,"W"};
    private char[,] _board = new char[5, 5];
    private int[,,] _position =
    {
        {{-180,180} ,{-90,180} ,{0,180} ,{90,180} ,{180,180}},
        {{-180,90}  ,{-90,90}  ,{0,90}  ,{90,90}  ,{180,90}},
        {{-180,0}   ,{-90,0}   ,{0,0}   ,{90,0}   ,{180,0}}, 
        {{-180,-90} ,{-90,-90} ,{0,-90} ,{90,-90} ,{180,-90}},
        {{-180,-180},{-90,-180},{0,-180},{90,-180},{180,-180}}
    };
    
    private GameObject[,] _blocks = new GameObject[5,5]; 
    
    [SerializeField] private GameObject _block;

    void Start()
    { 
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
        
        for (int i = 0; i < _position.GetLength(0); i++)
        {
            for (int j = 0; j < _position.GetLength(1); j++)
            {
                int x = _position[i,j, 0];
                int y = _position[i,j, 1];
                var instantiateBlock = Instantiate(_block,this.transform,false);
                instantiateBlock.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                _blocks[i,j] = instantiateBlock;
                instantiateBlock.GetComponent<Blocks>().SetPosi(i, j);
                instantiateBlock.GetComponent<Blocks>().SetChar(_board[i, j].ToString());
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
    
    public bool CheckWord(string word)
    {
        bool correct = _words.Contains(word);
        if (correct || true)
        {
            RemoveWordFromBlocks();
            FallOfBlocks();
            for (int i = 0; i < _countNewBlocks.GetLength(0); i++)
            {
                // GameObject[] tempArrayForBlocks = new GameObject[_countNewBlocks[i]];
                List <GameObject> tempArrayForBlocks = new List<GameObject>();
                for (int j = 0; j < _countNewBlocks[i]; j++)
                {
                    var instantiateBlock = Instantiate(_block, this.transform, false);
                    instantiateBlock.GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(-180 + (i * 90), 270 + j * 90);
                    instantiateBlock.GetComponent<Blocks>().SetPosi(i,-1);
                    // tempArrayForBlocks[j] = instantiateBlock;
                    tempArrayForBlocks.Add(instantiateBlock);
                }

                tempArrayForBlocks.Reverse();
                for (int j = _countNewBlocks[i] - 1; j >= 0; j--)
                {
                    tempArrayForBlocks[j].GetComponent<Blocks>().SetDestPosi(j,i);
                    _blocks[j, i] = tempArrayForBlocks[j];
                    var value = GenerateCharacter();
                    _blocks[j,i].GetComponent<Blocks>().SetChar(value.ToString());
                    _board[j,i] = value;
                }
            }
            _countNewBlocks = new[] {0, 0, 0, 0, 0};
        }

        foreach (var b in _blocks)
        {

            Blocks block = b.GetComponent<Blocks>();
            if (block.GetPosi()[0] != block.GetDestPosi()[0] || block.GetPosi()[1] != block.GetDestPosi()[1])
            {
                block.MoveBlock(new Vector2(_position[block.GetDestPosi()[0], block.GetDestPosi()[1], 0],
                    _position[block.GetDestPosi()[0], block.GetDestPosi()[1], 1]));
                block.SetPosi(block.GetDestPosi()[0],block.GetDestPosi()[1]);
            }
            
        }
        return correct;
    }
    
    private void RemoveWordFromBlocks()
    {
        List<GameObject> selected = transform.GetChild(0).GetComponent<Blocks>().GetSelected();
        foreach (var selectedBlock in selected)
        {
            Destroy(selectedBlock);
            int[] posi = selectedBlock.GetComponent<Blocks>().GetPosi();
            _blocks[posi[0], posi[1]] = null;
            _countNewBlocks[posi[1]] += 1;
        }
    }
    
    private void FallOfBlocks()
    {
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                if (_blocks[i, j] == null)
                {
                    for (int z = i; z > 0; z--)
                    {
                        _blocks[z,j] = _blocks[z - 1, j];
                        _blocks[z - 1, j] = null;
                        _board[z,j] = _board[z - 1, j];
                        if (_blocks[z, j] != null)
                            _blocks[z, j].GetComponent<Blocks>().SetDestPosi(z, j);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string boardString = "";
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    boardString += _board[i, j] + " ";
                }
                boardString += "\n";
            }

            Debug.Log(boardString);
            var temp = _blocks;
            var a = _blocks[1,2].name;
            Debug.Log(a);
        }
    }
}
