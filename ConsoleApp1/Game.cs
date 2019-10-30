using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Raylib;
using rl = Raylib.Raylib;

namespace MatrixHeirarchy
{
    class Game
    {
        Stopwatch stopwatch = new Stopwatch();

        //Initialize game objects
        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();
        SceneObject bulletObject = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();
        SpriteObject bulletSprite = new SpriteObject();

        //Initialize object variables
        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;

        private float deltaTime = 0.005f;

        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            //Load sprites
            tankSprite.Load("tankBlue_outline.png");
            //Face tank the right way
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            //Set offset for base to rotate around center
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);

            bulletSprite.Load("tank_explosion5.png");
            bulletSprite.SetRotate(90 * (float)(Math.PI / 180.0f));
            bulletSprite.imgScale = 1.0f;

            turretSprite.Load("barrelBlue.png");
            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            //Set turret offset from tank base
            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);

            tankObject.SetPosition(rl.GetScreenWidth() / 2.0f, rl.GetScreenHeight() / 2.0f);

            //Attach children
            bulletObject.AddChild(bulletSprite);
            turretObject.AddChild(turretSprite);
            tankObject.AddChild(tankSprite);

            tankObject.AddChild(turretObject);
        }

        public void Shutdown()
        { }

        public void Update()
        {
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;

            timer += deltaTime;
            if(timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;

            //Keyboard presses
            if (rl.IsKeyDown(KeyboardKey.KEY_A))
            {
                tankObject.Rotate(-deltaTime);
            }
            if (rl.IsKeyDown(KeyboardKey.KEY_D))
            {
                tankObject.Rotate(deltaTime);
            }
            if (rl.IsKeyDown(KeyboardKey.KEY_W))
            {
                Vector3 facing = new Vector3(
               tankObject.LocalTransform.x1,
               tankObject.LocalTransform.y1, 1) * deltaTime * 100;
                tankObject.Translate(facing.x, facing.y);
            }
            if (rl.IsKeyDown(KeyboardKey.KEY_S))
            {
                Vector3 facing = new Vector3(
               tankObject.LocalTransform.x1,
               tankObject.LocalTransform.y1, 1) * deltaTime * -100;
                tankObject.Translate(facing.x, facing.y);
            }
            if(rl.IsKeyDown(KeyboardKey.KEY_Q))
            {
                turretObject.Rotate(-deltaTime);
            }
            if(rl.IsKeyDown(KeyboardKey.KEY_E))
            {
                turretObject.Rotate(deltaTime);
            }

            float xR = turretSprite.GlobalTransform.x1;
            float yR = turretSprite.GlobalTransform.x2;
            float rot = (float)Math.Atan2(xR, yR);

            if (rl.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                turretObject.AddChild(bulletObject);
                bulletObject.active = true;
                bulletObject.SetPosition(65, -5.5f);

                float xPos = bulletObject.GlobalTransform.x3;
                float yPos = bulletObject.GlobalTransform.y3;
      
                // unparent the bullet from the tank
                turretObject.RemoveChild(bulletObject);

                // restore the transform data
                bulletObject.SetRotate(rot);
                bulletObject.SetPosition(xPos, yPos);
            }
            
            //Collision boundaries
            if (bulletObject.GlobalTransform.x3 > 630 || bulletObject.GlobalTransform.x3 < 10 || bulletObject.GlobalTransform.y3 > 470 || bulletObject.GlobalTransform.y3 < 10)
            {
                bulletObject.active = false;
            }

            lastTime = currentTime;

            if (bulletObject.active)
            {
                bulletSprite.Draw();

                Vector3 facing = new Vector3(
                bulletObject.GlobalTransform.x1,
                bulletObject.GlobalTransform.y1, 1) 
                * deltaTime * 250;

                bulletObject.Translate(facing.x, facing.y);
            }
        }

        public void Draw()
        {
            rl.BeginDrawing();

            rl.ClearBackground(Color.WHITE);
            rl.DrawText(fps.ToString(), 10, 10, 12, Color.RED);

            tankObject.Draw();

            rl.EndDrawing();
        }
    }
}

