using UnityEngine;
using Mirror;

[RequireComponent(typeof(Player))]
public class PlayerUI : NetworkBehaviour
{
    private Player _player;

    [SyncVar]
    public int index;
    [SyncVar]
    public string playerName;

    public uint Score => _player.Score;

    private void Start()
    {
        _player = GetComponent<Player>();
        MatchManager.Instance.MatchRestarting += OnMatchRestarting;
        MatchManager.Instance.MatchRestarted += OnMatchRestarted;
    }

    private void OnDestroy()
    {
        MatchManager.Instance.MatchRestarting -= OnMatchRestarting;
        MatchManager.Instance.MatchRestarted -= OnMatchRestarted;
    }

    [Client]
    private void OnGUI()
    {
        GUI.Box(new Rect(10f + (index * 110), 10f, 100f, 25f), $"{playerName}: {Score}");
    }

    [Server]
    private void ShowWinner(string playerName)
    {
        TextMessageBox.Instance.text = $"Winner! Winner! Winner! Chicken dinner!\n" +
                                       $"{playerName} wins the match!";
    }

    [Server]
    private void OnMatchRestarting()
    {
        ShowWinner(MatchManager.Instance.winnerName);
    }

    [Server]
    private void OnMatchRestarted()
    {
        TextMessageBox.Instance.text = string.Empty;
    }
}

