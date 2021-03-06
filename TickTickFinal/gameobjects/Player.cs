using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
partial class Player : AnimatedGameObject
{
    protected Vector2 startPosition;
    public int Direction = 1; //richting waar player naar kijkt, Dit moet worden vervangen met hetgene dat bepaald welkekant de player op kijkt, kon ik niet vinden.
    protected bool isOnTheGround;
    protected float previousYPosition;
    protected bool isAlive;
    protected bool exploded;
    protected bool finished;
    protected bool walkingOnIce, walkingOnHot;
    
    //Added variables 
    public bool spawnTiny;
    public int lives;
    protected int airtime;
    public bool takedamage;
    public bool invulnerablity;
    private int counter;
    protected SpriteFont spriteFont;
    public List<Lives> drawlives = new List<Lives>();
    

    public Player(Vector2 start) : base(2, "player")
    {
        LoadAnimation("Sprites/Player/spr_idle", "idle", true); 
        LoadAnimation("Sprites/Player/spr_run@13", "run", true, 0.05f);
        LoadAnimation("Sprites/Player/spr_jump@14", "jump", false, 0.05f); 
        LoadAnimation("Sprites/Player/spr_celebrate@14", "celebrate", false, 0.05f);
        LoadAnimation("Sprites/Player/spr_die@5", "die", false);
        LoadAnimation("Sprites/Player/spr_explode@5x5", "explode", false, 0.04f);

        spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>("Fonts/HintFont");
        startPosition = start;
        Reset();
    }

    public override void Reset()
    {
        //start values
        takedamage = false;
        invulnerablity = false;
        position = startPosition;
        velocity = Vector2.Zero;
        isOnTheGround = true;
        isAlive = true;
        exploded = false;
        finished = false;
        walkingOnIce = false;
        walkingOnHot = false;
        PlayAnimation("idle");
        previousYPosition = BoundingBox.Bottom;
        lives = 3;
        airtime = 0;
        if (drawlives != null)
        {
            drawlives.Clear();
        }
        for (int i = 0; i < 10; i++)
        {
            Lives live = new Lives();
            drawlives.Add(live);
        }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        float walkingSpeed = 400;
        if (walkingOnIce)
        {
            walkingSpeed *= 1.5f;
        }
        if (!isAlive)
        {
            return;
        }
        //Added shooting function 
        if (inputHelper.IsKeyDown(Keys.F))
        {
            spawnTiny = true;
        }
        if (inputHelper.IsKeyDown(Keys.F) == false)
        {
            spawnTiny = false;
        }
        if (inputHelper.IsKeyDown(Keys.Left))
        {
            velocity.X = -walkingSpeed;
            Direction = -1;
        }
        else if (inputHelper.IsKeyDown(Keys.Right))
        {
            velocity.X = walkingSpeed;
            Direction = 1;
        }
        else if (!walkingOnIce && isOnTheGround)
        {
            velocity.X = 0.0f;
        }
        if (velocity.X != 0.0f)
        {
            Mirror = velocity.X < 0;
        }
        if ((inputHelper.KeyPressed(Keys.Space) || inputHelper.KeyPressed(Keys.Up)) && isOnTheGround)
        {
            Jump();
        }

    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        //added text with amount of lives information 
        spriteBatch.DrawString(spriteFont, "lives" + lives,  new Vector2(120, 10), Color.Black);        //ADDED
        for (int i = 0; i < lives; i++)
        {
            drawlives[i].Draw(gameTime, spriteBatch);
        }

    }
    public override void Update(GameTime gameTime)
    {
        //added: cam position set to player position. So the camera knows where the player is. 
        GameEnvironment.cameraPosition = position;

        //when the player will take damage it will have two seconds before next damage will be taken. 
        if(invulnerablity == false && takedamage == true)
        {
            lives--;
            invulnerablity = true;
        }
        if (invulnerablity)
        {
            counter++;
            if (counter == 120)
            {
                invulnerablity = false;
                counter = 0;
                takedamage = false;
            }

        }

        //Update  all the lives. 
        foreach (Lives live in drawlives)
        {
            live.Update(gameTime);
        }
        base.Update(gameTime);
        
        //place the lives above players head
        for (int i = 0; i < lives; i++)
        {
            drawlives[i].Position = position - new Vector2(100 - i * 50, 100);
        }

        if (!finished && isAlive)
        {
            if (isOnTheGround)
            {
                //fall damage
                if(airtime >= 33) 
                {
                    takedamage = true;
                }

                airtime = 0;
                if (velocity.X == 0)
                {
                    PlayAnimation("idle");
                }
                else
                {
                    PlayAnimation("run");
                }
            }
            else if (velocity.Y < 0)
            {
                PlayAnimation("jump");
            }
            else
            {
                airtime++;
            }
            if (lives <= 0)
            {
                Explode();
            }

            TimerGameObject timer = GameWorld.Find("timer") as TimerGameObject;
            if (walkingOnHot)
            {
                timer.Multiplier = 2;
            }
            else if (walkingOnIce)
            {
                timer.Multiplier = 0.5;
            }
            else
            {
                timer.Multiplier = 1;
            }

            TileField tiles = GameWorld.Find("tiles") as TileField;
            if (BoundingBox.Top >= tiles.Rows * tiles.CellHeight)
            {
                Die(true);
            }
        }

        DoPhysics();
    }

    public void Explode()
    {
        if (!isAlive || finished)
        {
            return;
        }
        isAlive = false;
        exploded = true;
        velocity = Vector2.Zero;
        position.Y += 15;
        PlayAnimation("explode");
    }

    public void Die(bool falling)
    {
        if (!isAlive || finished)
        {
            return;
        }
        isAlive = false;
        velocity.X = 0.0f;
        if (falling)
        {
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_fall");
        }
        else
        {
            velocity.Y = -900;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_die");
        }
        PlayAnimation("die");
    }

    public bool IsAlive
    {
        get { return isAlive; }
    }

    public bool Finished
    {
        get { return finished; }
    }

    public void LevelFinished()
    {
        finished = true;
        velocity.X = 0.0f;
        PlayAnimation("celebrate");
        GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_won");
    }
}

//ADDED CLASS for the lives!
class Lives : SpriteGameObject
{
    public Lives(int layer = 0, string id = "") : base(1,"Sprites/Lives", layer, id)
    {
    }
}