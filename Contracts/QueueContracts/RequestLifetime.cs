﻿namespace Contracts.QueueContracts;

public static class RequestLifetime
{
    private const int numberOfMinutes = 5;
    public static int LifetimeMinutes => numberOfMinutes;
    public static int LifetimeSeconds => numberOfMinutes * 60;
}
