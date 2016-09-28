using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
namespace Game1
{
    /// <summary>
    /// Основной класс нашей игры.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        //SpriteBatch - класс, позволяющий отрисовывать спрайты
        SpriteBatch spriteBatch;

        //Объявим глобальную переменную типа Texture2D для хранения информации о нашей текстуре.
        Texture2D big_field;
        Texture2D snake_head;
        Texture2D snake_body_even;
        Texture2D snake_body_odd;
        Texture2D snake_turn;
        Texture2D snake_tail1;
        Texture2D snake_tail2;
        Texture2D food;
        int score;
        int tail_oddity = 0;
        int[,] field;
        int x_head, y_head;// голова змеи
        int x_food, y_food;
        int move_direction; // 1 - up, 2 - down, 3 - right, 4 - left
        float rotation;
        int length;
        Random rand = new Random();
        private void init_field()
        {
            big_field = Content.Load<Texture2D>("Terrain1-1");
        }
        private void init_logic_field()
        {   
            field = new int[20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    field[i, j] = 0;
                }
            }
            score = 0;
            
        }
        private void init_snake()
        {
            
            x_head = rand.Next(2, 17);
            y_head = rand.Next(2, 17);
            field[x_head, y_head] = 1;
            field[x_head, y_head+1] = 2;
            field[x_head, y_head+2] = 3;
            move_direction = 1;
            length = 3;
            rotation = 0f;
            snake_head = Content.Load<Texture2D>("snakehead");
            snake_body_even = Content.Load<Texture2D>("snakebody1");
            snake_body_odd = Content.Load<Texture2D>("snakebody2");
            snake_tail1 = Content.Load<Texture2D>("Snaketail1");
            snake_tail2 = Content.Load<Texture2D>("Snaketail2");
            snake_turn = Content.Load<Texture2D>("Snaketurn");
        }
        private void init_food()
        {
            do
            {
                x_food = rand.Next(20);
                y_food = rand.Next(20);
            } while (field[y_food, x_food] > 0);
            food = Content.Load<Texture2D>("Apple");

        }

        private int where_is_min(int i, int j, int key)// 1 - снизу, 2 - сверху, 3 - слева, 4 - справа
        {
            int result = 0;
            if (field[(i + 21) % 20, j] == key - 1)
            {
                result = 4;
            }
            if (field[(i + 19) % 20, j] == key - 1)
            {
                result = 3;
            }
            if (field[i, (j + 19) % 20] == key - 1)
            {
                result = 2;
            }
            if (field[i, (j + 21) % 20] == key - 1)
            {
                result = 1;
            }
            return result;
        }
        private int where_is_max(int i, int j, int key)
        {
            int result = 0;
            if (field[(i + 21) % 20, j] == key + 1)
            {
                result = 4;
            }
            if (field[(i + 19) % 20, j] == key + 1)
            {
                result = 3;
            }
            if (field[i, (j + 19) % 20] == key + 1)
            {
                result = 2;
            }
            if (field[i, (j + 21) % 20] == key + 1)
            {
                result = 1;
            }
            return result;
        }
        private void draw_game()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    spriteBatch.Draw(big_field, new Vector2(i * 18+9, j * 18+9), new Rectangle(0, 0, big_field.Width, big_field.Height),
                                 Color.White, 0, new Vector2(big_field.Width/2, big_field.Height/2), 1f, SpriteEffects.None, 1f);
                    if (field[i, j] != 0)
                    {
                        if (field[i, j] == 1)//голова
                        {
                            switch (where_is_max(i,j,1))
                            {
                                case 4:
                                    rotation = -MathHelper.PiOver2;
                                    break;
                                case 3:
                                    rotation = MathHelper.PiOver2;
                                    break;
                                case 2:
                                    rotation = MathHelper.Pi;
                                    break;
                                case 1:
                                    rotation = 0;
                                    break;
                            }
                            spriteBatch.Draw(snake_head, new Vector2(x_head * 18 + 9, y_head * 18 + 9), new Rectangle(0, 0, snake_head.Width, snake_head.Height),
                                Color.White, rotation, new Vector2(snake_head.Width / 2, snake_head.Height / 2), 1f, SpriteEffects.None, 0f);
                            continue;
                        }
                        if (field[i, j] == length)//хвост
                        {
                            int temp = where_is_min(i, j, length);
                            float rotation = 0;
                            if (temp == 2)
                            {
                                rotation = 0;
                            }
                            if (temp == 1)
                            {
                                rotation = MathHelper.Pi;
                            }
                            if (temp == 4)
                            {
                                rotation = MathHelper.PiOver2;
                            }
                            if (temp == 3)
                            {
                                rotation = -MathHelper.PiOver2;
                            }
                            tail_oddity++;
                            tail_oddity %= 32;
                            spriteBatch.Draw(tail_oddity<16? snake_tail1:snake_tail2, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_tail1.Width, snake_tail1.Height),
                                Color.White, rotation, new Vector2(snake_tail1.Width / 2, snake_tail1.Height / 2), 1f, SpriteEffects.None, 0f);
                            continue;
                        }
                        int back = where_is_max(i, j, field[i, j]);
                        int forward = where_is_min(i, j, field[i, j]);
                        if (back == 1 && forward == 2)
                        {
                            spriteBatch.Draw(field[i,j]%2==0?snake_body_even:snake_body_odd, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, 0, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.None, 0f);
                            continue;
                        }
                        if (back == 2 && forward == 1)
                        {
                            spriteBatch.Draw(field[i, j] % 2 == 0 ? snake_body_even : snake_body_odd, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, MathHelper.Pi, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.None, 0f);
                            continue;
                        }
                        if (back == 3 && forward == 4)
                        {
                            spriteBatch.Draw(field[i, j] % 2 == 0 ? snake_body_even : snake_body_odd, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, MathHelper.PiOver2, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.None, 0f);
                            continue;
                        }
                        if (back == 4 && forward == 3)
                        {
                            spriteBatch.Draw(field[i, j] % 2 == 0 ? snake_body_even : snake_body_odd, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, -MathHelper.PiOver2, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.None, 0f);
                            continue;
                        }

                        //-------------------------------------------
                        if (back == 3 && forward == 2 )
                        {
                            spriteBatch.Draw(snake_turn, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, 0, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.None
                            , 0f);
                            continue;
                        }

                        if (back == 2 && forward == 3)
                        {
                            spriteBatch.Draw(snake_turn, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, -MathHelper.PiOver2, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.FlipHorizontally
                            , 0f);
                            continue;
                        }

                        //-------------------dwfcqefq34fc-----------------

                        if (back == 2 && forward == 4)
                        {
                            spriteBatch.Draw(snake_turn, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, MathHelper.PiOver2, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.None, 0f);
                            continue;
                        }

                        if (back == 4 && forward == 2)
                        {
                            spriteBatch.Draw(snake_turn, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, 0, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.FlipHorizontally
                            , 0f);
                            continue;
                        }

                        //------------------------

                        if (back == 1 && forward == 4)
                        {
                            spriteBatch.Draw(snake_turn, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, MathHelper.PiOver2, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.FlipHorizontally, 0f);
                            continue;
                        }
                        if (back == 4 && forward == 1)
                        {
                            spriteBatch.Draw(snake_turn, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, -MathHelper.PiOver2, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.FlipVertically, 0f);
                            continue;
                        }

                        if (back == 1 && forward == 3)
                        {
                            spriteBatch.Draw(snake_turn, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, -MathHelper.PiOver2, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.None, 0f);
                            continue;
                        }
                        if (back == 3 && forward == 1)
                        {
                            spriteBatch.Draw(snake_turn, new Vector2(i * 18 + 9, j * 18 + 9), new Rectangle(0, 0, snake_body_even.Width, snake_body_even.Height),
                                Color.White, 0, new Vector2(snake_body_even.Width / 2, snake_body_even.Height / 2), 1f, SpriteEffects.FlipVertically, 0f);
                            continue;
                        }

                    }
                }
            }
        }
        private void draw_food()
        {
            spriteBatch.Draw(food, new Vector2(x_food * 18 + 9, y_food * 18 + 9), new Rectangle(0, 0, food.Width, food.Height),
                        Color.White, 0, new Vector2(food.Width / 2, food.Height / 2), 1f, SpriteEffects.None, 0f);
        }
        private void move()
        {
            
            if (move_direction == 1)
            {
                y_head = (y_head + 19) % 20;
            }
            if (move_direction == 2)
            {
                y_head = (y_head + 21) % 20;
            }
            if (move_direction == 3)
            {
                x_head = (x_head + 21) % 20;
            }
            if (move_direction == 4)
            {
                x_head = (x_head + 19) % 20;
            }

            if (field[x_head, y_head] > 1)
                Initialize();

            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                    if (field[i, j] != 0)
                    {
                        field[i, j]++;
                        if (field[i, j] > length)
                            field[i, j] = 0;
                    }
                        
            field[x_head, y_head] = 1;

            // ---------------------------------------


           
           
            if (x_head == x_food && y_head == y_food)
            {
                score++;
                while (field[x_food, y_food] > 0)
                {
                    x_food = rand.Next(20);
                    y_food = rand.Next(20);
                }
                length++;
            }

            
                                          
        }
        private void keyboard_move()
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.W) && move_direction != 2)
            {
                move_direction = 1;
               // rotation = 0;
            }
            if (ks.IsKeyDown(Keys.S) && move_direction != 1)
            {
                move_direction = 2;
                //rotation = MathHelper.Pi;
            }
            if (ks.IsKeyDown(Keys.A) && move_direction != 3)
            {
                move_direction = 4;
                //rotation = -MathHelper.PiOver2;

            }
            if (ks.IsKeyDown(Keys.D) && move_direction != 4)
            {
                move_direction = 3;
                //rotation = MathHelper.PiOver2;
            }

        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
            graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            init_logic_field();
            base.Initialize();
        }

        /// <summary>
        /// Загрузка графического контета игры. Метод будет вызван один раз.
        /// </summary>
        protected override void LoadContent()
        {
            // Создание экземпляра класс SpriteBatсh.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            init_field();
            init_snake();
            init_food();
        }

        /// <summary>
        /// Выгружаем контент, который создали во время игры, без использование ContentManager
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Реализация игровой логики должны быть в данном методе.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        int delta = 0;
        protected override void Update(GameTime gameTime)
        {
            keyboard_move();
            delta = (delta + 1) % 10;
            if (delta == 0)
            {
                move();
            }
        }

        /// <summary>
        /// Метод для рендера
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //ВАЖНО, для того, чтобы работала сортировка спрайтов надо в spriteBatch.Begin() передать SpriteSortMode.BackToFront или SpriteSortMode.FrontToBack

            spriteBatch.Begin(SpriteSortMode.BackToFront);
            draw_game();
            draw_food();

            //В режиме BackToFront мы увидим красный спрайт(он обладает наименьшим layerDepth == 0f)
            //В режиме FrontToBack мы увидим желтый спрайт(он обладает наибольшим layerDpth == 0.5f). Попробуйте!
            //Посмотрите, что будет если убрать SpriteSortMode из spriteBatch.Begin()

            spriteBatch.End();




            base.Draw(gameTime);
        }
    }
}
