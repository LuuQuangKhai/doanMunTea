using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MunTea.Models;
using System.IO;

namespace MunTea.Controllers
{
    public class ThucDonController : Controller
    {
        //
        // GET: /ThucDon/
        dbMunTeaLinQDataContext data = new dbMunTeaLinQDataContext();

        public ActionResult HienDanhSach()
        {
            List<ThucDon> ds = data.ThucDons.ToList();
            return View(ds);
        }

        [HttpGet]
        public ActionResult Them()
        {
            ViewBag.maloaithucdon = new SelectList(data.LoaiThucDons.ToList().OrderBy(n => n.TenLoaiThucDon), "MaLoaiThucDon", "TenLoaiThucDon");
            return View();
        }

        [HttpPost]
        public ActionResult Them(ThucDon thucdon, HttpPostedFileBase fileupload)
        {
            // trường hợp nhập lại
            ViewBag.maloaithucdon = new SelectList(data.LoaiThucDons.ToList().OrderBy(n => n.TenLoaiThucDon), "MaLoaiThucDon", "TenLoaiThucDon");

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
                    var duongdan = Path.Combine(Server.MapPath("~/HinhAnh/ThucDon"), tenFile);

                    if (System.IO.File.Exists(duongdan))
                        ViewBag.thongbao = "Hình ảnh đã tồn tại!";
                    else
                    {
                        // Lưu hình ảnh vào đường dẫn
                        fileupload.SaveAs(duongdan);
                    }
                    thucdon.HinhAnh = tenFile;

                    // lưu vào cơ sở dữ liệu
                    data.ThucDons.InsertOnSubmit(thucdon);
                    data.SubmitChanges();
                }
                return RedirectToAction("HienDanhSach", "ThucDon");
            }
        }

        public ActionResult Xoa(int mathucdon)
        {
            ThucDon thucdon = data.ThucDons.FirstOrDefault(t => t.MaThucDon == mathucdon);

            if (thucdon.ChiTietHoaDons.Count == 0)
            {
                data.ThucDons.DeleteOnSubmit(thucdon);
                data.SubmitChanges();
                return RedirectToAction("HienDanhSach", "ThucDon");
            }
            ViewBag.thongbao = "Còn dữ liệu của thực đơn trong tình trạng hóa đơn!";
            return View();
        }

        public ActionResult ChiTiet(int mathucdon)
        {
            ThucDon thucdon = data.ThucDons.FirstOrDefault(t => t.MaThucDon == mathucdon);
            return View(thucdon);
        }

        [HttpGet]
        public ActionResult Sua(int mathucdon)
        {
            ThucDon thucdon = data.ThucDons.FirstOrDefault(t => t.MaThucDon == mathucdon);
            ViewBag.maloaithucdon = new SelectList(data.LoaiThucDons.ToList().OrderBy(n => n.TenLoaiThucDon), "MaLoaiThucDon", "TenLoaiThucDon");
            return View(thucdon);
        }

        [HttpPost]
        public ActionResult Sua(ThucDon td, HttpPostedFileBase fileupload)
        {
            ViewBag.maloaithucdon = new SelectList(data.LoaiThucDons.ToList().OrderBy(n => n.TenLoaiThucDon), "MaLoaiThucDon", "TenLoaiThucDon");
            ThucDon thucdon = data.ThucDons.FirstOrDefault(t => t.MaThucDon == td.MaThucDon);
            if (thucdon == null)
            {
                ViewBag.thongbao = "Mã thực đơn không tồn tại!";
                return View(td);
            }
            if (fileupload == null)
            {
                thucdon.TenThucDon = td.TenThucDon;
                thucdon.Size = td.Size;
                thucdon.GiaTien = td.GiaTien;
                thucdon.MaLoaiThucDon = td.MaLoaiThucDon;
                // lưu vào cơ sở dữ liệu
                data.SubmitChanges();
                return RedirectToAction("HienDanhSach", "ThucDon");
            }
            //thêm vào cơ sở dữ liệu
            else
            {
                if (ModelState.IsValid)
                {
                    // lưu tên file * bổ sung thư viện System.IO
                    var tenFile = Path.GetFileName(fileupload.FileName);
                    // lưu đường dẫn của file
                    var duongdan = Path.Combine(Server.MapPath("~/HinhAnh/ThucDon"), tenFile);

                    if (System.IO.File.Exists(duongdan))
                        ViewBag.thongbao = "Hình ảnh đã tồn tại!";
                    else
                    {
                        // Lưu hình ảnh vào đường dẫn
                        fileupload.SaveAs(duongdan);
                    }
                    thucdon.TenThucDon = td.TenThucDon;
                    thucdon.Size = td.Size;
                    thucdon.GiaTien = td.GiaTien;
                    thucdon.MaLoaiThucDon = td.MaLoaiThucDon;
                    thucdon.HinhAnh = tenFile;
                    // lưu vào cơ sở dữ liệu
                    data.SubmitChanges();
                }
                return RedirectToAction("HienDanhSach", "ThucDon");
            }
        }
    }
}
