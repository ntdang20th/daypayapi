﻿using System;

namespace DayPay.Dtos.RedisDto;

public class DayPayRedisDto
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }
}
