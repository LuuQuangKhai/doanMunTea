using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MunTea.Models;

namespace MunTea.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        dbMunTeaLinQDataContext data = new dbMunTeaLinQDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HienThiSanPham()
        {
            List<ThucDon> ds = data.ThucDons.ToList();
            return View(ds);
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(string tentaikhoan, string matkhau)
        {
            try
            {
                int tknhanvien = int.Parse(tentaikhoan);
                NhanVien tim = data.NhanViens.FirstOrDefault(t => t.MaNhanVien == tknhanvien);
                if (tim == null)
                {
                    ViewBag.thongbao = "Tên tài khoản không tồn tại!";
                    return View();
                }
                else
                {
                    if(tim.MatKhau.Equals(matkhau) == true)
                    {
                        Session["MyTenTaiKhoan"] = tentaikhoan;
                        if (tim.ChucVu.TenChucVu.Equals("Nhân viên") == true)
                        {
                            return RedirectToAction("Index", "HoaDonNhanVien");
                        }
                        if (tim.ChucVu.TenChucVu.Equals("Giao hàng") == true)
                        {
                            return RedirectToAction("Index", "HoaDonGiaoHang");
                        } 
                        if (tim.ChucVu.TenChucVu.Equals("Admin") == true)
                        {
                            return RedirectToAction("HienDanhSach", "NhanVien");
                        }
                    }
                    ViewBag.ttk = tentaikhoan;
                    ViewBag.thongbao = "Sai mật khẩu!";
                    return View();
                }

            }
            catch(Exception ex)
            {
                KhachHang tim = data.KhachHangs.FirstOrDefault(t => t.TenTaiKhoan == tentaikhoan);
                if (tim == null)
                {
                    ViewBag.thongbao = "Tên tài khoản không tồn tại!";
                    return View();
                }
                else
                {
                    if (tim.MatKhau.Equals(matkhau) == true)
                    {
                        Session["MyTenTaiKhoan"] = tentaikhoan;
                        return RedirectToAction("HienThiDanhSach", "Home");
                    }
                    ViewBag.ttk = tentaikhoan;
                    ViewBag.thongbao = "Sai mật khẩu!";
                    return View();
                }
            }
        }

        public List<GioHang> LayGioHang()
        {
            List<GioHang> gio = Session["gh"] as List<GioHang>;
            if (gio == null)
            {
                gio = new List<GioHang>();
                Session["gh"] = gio;
            }

            return gio;
        }

        public ActionResult ThemGioHang(int id)
        {
            List<GioHang> gio = LayGioHang();
            if (gio == null || gio.Count == 0)
            {
                GioHang sp = new GioHang(id);
                gio = new List<GioHang>();
                gio.Add(sp);
            }
            else
            {
                GioHang sach = gio.FirstOrDefault(t => t.mathucdon == id);
                if (sach == null)
                {
                    GioHang sp = new GioHang(id);
                    gio.Add(sp);
                }
                else
                {
                    sach.soluong++;
                }
            }
            Session["gh"] = gio;
            return RedirectToAction("HienThiSanPham");
        }
        private int TongSl()
        {
            int tsl = 0;
            List<GioHang> gio = Session["gh"] as List<GioHang>;
            if (gio != null)
            {
                tsl = gio.Sum(sp => sp.soluong);
            }
            return tsl;
        }

        private double ThanhTien()
        {
            double tt = 0;
            List<GioHang> gio = Session["gh"] as List<GioHang>;
            if (gio != null)
            {
                tt = gio.Sum(sp => sp.thanhtien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            if (Session["gh"] == null)
            {
                return RedirectToAction("Index");
            }
            List<GioHang> gio = LayGioHang();

            ViewBag.TongSoLuong = TongSl();
            ViewBag.ThanhTien = ThanhTien();
            return View(gio);
        }
        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> gio = LayGioHang();
            GioHang sp = gio.FirstOrDefault(s => s.mathucdon == id);

            if (sp != null)
            {
                gio.RemoveAll(s => s.mathucdon == id);
                return RedirectToAction("GioHang", "Home");
            }
            if (gio.Count == 0)
            {
                return RedirectToAction("HienThiSanPham", "Home");
            }
            return RedirectToAction("GioHang", "Home");
        }

        public ActionResult CapNhatGio(int mathucdon, FormCollection c)
        {
            List<GioHang> gio = LayGioHang();
            GioHang sp = gio.FirstOrDefault(s => s.mathucdon == mathucdon);

            if (sp != null)
            {
                sp.soluong = int.Parse(c["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult DatHang(List<GioHang> gh)
        {
            

            return View();
        }

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(KhachHang s)
        {
            data.KhachHangs.InsertOnSubmit(s);
            data.SubmitChanges();
            return RedirectToAction("DangNhap","Home");
        }
    }
}
