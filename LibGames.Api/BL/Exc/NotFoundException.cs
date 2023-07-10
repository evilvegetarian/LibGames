﻿
namespace LibGames.Api.BL.Exc;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key) :
        base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}
