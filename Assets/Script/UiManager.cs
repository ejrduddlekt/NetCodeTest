using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UiManager : MonoBehaviour
{
    public Button Host;
    public Button Client;

    private void Start()
    {
        Host.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        Client.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
