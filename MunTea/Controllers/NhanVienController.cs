using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MunTea.Models;
using System.IO;

namespace MunTea.Controllers
{
    public class NhanVienController : Controller
    {
        //
        // GET: /NhanVien/
        dbMunTeaLinQDataContext data = new dbMunTeaLinQDataContext();

        public ActionResult HienDanhSach()
        {
            List<NhanVien> ds = data.NhanViens.ToList();
            return View(ds);
        }

        [HttpGet]
        public ActionResult Them()
        {
            ViewBag.machucvu = new SelectList(data.ChucVus.ToList().OrderBy(n => n.TenChucVu), "MaChucVu", "TenChucVu");
            return View();
        }

        [HttpPost]
        public ActionResult Them(NhanVien nhanvien, HttpPostedFileBase fileupload)
        {
            // trường hợp nhập lại
            ViewBag.machucvu = new SelectList(data.ChucVus.ToList().OrderBy(n => n.TenChucVu), "MaChucVu", "TenChucVu");

            if (fileupload == null)
            {
                ViewBag.thongbao = "Vui lòng chọn lại hình ảnh";
                return View();
            }
            //thêm vào cơ sở dữ liệu
            else
            {
                if (ModelState.IsValid)
                {
                    // lưu tên file * bổ sung thư viện System.IO
                    var tenFile = Path.GetFileName(fileupload.FileName);
                    // lưu đường dẫn của file
                    var duongdan = Path.Combine(Server.MapPath("~/HinhAnh/NhanVien"), tenFile);

                    if (System.IO.File.Exists(duongdan))
                        ViewBag.thongbao = "Hình ảnh đã tồn tại!";
                    else
                    {
                        // Lưu hình ảnh vào đường dẫn
                        fileupload.SaveAs(duongdan);
                    }
                    nhanvien.HinhAnh = tenFile;

                    // lưu vào cơ sở dữ liệu
                    data.NhanViens.InsertOnSubmit(nhanvien);
                    data.SubmitChanges();
                }
                return RedirectToAction("HienDanhSach", "NhanVien");
            }
        }

        public ActionResult Xoa(int manhanvien)
        {
            NhanVien nhanvien = data.NhanViens.FirstOrDefault(t => t.MaNhanVien == manhanvien);

            if (nhanvien.TinhTrangHoaDons.Count == 0 && nhanvien.PhieuNhapHangs.Count == 0)
            {
                data.NhanViens.DeleteOnSubmit(nhanvien);
                data.SubmitChanges();
                return RedirectToAction("HienDanhSach", "NhanVien");
            }
            ViewBag.thongbao = "Còn dữ liệu của nhân viên trong phiếu nhập hàng và tình trạng hóa đơn!";
            return View();
        }

        public ActionResult ChiTiet(int manhanvien)
        {
            NhanVien nhanvien = data.NhanViens.FirstOrDefault(t => t.MaNhanVien == manhanvien);
            return View(nhanvien);
        }

        [HttpGet]
        public ActionResult Sua(int manhanvien)
        {
            NhanVien nhanvien = data.NhanViens.FirstOrDefault(t => t.MaNhanVien == manhanvien);
            ViewBag.machucvu = new SelectList(data.ChucVus.ToList().OrderBy(n => n.TenChucVu), "MaChucVu", "TenChucVu");
            return View(nhanvien);
        }

        [HttpPost]
        public ActionResult Sua(NhanVien nhanvien, HttpPostedFileBase fileupload)
        {
            ViewBag.machucvu = new SelectList(data.ChucVus.ToList().OrderBy(n => n.TenChucVu), "MaChucVu", "TenChucVu");
            NhanVien tim = data.NhanViens.FirstOrDefault(t => t.MaNhanVien == nhanvien.MaNhanVien);
            if (tim == null)
            {
                ViewBag.thongbao = "Mã nhân viên không tồn tại!";
                return View(nhanvien);
            }
            if (fileupload == null)
            {
                tim.MatKhau = nhanvien.MatKhau;
                tim.TenNhanVien = nhanvien.TenNhanVien;
                tim.SDT = nhanvien.SDT;
                tim.GioiTinh = nhanvien.GioiTinh;
                tim.NoiSinh = nhanvien.NoiSinh;
                tim.MaChucVu = nhanvien.MaChucVu;
                // lưu vào cơ sở dữ liệu
                data.SubmitChanges();
                return RedirectToAction("HienDanhSach", "NhanVien");
            }
            //thêm vào cơ sở dữ liệu
            else
            {
                if (ModelState.IsValid)
                {
                    // lưu tên file * bổ sung thư viện System.IO
                    var tenFile = Path.GetFileName(fileupload.FileName);
                    // lưu đường dẫn của file
                    var duongdan = Path.Combine(Server.MapPath("~/HinhAnh/NhanVien"), tenFile);

                    if (System.IO.File.Exists(duongdan))
                        ViewBag.thongbao = "Hình ảnh đã tồn tại!";
                    else
                    {
                        // Lưu hình ảnh vào đường dẫn
                        fileupload.SaveAs(duongdan);
                    }
                    tim.MatKhau = nhanvien.MatKhau;
                    tim.TenNhanVien = nhanvien.TenNhanVien;
                    tim.SDT = nhanvien.SDT;
                    tim.GioiTinh = nhanvien.GioiTinh;
                    tim.NoiSinh = nhanvien.NoiSinh;
                    tim.HinhAnh = tenFile;
                    tim.MaChucVu = nhanvien.MaChucVu;
                    // lưu vào cơ sở dữ liệu
                    data.SubmitChanges();
                }
                return RedirectToAction("HienDanhSach", "NhanVien");
            }
        }
    }
}
