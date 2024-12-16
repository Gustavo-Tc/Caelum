
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Caelum
{
    public class Sprite
    {
        public static readonly int G_Scale = 4;

        public static int TILESIZE = 64;

        public static ContentManager Content;

        public Texture2D texture;

        public Actor Actor;
        
        public GameTime _gameTime;

        public Sprite(string _texture, Actor _actor)
        {
            this.texture = Content.Load<Texture2D>("Sprites/" + texture);
            this.Actor = _actor;
        }
        public Sprite(string _texture)
        {
            this.texture = Content.Load<Texture2D>("Sprites/" + _texture);
            this.Actor = null;
        }

        public Sprite(){
            this.texture = null;
            this.Actor = null;
        }

        public virtual Rectangle Rect()
        {   
            return new Rectangle((int)Actor.position.X, (int)Actor.position.Y, (int) texture.Width * (int)G_Scale, (int) texture.Height * (int)G_Scale);
        }

        public virtual Rectangle Rect(Vector2 _Position)
        {   
            return new Rectangle((int) _Position.X, (int) _Position.Y, (int) texture.Width * (int)G_Scale, (int) texture.Height * (int)G_Scale);
        }
        public virtual void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rect(), Color.White);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {   
            spriteBatch.Draw(texture, new Rectangle(Rect().X + (int)cameraPosition.X, Rect().Y + (int)cameraPosition.Y, Rect().Width,  Rect().Height), Color.White);
        }
    
        public virtual void SpecialDraw(SpriteBatch spriteBatch, Vector2 cameraPosition, Effect _effect, Texture2D _texture, Texture2D _texture2)
        {   
            spriteBatch.End();


            _effect.Parameters["SpriteTexture2"].SetValue( _texture2);
            _effect.Parameters["ElapsedTime"].SetValue((float) _gameTime.TotalGameTime.TotalSeconds);

            spriteBatch.Begin(effect: _effect);
            spriteBatch.Draw(texture, new Rectangle(Rect().X + (int)cameraPosition.X, Rect().Y + (int)cameraPosition.Y, Rect().Width,  Rect().Height), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(effect: null, samplerState: SamplerState.PointClamp);
        
        }
    }

    
    public class TilemapRenderer : Sprite
    {
        public string mapPath;
        public Dictionary<Vector2, int> tilemap;
        public TilemapRenderer(string mapPath, string texture, Actor actor)
        {
            this.texture = Content.Load<Texture2D>("Sprites/Tilemaps/" + texture);
            this.mapPath = mapPath;
            this.Actor = actor;

        }
        public Rectangle SourceRect(int id, Vector2 spriteSize, Vector2 grid)
        {
            int xpos = id;
            int ypos = 0;

            while (xpos > grid.X - 1)
            {
                xpos -= (int)grid.X;
                ypos++;
            }

            return new Rectangle(xpos * (int)spriteSize.X, ypos * (int)spriteSize.Y, (int)spriteSize.X, (int)spriteSize.Y);
        }
        public Dictionary<Vector2, int> LoadMap(string filepath)
        {
            Dictionary<Vector2, int> result = new();
            StreamReader reader = new(filepath);
            int y = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] cells = line.Split(",");
                for (int x = 0; x < cells.Length; x++)
                {
                    if (int.TryParse(cells[x], out int value))
                    {
                        result[new Vector2(x, y)] = value;
                    }
                }
                y++;
            }
            return result;
        }


        public override void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {
            foreach (var tile in LoadMap(mapPath))
            {
                Rectangle dest = new Rectangle(
                    (int)tile.Key.X * 16 * (int)Sprite.G_Scale + (int)cameraPosition.X  + (int) ScreenOffset().X,
                    (int)tile.Key.Y * 16 * (int)Sprite.G_Scale + (int)cameraPosition.Y + (int) ScreenOffset().Y,
                    16 * (int)Sprite.G_Scale,
                    16 * (int)Sprite.G_Scale
                );

                spriteBatch.Draw(texture, dest, SourceRect(tile.Value, new Vector2(16, 16), new Vector2(16, 68)), Color.White);
            }
        }

        public Vector2 ScreenOffset()
        {
            return new Vector2((GraphicsDeviceManager.DefaultBackBufferWidth / 2) , (GraphicsDeviceManager.DefaultBackBufferHeight / 2));
        }
    }

    public class SpriteAnimation : Sprite
    {
        int gridWidth;
        int gridHeight;

        public Animation animation;
        Vector2 size;


        int counter;
        int interval = 30;

        public int frame = 0;

        public Dictionary<string,Animation> animations;

        public SpriteAnimation(string texture, Actor actor, string _animation) : base(texture, actor)
        {
            AnimationsLoad(texture);
            ChangeAnimation(_animation);

            this.counter = 0;
            this.frame = 0;
        }

        public void AnimationsLoad(string _spriteName){
            Dictionary<string,Animation> result = new();

            if(!File.Exists("Content/Animations/" + _spriteName + "_Animations.csv")){Console.WriteLine(_spriteName + " has not a Animation Sheet!"); return;}
            
            StreamReader reader = new StreamReader("Content/Animations/" + _spriteName + "_Animations.csv");

            string line = reader.ReadLine();

            List<string> indexCells = line.Split(";").ToList<string>();

            if( indexCells[0] != ""){
                    //Sprite sheet name
                    //Console.WriteLine(indexCells[0]);
                    //Sprite sheet columns
                    if(indexCells[1] != null)
                    {
                        this.gridWidth = int.Parse(indexCells[1]);
                        //Console.WriteLine(indexCells[1]);
                    }
                    //Sprite sheet lines
                    if(indexCells[2] != null)
                    {
                        this.gridHeight = int.Parse(indexCells[2]);
                        //Console.WriteLine(indexCells[2]);
                    }
                    //Sprite sheet single sprite size
                    if(indexCells[3] != null)
                    {
                        this.size = new Vector2(int.Parse(indexCells[3].Split(",")[0]), int.Parse(indexCells[3].Split(",")[1]));
                        //Console.WriteLine("Vector 2: " + int.Parse(indexCells[3].Split(",")[0]) + " , " + int.Parse(indexCells[3].Split(",")[1]));
                    }
                    //Animation interval
                    if(indexCells[4] != null)
                    {
                        this.interval = int.Parse(indexCells[4]);
                        //Console.WriteLine(indexCells[4]);
                    }

            }


            while((line = reader.ReadLine()) != null){
                //Console.WriteLine(line);
                List<string> cells = line.Split(";").ToList<string>();

                Animation _animation = new();

                if( cells[0] != ""){

                    if(cells[0] != null)
                    {
                        _animation.Name = cells[0];
                    }
                    
                    if(cells[1] != null)
                    {
                        _animation.Loop = bool.Parse(cells[1]);
                    }

                    for(int i = 2; i < cells.Count; i++)
                    {
                        if(cells[i] != ""){
                        int value = int.Parse(cells[i]);
                        _animation.Frames.Add(value);
                        }
                    }
                }

                result.Add(_animation.Name, _animation);
            }

            animations = result;
        }

        public void ChangeAnimation(string _AnimationName){
            if(animations.Keys.ToList<string>().Contains(_AnimationName)){
                this.animation = animations[_AnimationName];
            }
        }

        public override void Update(GameTime gameTime)
        {
            //Debug.WriteLine("Played the frame " + frame);

            counter++;
            if (counter >= interval)
            {
                counter = 0;
                UpdateFrame();
            }
        }

        private void UpdateFrame(){
            
                if(frame < animation.Frames.Count - 1){
                    frame++;
                    //Debug.WriteLine("Updated frame " + frame);
                }else if(animation.Loop){
                    frame = 0;
                    //Debug.WriteLine("Loop reseted " + frame); 
                }
        }

        Vector2 Target(int _frame){
            int x = 0;
            if(_frame != 0){
                
                    x = _frame % gridWidth;
            }

            int y = (int) Math.Floor(_frame / (double) gridWidth);
            return new Vector2(x,y);
        }

        public Rectangle GetFrame()
        {
            if(animation.Frames.Count <= frame){ 
                Debug.WriteLine("Fudeo");
                Debug.WriteLine("Animação que deu o carai: " + animation.Name);
                Debug.WriteLine("Quantos frames essa animação tem: " + animation.Frames.Count);
                Debug.WriteLine("Qual frame que ia tocar: " + frame);
                }

            return new Rectangle((int) Target(animation.Frames[frame]).X * (int)size.X, (int) Target(animation.Frames[frame]).Y * (int)size.Y, (int)size.X, (int)size.Y);
        }

        public override Rectangle Rect()
        {
            return new Rectangle((int)Actor.position.X, (int)Actor.position.Y, (int)size.X * (int)G_Scale, (int)size.Y * (int)G_Scale);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {
            spriteBatch.Draw(texture, new Rectangle(Rect().X + (int)cameraPosition.X, Rect().Y + (int)cameraPosition.Y, Rect().Width, Rect().Height), GetFrame(), Color.White);
        }


    }

    public class Animation
    {
        public string Name;
        public bool Loop;
        public List<int> Frames = new();

        public Animation(string _Name, bool _Loop, List<int> _Frames){
            this.Name = _Name;
            this.Loop = _Loop;
            this.Frames = _Frames;
        }

        public Animation(string _Name, string _AnimationPath){
            Animation load = AnimationLoad(_AnimationPath, _Name);
            
            this.Name = load.Name;
            this.Loop = load.Loop;
            this.Frames = load.Frames;

        }

        public Animation(){
            this.Name = "Null";
            this.Loop = false;
            this.Frames = new();

        }

        public Animation AnimationLoad(string _filePath, string _animation)
        {
            Animation result = new();
            
             StreamReader reader = new(_filePath);
            
            
            string line;
            reader.ReadLine();
            while ((line = reader.ReadLine()) != null)
            {
                if( line.Split(";")[0] == _animation){

                    List<string> cells = line.Split(";").ToList<string>();
                    
                    //Debug.WriteLine("The animation " + _animation + " has " + cells.Count + " cells");
                    if(cells[0] != null)
                    {
                        result.Name = cells[0];
                    }
                    
                    if(cells[1] != null)
                    {
                        result.Loop = bool.Parse(cells[1]);
                    }

                    for(int i = 2; i < cells.Count; i++)
                    {
                        if(cells[i] != ""){
                        int value = int.Parse(cells[i]);
                        //Debug.WriteLine(value + " Has been added");
                        result.Frames.Add(value);
                        }
                    }
                    
                    //Debug.WriteLine("The animation " + _animation + " has " + result.Frames.Count + " frames");
                }
            }

            return result;
        }

    }

}
