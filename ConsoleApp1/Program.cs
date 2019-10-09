
using Raylib;
using rl = Raylib.Raylib;

namespace MatrixHeirarchy
{
    static class Program
    {
        public static int Main()
        {
            Game game = new Game();
            // Initialization
            //--------------------------------------------------------------------------------------
            int screenWidth = 640;
            int screenHeight = 480;

            rl.InitWindow(screenWidth, screenHeight, "Tanks For Everything!");

            game.Init();
            rl.SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!rl.WindowShouldClose())    // Detect window close button or ESC key
            {
                // TODO: Update your variables here
                //----------------------------------------------------------------------------------

                game.Update();
                game.Draw();

                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            game.Shutdown();

            rl.CloseWindow();        // Close window and OpenGL context
                                     //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
