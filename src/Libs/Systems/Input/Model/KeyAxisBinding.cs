using Microsoft.Xna.Framework.Input;

namespace Systems.Input.Model;

public readonly record struct KeyAxisBinding(Keys[] Negative, Keys[] Positive);
