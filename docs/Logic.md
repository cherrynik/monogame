# Logic

## Movement Math

Vectors should be normalized for the movement in diagonal directions.

More here - [Link](https://www.youtube.com/watch?v=u70ZpQH1muc)

> MonoGame coordinate system is Y-flipped.
> 
> Therefore, you might see something like `-YFlipped` method names in the project.

## Movement Direction

1. Get axis directions as a vector when you press keys
2. Get direction in radians
3. Associate it with the enum named in 8 directions
4. Use the enum for different purposes: SpriteSheets, etc.

> Remember about this - [Link](#movement-math)

<img src="https://wumbo.net/concepts/unit-circle-chart-degrees/unit-circle-chart-degrees-12-650-650.svg" width="400px" alt="Radian Circle"/>

