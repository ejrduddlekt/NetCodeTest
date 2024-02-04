using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;

public class RelayManager : MonoBehaviour
{
    public struct RelayHostData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDByte;
        public byte[] ConnectionData;
        public byte[] Key;
    }

    public struct RelayJoinDatas
    {
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDByte;
        public byte[] ConnectionData;
        public byte[] HostConnectionData;
        public byte[] Key;
    }


    //ȣ��Ʈ���� �� Relay
    public static async Task<RelayHostData> SetUpRelay(int maxConn, string environment)
    {
        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment); //environment ����

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //�α����� �ȵǾ� �ִٸ� �͸����� �ϴ� �����Ѵ�.
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Allocation allocation = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(maxConn);

        RelayHostData data = new RelayHostData()
        {
            IPv4Address = allocation.RelayServer.IpV4,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            ConnectionData = allocation.ConnectionData,
            Key = allocation.Key,
        };
        data.JoinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        return data;
    }

    public static async Task<RelayJoinDatas> JoinRelay(string joinCode, string environment)
    {
        //Debug.LogError($"Start Join by {joinCode}");

        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        JoinAllocation allocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(joinCode);

        RelayJoinDatas data = new RelayJoinDatas()
        {
            IPv4Address = allocation.RelayServer.IpV4,
            Port = (ushort)allocation.RelayServer?.Port,

            AllocationID = allocation.AllocationId,
            AllocationIDByte = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            Key = allocation.Key,
        };

        return data;
    }
}
