using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (pv.IsMine)
        {
            CreatePlayerController();
        }
    }

    public void CreatePlayerController()
    {
        // de momento iniciara en el centro pero despues se cambiara a ramdons positions
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
    }

}
