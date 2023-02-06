using androkat.application.Interfaces;
using System;

namespace androkat.application.Service;

public class Clock : IClock
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow.AddHours(1);
}