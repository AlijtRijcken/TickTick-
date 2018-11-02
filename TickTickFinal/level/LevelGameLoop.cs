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
         
        TileField tiles = GameWorld.Find("tiles") as TileField;
        
        //Maxiamle grote v/d levels
        Camera.maxSize = new Vector2(tiles.Columns * tiles.CellWidth, tiles.Rows * tiles.CellHeight);
        
        //ADDED TinyBomb, zodat alles in dezelfde laag zit en enemies kunnen worden gedood. 

        if (player.spawnTiny && tinyBomb == null)
        {
            //spawn TinyBomb
            tinyBomb = new TinyBomb();
            tinyBomb.Position = player.Position;
            tinyBomb.direction = player.Direction;
            tinyBomb.tiles = tiles;
        }

        //Update TinyBomb and explosion when colision with enemie.
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

        //Kills the enemies. 
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

        //Draw TinyBomb
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
}
