﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanCafeForm.Models;
using QuanCafeForm.DAO;

namespace QuanCafeForm.GUI
{
    public partial class ucNHAPHANG : UserControl
    {
        private NhaCungCapDAO nccDAO = new NhaCungCapDAO();
        private HangSxDAO hsxDAO = new HangSxDAO();
        private LoaiSpDAO lspDAO = new LoaiSpDAO();
        private SanphamDAO spDAO = new SanphamDAO();
        private NhapHangDAO nhDAO = new NhapHangDAO();
        private List<TEMP> lstTemp = new List<TEMP>();
        public ucNHAPHANG()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Set DataSource cho các Combobox Hãng sản xuất và Nhà cung cấp
        /// </summary>
        private void setCBB_HSX_NCC_Source()
        {
            cbBNhap_HSX.DataSource = hsxDAO.getListHANGSX();
            cbBNhap_HSX.DisplayMember = "TEN";
            cbBNhap_HSX.ValueMember = "ID";
            cbBNhap_NCC.DataSource = nccDAO.getListNCC();
            cbBNhap_NCC.DisplayMember = "TEN";
            cbBNhap_NCC.ValueMember = "ID";
            cbbHSX.DataSource = hsxDAO.getListHANGSX();
            cbbHSX.DisplayMember = "TEN";
            cbbHSX.ValueMember = "ID";
            cbbNCC.DataSource = nccDAO.getListNCC();
            cbbNCC.DisplayMember = "TEN";
            cbbNCC.ValueMember = "ID";
        }
        /// <summary>
        /// Set DataSource cho Combobox Loại sản phẩm khi Nhập hàng
        /// </summary>
        private void setCBBNhap_LSP_Source()
        {
            int idHSX = (int)cbBNhap_HSX.SelectedValue;
            cbBNhap_LSP.DataSource = lspDAO.getLOAISPByHSX(idHSX);
            cbBNhap_LSP.DisplayMember = "TEN";
            cbBNhap_LSP.ValueMember = "ID";
        }
        /// <summary>
        /// Set DataSource cho Combobox Loại sản phẩm khi thêm sản phẩm mới
        /// </summary>
        private void setCBB_LSP_Source()
        {
            int idHSX = (int) cbbHSX.SelectedValue;
            cbbLSP.DataSource = lspDAO.getLOAISPByHSX(idHSX);
            cbbLSP.DisplayMember = "TEN";
            cbbLSP.ValueMember = "ID";
        }
        private void ucNHAPHANG_Load(object sender, EventArgs e)
        {
            setCBB_HSX_NCC_Source();
            cbBNhap_HSX.SelectedValueChanged += new EventHandler(setCBBNhap_LSP_Source);
            cbbHSX.SelectedValueChanged += new EventHandler(setCBB_LSP_Source);
            setCBBNhap_LSP_Source();
            setCBB_LSP_Source();
            cbBNhap_LSP.SelectedValueChanged += new EventHandler(setCBBNhap_SP_Source);
            setCBBNhap_SP_Source(null, null);
            gridCtrLSNH.DataSource = nhDAO.getListNhapHang();
        }
        private void setCBBNhap_LSP_Source(object s, EventArgs e)
        {
            setCBBNhap_LSP_Source();
        }
        private void setCBB_LSP_Source(object s, EventArgs e)
        {
            setCBB_LSP_Source();
        }
        /// <summary>
        /// Set DataSources cho Combobox sản phẩm khi nhập hàng
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void setCBBNhap_SP_Source(object s, EventArgs e)
        {
            cbBNhap_SP.DataSource = spDAO.getListSPByLSP((int)cbBNhap_LSP.SelectedValue);
            cbBNhap_SP.DisplayMember = "KHOILUONG";
            cbBNhap_SP.ValueMember = "ID";
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            gridCtrLSNH.DataSource = nhDAO.getListNhapHangByTime(dateFrom.Value, dateTo.Value);
        }

        private void gridViewLSNH_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gridCtrLSCTNH.DataSource = nhDAO.getListCTNhapHangByIdNH((int)gridViewLSNH.GetFocusedRowCellValue("ID"));
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            SANPHAM1 sp = new SANPHAM1();
            sp.KHOILUONG = (int)spinKL.Value;
            sp.ANH = "/Resources/Image/empty.jpg";
            sp.MOTA = "";
            sp.SOLUONG = 0;
            sp.LOAISP_ID = (int)cbbLSP.SelectedValue;
            spDAO.insertSANPHAM(sp);
            spinKL.Value = 0;
            MessageBox.Show("Thêm thành công!");
            setCBBNhap_SP_Source(null, null);
        }

        private void btnThemHSX_Click(object sender, EventArgs e)
        {
            HANGSX h = new HANGSX();
            h.TEN = txtTenHSX.Text;
            hsxDAO.InsertHSX(h);
            txtTenHSX.Text = "";
            MessageBox.Show("Thêm thành công!");
            setCBB_HSX_NCC_Source();
        }

        private void btnThemNCC_Click(object sender, EventArgs e)
        {
            NHACUNGCAP n = new NHACUNGCAP();
            n.TEN = txtTenNCC.Text;
            n.DIACHI = txtDiaChiNCC.Text;
            n.SDT = txtSdtNCC.Text;
            nccDAO.InsertNCC(n);
            txtTenNCC.Text = "";
            txtDiaChiNCC.Text = "";
            txtSdtNCC.Text = "";
            MessageBox.Show("Thêm thành công!");
            setCBB_HSX_NCC_Source();
        }

        private void btnThemNhap_Click(object sender, EventArgs e)
        {
            TEMP tmp = new TEMP();
            tmp.IDSP = (int)cbBNhap_SP.SelectedValue;
            tmp.SANPHAM = cbBNhap_LSP.Text;
            tmp.KHOILUONG = Convert.ToInt32(cbBNhap_SP.Text);
            tmp.SOLUONGNHAP = (int)spinNhapSL.Value;
            tmp.GIANHAP = (int)spinNhapGIANHAP.Value;
            lstTemp.Add(tmp);
            gridCtrCTNhap.DataSource = null;
            gridCtrCTNhap.DataSource = lstTemp;
        }

        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            NHAPHANG nh = new NHAPHANG();
            nh.NHACC_ID = (int)cbBNhap_NCC.SelectedValue;
            int idNh = nhDAO.InsertNhapHang(nh).ID;
            foreach (TEMP tmp in lstTemp)
            {
                CHITIETNHAPHANG ct = new CHITIETNHAPHANG();
                ct.SANPHAM_ID = tmp.IDSP;
                ct.NHAPHANG_ID = idNh;
                ct.SOLUONGNHAP = tmp.SOLUONGNHAP;
                ct.GIANHAP = tmp.GIANHAP;
                nhDAO.InsertChiTietNhapHang(ct);
            }
            gridCtrCTNhap.DataSource = null;
            lstTemp = new List<TEMP>();
            spinNhapGIANHAP.Value = 0;
            spinNhapSL.Value = 0;
            MessageBox.Show("Nhập thành công!");
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            spinNhapGIANHAP.Value = 0;
            spinNhapSL.Value = 0;
            lstTemp = new List<TEMP>();
        }
    }
    class TEMP
    {
        public int IDSP { get; set; }
        public string SANPHAM { get; set; }
        public int KHOILUONG { get; set; }
        public int SOLUONGNHAP { get; set; }
        public int GIANHAP { get; set; }
    }
}
