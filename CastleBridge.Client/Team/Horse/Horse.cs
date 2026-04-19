using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Horse {
 
        private Rectangle Rectangle; //Horse's rectangle
        private HorseState State; //Horse's state
        private TeamName TeamName; //Horse's team
        public Animation AfkAnimation; //Horse's Afk animation
        public Animation WalkAnimation; //Horse's Walk animation
        private Animation CurrentAnimation; //Horse's current animation
        private int Speed; //Horse's speed
        private Direction Direction; //Horse's direction
        private Player Owner; //Horse's player owner (riding player)
        private Text Tooltip; //Horse's tooltip when in touch with player
        private Location CurrentLocation; //Horse's current location on map

        /// <summary>
        /// Receives team, position coordinates and size
        /// and creates a horse.
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Horse(TeamName teamName, int x, int y, int width, int height) {
            TeamName = teamName;
            Rectangle = new Rectangle(x, y, width, height);
            AfkAnimation = new Animation("horse/teams/" + teamName + "/afk/horse_afk_", new Rectangle(x, y, width, height), 0, 7, 7, 6, true, true);
            WalkAnimation = new Animation("horse/teams/" + teamName + "/walk/horse_walk_", new Rectangle(x, y, width, height), 0, 4, 5, 2, true, true);
            Speed = 7;

            //Set horse's state:
            SetState(HorseState.Afk);

            Direction = Direction.Left;
            Owner = null;
            Tooltip = new Text(FontType.Default, "Press 'E' to mount horse.", new Vector2(x + 50, y - 65), Color.Gold, true, Color.Black);
            
            //Set tooltip visible:
            Tooltip.SetVisible(false);
            CurrentLocation = Location.Outside;
        }

        /// <summary>
        /// Receives a new horse state
        /// and applies it.
        /// </summary>
        /// <param name="state"></param>
        public void SetState(HorseState state) {

            State = state;
            switch (State) {
                case HorseState.Afk:
                    CurrentAnimation = AfkAnimation;
                    break;
                case HorseState.Walk:
                    CurrentAnimation = WalkAnimation;
                    break;
            }
            CurrentAnimation.Start();
        }

        /// <summary>
        /// Update horse's stuff like animations and checks for player's owner riding on him.
        /// </summary>
        public void Update() {
            CurrentAnimation.Play();
            CurrentAnimation.Start();

            if (Owner != null)
                CheckOwnerRiding();
        }

        /// <summary>
        /// Do what owner does when he rids on horse (move/afk).
        /// </summary>
        private void CheckOwnerRiding() {

            if (Owner.GetState() == PlayerState.Afk)
                SetState(HorseState.Afk);
            else if (Owner.GetState() == PlayerState.Walk)
                SetState(HorseState.Walk);

            SetDirection(Owner.GetDirection());

            SetRectangle(new Rectangle(Owner.GetRectangle().Left + Owner.GetRectangle().Width / 2 - Rectangle.Width / 2,
                                       Owner.GetRectangle().Top + Owner.GetRectangle().Height / 3 - 10,
                                       Rectangle.Width,
                                       Rectangle.Height));

        }

        /// <summary>
        /// Get team name
        /// </summary>
        /// <returns></returns>
        public TeamName GetTeamName() {
            return TeamName;
        }

        /// <summary>
        /// Get speed
        /// </summary>
        /// <returns></returns>
        public int GetSpeed() {
            return Speed;
        }

        /// <summary>
        /// Set new direction
        /// </summary>
        /// <param name="newDirection"></param>
        public void SetDirection(Direction newDirection) {

            Direction = newDirection;
            WalkAnimation.SetDirection(newDirection);
            AfkAnimation.SetDirection(newDirection);
            CurrentAnimation.SetDirection(newDirection);
        }

        /// <summary>
        /// Set new rectangle
        /// </summary>
        /// <param name="newRectangle"></param>
        public void SetRectangle(Rectangle newRectangle) {

            Rectangle.X = newRectangle.X;
            Rectangle.Y = newRectangle.Y;
            Rectangle.Width = newRectangle.Width;
            Rectangle.Height = newRectangle.Height;

            WalkAnimation.SetRectangle(newRectangle.X, newRectangle.Y, newRectangle.Width, newRectangle.Height);
            AfkAnimation.SetRectangle(newRectangle.X, newRectangle.Y, newRectangle.Width, newRectangle.Height);
            CurrentAnimation.SetRectangle(newRectangle.X, newRectangle.Y, newRectangle.Width, newRectangle.Height);
        }

        /// <summary>
        /// Get rectangle
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle() {
            return Rectangle;
        }

        /// <summary>
        /// Get horse's state
        /// </summary>
        /// <returns></returns>
        public HorseState GetState() {
            return State;
        }

        /// <summary>
        /// Sets a new horse owner
        /// </summary>
        /// <param name="player"></param>
        public void SetOwner(Player player) {
            Owner = player;
        }

        /// <summary>
        /// Removes owner from horse
        /// </summary>
        public void RemoveOwner() {
            Owner = null;
            SetState(HorseState.Afk);
        }

        /// <summary>
        /// Returns true if has an owner else returns false
        /// </summary>
        /// <returns></returns>
        public bool IsHasOwner() {
            return Owner != null;
        }

        /// <summary>
        /// Checks if horse is on top of map.
        /// Returns true if yes, else returns false.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public bool IsOnTopMap(Map map) {
            if (Owner != null)
                if (Rectangle.Bottom - Rectangle.Height / 2 < map.GetGrass().GetRectangle().Top + Owner.GetRectangle().Height / 3)
                    return true;

            return false;
        }

        /// <summary>
        /// Checks if horse is on left of map.
        /// Returns true if yes, else returns false.
        /// </summary>
        /// <returns></returns>
        public bool IsOnLeftMap() {
            if (Rectangle.Left <= 0)
                return true;

            return false;
        }

        /// <summary>
        /// Checks if horse is on right of map.
        /// Returns true if yes, else returns false.
        /// </summary>
        /// <returns></returns>
        public bool IsOnRightMap() {
            if (Rectangle.Right >= Map.WIDTH)
                return true;

            return false;
        }

        /// <summary>
        /// Checks if horse is on bottom of map.
        /// Returns true if yes, else returns false.
        /// </summary>
        /// <returns></returns>
        public bool IsOnBottomMap() {
            if (Rectangle.Bottom > Map.HEIGHT)
                return true;

            return false;
        }

        /// <summary>
        /// Get current location
        /// </summary>
        /// <returns></returns>
        public Location GetCurrentLocation() {
            return CurrentLocation;
        }


        /// <summary>
        /// Receives a new location and applies
        /// </summary>
        /// <param name="newLocation"></param>
        public void ChangeLocationTo(Location newLocation) {
            CurrentLocation = newLocation;
        }

        /// <summary>
        /// Get current horse's location
        /// </summary>
        /// <returns></returns>
        public Animation GetCurrentAnimation() {
            return CurrentAnimation;
        }

        /// <summary>
        /// Get tooltip
        /// </summary>
        /// <returns></returns>
        public Text GetTooltip() {
            return Tooltip;
        }

        /// <summary>
        /// Draw horse.
        /// </summary>
        public void Draw() {

            //Draw tooltip:
            Tooltip.Draw();

            //Draw horse's animation:
            CurrentAnimation.Draw();
        }
    }
}
