using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Allegiance
{
    public class Player
    {
        List<Rectangle> rectangles = new List<Rectangle>();

        private Texture2D texture;
        private Rectangle playerRect = new Rectangle(300, 200, 100, 100);
        private Rectangle interRect = new Rectangle(0, 0, 0, 0);
        private KeyboardState keyState = new KeyboardState();

        private int health;
        private int moveSpeed;
        private int attackSpeed;
        private int reloadSpeed;
        private int damage;
        private int range;

        bool jumping = true;
        float jumpSpeed = 0;

        public Player(Texture2D _texture, int _health, int _moveSpeed, int _attackSpeed, int _reloadSpeed, int _damage, int _range)
        {
            texture = _texture;
            health = _health;
            moveSpeed = _moveSpeed;
            attackSpeed = _attackSpeed;
            reloadSpeed = _reloadSpeed;
            damage = _damage;
            range = _range;
        }

        public void SetTexture(Texture2D _texture) { texture = _texture; }

        public void SetHealth(int _health) { health = _health; }
        public void IncreaseHealth(int _health) { health += _health; }
        public void DecreaseHealth(int _health) { health -= _health; }

        public void SetMoveSpeed(int _moveSpeed) { moveSpeed = _moveSpeed; }
        public void IncreaseMoveSpeed(int _moveSpeed) { moveSpeed += _moveSpeed; }
        public void DecreaseMoveSpeed(int _moveSpeed) { moveSpeed -= _moveSpeed; }

        public void SetAttackSpeed(int _attackSpeed) { attackSpeed = _attackSpeed; }
        public void IncreaseAttackSpeed(int _attackSpeed) { attackSpeed += _attackSpeed; }
        public void DecreaseAttackSpeed(int _attackSpeed) { attackSpeed -= _attackSpeed; }

        public void SetReloadSpeed(int _reloadSpeed) { reloadSpeed = _reloadSpeed; }
        public void IncreaseReloadSpeed(int _reloadSpeed) { reloadSpeed += _reloadSpeed; }
        public void DecreaseReloadSpeed(int _reloadSpeed) { reloadSpeed -= _reloadSpeed; }

        public void SetDamage(int _damage) { damage = _damage; }
        public void IncreaseDamage(int _damage) { damage += _damage; }
        public void DecreaseDamage(int _damage) { damage -= _damage; }

        public void SetRange(int _range) { range = _range; }
        public void IncreaseRange(int _range) { range += _range; }
        public void DecreaseRange(int _range) { range -= _range; }

        public bool isInBounds(Rectangle rect, int x, int y)
        {
            if (x >= rect.X && x <= rect.X + rect.Width && y >= rect.Y && y <= rect.Y + rect.Height)
                return true;

            return false;
        }

        public void DetectCollision()
        {
            foreach (Rectangle rect in rectangles)
            {
                if (playerRect.Intersects(rect))
                {
                    // Check if the top of the player is intersecting the platform
                    if (playerRect.Y > rect.Y && playerRect.Y < rect.Y + rect.Height)
                    {
                        // Check if both the top and bottom are intersecting
                        if (playerRect.Y + playerRect.Height > rect.Y && playerRect.Y + playerRect.Height < rect.Y + rect.Height)
                            interRect.Height = playerRect.Height;
                        else
                            interRect.Height = (rect.Y + rect.Height) - playerRect.Y;

                        // Check if the player's top-left corner is in the platform
                        if (playerRect.X >= rect.X && playerRect.X < rect.X + rect.Width)
                        {
                            interRect.X = playerRect.X;
                            interRect.Y = playerRect.Y;

                            // Check if the player's top-right corner is in the platform
                            if (playerRect.X + playerRect.Width <= rect.X + rect.Width)
                                interRect.Width = playerRect.Width;
                            else
                                interRect.Width = (rect.X + rect.Width) - playerRect.X;
                        }
                        // Check if the player's top-left corner is to the left of the platform AND the top-right corner is in the platform
                        else if (playerRect.X < rect.X && playerRect.X + playerRect.Width > rect.X && playerRect.X + playerRect.Width < rect.X + rect.Width)
                        {
                            interRect.X = rect.X;
                            interRect.Y = playerRect.Y;
                            interRect.Width = (playerRect.X + playerRect.Width) - rect.X;
                        }
                    }
                    // Check if the bottom of the platform is intersecting the platform
                    else if (playerRect.Y + playerRect.Height > rect.Y && playerRect.Y + playerRect.Height < rect.Y + rect.Height)
                    {
                        // Check if the player's bottom-left corner is in the platform
                        if (playerRect.X >= rect.X && playerRect.X < rect.X + rect.Width)
                        {
                            interRect.X = playerRect.X;
                            interRect.Y = rect.Y;

                            // Check if the player's bottom-right corner is in the platform
                            if (playerRect.X + playerRect.Width <= rect.X + rect.Width)
                                interRect.Width = playerRect.Width;
                            else
                                interRect.Width = (rect.X + rect.Width) - playerRect.X;
                        }
                        else if (playerRect.X < rect.X && playerRect.X + playerRect.Width > rect.X && playerRect.X + playerRect.Width < rect.X + rect.Width)
                        {
                            interRect.X = rect.X;
                            interRect.Y = rect.Y;
                            interRect.Width = (playerRect.X + playerRect.Width) - rect.X;
                        }
                    }

                    if (interRect.Height < interRect.Width)
                    {
                        if (jumpSpeed >= 0)
                        {
                            playerRect.Y -= interRect.Height;
                            jumping = false;
                        }
                        else if (jumpSpeed < 0)
                        {
                            playerRect.Y += interRect.Height;
                            jumpSpeed = 0;
                        }
                    }
                    if (interRect.Width < interRect.Height)
                    {
                        if (playerRect.X < rect.X)
                            playerRect.X -= interRect.Width;
                        else if (playerRect.X < rect.X)
                            playerRect.X += interRect.Width;
                    }
                }
            }
        }

        public void AddRectToColl(Rectangle rect)
        {
            rectangles.Add(rect);
        }

        public void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.Left))
                playerRect.X -= moveSpeed;

            if (keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.Right))
                playerRect.X += moveSpeed;

            if (jumping)
            {
                playerRect.Y += (int)jumpSpeed;
                jumpSpeed += 1;
                DetectCollision();
            }
            else
            {
                if (keyState.IsKeyDown(Keys.Space))
                {
                    jumping = true;
                    jumpSpeed = -14;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, playerRect, Color.White);
        }
    }
}
