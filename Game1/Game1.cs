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
        Texture2D snake_body;
        Texture2D snake_tail;
        Texture2D food;
        int score;
        int[,] field;
        int x_head, y_head;// голова змеи
        int x_food, y_food;
        int move_direction; // 1 - up, 2 - down, 3 - right, 4 - left
        float rotation;
        int length;
        Random rand;
        private void init_field()
        {
            big_field = Content.Load<Texture2D>("Terrain1-1");
        }
        private void init_logic_field()
        {
            field = new int[20, 20];
            for(int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    field[i, j] = 0;
                }
            }
            score = 0;
            field[y_head,x_head] = 1;
            field[y_head + 1, x_head] = 2;
            field[y_head + 2, x_head] = 3;
        }
        private void init_snake()
        {
            rand = new Random();
            x_head = rand.Next(2,18);
            y_head = rand.Next(2,18);
            move_direction = 1;
            length = 3;
            rotation = 0f;
            snake_head = Content.Load<Texture2D>("snakehead");
            snake_body = Content.Load<Texture2D>("snakebody1");
            snake_tail = Content.Load<Texture2D>("Snaketail1");
        }
        private void init_food()
        {
            x_food = rand.Next(20);
            x_food = rand.Next(20);
            y_food = rand.Next(20);
            if(x_food == x_head && y_food == x_head)
            {
                x_food = rand.Next(20);
            }
            food = Content.Load<Texture2D>("Apple");

        }
        
        private void draw_field(int a, int b)
        {
            for(int i = 0; i < a; i++)
            {
                for(int j = 0; j < b; j++)
                {
                    spriteBatch.Draw(big_field, new Vector2(i*18, j*18), new Rectangle(0, 0, big_field.Width, big_field.Height), 
                        Color.White, 0, new Vector2(big_field.Width / 2, big_field.Height / 2), 1f, SpriteEffects.None, 1f);
                }
            }
        }
        private void draw_snake()
        {
            spriteBatch.Draw(snake_head, new Vector2(x_head * 18, y_head * 18), new Rectangle(0, 0, snake_head.Width, snake_head.Height),
                        Color.White, rotation, new Vector2(snake_head.Width / 2, snake_head.Height / 2), 1f, SpriteEffects.None, 0f);
            int cur = 2;
            int cur_pos_y = y_head;
            int cur_pos_x = x_head;
            while(cur <= length)
            {
                if(cur_pos_y > 0 && field[cur_pos_y - 1, cur_pos_x] == cur)
                {
                    spriteBatch.Draw(snake_body, new Vector2(cur_pos_x * 18, (cur_pos_y-1) * 18), new Rectangle(0, 0, snake_body.Width, snake_body.Height),
                        Color.White, 0, new Vector2(snake_body.Width / 2, snake_body.Height / 2), 1f, SpriteEffects.None, 0f);
                    cur_pos_y--;
                    continue;
                }
                if(cur_pos_y < 19 && field[cur_pos_y + 1, cur_pos_x] == cur)
                {

                    spriteBatch.Draw(snake_body, new Vector2(cur_pos_x * 18, (cur_pos_y + 1) * 18), new Rectangle(0, 0, snake_body.Width, snake_body.Height),
                        Color.White, 0, new Vector2(snake_body.Width / 2, snake_body.Height / 2), 1f, SpriteEffects.FlipHorizontally, 0f);
                    cur_pos_y++;
                    continue;
                }
                if(cur_pos_x > 0 && field[cur_pos_y, cur_pos_x-1] == cur)
                {
                    spriteBatch.Draw(snake_body, new Vector2((cur_pos_x-1) * 18, cur_pos_y * 18), new Rectangle(0, 0, snake_body.Width, snake_body.Height),
                        Color.White, 0, new Vector2(snake_body.Width / 2, snake_body.Height / 2), 1f, SpriteEffects.None, 0f);
                    cur_pos_x--;
                    continue;
                }
                if(cur_pos_x < 19 && field[cur_pos_y, cur_pos_x + 1] == cur)
                {
                    spriteBatch.Draw(snake_body, new Vector2((cur_pos_x + 1) * 18, cur_pos_y * 18), new Rectangle(0, 0, snake_body.Width, snake_body.Height),
                       Color.White, 0, new Vector2(snake_body.Width / 2, snake_body.Height / 2), 1f, SpriteEffects.None, 0f);
                    cur_pos_x++;
                    continue;
                }
                cur++;
            }
        }
        private void draw_food()
        {
            spriteBatch.Draw(food, new Vector2(x_food * 18, y_food * 18), new Rectangle(0, 0, food.Width, food.Height),
                        Color.White, 0, new Vector2(food.Width / 2, food.Height / 2), 1f, SpriteEffects.None, 0f);
        }
        private void move()
        {
            field[y_head, x_head] = 0;
            if(move_direction == 1)
            {
                y_head--;
                if (y_head < 0)
                {
                    y_head = 19;
                }
            }
            if(move_direction == 2)
            {
                y_head = (y_head + 1) % 20;
            }
            if(move_direction == 3)
            {
                x_head = (x_head + 1) % 20;
            }
            if(move_direction == 4)
            {
                x_head--;
                if(x_head < 0)
                {
                    x_head = 19;
                }
            }
            field[y_head, x_head] = 1;
        }
        private void eat()
        {
            if (x_head == x_food & y_head == y_food)
            {
                while (field[y_food, x_food] == 1) { 
                  x_food = rand.Next(20);
                  y_food = rand.Next(20);
                }
            }
            length++;
            for (int i = 0; i < 20; i++)
            {
                for(int j = 0; j < 20; j++)
                {
                    if(field[i,j] == length)
                    {

                    }
                }
            }
        }
        private void keyboard_move()
        {
            KeyboardState ks = Keyboard.GetState();
            if(ks.IsKeyDown(Keys.W) && move_direction!=2){
                move_direction = 1;
                rotation = 0;
            }
            if(ks.IsKeyDown(Keys.S)&& move_direction != 1){
                move_direction = 2;
                rotation = MathHelper.Pi;
            }
            if(ks.IsKeyDown(Keys.A) && move_direction != 3){
                move_direction = 4;
                rotation = -MathHelper.PiOver2;

            }
            if (ks.IsKeyDown(Keys.D)&& move_direction != 4)
            {
                move_direction = 3;
                rotation = MathHelper.PiOver2;
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
            eat();
            delta = (delta+1)%10;
            if(delta == 0) {
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
            draw_field(20, 20);
            draw_snake();
            draw_food();

            //В режиме BackToFront мы увидим красный спрайт(он обладает наименьшим layerDepth == 0f)
            //В режиме FrontToBack мы увидим желтый спрайт(он обладает наибольшим layerDpth == 0.5f). Попробуйте!
            //Посмотрите, что будет если убрать SpriteSortMode из spriteBatch.Begin()

            spriteBatch.End();




            base.Draw(gameTime);
        }
    }
}