﻿namespace Entities;

public class Subforum
{
    public int SubforumId { get; set; }
    public string Name { get; set; }
    public int ModeratedBy { get; set; }
}