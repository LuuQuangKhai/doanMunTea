using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MunTea.Models;


namespace MunTea.Models
{
    public class GioHang
    {
        dbMunTeaLinQDataContext data = new dbMunTeaLinQDataContext();
        public int mathucdon { get; set; }
        public string tenthucdon { get; set; }
        public string size { get; set; }
        public string hinhanh { get; set; }
        public double giatien { get; set; }
        public int soluong { get; set; }
        public double thanhtien
        {
            get { return soluong * giatien; }
        }
        public GioHang(int id)
        {
            mathucdon = id;
            ThucDon thucdon = data.ThucDons.Single(t => t.MaThucDon == id);
            tenthucdon = thucdon.TenThucDon;
            size = thucdon.Size;
            hinhanh = thucdon.HinhAnh;
            giatien = (double)thucdon.GiaTien;
            soluong = 1;
        }
    }
}