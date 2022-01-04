using Andromeda;

namespace Samples
{
    public struct SpaceKeyPressedEvent : IEvent
    {
        public SpaceKeyPressedEvent(bool downPress)
        {
            DownPress = downPress;
        }
        
        public bool DownPress { get; }
    }
}