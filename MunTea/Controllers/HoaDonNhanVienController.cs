using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MunTea.Models;

namespace MunTea.Controllers
{
    public class HoaDonNhanVienController : Controller
    {
        //
        // GET: /HoaDonNhanVien/

        dbMunTeaLinQDataContext data = new dbMunTeaLinQDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HoaDon()
        {
            List<HoaDon> ds = data.HoaDons.ToList();
            return View(ds);
        }

    }
}
