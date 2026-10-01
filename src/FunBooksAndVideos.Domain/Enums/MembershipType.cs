namespace FunBooksAndVideos.Domain.Enums;

/// <summary>
/// Club memberships a customer can hold. Modelled as flags so that
/// holding both the Book Club and the Video Club is, by definition, Premium.
/// </summary>
[Flags]
public enum MembershipType
{
    None = 0,
    BookClub = 1,
    VideoClub = 2,
    Premium = BookClub | VideoClub
}
