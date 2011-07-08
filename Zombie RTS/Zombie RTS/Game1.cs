using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Zombie_RTS
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int BUTTON_HEIGHT = 50;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        RTSManager manager;

        enum gameState
        {
            Start,
            Playing,
            Options,
            OptionsDifficulty,
            OptionsResolution,
            Paused,
            GameOver
        }

        //start menu
        Button start, exit, options, save, load, resume;

        //options menu
        Button difficulty, easy, medium, hard;
        Button resolution, small, mediumRes, large;
        Button fullScreen;

        //game over menu
        Button PlayAgain, mainMenu, showScore;

        Texture2D bckTex;
        Rectangle backg;

        MouseState oldMouse;
        KeyboardState oldKeys;

        gameState curState = gameState.Start;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            IsMouseVisible = true;
            //gameOptions = GameOptions.getInstance();
            GameOptions.setGraphics(graphics);
            GameOptions.setScreen(1920, 1080);
            GameOptions.setDifficulty(GameOptions.GameDifficulties.Hard);
            //GameOptions.toggleFullScreen();

            start = new Button(0, Window.ClientBounds.Height / 2, Window.ClientBounds.Width, BUTTON_HEIGHT, 1);
            resume = new Button(0, Window.ClientBounds.Height / 2, Window.ClientBounds.Width, BUTTON_HEIGHT, 1);
            exit = new Button(0, (Window.ClientBounds.Height / 2) + 400, Window.ClientBounds.Width, BUTTON_HEIGHT, 5);
            options = new Button(0, (Window.ClientBounds.Height / 2) + 100, Window.ClientBounds.Width, BUTTON_HEIGHT, 2);
            save = new Button(0, (Window.ClientBounds.Height / 2) + 200, Window.ClientBounds.Width, BUTTON_HEIGHT, 3);
            load = new Button(0, (Window.ClientBounds.Width / 2) + 300, Window.ClientBounds.Width, BUTTON_HEIGHT, 4);

            start.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            resume.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            exit.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            options.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            save.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            load.setFont(Content.Load<SpriteFont>("displayInfoFont"));

            start.setTexture(Content.Load<Texture2D>("button"));
            resume.setTexture(Content.Load<Texture2D>("button"));
            exit.setTexture(Content.Load<Texture2D>("button"));
            options.setTexture(Content.Load<Texture2D>("button"));
            save.setTexture(Content.Load<Texture2D>("button"));
            load.setTexture(Content.Load<Texture2D>("button"));

            start.setText("Start Game");
            resume.setText("Resume");
            exit.setText("Exit Game");
            options.setText("Options");
            save.setText("Save - not working");
            load.setText("Load - not working");

            backg = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            bckTex = Content.Load<Texture2D>("background");

            difficulty = new Button(0, 200, GameOptions.getWidth(), BUTTON_HEIGHT, 1);
            resolution = new Button(0, 300, GameOptions.getWidth(), BUTTON_HEIGHT, 2);
            fullScreen = new Button(0, 400, GameOptions.getWidth(), BUTTON_HEIGHT, 3);

            difficulty.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            resolution.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            fullScreen.setFont(Content.Load<SpriteFont>("displayInfoFont"));

            difficulty.setTexture(Content.Load<Texture2D>("button"));
            resolution.setTexture(Content.Load<Texture2D>("button"));
            fullScreen.setTexture(Content.Load<Texture2D>("button"));

            difficulty.setText("Difficulty");
            resolution.setText("Resolution");
            fullScreen.setText("Toggle Fullscreen");

            easy = new Button(0, 200, GameOptions.getWidth(), BUTTON_HEIGHT, 1);
            medium = new Button(0, 300, GameOptions.getWidth(), BUTTON_HEIGHT, 2);
            hard = new Button(0, 400, GameOptions.getWidth(), BUTTON_HEIGHT, 3);

            easy.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            medium.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            hard.setFont(Content.Load<SpriteFont>("displayInfoFont"));

            easy.setTexture(Content.Load<Texture2D>("button"));
            medium.setTexture(Content.Load<Texture2D>("button"));
            hard.setTexture(Content.Load<Texture2D>("button"));

            easy.setText("Easy");
            medium.setText("Medium");
            hard.setText("Hard");

            small = new Button(0, 200, GameOptions.getWidth(), BUTTON_HEIGHT, 1);
            mediumRes = new Button(0, 300, GameOptions.getWidth(), BUTTON_HEIGHT, 2);
            large = new Button(0, 400, GameOptions.getWidth(), BUTTON_HEIGHT, 3);

            small.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            mediumRes.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            large.setFont(Content.Load<SpriteFont>("displayInfoFont"));

            small.setTexture(Content.Load<Texture2D>("button"));
            mediumRes.setTexture(Content.Load<Texture2D>("button"));
            large.setTexture(Content.Load<Texture2D>("button"));

            small.setText("800x600");
            mediumRes.setText("1280x1024");
            large.setText("1920x1080");

            PlayAgain = new Button(0, 200, GameOptions.getWidth(), BUTTON_HEIGHT, 1);
            mainMenu = new Button(0, 300, GameOptions.getWidth(), BUTTON_HEIGHT, 2);
            showScore = new Button(0, 400, GameOptions.getWidth(), BUTTON_HEIGHT, 0);

            PlayAgain.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            mainMenu.setFont(Content.Load<SpriteFont>("displayInfoFont"));
            showScore.setFont(Content.Load<SpriteFont>("displayInfoFont"));

            PlayAgain.setTexture(Content.Load<Texture2D>("button"));
            mainMenu.setTexture(Content.Load<Texture2D>("button"));
            showScore.setTexture(Content.Load<Texture2D>("button"));

            PlayAgain.setText("Play Again");
            mainMenu.setText("Back to Main Menu");
            showScore.setText(RTSManager.getScore().ToString());

            oldMouse = Mouse.GetState();
            oldKeys = Keyboard.GetState();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content her
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            MouseState mouse = Mouse.GetState();
            KeyboardState keys = Keyboard.GetState();

            switch(curState)
            {
                case gameState.Start:
                    gameStart(gameTime, ref mouse, ref keys);
                    break;
                case gameState.Playing:
                    gamePlaying(gameTime, ref mouse, ref keys);
                    break;
                case gameState.Options:
                    Options(gameTime, ref mouse, ref keys);
                    break;
                case gameState.OptionsResolution:
                    resOptions(gameTime, ref mouse, ref keys);
                    break;
                case gameState.OptionsDifficulty:
                    diffOptions(gameTime, ref mouse, ref keys);
                    break;
                case gameState.Paused:
                    gamePaused(gameTime, ref mouse, ref keys);
                    break;
                case gameState.GameOver:
                    gameOver(gameTime, ref mouse, ref keys);
                    break;
                default:
                    break;
            }

            oldMouse = mouse;
            oldKeys = keys;

            base.Update(gameTime);
        }

        private void gameOver(GameTime gameTime, ref MouseState mouse, ref KeyboardState keys)
        {
            PlayAgain.mouseOver(mouse);
            mainMenu.mouseOver(mouse);
            showScore.setText("Score: " + RTSManager.getScore().ToString());

            if (PlayAgain.pressed(mouse, oldMouse))
            {
                newGame();
            }

            if (mainMenu.pressed(mouse, oldMouse))
            {
                curState = gameState.Start;
            }

        }

        private void diffOptions(GameTime gameTime, ref MouseState mouse, ref KeyboardState keys)
        {
            easy.mouseOver(mouse);
            medium.mouseOver(mouse);
            hard.mouseOver(mouse);

            if (easy.pressed(mouse, oldMouse))
            {
                GameOptions.setDifficulty(GameOptions.GameDifficulties.Easy);

                if (GameOptions.isGamePlaying())
                {
                    curState = gameState.Paused;
                }
                else
                {
                    curState = gameState.Start;
                }
            }

            if (medium.pressed(mouse, oldMouse))
            {
                GameOptions.setDifficulty(GameOptions.GameDifficulties.Medium);

                if (GameOptions.isGamePlaying())
                {
                    curState = gameState.Paused;
                }
                else
                {
                    curState = gameState.Start;
                }
            }

            if (hard.pressed(mouse, oldMouse))
            {
                GameOptions.setDifficulty(GameOptions.GameDifficulties.Hard);

                if (GameOptions.isGamePlaying())
                {
                    curState = gameState.Paused;
                }
                else
                {
                    curState = gameState.Start;
                }
            }
        }

        private void resOptions(GameTime gameTime, ref MouseState mouse, ref KeyboardState keys)
        {
            small.mouseOver(mouse);
            mediumRes.mouseOver(mouse);
            large.mouseOver(mouse);

            if (small.pressed(mouse, oldMouse))
            {
                GameOptions.setScreen(800, 600);

                if (GameOptions.isGamePlaying())
                {
                    curState = gameState.Paused;
                }
                else
                {
                    curState = gameState.Start;
                } 
            }

            if (mediumRes.pressed(mouse, oldMouse))
            {
                GameOptions.setScreen(1280, 1024);

                if (GameOptions.isGamePlaying())
                {
                    curState = gameState.Paused;
                }
                else
                {
                    curState = gameState.Start;
                }
            }

            if (large.pressed(mouse, oldMouse))
            {
                GameOptions.setScreen(1920, 1080);

                if (GameOptions.isGamePlaying())
                {
                    curState = gameState.Paused;
                }
                else
                {
                    curState = gameState.Start;
                }
            }
        }

        private void Options(GameTime gameTime, ref MouseState mouse, ref KeyboardState keys)
        {
            if (!GameOptions.isGamePlaying())
            {
                difficulty.mouseOver(mouse);
            }

            resolution.mouseOver(mouse);
            fullScreen.mouseOver(mouse);
            
            if (difficulty.pressed(mouse, oldMouse) && !GameOptions.isGamePlaying())
            {
                curState = gameState.OptionsDifficulty;
            }

            if (resolution.pressed(mouse, oldMouse))
            {
                curState = gameState.OptionsResolution;
            }

            if (fullScreen.pressed(mouse, oldMouse))
            {
                GameOptions.toggleFullScreen();

                if (GameOptions.isGamePlaying())
                {
                    curState = gameState.Paused;
                }
                else
                {
                    curState = gameState.Start;
                }
            }
        }

        private void gameStart(GameTime gameTime, ref MouseState mouse, ref KeyboardState keys)
        {
            start.mouseOver(mouse);
            exit.mouseOver(mouse);
            options.mouseOver(mouse);
            save.mouseOver(mouse);
            load.mouseOver(mouse);

            if (start.pressed(mouse, oldMouse))
            {
                newGame();
            }

            if (exit.pressed(mouse, oldMouse))
            {
                this.Exit();
            }

            if (options.pressed(mouse, oldMouse))
            {
                curState = gameState.Options;
            }

            if (save.pressed(mouse, oldMouse))
            {
            }

            if (load.pressed(mouse, oldMouse))
            {
            }
        }

        private void newGame()
        {
            manager = null;
            manager = RTSManager.Instance(Window.ClientBounds.Width, Window.ClientBounds.Height, Content);
            RTSManager.NewGame();

            curState = gameState.Playing;
        }

        private void gamePaused(GameTime gameTime, ref MouseState mouse, ref KeyboardState keys)
        {
            resume.mouseOver(mouse);
            exit.mouseOver(mouse);
            options.mouseOver(mouse);
            save.mouseOver(mouse);
            load.mouseOver(mouse);

            if (resume.pressed(mouse, oldMouse))
            {
                curState = gameState.Playing;
            }

            if (exit.pressed(mouse, oldMouse))
            {
                this.Exit();
            }

            if (options.pressed(mouse, oldMouse))
            {
                curState = gameState.Options;
            }

            if (save.pressed(mouse, oldMouse))
            {
            }

            if (load.pressed(mouse, oldMouse))
            {
            }
        }

        const int SLOWCAM = 200;
        const int FASTCAM = 100;
        const int FASTCAM_SPEED = 15;
        const int SLOWCAM_SPEED = 5;

        private void gamePlaying(GameTime gameTime, ref MouseState mouse, ref KeyboardState keys)
        {
            if (keys.IsKeyDown(Keys.Escape) && oldKeys.IsKeyUp(Keys.Escape))
            {
                curState = gameState.Paused;
            }

            if (mouse.X < SLOWCAM)
            {
                if (mouse.X < FASTCAM)
                {
                    manager.setCameraSpeed(FASTCAM_SPEED);
                    manager.moveWorldLeft();
                }
                else
                {
                    manager.setCameraSpeed(SLOWCAM_SPEED);
                    manager.moveWorldLeft();
                }
            }
            else if (mouse.X > (GameOptions.getWidth() - SLOWCAM))
            {
                if (mouse.X > (GameOptions.getWidth() - FASTCAM))
                {
                    manager.setCameraSpeed(FASTCAM_SPEED);
                    manager.moveWorldRight();
                }
                else
                {
                    manager.setCameraSpeed(SLOWCAM_SPEED);
                    manager.moveWorldRight();
                }
            }

            if (mouse.Y < SLOWCAM)
            {
                if (mouse.Y < FASTCAM)
                {
                    manager.setCameraSpeed(FASTCAM_SPEED);
                    manager.moveWorldUp();
                }
                else
                {
                    manager.setCameraSpeed(SLOWCAM_SPEED);
                    manager.moveWorldUp();
                }
            }
            else if (mouse.Y > (GameOptions.getHeight() - SLOWCAM))
            {
                if (mouse.Y > (GameOptions.getHeight() - FASTCAM))
                {
                    manager.setCameraSpeed(FASTCAM_SPEED);
                    manager.moveWorldDown();
                }
                else
                {
                    manager.setCameraSpeed(SLOWCAM_SPEED);
                    manager.moveWorldDown();
                }
            }

            manager.update(new Vector2(mouse.X, mouse.Y), mouse.LeftButton, mouse.RightButton, keys, gameTime);

            if (keys.IsKeyDown(Keys.Space))
            {
                RTSManager.REMOVEME();
            }

            if (GameOptions.isGameOver())
            {
                curState = gameState.GameOver;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            switch (curState)
            {
                case gameState.Playing:
                    manager.draw(spriteBatch);
                    break;
                case gameState.Start:
                    spriteBatch.Begin();
                    spriteBatch.Draw(bckTex, backg, Color.White);
                    spriteBatch.End();
                    start.draw(spriteBatch);
                    exit.draw(spriteBatch);
                    options.draw(spriteBatch);
                    save.draw(spriteBatch);
                    load.draw(spriteBatch);
                    break;
                case gameState.Options:
                    spriteBatch.Begin();
                    spriteBatch.Draw(bckTex, backg, Color.White);
                    spriteBatch.End();
                    if (!GameOptions.isGamePlaying())
                    {
                        difficulty.draw(spriteBatch);
                    }
                    resolution.draw(spriteBatch);
                    fullScreen.draw(spriteBatch);
                    break;
                case gameState.OptionsDifficulty:
                    spriteBatch.Begin();
                    spriteBatch.Draw(bckTex, backg, Color.White);
                    spriteBatch.End();
                    easy.draw(spriteBatch);
                    medium.draw(spriteBatch);
                    hard.draw(spriteBatch);
                    break;
                case gameState.OptionsResolution:
                    spriteBatch.Begin();
                    spriteBatch.Draw(bckTex, backg, Color.White);
                    spriteBatch.End();
                    small.draw(spriteBatch);
                    mediumRes.draw(spriteBatch);
                    large.draw(spriteBatch);
                    break;
                case gameState.Paused:
                    spriteBatch.Begin();
                    spriteBatch.Draw(bckTex, backg, Color.White);
                    spriteBatch.End();
                    resume.draw(spriteBatch);
                    exit.draw(spriteBatch);
                    options.draw(spriteBatch);
                    save.draw(spriteBatch);
                    load.draw(spriteBatch);
                    break;
                case gameState.GameOver:
                    spriteBatch.Begin();
                    spriteBatch.Draw(bckTex, backg, Color.White);
                    spriteBatch.End();
                    PlayAgain.draw(spriteBatch);
                    mainMenu.draw(spriteBatch);
                    showScore.draw(spriteBatch);
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
