using System;
using System.Collections.Generic;

namespace Caelum;

public class SceneManager
{
    List<Scene> Scenes = new();
    int ActualScene = 0;

    public void Initialize(){
        AddScene(new TestScene());
        
    }

    public void Load(){

    }

    public void AddScene(Scene scene){
        this.Scenes.Add(scene);
    }

    public void SetScene(int i){
        ActualScene = i;
    }

    public Scene GetScene(){
        return Scenes[ActualScene];
    }
}
