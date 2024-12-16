using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Caelum;

public class GameRuntime : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;

    private SceneManager sceneManager = new SceneManager();

    public GameRuntime()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        this.IsFixedTimeStep = false;
        sceneManager.Initialize();
        sceneManager.GetScene().Initialize();
        Sprite.Content = Content;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        sceneManager.Load();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();


        sceneManager.GetScene().Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        sceneManager.GetScene().Draw(spriteBatch);

        base.Draw(gameTime);
    }
}
