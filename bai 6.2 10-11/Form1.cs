using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace bai_6._2_10_11
{
    public partial class Form1 : Form
    {
        // 1. Chuỗi kết nối trực tiếp đến Database HDMPRDB trên LocalDB
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HRMPRDB;Integrated Security=True";

        // 2. Biến phục vụ Disconnected Model cho Chấm Công
        private SqlDataAdapter daChamCong;
        private DataTable dtChamCong;
        private SqlCommandBuilder cbChamCong;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadPhongBanCombobox();
            LoadDanhSachNhanVien();
        }

        #region 1. QUẢN LÝ NHÂN SỰ (CRUD & TÌM KIẾM)

        private void LoadPhongBanCombobox()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaPB, TenPB FROM PhongBan", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboPhongBan.DataSource = dt;
                cboPhongBan.DisplayMember = "TenPB";
                cboPhongBan.ValueMember = "MaPB";
            }
        }

        private void LoadDanhSachNhanVien()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT nv.MaNV, nv.HoTen, nv.NgaySinh, nv.CCCD, 
                                      pb.TenPB, nv.LuongCoBan, nv.HeSoLuong, nv.MaPB 
                               FROM NhanVien nv 
                               JOIN PhongBan pb ON nv.MaPB = pb.MaPB";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvNhanVien.DataSource = dt;

                if (dgvNhanVien.Columns["MaPB"] != null)
                    dgvNhanVien.Columns["MaPB"].Visible = false;
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtMaNV.Text = row.Cells["MaNV"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                if (row.Cells["NgaySinh"].Value != DBNull.Value && row.Cells["NgaySinh"].Value != null)
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtCCCD.Text = row.Cells["CCCD"].Value?.ToString();
                cboPhongBan.SelectedValue = row.Cells["MaPB"].Value;
                txtLuongCoBan.Text = row.Cells["LuongCoBan"].Value?.ToString();
                txtHeSoLuong.Text = row.Cells["HeSoLuong"].Value?.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string sql = @"INSERT INTO NhanVien (HoTen, NgaySinh, CCCD, MaPB, LuongCoBan, HeSoLuong) 
                           VALUES (@HoTen, @NgaySinh, @CCCD, @MaPB, @LuongCoBan, @HeSoLuong)";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value);
                cmd.Parameters.AddWithValue("@CCCD", txtCCCD.Text.Trim());
                cmd.Parameters.AddWithValue("@MaPB", cboPhongBan.SelectedValue);
                cmd.Parameters.AddWithValue("@LuongCoBan", decimal.Parse(txtLuongCoBan.Text));
                cmd.Parameters.AddWithValue("@HeSoLuong", float.Parse(txtHeSoLuong.Text));

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm nhân viên thành công!");
                    LoadDanhSachNhanVien();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNV.Text)) return;

            string sql = @"UPDATE NhanVien 
                           SET HoTen = @HoTen, NgaySinh = @NgaySinh, CCCD = @CCCD, 
                               MaPB = @MaPB, LuongCoBan = @LuongCoBan, HeSoLuong = @HeSoLuong 
                           WHERE MaNV = @MaNV";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@MaNV", int.Parse(txtMaNV.Text));
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value);
                cmd.Parameters.AddWithValue("@CCCD", txtCCCD.Text.Trim());
                cmd.Parameters.AddWithValue("@MaPB", cboPhongBan.SelectedValue);
                cmd.Parameters.AddWithValue("@LuongCoBan", decimal.Parse(txtLuongCoBan.Text));
                cmd.Parameters.AddWithValue("@HeSoLuong", float.Parse(txtHeSoLuong.Text));

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thông tin thành công!");
                    LoadDanhSachNhanVien();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNV.Text)) return;

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM NhanVien WHERE MaNV = @MaNV", con);
                    cmd.Parameters.AddWithValue("@MaNV", int.Parse(txtMaNV.Text));

                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa thành công!");
                        LoadDanhSachNhanVien();
                        btnLamMoi_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể xóa (có thể do ràng buộc dữ liệu): " + ex.Message);
                    }
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT nv.MaNV, nv.HoTen, nv.NgaySinh, nv.CCCD, 
                                      pb.TenPB, nv.LuongCoBan, nv.HeSoLuong, nv.MaPB 
                               FROM NhanVien nv 
                               JOIN PhongBan pb ON nv.MaPB = pb.MaPB 
                               WHERE nv.HoTen LIKE @keyword OR nv.CCCD LIKE @keyword OR pb.TenPB LIKE @keyword";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@keyword", "%" + txtTimKiem.Text.Trim() + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvNhanVien.DataSource = dt;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtCCCD.Clear();
            txtLuongCoBan.Clear();
            txtHeSoLuong.Text = "1.0";
            txtTimKiem.Clear();
            LoadDanhSachNhanVien();
        }

        #endregion

        #region 2. CHẤM CÔNG NGẮT KẾT NỐI (DISCONNECTED MODEL)

        private void btnTaiChamCong_Click(object sender, EventArgs e)
        {
            string thangNam = txtThangNamCong.Text.Trim();
            if (string.IsNullOrEmpty(thangNam))
            {
                MessageBox.Show("Vui lòng nhập Tháng/Năm (VD: 11/2026)");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sqlInit = @"INSERT INTO BangChamCong (MaNV, ThangNam, SoNgayCong, SoGioOT)
                                   SELECT MaNV, @ThangNam, 0, 0 
                                   FROM NhanVien nv
                                   WHERE NOT EXISTS (
                                       SELECT 1 FROM BangChamCong cc 
                                       WHERE cc.MaNV = nv.MaNV AND cc.ThangNam = @ThangNam
                                   )";
                SqlCommand cmdInit = new SqlCommand(sqlInit, con);
                cmdInit.Parameters.AddWithValue("@ThangNam", thangNam);
                cmdInit.ExecuteNonQuery();

                string selectSql = "SELECT MaCong, MaNV, ThangNam, SoNgayCong, SoGioOT FROM BangChamCong WHERE ThangNam = @ThangNam";
                SqlCommand cmdSelect = new SqlCommand(selectSql, con);
                cmdSelect.Parameters.AddWithValue("@ThangNam", thangNam);

                daChamCong = new SqlDataAdapter(cmdSelect);
                cbChamCong = new SqlCommandBuilder(daChamCong);

                dtChamCong = new DataTable();
                daChamCong.Fill(dtChamCong);

                dgvChamCong.DataSource = dtChamCong;
                dgvChamCong.Columns["MaCong"].ReadOnly = true;
                dgvChamCong.Columns["MaNV"].ReadOnly = true;
                dgvChamCong.Columns["ThangNam"].ReadOnly = true;
            }
        }

        private void btnLuuChamCong_Click(object sender, EventArgs e)
        {
            try
            {
                if (daChamCong != null && dtChamCong != null)
                {
                    dgvChamCong.EndEdit();
                    daChamCong.Update(dtChamCong);
                    MessageBox.Show("Đồng bộ dữ liệu chấm công về SQL Server thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message);
            }
        }

        #endregion

        #region 3. TÍNH LƯƠNG & CHỐT LƯƠNG (SQLTRANSACTION)

        private void btnXemTruocLuong_Click(object sender, EventArgs e)
        {
            string thangNam = txtThangNamLuong.Text.Trim();
            if (string.IsNullOrEmpty(thangNam))
            {
                MessageBox.Show("Vui lòng nhập Tháng/Năm tính lương!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = @"SELECT nv.MaNV, nv.HoTen, cc.ThangNam, cc.SoNgayCong, cc.SoGioOT,
                               ROUND(((nv.LuongCoBan * nv.HeSoLuong / 22.0) * cc.SoNgayCong) + (cc.SoGioOT * 50000), 2) AS ThucLinh
                               FROM BangChamCong cc
                               JOIN NhanVien nv ON cc.MaNV = nv.MaNV
                               WHERE cc.ThangNam = @ThangNam";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ThangNam", thangNam);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvBangLuong.DataSource = dt;
            }
        }

        private void btnChotLuong_Click(object sender, EventArgs e)
        {
            string thangNam = txtThangNamLuong.Text.Trim();
            if (string.IsNullOrEmpty(thangNam))
            {
                MessageBox.Show("Vui lòng nhập Tháng/Năm để chốt lương!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    string sqlGetLuong = @"SELECT nv.MaNV, cc.ThangNam,
                                           ROUND(((nv.LuongCoBan * nv.HeSoLuong / 22.0) * cc.SoNgayCong) + (cc.SoGioOT * 50000), 2) AS ThucLinh
                                           FROM BangChamCong cc
                                           JOIN NhanVien nv ON cc.MaNV = nv.MaNV
                                           WHERE cc.ThangNam = @ThangNam";

                    SqlCommand cmdGet = new SqlCommand(sqlGetLuong, con, trans);
                    cmdGet.Parameters.AddWithValue("@ThangNam", thangNam);

                    DataTable dtLuong = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmdGet))
                    {
                        da.Fill(dtLuong);
                    }

                    if (dtLuong.Rows.Count == 0)
                    {
                        trans.Rollback();
                        MessageBox.Show("Không có dữ liệu chấm công tháng này để chốt lương!");
                        return;
                    }

                    string sqlDelete = "DELETE FROM BangLuong WHERE ThangNam = @ThangNam";
                    SqlCommand cmdDelete = new SqlCommand(sqlDelete, con, trans);
                    cmdDelete.Parameters.AddWithValue("@ThangNam", thangNam);
                    cmdDelete.ExecuteNonQuery();

                    string sqlInsert = "INSERT INTO BangLuong (MaNV, ThangNam, ThucLinh) VALUES (@MaNV, @ThangNam, @ThucLinh)";
                    foreach (DataRow row in dtLuong.Rows)
                    {
                        SqlCommand cmdInsert = new SqlCommand(sqlInsert, con, trans);
                        cmdInsert.Parameters.AddWithValue("@MaNV", row["MaNV"]);
                        cmdInsert.Parameters.AddWithValue("@ThangNam", row["ThangNam"]);
                        cmdInsert.Parameters.AddWithValue("@ThucLinh", row["ThucLinh"]);
                        cmdInsert.ExecuteNonQuery();
                    }

                    trans.Commit();
                    MessageBox.Show($"Chốt bảng lương tháng {thangNam} thành công!");
                    btnXemTruocLuong_Click(null, null);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Đã xảy ra lỗi, giao dịch bị hủy bỏ: " + ex.Message);
                }
            }
        }

        #endregion
    }
}