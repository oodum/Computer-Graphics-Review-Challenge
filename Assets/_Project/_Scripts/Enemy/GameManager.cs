using System;
using UnityEngine.Serialization;
using Utility;
public class GameManager : Singleton<GameManager> {
    public static event Action AttackEvent = delegate { };
    public bool IsGameOver { get; private set; }

    public void StartGame() => IsGameOver = false;

    public void EndGame() => IsGameOver = true;

    void Start() {
        StartGame();
    }

    public void Attack() {
        AttackEvent.Invoke();
    }
}
