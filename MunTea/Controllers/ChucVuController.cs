using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MunTea.Models;

namespace MunTea.Controllers
{
    public class ChucVuController : Controller
    {
        //
        // GET: /ChucVu/

        dbMunTeaLinQDataContext data = new dbMunTeaLinQDataContext();

        public ActionResult HienDanhSach()
        {
            List<ChucVu> ds = data.ChucVus.ToList();
            return View(ds);
        }

        public int KiemTra(string tenchucvu)
        {
            ChucVu tim = data.ChucVus.FirstOrDefault(t => t.TenChucVu == tenchucvu);
            if(tim == null)
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
        public ActionResult Them(ChucVu chucvu)
        {
            int kiemtra = KiemTra(chucvu.TenChucVu);
            if(kiemtra == 1)
            {
                data.ChucVus.InsertOnSubmit(chucvu);
                data.SubmitChanges();
                return RedirectToAction("HienDanhSach", "ChucVu");
            }
            else
            {
                ViewBag.thongbao = "Trùng tên chức vụ!";
                return View();
            }
        }

        public ActionResult Xoa(int machucvu)
        {
            ChucVu chucvu = data.ChucVus.FirstOrDefault(t => t.MaChucVu == machucvu);
            
            if(chucvu.NhanViens.Count == 0)
            {
                data.ChucVus.DeleteOnSubmit(chucvu);
                data.SubmitChanges();
                return RedirectToAction("HienDanhSach", "ChucVu");
            }
            ViewBag.thongbao = "Có nhân viên trong chức vụ!";
            return View();
        }

        public ActionResult Sua(int machucvu)
        {
            ChucVu chucvu = data.ChucVus.FirstOrDefault(t => t.MaChucVu == machucvu);

            return View(chucvu);
        }

        [HttpPost]
        public ActionResult Sua(ChucVu chucvu)
        {
            ChucVu tim = data.ChucVus.FirstOrDefault(t => t.MaChucVu == chucvu.MaChucVu);
            if (tim == null)
            {
                ViewBag.thongbao = "Không tìm thấy mã chức vụ!";
                return View(chucvu);
            }
            tim.TenChucVu = chucvu.TenChucVu;
            data.SubmitChanges();
            return RedirectToAction("HienDanhSach", "ChucVu");
        }

        public ActionResult ChiTiet(int machucvu)
        {
            ChucVu tim = data.ChucVus.FirstOrDefault(t => t.MaChucVu == machucvu);

            return View(tim);
        }
    }
}
