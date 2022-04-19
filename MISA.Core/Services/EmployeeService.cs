using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Validate dữ liệu cho từng đối tượng
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>
        /// - true: đã validate,
        /// - false: chưa validate
        /// </returns>
        /// CreatedBy: NVLINH (11/03/2022)
        protected override bool ValidateCustom(Employee employee, string mode)
        {
            // EmployeeCode đã tồn tại
            if (mode == MISAFormMode.Create.ToString())
            {
                if (CheckDuplicateCode(employee.employee_code))
                {
                    errorData.Add("EmployeeCode", String.Format(Resources.ResourceVN.ValidateError_DuplicateEmployeeCode, employee.employee_code));
                    return false;
                }
            }
            else if (mode == MISAFormMode.Update.ToString())

            {
                if (CheckDuplicateCode(employee.employee_id, employee.employee_code))
                {
                    errorData.Add("EmployeeCode", String.Format(Resources.ResourceVN.ValidateError_DuplicateEmployeeCode, employee.employee_code));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Hàm export dữ liệu ra file excel
        /// </summary>
        /// <param name="tableExports"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (22/03/2022)
        public MemoryStream Export(List<TableExport> tableExports)
        {
            //lấy toàn bộ danh sách nhân viên
            var records = _employeeRepository.Get();
            var totalHeader = tableExports.Count + 1;

            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);

            var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH NHÂN VIÊN");
            workSheet.DefaultColWidth = 5;
            var modelTable = workSheet.Cells;
            //style chung
            modelTable.Style.Font.Name = "Arial";
            BindStyle(workSheet, records.Count(), totalHeader);

            //format tiêu đề
            workSheet.Cells[1, 1, 2, totalHeader].Merge = true;
            workSheet.Cells[1, 1, 2, totalHeader].Value = "DANH SÁCH NHÂN VIÊN";
            workSheet.Cells[1, 1, 2, totalHeader].Style.Font.Size = 16;
            workSheet.Cells[1, 1, 2, totalHeader].Style.Font.Bold = true;
            workSheet.Cells[1, 1, 2, totalHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //hàng bắt đầu in dữ liệu
            var row = 3;
            var col = 1;
            var index = 1;

            //Cột STT
            BindValue<string>(workSheet, row, col, "STT");

            //Xuất danh sách các Tên cột
            foreach (var colName in tableExports)
            {
                BindValue<string>(workSheet, row, col + 1, colName.Name);
                col++;
            }
            row++;
            //duyệt từng employee trong database
            foreach (Employee employee in records)
            {
                //lấy ra tất cả property
                var properties = employee.GetType().GetProperties();

                //Cột STT
                //workSheet.Cells[row, 1].Value = index;
                BindValue<int>(workSheet, row, 1, index, false);

                //duyệt để map với header Client
                for (var i = 0; i < tableExports.Count; i++)
                {
                    string? fieldName = tableExports[i].Key;
                    if (fieldName == "date_of_birth" || fieldName == "identity_date")
                    {
                        var value = employee.GetType().GetProperty(fieldName).GetValue(employee);
                        DateTime? date = value as DateTime?;
                        BindValue<DateTime?>(workSheet, row, i + 2, date, false);
                    }
                    else
                    {
                        var value = employee.GetType().GetProperty(fieldName).GetValue(employee);
                        BindValue<string?>(workSheet, row, i + 2, (string?)value, false);
                    }

                }
                row++;
                index++;
            }
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            package.Save();

            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Kiểm tra mã nhân viên khi create
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (09/03/2022)
        private bool CheckDuplicateCode(string employeeCode)
        {
            return _employeeRepository.CheckDuplicateCode(employeeCode);
        }

        /// <summary>
        /// Kiểm tra mã nhân viên khi update
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="employeeCode"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (09/03/2022)
        private bool CheckDuplicateCode(Guid employeeId, string employeeCode)
        {
            return _employeeRepository.CheckDuplicateCode(employeeId, employeeCode);
        }

        /// <summary>
        /// Hàm bind giá trị ra cột
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        /// <param name="isHeader"></param>
        /// CreatedBy: NVLINH (18/03/2022)
        private void BindValue<MISA>(ExcelWorksheet workSheet, int row, int col, MISA value, bool isHeader = true)
        {
            if (isHeader == true)
            {
                workSheet.Cells[row, col].Value = value;
                workSheet.Cells[row, col].Style.Font.Size = 10;
                workSheet.Cells[row, col].Style.Font.Bold = true;
                workSheet.Cells[row, col].Style.Font.Name = "Arial";
                workSheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            else
            {
                var type = typeof(MISA).Name;
                workSheet.Cells[row, col].Value = value;
                if (type == "Int32" || type == "String")
                {
                    workSheet.Cells[row, col].Style.Numberformat.Format = "@";
                    workSheet.Cells[row, col].Style.Font.Name = "Times New Roman";
                }
                else
                {
                    workSheet.Cells[row, col].Style.Numberformat.Format = "dd/MM/yyyy";
                    workSheet.Cells[row, col].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
            }
        }

        /// <summary>
        /// Bind Style cho cột excel
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// CreatedBy: NVLINH (18/03/2022)
        private void BindStyle(ExcelWorksheet workSheet, int row, int col)
        {
            workSheet.Cells[3, 1, row + 3, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[3, 1, row + 3, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[3, 1, row + 3, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[3, 1, row + 3, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        }
    }
}
