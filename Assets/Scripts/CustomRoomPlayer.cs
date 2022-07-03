using UnityEngine;
using Mirror;

public class CustomRoomPlayer : NetworkRoomPlayer
{
    [SyncVar]
    private int _ñonnectionId;

    [SyncVar]
    public string playerName;

    [ClientRpc]
    public void RpcSetPlayerName(string newName)
    {
        playerName = newName;
    }

    public void SetConnectionId(int id)
    {
        _ñonnectionId = id;
    }

    public override void OnGUI()
    {
        if (!showRoomGUI)
            return;

        NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
        if (room)
        {
            if (!room.showRoomGUI)
                return;

            if (!NetworkManager.IsSceneActive(room.RoomScene))
                return;

            DrawPlayerReadyState();
            DrawPlayerReadyButton();
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(PlayerPrefs.GetString(MainMenuManager.PlayerPrefsNameKey));
    }

    [Command]
    private void CmdSetPlayerName(string name)
    {
        playerName = name;
        RpcSetPlayerName(name);
    }

    private void DrawPlayerReadyState()
    {
        GUILayout.BeginArea(new Rect(20f + (index * 100), 200f, 90f, 130f));

        GUILayout.Label($"{playerName}");

        if (readyToBegin)
            GUILayout.Label("Ready");
        else
            GUILayout.Label("Not Ready");

        if (((isServer && index > 0) || isServerOnly) && GUILayout.Button("REMOVE"))
        {
            // This button only shows on the Host for all players other than the Host
            // Host and Players can't remove themselves (stop the client instead)
            // Host can kick a Player this way.
            GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
        }

        GUILayout.EndArea();
    }

    private void DrawPlayerReadyButton()
    {
        if (NetworkClient.active && isLocalPlayer)
        {
            GUILayout.BeginArea(new Rect(20f, 300f, 120f, 20f));

            if (readyToBegin)
            {
                if (GUILayout.Button("Cancel"))
                    CmdChangeReadyState(false);
            }
            else
            {
                if (GUILayout.Button("Ready"))
                    CmdChangeReadyState(true);
            }

            GUILayout.EndArea();
        }
    }
}
