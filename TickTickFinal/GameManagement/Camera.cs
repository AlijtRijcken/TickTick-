using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework; 

public class Camera : GameObject, IGameLoopObject
{
    public Vector2 cornerPosition = new Vector2(); 
    IGameLoopObject state;
    GameStateManager manager; 

    public Camera() : base(0, "Camera")
    {

        position = new Vector2(0, 0); 
    }

    public override void Update(GameTime gameTime)
    {

        //player position uptaden <= cam pos
        state = manager.CurrentGameState;
        if(state == )
        position = cornerPosition - GameEnvironment.windowsize/2; 

        base.Update(gameTime);
    }
}
