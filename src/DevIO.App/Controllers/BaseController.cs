using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificator _notificator;

        protected BaseController(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected bool isValid()
        {
            return !_notificator.HasNotification();
        }
    }
}
