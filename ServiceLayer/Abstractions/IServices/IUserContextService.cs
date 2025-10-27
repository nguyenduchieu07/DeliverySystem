using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IUserContextService
    {
        string? GetUserId();
        string? GetUserName();
    }
}
