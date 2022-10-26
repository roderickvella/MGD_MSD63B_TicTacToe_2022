using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);
    }

    /// <summary>
    /// This method is called by one of the devices (when the player clicks on any boardpiece (from the canvasmanager)
    /// It informs PhotonCloud to run RPC_NotifySelectBoardPiece on every connected device in the room
    /// </summary>
    /// <param name="gameObjectBoardPiece">the board piece that has been selected (clicked) by the user</param>
    public void NotifySelectBoardPiece(GameObject gameObjectBoardPiece)
    {
        if((int)GetComponent<GameManager>().currentActivePlayer.id == PhotonNetwork.LocalPlayer.ActorNumber)
            photonView.RPC("RPC_NotifySelectBoardPiece", RpcTarget.All, gameObjectBoardPiece.name);
    }

    /// <summary>
    /// This method is run automatically by Photon when triggered from NotifySelectBoardPiece method.
    /// So basically the SelectBoardPiece method is called on every connected device (separately)
    /// </summary>
    /// <param name="gameObjectBoardPieceName">the name of the boardpiece game object. ex: Loc0-0</param>
    [PunRPC]
    public void RPC_NotifySelectBoardPiece(string gameObjectBoardPieceName)
    {
        GetComponent<GameManager>().SelectBoardPiece(GameObject.Find(gameObjectBoardPieceName));
    }
}
