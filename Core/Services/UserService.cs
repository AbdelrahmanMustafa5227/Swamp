using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using DataAccess.Data;
using _DataAccess.Repository.IRepository;
namespace Core
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IHttpContextAccessor httpContext, IUnitOfWork unitOfWork)
        {
            _contextAccessor = httpContext;
            _unitOfWork = unitOfWork;
        }
        public string GetCurrentUserId()
        {
            return _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public string GetCurrentUserName()
        {
            if (_unitOfWork.ApplicationRepo.Get(x => x.Id == GetCurrentUserId()) != null)
            {
                return _unitOfWork.ApplicationRepo.Get(x => x.Id == GetCurrentUserId()).Fullname;
            }
            else
            {
                return string.Empty;
            } 
        }

        public bool IsLoggedIn()
        {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

    }
}
