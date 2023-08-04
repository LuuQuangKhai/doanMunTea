using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MunTea.Models;

namespace MunTea.Controllers
{
    public class LoaiThucDonController : Controller
    {
        //
        // GET: /LoaiThucDon/
        dbMunTeaLinQDataContext data = new dbMunTeaLinQDataContext();

        public ActionResult HienDanhSach()
        {
            List<LoaiThucDon> ds = data.LoaiThucDons.ToList();
            return View(ds);
        }

        public int KiemTra(string tenloaithucdon)
        {
            LoaiThucDon tim = data.LoaiThucDons.FirstOrDefault(t => t.TenLoaiThucDon == tenloaithucdon);
            if (tim == null)
            {
                return 1;
            }
            return 0;
        }

        public ActionResult Them()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Them(LoaiThucDon loaithucdon)
        {
            int kiemtra = KiemTra(loaithucdon.TenLoaiThucDon);
            if (kiemtra == 1)
            {
                data.LoaiThucDons.InsertOnSubmit(loaithucdon);
                data.SubmitChanges();
                return RedirectToAction("HienDanhSach", "LoaiThucDon");
            }
            else
            {
                ViewBag.thongbao = "Trùng tên loại thực đơn!";
                return View();
            }
        }

        public ActionResult Xoa(int maloaithucdon)
        {
            LoaiThucDon loaithucdon = data.LoaiThucDons.FirstOrDefault(t => t.MaLoaiThucDon == maloaithucdon);

            if (loaithucdon.ThucDons.Count == 0)
            {
                data.LoaiThucDons.DeleteOnSubmit(loaithucdon);
                data.SubmitChanges();
                return RedirectToAction("HienDanhSach", "LoaiThucDon");
            }
            ViewBag.thongbao = "Có thực đơn trong loại thực đơn!";
            return View();
        }

        public ActionResult Sua(int maloaithucdon)
        {
            LoaiThucDon loaithucdon = data.LoaiThucDons.FirstOrDefault(t => t.MaLoaiThucDon == maloaithucdon);

            return View(loaithucdon);
        }

        [HttpPost]
        public ActionResult Sua(LoaiThucDon loaithucdon)
        {
            LoaiThucDon tim = data.LoaiThucDons.FirstOrDefault(t => t.MaLoaiThucDon == loaithucdon.MaLoaiThucDon);
            if (tim == null)
            {
                ViewBag.thongbao = "Không tìm thấy mã loại thực đơn!";
                return View(loaithucdon);
            }
            tim.TenLoaiThucDon = loaithucdon.TenLoaiThucDon;
            data.SubmitChanges();
            return RedirectToAction("HienDanhSach", "LoaiThucDon");
        }

        public ActionResult ChiTiet(int maloaithucdon)
        {
            LoaiThucDon tim = data.LoaiThucDons.FirstOrDefault(t => t.MaLoaiThucDon == maloaithucdon);

            return View(tim);
        }

    }
}
