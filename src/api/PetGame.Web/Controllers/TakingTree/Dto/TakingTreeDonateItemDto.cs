using System;
using System.ComponentModel.DataAnnotations;

public class TakingTreeDonateItemDto
{
    // Required
    [Required]
    public Guid? playerInventoryItemId { get; set; }

    // Optional
    // - None at present
}