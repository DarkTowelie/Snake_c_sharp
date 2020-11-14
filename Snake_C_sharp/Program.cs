using System;
using System.Threading;

namespace Snake_C_sharp
{
    public class Snake
    {
        public  bool gameOver;
        public int score, speed;

        const int width = 20;
        const int height = 20;
        int headX, headY, fruitX, fruitY;
        int[] tailX = new int[100];
        int[] tailY = new int[100];
        int nTail;

        enum eDirection
        {
            STOP = 0,
            LEFT,
            RIGHT,
            UP,
            DOWN
        }
        eDirection dir;
        //---------------------------------------------------------------------------

        void getFruitCoords()
        {
            Random r = new Random();
            fruitX = r.Next(1, width - 2);
            fruitY = r.Next(1, height - 2);
        }
        public Snake()
        {
            gameOver = false;
            score = 0;
            speed = 90;
            dir = eDirection.STOP;
            headX = width / 2 - 1;
            headY = height / 2 - 1;
            getFruitCoords();

            nTail = 0;
            for (int i = 0; i < 100; i++)
            {
                tailX[i] = 0;
                tailY[i] = 0;
            }
            score = 0;
        }
        public void Draw()
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < width; i++)
            {
                Console.Write("#");
            }
            Console.Write("\n");

            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    bool isEmpty = true;
                    if (j == 0 || j == width - 1)
                    {
                        Console.Write("#");
                        isEmpty = false;
                    }
                   
                    if (i == headY && j == headX)
                    {
                        Console.Write("O");
                        isEmpty = false;
                    }

                    if (i == fruitY && j == fruitX)
                    {
                        Console.Write("F");
                        isEmpty = false;
                    }

                    for (int k = 0; k < nTail; k++)
                    {
                        if (tailY[k] == i && tailX[k] == j)
                        {
                            Console.Write("o");
                            isEmpty = false;
                        }
                    }

                    if (isEmpty == true)
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
            }

            for (int i = 0; i < width; i++)
            {
                Console.Write("#");
            }
            Console.Write("\nScore = " + score);
            Console.Write("\nControl - WASD");
            Console.Write("\nSpeed - []");
            Console.Write("\nCurrent speed = " + (100 - speed));
        }
        public void Input()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo name = Console.ReadKey(true);
                switch (name.KeyChar)
                {
                    case 'a':
                        dir = eDirection.LEFT;
                        break;
                    case 'd':
                        dir = eDirection.RIGHT;
                        break;
                    case 's':
                        dir = eDirection.DOWN;
                        break;
                    case 'w':
                        dir = eDirection.UP;
                        break;
                    case 'x':
                        gameOver = true;
                        break;
                    case '[':
                        if (speed <= 80)
                            speed += 10;
                        break;
                    case ']':
                        if (speed >= 20)
                            speed -= 10;
                        break;  
                }
            }
        }
        public void Logic()
        {
            int prevX = tailX[0];
            int prevY = tailY[0];
            int prev2X, prev2Y;
            tailX[0] = headX;
            tailY[0] = headY;
            for (int i = 1; i < nTail; i++)
            {
                prev2X = tailX[i];
                prev2Y = tailY[i];
                tailX[i] = prevX;
                tailY[i] = prevY;
                prevX = prev2X;
                prevY = prev2Y;
            }

            for (int i = 1; i < nTail; i++)
            {
                if (tailX[i] == headX && tailY[i] == headY)
                {
                    gameOver = true;
                }
            }

            switch (dir)
            {
                case (eDirection.LEFT):
                    headX -= 1;
                    break;
                case (eDirection.RIGHT):
                    headX+=1;
                    break;
                case (eDirection.DOWN):
                    headY++;
                    break;
                case (eDirection.UP):
                    headY--;
                    break;
            }

            if (headX < 1 || headX > width - 2 || headY < 1 || headY > height - 2)
                gameOver = true;

            if (headX == fruitX && headY == fruitY)
            {
                nTail++;
                getFruitCoords();

                for (int k = 0; k < nTail; k++)
                {
                    int n = 0;
                    if (tailY[k] == fruitY && tailX[k] == fruitX)
                        while (tailY[k] == fruitY && tailX[k] == fruitX)
                        {
                            getFruitCoords();
                            k = 0;

                            n++;
                            if (n > 15)
                            {
                                gameOver = true;
                                break;
                            }
                        }
                }

                score += 10 + ((90 - speed) / 10);
            }
        }
    }
    class Program
    {
        static void Main()
        {
            Snake snake = new Snake();
            Console.CursorVisible = false;

            while (!snake.gameOver)
            {
                Thread.Sleep(snake.speed);
                snake.Input();
                snake.Logic();
                snake.Draw();
            }

            if (snake.gameOver)
            {
                Console.Clear();
                Console.Write("Your score = " + snake.score);
                snake.gameOver = false;
                Console.ReadKey();
            }
            Main();
        }
    }
}
