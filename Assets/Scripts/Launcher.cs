using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text errorMessage;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomItemPrefab;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerItemPrefab;
    [SerializeField] GameObject BotonStart;
    [SerializeField] TMP_InputField nicknameInputField;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        // conectar al master 
        Debug.Log("Conectando");
        //MenuManager.Instance.OpenMenuName("Loading"); (paso 60)
        PhotonNetwork.ConnectUsingSettings();
        MenuManager.Instance.OpenMenuName("Loading");
    }
    public override void OnConnectedToMaster()
    {
        //borran la siguiente linea es el comportamiento base del metodo 
        // base.OnConnectedToMaster(); 
        Debug.Log("Conectado");
        MenuManager.Instance.OpenMenuName("Start");
        PhotonNetwork.NickName = "Jugador" + Random.Range(0, 1000).ToString("0000");
    }

    public void StartMenu()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        //borran la base 
        // base.OnJoinedLobby();
        //MenuManager.Instance.OpenMenuName("Home"); (paso 61) 
        Debug.Log("Conectado al lobby ");
        MenuManager.Instance.OpenMenuName("Home");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        if(string.IsNullOrEmpty(nicknameInputField.text))
        {
            return;
        }

        PhotonNetwork.NickName = nicknameInputField.text + Random.Range(0, 1000).ToString("0000");

        PlayerPrefs.SetString("Username", nicknameInputField.text);

        PhotonNetwork.CreateRoom(roomNameInputField.text);

        MenuManager.Instance.OpenMenuName("Loading");
    }


    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenuName("Room");
        roomName.text = PhotonNetwork.CurrentRoom.Name;

        foreach (Transform playerT in PlayerListContent)
        { Destroy(playerT.gameObject); }

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerItemPrefab,
            PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        BotonStart.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorMessage.text = "Error al crear la sala:" + message;
        MenuManager.Instance.OpenMenuName("Error");
    }

    public void JoinRoom(RoomInfo _info)
    {
        PhotonNetwork.JoinRoom(_info.Name);
        MenuManager.Instance.OpenMenuName("Loading");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenuName("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenuName("Home");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transfo in roomListContent)
        {
            //destruimos prefabs de todas las salas anteriores para 
            //cargar la nueva lista de salas
            Destroy(transfo.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            { continue; }
            // Creamos un prefab por cada sala que exista
            //Se Agrega la informacion de la sala
            // LLamamos Setup para que agrege las salas existentes
            Instantiate(roomItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void StartGame()
    {
        // el 1 es porque es el numero de build de nuestra escena de juego
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        BotonStart.SetActive(PhotonNetwork.IsMasterClient);
    }

}
