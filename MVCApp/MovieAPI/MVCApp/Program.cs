using System.Threading.Tasks;
using MVCApp.Controllers;
using MVCApp.Views;

class Program
{
    static async Task Main()
    {
        View view = new View();
        Controller controller = new Controller(view);
        await controller.Run();
    }
}
