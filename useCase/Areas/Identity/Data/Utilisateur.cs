using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace useCase.Areas.Identity.Data;

public class Utilisateur : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string MobilePhone { get; set; }
    public string Role { get; set; }

}

