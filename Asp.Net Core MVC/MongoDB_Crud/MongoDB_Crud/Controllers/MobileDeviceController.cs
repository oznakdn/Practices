using Microsoft.AspNetCore.Mvc;
using MongoDB_Crud.Abstracts;
using MongoDB_Crud.Entities;

namespace MongoDB_Crud.Controllers
{
    public class MobileDeviceController : Controller
    {
        private readonly IMobileStoreService mobileStoreService;

        public MobileDeviceController(IMobileStoreService mobileStoreService)
        {
            this.mobileStoreService = mobileStoreService;
        }

        public IActionResult Index()
        {
            var result = mobileStoreService.GetAllMobileDevices();
            return View(result);
        }

        public IActionResult Details(string Name)
        {
            var result = mobileStoreService.GetMobileDeviceDetails(Name);
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MobileDevice mobileDevice)
        {
                mobileStoreService.Create(mobileDevice);
                return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(string Name)
        {
            var mobile = mobileStoreService.GetMobileDeviceDetails(Name);
            return View(mobile);
        }

        [HttpPost]
        public IActionResult Edit(string _id,MobileDevice mobileDevice)
        {
            mobileStoreService.Update(_id,mobileDevice);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string Name)
        {
            var mobile = mobileStoreService.GetMobileDeviceDetails(Name);
            return View(mobile);
        }

        [HttpPost]
        public IActionResult DeletePost(string Name)
        {
            mobileStoreService.Delete(Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
