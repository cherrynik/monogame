namespace Services.Factories;

public class AbstractFactory<T>(Func<T> factory)
{
    public T Create() => factory();
}
