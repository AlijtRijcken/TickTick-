using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

partial class Level : GameObjectList
{

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (quitButton.Pressed)
        {
            Reset();
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        }      
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        TimerGameObject timer = Find("timer") as TimerGameObject;
        Player player = Find("player") as Player;
        //lives.Text = "lives" + player.lives;
      //  lives. = player.GlobalPosition - new Vector2(0, 100);
         
        TileField tiles = GameWorld.Find("tiles") as TileField;;
        Camera.maxSize = new Vector2(length * tiles.CellWidth, tiles.Rows * tiles.CellWidth);  //added
        if (player.spawnTiny && tinyBomb == null)
        {

            tinyBomb = new TinyBomb();
            tinyBomb.Position = player.Position;
            tinyBomb.direction = player.Direction;
            tinyBomb.tiles = tiles;
        }

        if (tinyBomb != null)
        {
            tinyBomb.Update(gameTime);
            GameObjectList enemies = Find("enemies") as GameObjectList;
            foreach (AnimatedGameObject p in enemies.Children)
            {
                if (tinyBomb.CollidesWith(p))
                {
                    tinyBomb.explode = true;

                }
            }
            if (tinyBomb.explode)
            {
                explosion = new Explosion();
                explosion.Position = tinyBomb.Position;
                tinyBomb = null;
            }
        }

        if (explosion != null)
        {
            explosion.Update(gameTime);
            GameObjectList enemies = Find("enemies") as GameObjectList;
            foreach (AnimatedGameObject p in enemies.Children)
            {
                if(explosion.CollidesWith(p))
                {
                    p.Reset();

                }
            }
        }
        // check if we died
        if (!player.IsAlive)
        {
            timer.Running = false;
        }

        // check if we ran out of time
        if (timer.GameOver)
        {
            player.Explode();
        }
                       
        // check if we won
        if (Completed && timer.Running)
        {
            player.LevelFinished();
            timer.Running = false;
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        if (tinyBomb != null)
        {
            tinyBomb.Draw(gameTime,spriteBatch); 
        }
        if (explosion != null)
        {
            explosion.Draw(gameTime, spriteBatch);
        }
    }

    public override void Reset()
    {
        base.Reset();
        VisibilityTimer hintTimer = Find("hintTimer") as VisibilityTimer;
        hintTimer.StartVisible();
    }
    void SpawnTinyBomb()
    {

    }
}
