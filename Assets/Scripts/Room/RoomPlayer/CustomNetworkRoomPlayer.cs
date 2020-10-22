﻿using UnityEngine;
using Mirror;
using System;

/*
	Documentation: https://mirror-networking.com/docs/Components/NetworkRoomPlayer.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkRoomPlayer.html
*/

/// <summary>
/// This component works in conjunction with the NetworkRoomManager to make up the multiplayer room system.
/// The RoomPrefab object of the NetworkRoomManager must have this component on it.
/// This component holds basic room player data required for the room to function.
/// Game specific data for room players can be put in other components on the RoomPrefab or in scripts derived from NetworkRoomPlayer.
/// </summary>
public class CustomNetworkRoomPlayer : NetworkRoomPlayer
{
    private RoomPlayerData roomPlayerData;

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() { }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistant storage</para>
    /// </summary>
    public override void OnStopServer() { }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient()
    {
        this.roomPlayerData = GetComponent<RoomPlayerData>();
    }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient() { }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer() { }

    /// <summary>
    /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
    /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
    /// <para>When <see cref="NetworkIdentity.AssignClientAuthority"/> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnection parameter included, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStartAuthority() { }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority() { }

    #endregion

    #region Room Client Callbacks

    /// <summary>
    /// This is a hook that is invoked on all player objects when entering the room.
    /// <para>Note: isLocalPlayer is not guaranteed to be set until OnStartLocalPlayer is called.</para>
    /// </summary>
    public override void OnClientEnterRoom() { }

    /// <summary>
    /// This is a hook that is invoked on all player objects when exiting the room.
    /// </summary>
    public override void OnClientExitRoom() { }

    #endregion

    #region SyncVar Hooks

    /// <summary>
    /// This is a hook that is invoked on clients when the index changes.
    /// </summary>
    /// <param name="oldIndex">The old index value</param>
    /// <param name="newIndex">The new index value</param>
    public override void IndexChanged(int oldIndex, int newIndex) { }

    /// <summary>
    /// This is a hook that is invoked on clients when a RoomPlayer switches between ready or not ready.
    /// <para>This function is called when the a client player calls SendReadyToBeginMessage() or SendNotReadyToBeginMessage().</para>
    /// </summary>
    /// <param name="readyState">Whether the player is ready or not.</param>
    public override void ReadyStateChanged(bool _, bool readyState)
    {
        base.ReadyStateChanged(_, readyState);
    }

    #endregion

    #region Optional UI

    public override void OnGUI()
    {
        if (!showRoomGUI)
            return;

        NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
        if (room)
        {
            if (!NetworkManager.IsSceneActive(room.RoomScene))
                return;

            DrawPlayerReadyState();
            DrawPlayerReadyButton();
        }
    }

    void DrawPlayerReadyState()
    {
        GUILayout.BeginArea(new Rect(20f + (index * 100), 200f, 90f, 130f));

        GUILayout.Label(this.roomPlayerData.PlayerIndentifier);

        if (readyToBegin)
            GUILayout.Label("Ready");
        else
            GUILayout.Label("Not Ready");

        if (isLocalPlayer)
        {
            switch (roomPlayerData.PlayerType)
            {
                case PlayerType.THIEF:
                    if (GUILayout.Button($"{PlayerType.THIEF}"))
                        roomPlayerData.CmdChangePlayerType(PlayerType.VIGIL);
                    break;
                case PlayerType.VIGIL:
                    if (GUILayout.Button($"{PlayerType.VIGIL}"))
                        roomPlayerData.CmdChangePlayerType(PlayerType.SPECTATOR);
                    break;
                case PlayerType.SPECTATOR:
                    if (GUILayout.Button($"{PlayerType.SPECTATOR}"))
                        roomPlayerData.CmdChangePlayerType(PlayerType.THIEF);
                    break;
                default:
                    throw new NotImplementedException($"The value {roomPlayerData.PlayerType} is not implemented");
            }
        }
        else
        {
            GUILayout.Label($"{roomPlayerData.PlayerType}");
        }
        

        if (((isServer && index > 0) || isServerOnly) && GUILayout.Button("REMOVE"))
        {
            // This button only shows on the Host for all players other than the Host
            // Host and Players can't remove themselves (stop the client instead)
            // Host can kick a Player this way.
            GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
        }

        GUILayout.EndArea();
    }

    void DrawPlayerReadyButton()
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
                if(VivoxVoiceManager.Instance.LoginState != VivoxUnity.LoginState.LoggedIn)
                {
                    GUILayout.Label("Loading");
                }
                else
                {
                    if (GUILayout.Button("Ready"))
                        CmdChangeReadyState(true);
                }
            }

            GUILayout.EndArea();
        }
    }

    #endregion
}
