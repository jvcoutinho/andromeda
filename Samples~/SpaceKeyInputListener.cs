using System;
using Andromeda;
using UnityEngine;

namespace Samples
{

    public class SpaceKeyInputListener : EventOrientedBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Raise(new SpaceKeyPressedEvent(true));
            }
            
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                Raise(new SpaceKeyPressedEvent(false));
            }
        }
    }
}