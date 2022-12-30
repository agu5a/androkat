using System;

namespace androkat.application.Interfaces;

public interface IClock
{
    DateTimeOffset Now { get; }
}