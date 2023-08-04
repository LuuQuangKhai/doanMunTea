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

        public ActionResult Index()
        {
            return View();
        }

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
    }
}
