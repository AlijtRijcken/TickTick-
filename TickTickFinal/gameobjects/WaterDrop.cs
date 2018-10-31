using System;
using Microsoft.Xna.Framework;

class WaterDrop : SpriteGameObject
{
    protected float bounce;

    public WaterDrop(int layer=0, string id="") : base(1,"Sprites/spr_water", layer, id) 
    {
    }

    public override void Update(GameTime gameTime)
    {
        //Movedrop(); 

        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + Position.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        position.Y += bounce;
        Player player = GameWorld.Find("player") as Player;
        if (visible && CollidesWith(player))
        {
            visible = false;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_watercollected");
        }
    }

    //public abstract void Movedrop();                    //does not implement a body

}

//class WobblyDrop : WaterDrop
//{
//    int counter; 

//    public WobblyDrop() { }

//    public void Update() { }                //onnodig, want al geimplementeerd in de abstract class

//    public override void Movedrop()
//    {

//        counter++;
//        if(counter < 10)
//            position.X += 2;
//        else
//            position.X -= 2;

//        if (counter == 18)
//            counter = 0;  
//    }

//}