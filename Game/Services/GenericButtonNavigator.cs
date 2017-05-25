using System;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Services
{
    public class GenericButtonNavigator<T>
    {
        public T CurrentPosition { get; set; }
        public T[] Positions { get; }
        public bool HorizontalNavigation { get; }
        public ButtonNavigator ButtonNavigator { get; }

        public GenericButtonNavigator(T[] positions, bool horizontalNavigation = false)
        {
            if (positions.Length < 1)
            {
                throw new Exception("Need at least one element to init GenericButtonNavigator");
            }
            
            Positions = positions;
            HorizontalNavigation = horizontalNavigation;
            CurrentPosition = positions[0];
            ButtonNavigator = new ButtonNavigator(positions.Length);
        }

        public void UpdatePosition(VirtualGamePad virtualGamePad)
        {
            var position = ButtonNavigator.CurrentIndex;
            if (GoNext(virtualGamePad))
            {
                position = ButtonNavigator.NextIndex();
            }
            if (GoPrevious(virtualGamePad))
            {
                position = ButtonNavigator.PreviousIndex();
            }
            CurrentPosition = Positions[position];
        }

        public bool GoNext(VirtualGamePad virtualGamePad)
        {
            if (HorizontalNavigation)
            {
                return virtualGamePad.Is(Right, Pressed);
            }
            return virtualGamePad.Is(Down, Pressed);
        }

        private bool GoPrevious(VirtualGamePad virtualGamePad)
        {
            if (HorizontalNavigation)
            {
                return virtualGamePad.Is(Left, Pressed);
            }
            return virtualGamePad.Is(Up, Pressed);
        }
    }
}