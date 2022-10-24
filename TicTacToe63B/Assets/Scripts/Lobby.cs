using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

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

    List<RoomInfo> availableRooms = new List<RoomInfo>();

    UnityEngine.Events.UnityAction buttonCallback;


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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("Number of rooms:"+roomList.Count);
        //update the availableRooms with rooms that were created in the lobby
        availableRooms = roomList;
        UpdateRoomList();

    }

    private void UpdateRoomList()
    {
        foreach (RoomInfo roomInfo in availableRooms)
        {
            GameObject rowRoom = Instantiate(RowRoom);
            rowRoom.transform.parent = ScrollViewContent.transform;
            rowRoom.transform.localScale = Vector3.one;

            rowRoom.transform.Find("RoomName").GetComponent<TextMeshProUGUI>().text = roomInfo.Name;
            rowRoom.transform.Find("RoomPlayers").GetComponent<TextMeshProUGUI>().text = roomInfo.PlayerCount.ToString();

            buttonCallback = () => OnClickJoinRoom(roomInfo.Name);
            rowRoom.transform.Find("BtnJoin").GetComponent<Button>().onClick.AddListener(buttonCallback);
        }
    }

    public void OnClickJoinRoom(string roomName)
    {
 
        //set our player name
        PhotonNetwork.NickName = InputPlayerName.GetComponent<TMP_InputField>().text;

        //join the room
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        PanelLobby.SetActive(false);
        PanelWaitingForPlayer.SetActive(true);
        print("OnJoinedRoom");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        print("test");
        //base.OnPlayerEnteredRoom(newPlayer);
        LoadMainGame();
    }

    private void LoadMainGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //load the scene to start the game (make sure it is added to the build settings)
            PhotonNetwork.LoadLevel("MainGame");
        }
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
