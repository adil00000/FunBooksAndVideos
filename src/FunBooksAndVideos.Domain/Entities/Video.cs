using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Domain.Entities;

/// <summary>An online video. It is streamed, so nothing is shipped.</summary>
public sealed class Video : Product
{
    public Video(int id, string name, decimal price, TimeSpan duration)
        : base(id, name, price)
    {
        Duration = duration;
    }

    public TimeSpan Duration { get; }

    public override ProductType Type => ProductType.Video;

    public override bool IsPhysical => false;
}
