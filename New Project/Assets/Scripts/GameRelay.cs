using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class GameRelay : MonoBehaviour
{
    public GameRelay Instance{ get; private set; }  


    public async Task<Allocation> CreateRelay(int maxPlayers)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            RelayServerData serverData = new RelayServerData(allocation, "udp");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
            return allocation;
        }
        catch (RelayServiceException ex)
        {
            throw ex;
        }
    }

    public async Task<JoinAllocation> JoinRelay(string joinCode)
    {
        try
        {
           JoinAllocation joinallocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData serverData = new RelayServerData(joinallocation, "udp");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
            return joinallocation;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task<string> GetRelayJoinCode(Allocation allocation)
    {
        try
        {
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return relayJoinCode;
        }
        catch (RelayServiceException ex)
        {
            throw ex;
        }
        
    }
}
