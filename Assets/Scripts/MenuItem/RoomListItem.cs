//librería necesaria para usar RoomInfo
using Photon.Realtime;
using TMPro;
using UnityEngine;
public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text RoomListName;
    public RoomInfo roomInfo;

    //Creamos la Funcion SetUp que tomara la informacion del room en el que estamos
    //guardaremos los datos de la sala en la roomInfo 
    public void SetUp(RoomInfo _info)
    {
        roomInfo = _info;
        RoomListName.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(roomInfo);
    }
}
