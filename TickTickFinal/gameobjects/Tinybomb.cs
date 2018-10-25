using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class TinyBomb : SpriteGameObject
{
    private bool started = false;
    public int direction;
    private int ThrowSpeed=500;
    private int counter;
    private int counter2;
    public bool explode = false;
    public TileField tiles;
    public TinyBomb(int layer = 0, string id = "") : base("Sprites/spr_water", layer, id)
    {
        //position += new Vector2(40, -50);//werkt for some reason niet
    }
    public override void Update(GameTime gameTime)
    {
        counter++;
        if(counter == 150)
        {
            explode = true;
        }
        base.Update(gameTime);
        if (started == false)
        {
            position += new Vector2(50*direction, -50);
            velocity.Y = -1000;
            started = true;
        }
        if (explode != true)
        {
            counter2 = 0;
            Dosomecoolstuff();
        }

    }
    
    private void Dosomecoolstuff()
    {
        DoPhysics();
        //KillEnemies();
    }
    public void DoPhysics()
    {

        velocity.X = direction * ThrowSpeed;
        velocity.Y += 55;
        if (Colision())
        {
            int moveit = 1;

            velocity *= -0.5f;
            while (Colision()&&counter2 <=300)
            {
                counter2++;
                position.Y += moveit;
                if (moveit < 0)
                {
                    moveit *= -1;
                    moveit += 1;
                }
                else if (moveit > 0)
                {
                    moveit *= -1;
                    moveit -= +1;
                }
            }
        }
    }
    public bool Colision()
    {
        int xFloor = (int)position.X / tiles.CellWidth;
        int yFloor = (int)position.Y / tiles.CellHeight;

        for (int y = yFloor - 2; y <= yFloor + 1; ++y)
        {
            for (int x = xFloor - 1; x <= xFloor + 1; ++x)
            {
                TileType tileType = tiles.GetTileType(x, y);
                if (tileType == TileType.Background)
                {
                    continue;
                }
                Tile currentTile = tiles.Get(x, y) as Tile;
                Rectangle tileBounds = new Rectangle(x * tiles.CellWidth, y * tiles.CellHeight,
                                                        tiles.CellWidth, tiles.CellHeight);
                Rectangle boundingBox = this.BoundingBox;
                boundingBox.Height += 1;
                if (((currentTile != null && !currentTile.CollidesWith(this)) || currentTile == null) && !tileBounds.Intersects(boundingBox))
                {
                    continue;
                }
                Vector2 depth = Collision.CalculateIntersectionDepth(boundingBox, tileBounds);
                if (Math.Abs(depth.X) < Math.Abs(depth.Y))
                {
                    if (tileType == TileType.Normal)
                    {
                        return true;
                    }
                    continue;
                }
            }
        }
        return false;
    }

}
