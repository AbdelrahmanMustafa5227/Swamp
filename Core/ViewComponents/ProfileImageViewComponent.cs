using _DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Core.ViewComponents
{
    public class ProfileImageViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public ProfileImageViewComponent(IUnitOfWork unitOfWork , IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string loggedUserId = _userService.GetCurrentUserId();
            var user = _unitOfWork.ApplicationRepo.Get(u => u.Id == loggedUserId);
            return View(user);

        }

    }
}
