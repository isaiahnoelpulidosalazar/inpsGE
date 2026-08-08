using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace inpsGE
{
    public class Input
    {
        public static bool Up, Down, Left, Right, MouseDown, MousePress, MouseUp, Interact, InteractReset;
        public static int MouseX, MouseY;
        public static float Theta, AngleInDegrees;

        public static void Update()
        {
            var EngineCheck = new StackFrame(1).GetMethod()?.DeclaringType;

            if (EngineCheck != typeof(Engine))
            {
                throw new InvalidOperationException("The Input.Update() method can only be called from the Engine class.");
            }

            MouseX = Mouse.GetState().X;
            MouseY = Mouse.GetState().Y;

            Theta = (float)Math.Atan2(MouseY - (Core.GetScreenHeight() / 2), MouseX - (Core.GetScreenWidth() / 2)) + MathHelper.PiOver2;
            AngleInDegrees = MathHelper.ToDegrees(Theta);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                MouseDown = true;
                MousePress = true;
            }
            else
            {
                MouseDown = false;
            }
            if (MousePress && !MouseDown)
            {
                MouseUp = true;
                MousePress = false;
            }
            else
            {
                MouseUp = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Up = true;
            }
            else
            {
                Up = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Down = true;
            }
            else
            {
                Down = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Left = true;
            }
            else
            {
                Left = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Right = true;
            }
            else
            {
                Right = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                InteractReset = true;
            }
            if (InteractReset && !Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Interact = true;
                InteractReset = false;
            }
            else
            {
                Interact = false;
            }
        }
    }
}
