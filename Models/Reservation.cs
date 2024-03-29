﻿using System.ComponentModel.DataAnnotations.Schema;
using QFX.Entity;
using QFX.Manager.Interface;

namespace QFX.Models;

public class Reservation : ISoftDelete
{
    public long ID { get; set; }
    public string? SessionID { get; set; }
    public long? ShowTimeID { get; set; }
    [ForeignKey("ShowTimeID")]
    public virtual ShowTime? ShowTime { get; set; }
    public DateTime ReservationDate { get; set; } = DateTime.Now;
    public long ShowID { get; set; }
    [ForeignKey("ShowID")]
    public virtual Show? Show { get; set; }
    public long UserID { get; set; }
    [ForeignKey("UserID")]
    public virtual User? User { get; set; }
    public bool IsDeleted  { get; set; } = false;
}
