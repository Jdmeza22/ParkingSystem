using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingSystem.Domain.Common;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; } 
}
