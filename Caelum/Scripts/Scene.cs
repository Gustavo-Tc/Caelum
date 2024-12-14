using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Caelum;

public class Scene
{
    protected List<Actor> actors = new();
    protected Actor root = new Actor();
    public virtual void Initialize(){
        root.Initialize();
    }

    public virtual void Update(GameTime gameTime){
        root.Update(gameTime);
        foreach(Actor actor in root.children){
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch){
        root.Draw(spriteBatch);
    }

}
