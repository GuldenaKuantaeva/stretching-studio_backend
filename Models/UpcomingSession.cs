﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StretchingStudioAPI.Models;

public class UpcomingSession
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid SessionTypeId { get; set; }

    [ForeignKey(nameof(SessionTypeId))]
    public SessionType SessionType { get; set; } = null!;

    public DateTime StartingDate { get; set; }

    public int FreeSlotsNum { get; set; }
    
}