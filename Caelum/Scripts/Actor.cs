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
    public Vector2 position;

    public Sprite sprite;
    

    public Actor(Actor _parent, string _name){
        this.name = _name;
        this.parent = _parent;
        _parent.children.Add(this);
    }
    public Actor(){
        this.name = "none";
        this.parent = null;
    }

    public virtual void Initialize(){

    }

    public virtual void Load(){
        sprite = new Sprite(name);       
    }

    public virtual void Update(GameTime gameTime){

    }

    public virtual void Draw(SpriteBatch spriteBatch){
        sprite.Draw(spriteBatch);
    }

    public Vector2 GetWorldPosition(){
        if(parent != null){
            return this.position + parent.position;
        }
        return this.position;
    }
}
