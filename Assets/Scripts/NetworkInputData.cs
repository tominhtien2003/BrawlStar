using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector2 direction;
    public const byte BUTTONSHOOT = 1;
    public NetworkButtons buttons;
}
