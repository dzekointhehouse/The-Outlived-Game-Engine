using System.Reflection;

namespace Game.Services
{
    public class ButtonNavigator
    {
        public int CurrentIndex { get; set; }
        //When true will rotate last index to the first when reached max indices
        private bool RotateIndices { get; }

        private int ButtonCount { get; }

        public ButtonNavigator(int buttonCount, bool rotateIndices = false)
        {
            ButtonCount = buttonCount;
            RotateIndices = rotateIndices;
        }

        public int NextIndex()
        {
            var atLastButton = CurrentIndex == ButtonCount - 1;
            if (atLastButton && RotateIndices)
            {
                CurrentIndex = 0;
            }
            else if (!atLastButton)
            {
                CurrentIndex++;
            }
            return CurrentIndex;
        }

        public int PreviousIndex()
        {
            var atFirstButton = CurrentIndex == 0;
            if (atFirstButton && RotateIndices)
            {
                CurrentIndex = ButtonCount - 1;
            }
            else if (!atFirstButton)
            {
                CurrentIndex--;
            }
            return CurrentIndex;
        }
    }
}