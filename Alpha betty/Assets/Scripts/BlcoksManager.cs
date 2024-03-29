using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BlcoksManager : MonoBehaviour
{
    public TextAsset englishWordsFile;
    private int[] _countNewBlocks = new []{0,0,0,0,0};
    // Split the text file content into an array of lines
    private string[] _words;
    private char[,] _board = new char[5, 5];
    private int[,,] _position =
    {
        {{-306, 306}, {-153, 306}, {0, 306}, {153, 306}, {306, 306}},
        {{-306, 153}, {-153, 153}, {0, 153}, {153, 153}, {306, 153}},
        {{-306, 0},   {-153, 0},   {0, 0},   {153, 0},   {306, 0}},
        {{-306, -153},{-153, -153},{0, -153},{153, -153},{306, -153}},
        {{-306, -306},{-153, -306},{0, -306},{153, -306},{306, -306}}
    };

    // Dictionary to store frequencies of each letter
    private Dictionary<char, int> letterFrequencies = new Dictionary<char, int>()
    {
        {'A', 8167}, {'B', 1492}, {'C', 2782}, {'D', 4253}, {'E', 12702},
        {'F', 2228}, {'G', 2015}, {'H', 6094}, {'I', 6966}, {'J', 153},
        {'K', 772}, {'L', 4025}, {'M', 2406}, {'N', 6749}, {'O', 7507},
        {'P', 1929}, {'Q', 95}, {'R', 5987}, {'S', 6327}, {'T', 9056},
        {'U', 2758}, {'V', 978}, {'W', 2360}, {'X', 150}, {'Y', 1974},
        {'Z', 74}
    };
    private Dictionary<char, int> _letterScores = new Dictionary<char, int>()
    {
        {'E', 1}, {'T', 2}, {'A', 3}, {'O', 3}, {'I', 4}, {'N', 4}, 
        {'S', 5}, {'H', 5}, {'R', 6}, {'D', 6}, {'L', 7}, {'C', 7}, 
        {'U', 8}, {'M', 8}, {'W', 9}, {'F', 9}, {'G', 10}, {'Y', 10},
        {'P', 11}, {'B', 11}, {'V', 12}, {'K', 12}, {'J', 13}, {'X', 14}, 
        {'Q', 14}, {'Z', 15}
    };
    private GameObject[,] _blocks = new GameObject[5,5]; 
    
    [SerializeField] private GameObject _block;
    [SerializeField] private TypingMachine _typingMachine;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image _timePanel;
    
    void Start()
    {
        
        _timePanel.DOFillAmount(1f, 90).SetEase(Ease.Linear).OnComplete(() => SceneManager.LoadScene("Game Over"));
        _words = englishWordsFile.text.Split('\n');
        // Select a random word with less than 25 characters
        string chosenWord = _words[Random.Range(0,_words.Length)];
        chosenWord = chosenWord.Replace("\r","");
        chosenWord = chosenWord.ToUpper();
        Debug.Log(chosenWord);
        // Try to place the word
        if (!PlaceWordInBoard(_board, chosenWord))
        {
            Debug.LogError("Failed to place the word. Trying again...");
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
        // Calculate total frequency
        int totalFrequency = 0;
        foreach (int frequency in letterFrequencies.Values)
        {
            totalFrequency += frequency;
        }

        // Generate a random number within the total frequency
        int randomIndex = Random.Range(0, totalFrequency);

        // Iterate through the frequencies to find the corresponding letter
        int cumulativeFrequency = 0;
        foreach (KeyValuePair<char, int> pair in letterFrequencies)
        {
            cumulativeFrequency += pair.Value;
            if (randomIndex < cumulativeFrequency)
            {
                return pair.Key;
            }
        }

        // Default return value (should never happen)
        return 'A';
    }
    
    public bool SearchWord(string word)
    {
        if (word.Length <= 2)
            return false;
        // Ensure that the TextAsset is assigned
        if (englishWordsFile != null)
        {
            // Iterate over each word and check if it matches the input word
            foreach (string w in _words)
            {
                // Perform case-insensitive comparison
                if (string.Equals(w.Trim(), word, System.StringComparison.OrdinalIgnoreCase))
                {
                    return true; // Word exists
                }
            }
        }
        else
        {
            Debug.LogError("English words file is not assigned!");
        }

        return false; // Word does not exist or file is not assigned
    }
    private int CalculateWordScore(string word)
    {
        int score = 0;
        Dictionary<char, int> letterCounts = new Dictionary<char, int>();

        // Count the occurrence of each letter in the word
        foreach (char letter in word.ToUpper())
        {
            if (_letterScores.ContainsKey(letter))
            {
                if (!letterCounts.ContainsKey(letter))
                {
                    letterCounts[letter] = 0;
                }
                letterCounts[letter]++;
            }
        }

        // Calculate the score based on the letter scores and their counts
        foreach (var letterCount in letterCounts)
        {
            score += _letterScores[letterCount.Key] * letterCount.Value;
        }

        return score;
    }
    
    public bool CheckWord(string word)
    {
        bool correct = SearchWord(word.ToLower());
        _typingMachine.Confirm(correct);
        GameObject.Find("Sound Manager").GetComponent<SoundEffectControl>().OnEnd(correct);
        if (correct)
        {
            StaticValues.words.Add(word);
            StaticValues.score += CalculateWordScore(word);
            _scoreText.text = "Score: " + Convert.ToString(StaticValues.score);
            RemoveWordFromBlocks();
            FallOfBlocks();
            for (int i = 0; i < _countNewBlocks.GetLength(0); i++)
            {
                List <GameObject> tempArrayForBlocks = new List<GameObject>();
                for (int j = 0; j < _countNewBlocks[i]; j++)
                {
                    var instantiateBlock = Instantiate(_block, this.transform, false);
                    instantiateBlock.GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(-306 + (i * 153), 459 + j * 153);
                    instantiateBlock.GetComponent<Blocks>().SetPosi(i,-1);
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

        MoveBlocks();
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

    public void TypeInMachine(string word)
    {
        _typingMachine.InputWord(word);
    }

    private void MoveBlocks()
    {
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
    }
    public void Shuffle2DArray()
    {
        int rowCount = _blocks.GetLength(0);
        int columnCount = _blocks.GetLength(1);
        
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                int randomRow = Random.Range(0,rowCount);
                int randomColumn = Random.Range(0,columnCount);
                
                // Swap elements
                (_blocks[row, column], _blocks[randomRow, randomColumn]) = (_blocks[randomRow, randomColumn], _blocks[row, column]);
                (_board[row, column], _board[randomRow, randomColumn]) = (_board[randomRow, randomColumn], _board[row, column]);
                // Set new position for the swapped elements
                _blocks[row, column].GetComponent<Blocks>() .SetDestPosi(row, column);
                _blocks[randomRow, randomColumn].GetComponent<Blocks>().SetDestPosi(randomRow, randomColumn);
            }
        }
        MoveBlocks();
    }
}
