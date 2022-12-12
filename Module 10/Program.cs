using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;


namespace Module_10
{
    class Program
    {
        static RenderWindow window;

        static Texture ballTexture;
        static Texture stickTexture;
        static Texture HardBlockTexture;
        static Texture SoftBlockTexture;


        static float YposSoftBlock = 1.0f; 
        static int YposRatioSoftBlock = 50; 
        static int YposRatioHardBlock = 250; 
        static float YposHardBlock = 1.2f; 
        static float XposSoftBlock = 2.0f; 
        static int XposRatioSoftBlock = 50; 
        static int XposRatioHardBlock = 180; 
        static float XposHardBlock = 2.0f; 

        static int maxSoftBlocks = 6; 
        static int maxHardBlocks = 3; 
        static int Try = 3;
        static int level = 1;
        static int Score = 0;
        static Sprite stick;
        static Sprite[] HardBlocks;
        static Sprite[] SoftBlocks;
        static Ball ball;
        static int[] healthPoints;
        public static void SetStartPositionSoftBlocks()
        {
            SoftBlocks = new Sprite[maxSoftBlocks * maxSoftBlocks];
            for (int i = 0; i < SoftBlocks.Length; i++) SoftBlocks[i] = new Sprite(SoftBlockTexture);
            int index = 0;
            for (int y = 0; y < maxSoftBlocks; y++)
            {
                for (int x = 0; x < maxSoftBlocks; x++)
                {
                    SoftBlocks[index].Position = new Vector2f(x * (SoftBlocks[index].TextureRect.Width + 15)*XposSoftBlock + XposRatioSoftBlock, y * (SoftBlocks[index].TextureRect.Height + 15)*YposSoftBlock + YposRatioSoftBlock);
                    index++;
                }
            }
            Score = 0;
            stick.Position = new Vector2f(400, 550);
            ball.sprite.Position = new Vector2f(375, 450);
        }
        public static void SetStartPositionHardBlocks()
        {
            int index = 0;
            healthPoints = new int[maxHardBlocks*maxHardBlocks];
            HardBlocks = new Sprite[maxHardBlocks * maxHardBlocks];
            for (int i = 0; i < HardBlocks.Length; i++) HardBlocks[i] = new Sprite(HardBlockTexture);
            for (int y = 0; y < maxHardBlocks; y++)
            {
                for (int x = 0; x < maxHardBlocks; x++)
                {
                    healthPoints[index] = 3;
                    HardBlocks[index].Position = new Vector2f(x * (HardBlocks[index].TextureRect.Width + 50)*XposHardBlock + XposRatioHardBlock, y * (HardBlocks[index].TextureRect.Height + 15)*YposHardBlock + YposRatioHardBlock);
                    index++;
                }
            }
        }

        static void Main(string[] args)
        {
            window = new RenderWindow(new VideoMode(800, 600), "Game");
            window.SetFramerateLimit(60);
            window.Closed += Window_Closed;

            Font font;
            font = new Font("ARIALUNI.TTF");
            Text text;
            ballTexture = new Texture("Ball.png");
            stickTexture = new Texture("Stick.png");
            HardBlockTexture = new Texture("Block_2.png");
            SoftBlockTexture = new Texture("Block.png");

            ball = new Ball(ballTexture);
            stick = new Sprite(stickTexture);
            HardBlocks = new Sprite[maxHardBlocks*maxHardBlocks];
            SoftBlocks = new Sprite[maxSoftBlocks*maxSoftBlocks];
            for (int i = 0; i < SoftBlocks.Length; i++) SoftBlocks[i] = new Sprite(SoftBlockTexture);
            for (int i = 0; i < HardBlocks.Length; i++) HardBlocks[i] = new Sprite(HardBlockTexture);

            SetStartPositionSoftBlocks();
            SetStartPositionHardBlocks();

            while (window.IsOpen == true)
            {
                text = new Text($"Уровень {level}. Блоков уничтожено: {Score}. Осталось попыток: {Try}. ", font, 20);
                window.Clear();
                window.SetMouseCursorVisible(false);
                window.DispatchEvents();
                window.Draw(text);

                if (Try == 0) break;
                if (level == 3) break;
                if (Score == SoftBlocks.Length+HardBlocks.Length)
                {
                    level++;
                    XposHardBlock = 1.5f;
                    XposRatioHardBlock = 160;
                    XposRatioSoftBlock = 25;
                    XposSoftBlock = 1.5f;
                    YposHardBlock = 1.0f;
                    YposRatioHardBlock = 300;
                    Try = 3;
                    ball.speed = 0;
                    maxSoftBlocks = 8;
                    maxHardBlocks = 4;
                    SetStartPositionHardBlocks();
                    SetStartPositionSoftBlocks();
                }
                
                if (Mouse.IsButtonPressed(Mouse.Button.Left) == true)
                {
                    ball.Start(10, new Vector2f(0, -1));
                }

                if (ball.sprite.Position.Y == 600 || ball.sprite.Position.X == 800)
                {
                    ball.speed = 0;
                    SetStartPositionSoftBlocks();
                    SetStartPositionHardBlocks();
                    --Try;
                }

                ball.Move(new Vector2i(0,0), new Vector2i(800,600));

                ball.CheckCollision(stick, "Stick");

                for (int i = 0; i < SoftBlocks.Length; i++)
                {
                   if (ball.CheckCollision(SoftBlocks[i], "Block") == true)
                    {
                        SoftBlocks[i].Position = new Vector2f(1000, 1000);
                        Score++;
                        break;
                    }
                }
                for (int i = 0; i < HardBlocks.Length; i++)
                {
                   if (ball.CheckCollision(HardBlocks[i], "Block") == true)
                    {
                        healthPoints[i] -= 1;
                        if (healthPoints[i]==0) {
                            HardBlocks[i].Position = new Vector2f(1000, 1000);
                            Score++;
                            break;
                        }
                    }
                }

                stick.Position = new Vector2f(Mouse.GetPosition(window).X - stick.TextureRect.Width * 0.5f, stick.Position.Y);

             
                
                window.Draw(ball.sprite);
                window.Draw(stick);
                for(int i=0;i<SoftBlocks.Length; i++)
                {
                    window.Draw( SoftBlocks[i] );
                }

                for(int i=0;i<HardBlocks.Length; i++)
                {
                    window.Draw( HardBlocks[i] );
                }

                window.Display();
            }
            window.Clear();
            if (level == 3)
            {
                window.Draw(new Text("Вы выиграли!", font, 20));
            }
            else
            {
                window.Draw(new Text("Вы проиграли!", font, 20));
            }
            window.Display();
            Console.ReadLine();
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
