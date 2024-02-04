using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;

public class UiManager : MonoBehaviour
{
    public Button Host;
    public Button Client;
    public TMP_InputField inputField;

    private void Start()
    {
        Host.onClick.AddListener(async () =>
        {
            var data = await RelayManager.SetUpRelay(10, "production");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data.IPv4Address, data.Port, data.AllocationIDByte, data.Key, data.ConnectionData);

            inputField.text = data.JoinCode;

            NetworkManager.Singleton.StartHost();
        });

        Client.onClick.AddListener(async () =>
        {
            var data = await RelayManager.JoinRelay(inputField.text, "production");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data.IPv4Address, data.Port, data.AllocationIDByte, data.Key, data.ConnectionData, data.HostConnectionData);

            NetworkManager.Singleton.StartClient();
        });
    }
}
