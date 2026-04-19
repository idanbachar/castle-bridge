using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CastleBridge.Client
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class CastleBridge : Game
    {
        //Game graphics scale:
        public static GraphicsDeviceManager Graphics;

        //Game drawing var:
        public static SpriteBatch SpriteBatch;

        //Game's screen dictionary (Game screen/ Menu screen):
        public Dictionary<ScreenType, Screen> Screens;

        //Game's current displayed screen:
        public ScreenType CurrentScreen;

        //Game's content for loading stuff like textures/fonts/audio:
        public static ContentManager PublicContent;

        /// <summary>
        /// Castle bridge class base:
        /// </summary>
        public CastleBridge()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 1400;
            Graphics.PreferredBackBufferHeight = 728;
 
            Content.RootDirectory = "Content";
            PublicContent = Content;
            
            IsMouseVisible = true;

            Screens = new Dictionary<ScreenType, Screen>();
            CurrentScreen = ScreenType.Menu;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            //Create game's screens:
            Screens.Add(ScreenType.Menu, new MenuScreen(GraphicsDevice.Viewport));
            Screens.Add(ScreenType.Game, new GameScreen(GraphicsDevice.Viewport));

            //Init game events:
            InitEvents();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// Init game's screens events to communicate each other.
        /// </summary>
        private void InitEvents() {
            ((MenuScreen)Screens[ScreenType.Menu]).OnStartGame += StartJoinSession;
            ((GameScreen)Screens[ScreenType.Game]).OnStartGameAfterLoading += StartGame;
            ((GameScreen)Screens[ScreenType.Game]).GetGameClient().OnUpdateLoadingPercent += ((LoadingMenu)((MenuScreen)Screens[ScreenType.Menu]).GetMenu(MenuPage.Loading)).UpdateText;
            ((GameScreen)Screens[ScreenType.Game]).GetGameClient().OnConnectionLost += ((MenuScreen)Screens[ScreenType.Menu]).ConnectionLost;
        }

        /// <summary>
        /// When called, start creating client connection and load map entities items
        /// and finally starts receiving and sending player's data to the server.
        /// </summary>
        /// <param name="characterName"></param>
        /// <param name="team"></param>
        /// <param name="name"></param>
        private void StartJoinSession(CharacterName characterName, TeamName team, string name) {

            ((MenuScreen)Screens[ScreenType.Menu]).GoToPage(MenuPage.Loading);
            ((GameScreen)Screens[ScreenType.Game]).JoinGame(characterName, team, name);
            ((GameScreen)Screens[ScreenType.Game]).UpdateHud();
        }

        /// <summary>
        /// Changing current game screen to Game.
        /// </summary>
        private void StartGame() {
            CurrentScreen = ScreenType.Game;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Environment.Exit(Environment.ExitCode);

            // TODO: Add your update logic here

            //Updates only current displayed screen:
            Screens [CurrentScreen].Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(81, 234, 255));
 
            //Drawing only current displayed scren:
            Screens [CurrentScreen].Draw();

            base.Draw(gameTime);
        }
    }
}
