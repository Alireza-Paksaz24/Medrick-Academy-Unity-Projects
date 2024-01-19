using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _level = 1;

    public int level
    {
        get => _level;
        set => _level= value;
    }
}