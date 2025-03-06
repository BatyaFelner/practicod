using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class User
{
    public int? Id { get; set; }

    public string? Email { get; set; } 

    public string? PasswordHash { get; set; }
}

