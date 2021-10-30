using Core;
using System;

namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var userInterface = new ConsoleUserInterface();
            var gameManager = new GameManager(userInterface);
            gameManager.Run();
        }
    }
}
