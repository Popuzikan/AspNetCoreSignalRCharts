using System.Globalization;
using System.Security.Cryptography;

namespace Charts.Services;

/// <summary>
/// https://stackoverflow.com/questions/12294296/list-with-limited-item
/// </summary>
/// <typeparam name="T"></typeparam>
public class Buffer<T> : Queue<T>
{
    public int? MaxCapacity { get; }
    public Buffer(int capacity) { MaxCapacity = capacity; }
    public int TotalItemsAddedCount { get; private set; }

    public void Add(T newElement)
    {
        // not thread safe 🤷‍
        if (Count == (MaxCapacity ?? -1)) Dequeue();
        Enqueue(newElement);
        TotalItemsAddedCount++;
    }
}

public static class BufferExtensions
{

    static float [] x = new float[350];
    static float [] y = new float[350];



    public static Point AddNewRandomPoint(this Buffer<Point> buffer)
    {
        var now = DateTime.Now.AddMonths(buffer.TotalItemsAddedCount);
        var year = now.Year;
        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(now.Month);
        

        for (int i = 0; i < 350; i++)
        {
            x[i] = RandomNumberGenerator.GetInt32(1, 50);
            y[i] = i;

        }

  
        var point = new Point(x,y);
        buffer.Add(point);
        return point;
    }
}