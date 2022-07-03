using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using Mirror;

public class MatchManager : NetworkBehaviour
{
    [SerializeField] private float _matchRestartDelay;
    
    private CustomNetworkRoomManager _networkManager;
    private List<Player> _players = new List<Player>();

    [SyncVar]
    public string winnerName;

    public static MatchManager Instance { get; private set; }
    
    public event UnityAction MatchRestarting;
    public event UnityAction MatchRestarted;

    public void OnPlayerSpawned(Player player)
    {
        _players.Add(player);
        player.Hitted += CheckScores; 
    }

    private void Awake()
    {
        Instance = this;
        _networkManager = FindObjectOfType<CustomNetworkRoomManager>();
    }

    [Server]
    private void CheckScores()
    {
        foreach (var player in _players)
        {
            if (player.Score >= 3)
            {
                winnerName = player.playerName;
                MatchRestarting?.Invoke();
                StartCoroutine(Restart());
            }
        }
    }

    [Server]
    private IEnumerator Restart()
    {
        foreach (var player in _players)
            player.Hitted -= CheckScores; 

        yield return new WaitForSeconds(_matchRestartDelay);

        _networkManager.RestartGame();
        MatchRestarted?.Invoke();
        OnRestarted();
    }

    [Server]
    private void OnRestarted()
    {
        foreach (var player in _players)
            player.Hitted += CheckScores;

        winnerName = string.Empty;
    }
}
