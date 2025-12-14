using UnityEngine;
using System;

public static class GlobalTime
{
    public static long UnixTime => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
}