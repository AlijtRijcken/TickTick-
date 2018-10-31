using System;
using Microsoft.Xna.Framework;

class TimerGameObject : TextGameObject
{
    protected TimeSpan timeLeft;
    protected bool running;
    protected double multiplier;
    public double time; //ADDED ----------------------------------

    public TimerGameObject(double _time, int layer = 0, string id = "")
        : base("Fonts/Hud", layer, id)
    {
        time = _time;   //ADDED-----------------------------------
        multiplier = 1;

        Level level = GameWorld as Level;   //ADDED----------------------------------- 
        timeLeft = TimeSpan.FromSeconds(time);
        running = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (!running)
        {
            return;
        }
        double totalSeconds = gameTime.ElapsedGameTime.TotalSeconds * multiplier;
        timeLeft -= TimeSpan.FromSeconds(totalSeconds);
        if (timeLeft.Ticks < 0)
        {
            return;
        }
        DateTime timeleft = new DateTime(timeLeft.Ticks);
        text = timeleft.ToString("mm:ss");
        color = Color.Yellow;
        if (timeLeft.TotalSeconds <= 10 && (int)timeLeft.TotalSeconds % 2 == 0)
        {
            color = Color.Red;
        }
    }

    public override void Reset()
    {
        base.Reset();

        timeLeft = TimeSpan.FromSeconds(time);

        running = true;
    }

    public bool Running
    {
        get { return running; }
        set { running = value; }
    }

    public double Multiplier
    {
        get {return multiplier; }
        set { multiplier = value; }
    }

    public bool GameOver
    {
        get { return (timeLeft.Ticks <= 0); }
    }
}