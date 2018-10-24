using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class TinyBomb : SpriteGameObject
{
   // Player player = GameWorld.Find("player") as Player;

    public TinyBomb(Vector2 position,int layer = 0, string id = "") : base("Sprites/spr_water", layer, id)
    {
        this.position.Y = position.Y - 200;  //-0.5f*player.Sprite.Height;
        this.position.X = position.X;
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
    }
}
