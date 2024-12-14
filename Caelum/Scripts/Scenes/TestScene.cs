using System;
using Caelum.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Caelum;

public class TestScene : Scene
{
    public override void Initialize()
    {
        actors.Add(new Player(root));
        base.Initialize();
    }
    public override void Update(GameTime gameTime){
        base.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}
