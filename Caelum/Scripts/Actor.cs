using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Caelum;

public class Actor
{
    public string name {get; protected set;}
    public Actor parent;
    public List<Actor> children = new();

    public Actor(Actor _parent, string _name){
        this.name = _name;
        this.parent = _parent;
        _parent.children.Add(this);
    }
    public Actor(){
        this.name = "root";
        this.parent = null;
    }

    public virtual void Initialize(){
        for(int i = 0; i < children.Count; i++){
            children[i].Initialize();
        }
    }

    public virtual void Update(GameTime gameTime){
        for(int i = 0; i < children.Count; i++){
            children[i].Update(gameTime);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch){
        for(int i = 0; i < children.Count; i++){
            children[i].Draw(spriteBatch);
        }
    }
}
