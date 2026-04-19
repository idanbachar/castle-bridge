using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public abstract class Character {

        protected int Health; //Character's health
        protected int MaxHealth; //Character's max health
        protected int Level; //Character's level
        protected int Xp; //Character's xp
        protected int MaxXp; //Character's max xp
        protected CharacterName Name; //Character's type name
        protected Animation CurrentAnimation; //Character's current animation
        public Animation AfkAnimation; //Character's Afk animation
        public Animation WalkAnimation; //Character's Walk animation
        public Animation AttackAnimation; //Character's Attack animation
        public Animation DefenceAnimation; //Character's Defence animation
        public Animation LootAnimation; //Character's Loot animation
        protected Direction Direction; //Character's direction
        protected PlayerState State; //Character's state
        protected TeamName TeamName; //Character's team
        private Rectangle StartingRectangle; //Characters starting rectangle
        public bool IsDead; //Character's dead indication

        /// <summary>
        /// Receives character's type name, team, coordinates and size
        /// and creates a character
        /// </summary>
        /// <param name="name"></param>
        /// <param name="teamName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Character(CharacterName name, TeamName teamName, int x, int y, int width, int height) {

            Name = name;
            TeamName = teamName;
            StartingRectangle = new Rectangle(x, y, width, height);

            //Initialize animations:
            InitAnimations();

            Health = 100;
            MaxHealth = 100;
            State = PlayerState.Afk;

            //Set current animation by state:
            SetCurrentAnimation(State);
            
            Level = 0;
            Xp = 0;
            MaxXp = 100;
            IsDead = false;
        }

        /// <summary>
        /// Initializes animations
        /// </summary>
        private void InitAnimations() {
            AfkAnimation = new Animation("player/characters/teams/" + TeamName + "/" + Name + "/afk/" + Name + "_afk_", StartingRectangle, 0, 6, 6, 5, true, true);
            WalkAnimation = new Animation("player/characters/teams/" + TeamName + "/" + Name + "/walk/" + Name + "_walk_", StartingRectangle, 0, 4, 4, 3, true, true);
            AttackAnimation = new Animation("player/characters/teams/" + TeamName + "/" + Name + "/attack/" + Name + "_attack_", StartingRectangle, 0, 7, 7, 4, false, false);
            DefenceAnimation = new Animation("player/characters/teams/" + TeamName + "/" + Name + "/defence/" + Name + "_defence_", StartingRectangle, 0, 5, 5, 3, true, false);
            LootAnimation = new Animation("player/characters/teams/" + TeamName + "/" + Name + "/loot/" + Name + "_loot_", StartingRectangle, 0, 5, 5, 2, true, false);
        }

        /// <summary>
        /// Receives state and applies current animation
        /// </summary>
        /// <param name="State"></param>
        public void SetCurrentAnimation(PlayerState State) {

            switch (State) {
                case PlayerState.Afk:
                    CurrentAnimation = AfkAnimation;
                    break;
                case PlayerState.Walk:
                    CurrentAnimation = WalkAnimation;
                    break;
                case PlayerState.Attack:
                    CurrentAnimation = AttackAnimation;
                    break;
                case PlayerState.Defence:
                    CurrentAnimation = DefenceAnimation;
                    break;
                case PlayerState.Loot:
                    CurrentAnimation = LootAnimation;
                    break;
            }
        }

        /// <summary>
        /// Receives a new direction
        /// and applies it
        /// </summary>
        /// <param name="newDirection"></param>
        public void SetDirection(Direction newDirection) {

            Direction = newDirection;

            //Applies direction on all animations:
            AfkAnimation.SetDirection(newDirection);
            WalkAnimation.SetDirection(newDirection);
            AttackAnimation.SetDirection(newDirection);
            LootAnimation.SetDirection(newDirection);
            DefenceAnimation.SetDirection(newDirection);

        }

        /// <summary>
        /// Receives true/false of visibility and applies it on character's visibility
        /// </summary>
        /// <param name="value"></param>
        public void SetVisible(bool value) {

            //Applies visible on all animations:
            AfkAnimation.SetVisible(value);
            WalkAnimation.SetVisible(value);
            AttackAnimation.SetVisible(value);
            LootAnimation.SetVisible(value);
            DefenceAnimation.SetVisible(value);
        }

        /// <summary>
        /// Receives color and applies it on character's color
        /// </summary>
        public void SetColor(Color color) {

            //Applies color on all animations:
            AfkAnimation.SetColor(color);
            WalkAnimation.SetColor(color);
            AttackAnimation.SetColor(color);
            LootAnimation.SetColor(color);
            DefenceAnimation.SetColor(color);
        }

        /// <summary>
        /// Receives a new rectangle
        /// and applies it
        /// </summary>
        /// <param name="newRectangle"></param>
        public void SetRectangle(Rectangle newRectangle) {

            //Applies rectangle on all animations:
            AfkAnimation.SetRectangle(newRectangle.X, newRectangle.Y, newRectangle.Width, newRectangle.Height);
            WalkAnimation.SetRectangle(newRectangle.X, newRectangle.Y, newRectangle.Width, newRectangle.Height);
            AttackAnimation.SetRectangle(newRectangle.X, newRectangle.Y, newRectangle.Width, newRectangle.Height);
            LootAnimation.SetRectangle(newRectangle.X, newRectangle.Y, newRectangle.Width, newRectangle.Height);
            DefenceAnimation.SetRectangle(newRectangle.X, newRectangle.Y, newRectangle.Width, newRectangle.Height);

        }

        /// <summary>
        /// Receives team and applies it
        /// </summary>
        /// <param name="team"></param>
        public void ChangeTeam(TeamName team) {
            TeamName = team;
        }

        /// <summary>
        /// Checks if mouse over character (basiclly used on main menu for character selection)
        /// </summary>
        /// <returns></returns>
        public bool IsMouseOver() {

            Rectangle mouseRectangle = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 20, 20);

            if (mouseRectangle.Intersects(CurrentAnimation.GetCurrentSpriteImage().GetRectangle())) {
                SetColor(Color.DarkGray);
                return true;
            }

            SetColor(Color.White);
            return false;
        }

        /// <summary>
        /// Get current animation
        /// </summary>
        /// <returns></returns>
        public Animation GetCurrentAnimation() {
            return CurrentAnimation;
        }

        /// <summary>
        /// Updates current animation
        /// </summary>
        public virtual void Update() {
            CurrentAnimation.Play();
            CurrentAnimation.Start();
        }

        /// <summary>
        /// Receives hp and sets health by it
        /// </summary>
        /// <param name="hp"></param>
        public void SetHealth(int hp) {
            Health = hp;

            //Check if player is dead:
            CheckIfDead();
        }

        /// <summary>
        /// Checks if character is dead and indicates 'IsDead' var
        /// </summary>
        private void CheckIfDead() {
            if (Health <= 0) IsDead = true;
            else IsDead = false;
        }

        /// <summary>
        /// Receives hp and increases health by it
        /// </summary>
        /// <param name="hp"></param>
        public void IncreaseHp(int hp) {
            Health += hp;
            if (Health >= MaxHealth)
                Health = MaxHealth;

            //Check if player is dead:
            CheckIfDead();
        }

        /// <summary>
        /// Receives hp and decreases health by it
        /// </summary>
        /// <param name="hp"></param>
        public void DecreaseHp(int hp) {
            Health -= hp;
            if (Health <= 0)
                Health = 0;

            //Check if player is dead:
            CheckIfDead();
        }

        /// <summary>
        /// Get health
        /// </summary>
        /// <returns></returns>
        public int GetHealth() {
            return Health;
        }


        /// <summary>
        /// Get max health
        /// </summary>
        /// <returns></returns>
        public int GetMaxHealth() {
            return MaxHealth;
        }


        /// <summary>
        /// Get level
        /// </summary>
        /// <returns></returns>
        public int GetLevel() {
            return Level;
        }

        /// <summary>
        /// Get xp
        /// </summary>
        /// <returns></returns>
        public int GetXp() {
            return Xp;
        }

        /// <summary>
        /// Get max xp
        /// </summary>
        /// <returns></returns>
        public int GetMaxXp() {
            return MaxXp;
        }

        /// <summary>
        /// Increase level up by 1
        /// and resets xp to 0
        /// </summary>
        public void LevelUp() {
            Level++;
            Xp = 0;
            MaxXp += 100;
        }

        /// <summary>
        /// Receives xp points and increases it
        /// </summary>
        /// <param name="xp"></param>
        public void AddXp(int xp) {

            Xp += xp;
            if (Xp >= MaxXp)
                LevelUp();
        }

        /// <summary>
        /// Get name
        /// </summary>
        /// <returns></returns>
        public CharacterName GetName() {
            return Name;
        }

        /// <summary>
        /// Get team
        /// </summary>
        /// <returns></returns>
        public TeamName GetTeamName() {
            return TeamName;
        }


        /// <summary>
        /// Get direction
        /// </summary>
        /// <returns></returns>
        public Direction GetDirection() {
            return Direction;
        }

        /// <summary>
        /// Draw character
        /// </summary>
        public virtual void Draw() {
            CurrentAnimation.Draw();
        }
    }
}
