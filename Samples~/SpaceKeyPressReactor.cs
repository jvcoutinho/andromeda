using Andromeda;
using UnityEngine;

namespace Samples
{
    public class SpaceKeyPressReactor : EventOrientedBehaviour
    {
        [Observes]
        private void OnEventRaised(SpaceKeyPressedEvent @event)
        {
            Debug.Log($"Space got pressed {(@event.DownPress ? "down" : "up")}! Wait... Who pressed it?");
        }
    }
}