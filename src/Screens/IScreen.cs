using System.Threading.Tasks;

namespace LSeeDee.Screens
{
    public interface IScreen
    {
        bool IsEnabled();
        void Draw();
    }
}