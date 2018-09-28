using System;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Services
{
    public class OptionNavigator<T> where T : Enum
    {
        public T CurrentPosition { get; set; }
        public T[] Options { get; }
        public bool HorizontalNavigation { get; }
        public int CurrentIndex { get; set; }

        // if true, the options will go in a loop.
        private bool LoopOptions { get; }
        private int NumberOfOptions { get; }
        private T InitialPosition { get; }


        public OptionNavigator(T[] options, T current = default(T), bool horizontalNavigation = false, bool loop = false)
        {
            if (options.Length < 1)
            {
                throw new Exception("Need at least one element to init GenericMenuNavigator");
            }

            Options = options;
            HorizontalNavigation = horizontalNavigation;
            InitialPosition = current != null ? current : options[0];
            CurrentPosition = InitialPosition;
            CurrentIndex = EnumToInt(CurrentPosition);
            NumberOfOptions = options.Length;
            LoopOptions = loop;
        }

        #region public methods
        /// <summary>
        /// Takes in a virtual gamepad to se if a key has been pressed, then if
        /// so - move to correct state
        /// </summary>
        /// <param name="virtualGamePad"></param>
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

        public void ResetState()
        {
            CurrentPosition = InitialPosition;
            CurrentIndex = EnumToInt(CurrentPosition);
        }
        #endregion

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


        private int EnumToInt(T value) => (int)(object)value;
    }
}   