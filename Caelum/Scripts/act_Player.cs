using System;
using Microsoft.Xna.Framework;

namespace Caelum.Scripts;

public class Player : Actor
{
    public Player(Actor _parent){
        this.name = "Player";
        this.parent = _parent;
        _parent.children.Add(this);
    }

}
