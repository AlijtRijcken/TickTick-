using System;
using Microsoft.Xna.Framework;

class Extralife : SpriteGameObject
{
public Extralife(int layer = 0, string id = "") : base(1, "Sprites/lives", layer, id)
{
}

public override void Update(GameTime gameTime)
{
    Player player = GameWorld.Find("player") as Player;
    if (visible && CollidesWith(player))
    {
        visible = false;
        player.lives++;
    }
}
}
