using System;
using System.ComponentModel.DataAnnotations;

public class TakingTreeClaimItemDto
{
    // Required
    [Required]
    public Guid? takingTreeInventoryItemId { get; set; }

    // Optional
    // - None at present
}