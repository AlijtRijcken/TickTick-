using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteGameObject : GameObject
{
    protected SpriteSheet sprite;
    protected Vector2 origin;
    public bool PerPixelCollisionDetection = true;
    //ADDED
    private int camera_movement;
    Camera camera;                                                     

    public SpriteGameObject(int camera_movement,string assetName, int layer = 0, string id = "", int sheetIndex = 0)  //CHANGED
        : base(layer, id)
    {
        if (assetName != "")
        {
            sprite = new SpriteSheet(assetName, sheetIndex);
        }
        else
        {
            sprite = null;
        }
        //ADDED
        this.camera_movement = camera_movement;
        camera = GameEnvironment.camera;                                    
    }    

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible || sprite == null)
        {
            return;
        }

        //Parallax scrolling, with layering. case 0 is everything which is fixed on the screen, case 1 the player and the tiles, 
        //case 2 first layer of mountains, case 3 and 4 are also mountain layers. 
        switch (camera_movement)
        {
            case 0:
                {
                    sprite.Draw(spriteBatch, this.GlobalPosition, origin);
                    break;
                }
            case 1:
                {
                    sprite.Draw(spriteBatch, this.GlobalPosition - camera.Position, origin);
                    break;
                }
            case 2:
                {
                    sprite.Draw(spriteBatch, this.GlobalPosition - camera.Position*0.8f , origin);
                    break;
                }
            case 3:
                {
                    sprite.Draw(spriteBatch, this.GlobalPosition - camera.Position*0.4f , origin);
                    break;
                }
            case 4:
                {
                    sprite.Draw(spriteBatch, this.GlobalPosition - camera.Position*0.2f , origin);
                    break;
                }
        }
    }

    public SpriteSheet Sprite
    {
        get { return sprite; }
    }

    public Vector2 Center
    {
        get { return new Vector2(Width, Height) / 2; }
    }

    public int Width
    {
        get
        {
            return sprite.Width;
        }
    }

    public int Height
    {
        get
        {
            return sprite.Height;
        }
    }

    public bool Mirror
    {
        get { return sprite.Mirror; }
        set { sprite.Mirror = value; }
    }

    public Vector2 Origin
    {
        get { return origin; }
        set { origin = value; }
    }

    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)(GlobalPosition.X - origin.X);
            int top = (int)(GlobalPosition.Y - origin.Y);
            return new Rectangle(left, top, Width, Height);
        }
    }

    public bool CollidesWith(SpriteGameObject obj)
    {
        if (!visible || !obj.visible || !BoundingBox.Intersects(obj.BoundingBox))
        {
            return false;
        }
        if (!PerPixelCollisionDetection)
        {
            return true;
        }
        Rectangle b = Collision.Intersection(BoundingBox, obj.BoundingBox);
        for (int x = 0; x < b.Width; x++)
        {
            for (int y = 0; y < b.Height; y++)
            {
                int thisx = b.X - (int)(GlobalPosition.X - origin.X) + x;
                int thisy = b.Y - (int)(GlobalPosition.Y - origin.Y) + y;
                int objx = b.X - (int)(obj.GlobalPosition.X - obj.origin.X) + x;
                int objy = b.Y - (int)(obj.GlobalPosition.Y - obj.origin.Y) + y;
                if (sprite.IsTranslucent(thisx, thisy) && obj.sprite.IsTranslucent(objx, objy))
                {
                    return true;
                }
            }
        }
        return false;
    }
}

