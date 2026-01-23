using UnityEngine;

public interface IPlayerInputReceiver
{
    void ReceivePlayerCommand(PlayerCommand command);
}
