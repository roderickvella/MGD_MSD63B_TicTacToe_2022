using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks
{
    [Tooltip("Scroll View Content")]
    public GameObject ScrollViewContent;

    [Tooltip("UI Row Prefab containing the room details")]
    public GameObject RowRoom;

    [Tooltip("Input Player Name")]
    public GameObject InputPlayerName;

    [Tooltip("Input Room Name")]
    public GameObject InputRoomName;

    [Tooltip("Status Message")]
    public GameObject Status;

    [Tooltip("Button Create Room")]
    public GameObject BtnCreateRoom;

    [Tooltip("Panel Lobby")]
    public GameObject PanelLobby;

    [Tooltip("Panel Waiting for Player")]
    public GameObject PanelWaitingForPlayer;


    // Start is called before the first frame update
    void Start()
    {
        //this makes sure that when master client changes level (PhotonNetwork.LoadLevel() ) 
        //all clients will sync automatically
		PhotonNetwork.AutomaticallySyncScene = true;

		if (!PhotonNetwork.IsConnected)
		{
			//set the app version before connecting
			PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "1.0";

            //connect to the photon cloud service
            PhotonNetwork.ConnectUsingSettings();
		}

        InputRoomName.GetComponent<TMP_InputField>().text = "Room1";
        InputPlayerName.GetComponent<TMP_InputField>().text = "My Name";

    }


    public override void OnConnectedToMaster()
    {
        print("OnConnectedToMaster");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected:" + cause.ToString() + " Server Address:" + PhotonNetwork.ServerAddress);
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom(InputRoomName.GetComponent<TMP_InputField>().text, roomOptions, TypedLobby.Default);

    }

    public override void OnCreatedRoom()
    {
        print("Room Created");
        PhotonNetwork.NickName = InputPlayerName.GetComponent<TMP_InputField>().text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        Status.GetComponent<TextMeshProUGUI>().text = "Status:" + PhotonNetwork.NetworkClientState.ToString();
    }
}
