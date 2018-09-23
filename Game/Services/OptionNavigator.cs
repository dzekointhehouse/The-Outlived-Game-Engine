using System;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Services
{
    public class OptionNavigator<T>
    {
        public T CurrentPosition { get; set; }
        public T[] Options { get; }
        public bool HorizontalNavigation { get; }
        public int CurrentIndex { get; set; }
        // if true, the options will go in a loop.
        private bool LoopOptions { get; }
        private int NumberOfOptions { get; }

        public OptionNavigator(T[] options, bool horizontalNavigation = false, bool loop = false)
        {
            if (options.Length < 1)
            {
                throw new Exception("Need at least one element to init GenericMenuNavigator");
            }
            
            Options = options;
            HorizontalNavigation = horizontalNavigation;
            CurrentPosition = options[0];
            NumberOfOptions = options.Length;
            LoopOptions = loop;
        }

        public void UpdatePosition(VirtualGamePad virtualGamePad)
        {
            var position = CurrentIndex;
            if (GoNext(virtualGamePad))
            {
                position = MoveToNextOption();
            }
            if (GoPrevious(virtualGamePad))
            {
                position = MoveToPreviousOption();
            }
            CurrentPosition = Options[position];
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

        private int MoveToNextOption()
        {
            var isLastOption = CurrentIndex == NumberOfOptions - 1;
            if (isLastOption && LoopOptions)
            {
                CurrentIndex = 0;
            }
            else if (!isLastOption)
            {
                CurrentIndex++;
            }
            return CurrentIndex;
        }

        private int MoveToPreviousOption()
        {
            var isLastOption = CurrentIndex == 0;
            if (isLastOption && LoopOptions)
            {
                CurrentIndex = NumberOfOptions - 1;
            }
            else if (!isLastOption)
            {
                CurrentIndex--;
            }
            return CurrentIndex;
        }
    }
}