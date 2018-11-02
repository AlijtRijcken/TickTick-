using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework; 

public class Camera : GameObject
{
    public Vector2 cornerPosition = new Vector2(); 
    public IGameLoopObject state;
    public static Vector2 maxSize = new Vector2(0,0);
    int playersize = 100;

    public Camera() : base(0, "Camera")
    {
        //Default position camera
        position = new Vector2(0, 0); 
    }

    public override void Update(GameTime gameTime)
    {
        //only follow the player with the camera when gamestate = playingstate
        if(state.GetType() == typeof(PlayingState))
        {
            position = FollowPlayer();
        }
        else
        {
            position = new Vector2(0, 0);
        }
        base.Update(gameTime);
    }

    //calculate boundaries when camera follows player
    Vector2 FollowPlayer()
    {
        Vector2 Camposition;
        if(cornerPosition.X <= GameEnvironment.windowsize.X - playersize)
        {
            Camposition.X = 0;
        }
        else if(cornerPosition.X > GameEnvironment.windowsize.X - playersize&& cornerPosition.X <= maxSize.X - GameEnvironment.windowsize.X + playersize/2)
        {
            Camposition.X = cornerPosition.X - GameEnvironment.windowsize.X + playersize;
        }
        else
        {
            Camposition.X = maxSize.X - 2*GameEnvironment.windowsize.X + 1.5f*playersize;
        }
        if (cornerPosition.Y <= GameEnvironment.windowsize.Y - playersize)
        {
            Camposition.Y = 0;
        }
        else if (cornerPosition.Y > GameEnvironment.windowsize.Y - playersize && cornerPosition.Y <= maxSize.Y - GameEnvironment.windowsize.Y)
        {
            Camposition.Y = cornerPosition.Y - GameEnvironment.windowsize.Y + playersize;
        }
        else
        {
            Camposition.Y = maxSize.Y - 2 * GameEnvironment.windowsize.Y + 1.25f * playersize;
        }
        return Camposition;
    }
}
