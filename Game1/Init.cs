using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class init : Game
    {
        Texture2D big_texture;
        Texture2D small_texture;
        Texture2D snake_head;
        GraphicsDeviceManager graphics;
        GraphicsDevice gp;
        public init()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        public void create()
        {
            Texture2D big_texture = Content.Load<Texture2D>("Terrain1-1");
            Texture2D small_texture = Content.Load<Texture2D>("Terrain1-2");
            Texture2D snake_head = Content.Load<Texture2D>("Snakehead");
        }
        public void create_big_field()
        {
            GraphicsDevice.Clear(Color.Yellow);
            //spriteBatch.Begin(SpriteSortMode.BackToFront);
            //spriteBatch.Draw(big_texture, new Vector2(320, 320), new Rectangle(0, 0, big_texture.Width, big_texture.Height), Color.White, 0, new Vector2(big_texture.Width / 2, big_texture.Height / 2), 2f, SpriteEffects.None, 0f);
            //spriteBatch.End();
        }
        public void create_big_field(int n, int m)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    //spriteBatch.Begin();
                    //spriteBatch.Draw(small_texture, new);
                    //spriteBatch.End();
                }
            }
        }
    }
}
