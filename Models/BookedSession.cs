using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StretchingStudioAPI.Models;

public class BookedSession
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid SessionId { get; set; }

    [ForeignKey(nameof(SessionId))]
    public UpcomingSession Session { get; set; } = null!;
}