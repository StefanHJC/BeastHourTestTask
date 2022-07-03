using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    private List<Player> _activePlayers = new List<Player>();

    public void RestartGame()
    {
        Time.timeScale = 0;        
        foreach (var player in _activePlayers)
        {
            // if player leave the game during match remove him from list
            if (player == null)
            {
                _activePlayers.Remove(player);
                continue;
            }
            var playernNetworkTransform = player.GetComponent<NetworkTransform>();
            Transform startPos = GetStartPosition();
            playernNetworkTransform.CmdTeleport(startPos.position);
        }
        Time.timeScale = 1;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // increment the index before adding the player, so first player starts at 1
        clientIndex++;

        if (IsSceneActive(RoomScene))
        {
            if (roomSlots.Count == maxConnections)
                return;

            allPlayersReady = false;

            GameObject newRoomGameObject = OnRoomServerCreateRoomPlayer(conn);
            NetworkServer.AddPlayerForConnection(conn, newRoomGameObject);
        }
        else
            OnRoomServerAddPlayer(conn);
    }
    
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        CustomRoomPlayer newRoomPlayer = (CustomRoomPlayer)Instantiate(roomPlayerPrefab, Vector3.zero, Quaternion.identity);
        newRoomPlayer.SetConnectionId(conn.connectionId);

        return newRoomPlayer.gameObject;
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayerGameObject)
    {
        // get start position from base class
        Transform startPos = GetStartPosition();
        GameObject gamePlayerGameObject = SpawnPlayer(conn, startPos);

        if (gamePlayerGameObject.TryGetComponent<Player>(out var gamePlayer) == false)
            throw new System.InvalidOperationException("Game Player prefab should have player script attached");

        CustomRoomPlayer roomPlayer = roomPlayerGameObject.GetComponent<CustomRoomPlayer>();

        gamePlayer.playerName = roomPlayer.playerName;
        _activePlayers.Add(gamePlayer);

        return gamePlayerGameObject;
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayerGameObject, GameObject gamePlayer)
    {
        PlayerUI playerUI = gamePlayer.GetComponent<PlayerUI>();
        CustomRoomPlayer roomPlayer = roomPlayerGameObject.GetComponent<CustomRoomPlayer>();
        playerUI.index = roomPlayer.index;
        playerUI.playerName = roomPlayer.playerName;

        return true;
    }

    private GameObject SpawnPlayer(NetworkConnectionToClient conn, Transform startPos)
    {
            GameObject gamePlayerGameObject = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        return gamePlayerGameObject;
    }
}
