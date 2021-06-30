using LSeeDee.Services;

namespace LSeeDee.Screens
{
    public class HelloWorldScreen : IScreen
    {
        private readonly Display _display;

        public HelloWorldScreen(Display display)
        {
            _display = display;
        }

        public void Draw()
        {
            _display.WriteText(1, 7, "Hello");
            _display.WriteText(2, 7, "World");
        }

        public bool IsEnabled()
        {
            return true;
        }
    }
}