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
    public int maxWidth = 2400;
    int playersize = 100;
    public Camera() : base(0, "Camera")
    {

        position = new Vector2(0, 0); 
    }

    public override void Update(GameTime gameTime)
    {

        //player position uptaden <= cam pos
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
    Vector2 FollowPlayer()
    {
        Vector2 Camposition;
        if(cornerPosition.X <= GameEnvironment.windowsize.X - playersize)
        {
            Camposition.X = 0;
        }
        else if(cornerPosition.X > GameEnvironment.windowsize.X - playersize&& cornerPosition.X <= maxWidth)
        {
            Camposition.X = cornerPosition.X - GameEnvironment.windowsize.X + playersize;
        }
        else { Camposition.X = maxWidth-GameEnvironment.windowsize.X + playersize; };
        Camposition.Y = 0;
        return Camposition;
    }
}
