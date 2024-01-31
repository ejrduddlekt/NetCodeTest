using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerMovement : NetworkBehaviour
{
    //NetworkVariable�� T���� �ڵ����� ����ȭ ���ִ� Ŭ�����̴�.
    //�� ���� ���������� �ٲ� �� ������, Ŭ���̾�Ʈ�� �ٲٱ� ���ؼ� RPC�� ���� �ٲ� �� �ִ�.
    public NetworkVariable<FixedString32Bytes> nicName = new NetworkVariable<FixedString32Bytes>();

    private bool w;
    private bool a;
    private bool s;
    private bool d;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner) return;

        SendNicNameServerRpc($"Player {OwnerClientId}");
    }
    private void FixedUpdate()
    {
        if (!IsOwner) //RPC�� �ƹ��� ������ �ȵǱ⿡ üũ�� �� �ؾ��Ѵ�.
            return;

        w = Input.GetKey(KeyCode.W);
        a = Input.GetKey(KeyCode.A);
        s = Input.GetKey(KeyCode.S);
        d = Input.GetKey(KeyCode.D);

        SendInputServerRpc(w, a, s, d);
    }


    [ServerRpc]
    public void SendInputServerRpc(bool w, bool a, bool s, bool d)
    {
        Vector3 input = Vector3.zero;

        if (w)
            input += Vector3.forward;

        if (a)
            input += Vector3.left;

        if (s)
            input += Vector3.back;

        if (d)
            input += Vector3.right;

        transform.Translate(input * 0.01f, Space.World);
    }

    [ServerRpc]
    public void SendNicNameServerRpc(string e)
    {
        nicName.Value = e;
    }

    //[ClientRpc]
}
