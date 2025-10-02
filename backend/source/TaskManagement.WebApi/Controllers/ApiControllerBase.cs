using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskManagementWebApi.Controllers
{
    /// <summary>
    /// ApiControllerBase
    /// </summary>
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private Guid? _userId;

        /// <summary>
        /// User Identifire.
        /// </summary>
        protected Guid UserId
        {
            get
            {
                if (!_userId.HasValue)
                {
                    var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdStr))
                    {
                        throw new Exception("User Id cookie is empty");
                    }
                    _userId = Guid.Parse(userIdStr);
                }
                return _userId.Value;
            }
        }
    }
}







