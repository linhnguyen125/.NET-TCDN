using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Core.Interfaces.Base;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class CaPaymentService : BaseService<CaPayment>, ICaPaymentService
    {
        ICaPaymentRepository _caPaymentRepository;
        public CaPaymentService(ICaPaymentRepository caPaymentRepository) : base(caPaymentRepository)
        {
            _caPaymentRepository = caPaymentRepository;
        }

        /// <summary>
        /// Hàm export dữ liệu ra file excel
        /// </summary>
        /// <param name="tableExports"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (22/03/2022)
        public MemoryStream Export(FilterObject filterObject)
        {
            List<TableExport> tableExports = filterObject.tableExport;
            //lấy danh sách data cần filter
            var res = _caPaymentRepository.GetPaging(filterObject);
            var records = (IEnumerable<CaPayment>)res.GetType().GetProperties()[0].GetValue(res);
            var totalHeader = tableExports.Count + 1;

            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);

            var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH PHIẾU CHI");
            workSheet.DefaultColWidth = 5;
            var modelTable = workSheet.Cells;
            //style chung
            modelTable.Style.Font.Name = "Arial";
            BindStyle(workSheet, records.Count(), totalHeader);

            //format tiêu đề
            workSheet.Cells[1, 1, 2, totalHeader].Merge = true;
            workSheet.Cells[1, 1, 2, totalHeader].Value = "DANH SÁCH PHIẾU CHI";
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
            foreach (CaPayment payment in records)
            {
                //lấy ra tất cả property
                var properties = payment.GetType().GetProperties();

                //Cột STT
                //workSheet.Cells[row, 1].Value = index;
                BindValue<int>(workSheet, row, 1, index, false);

                //duyệt để map với header Client
                for (var i = 0; i < tableExports.Count; i++)
                {
                    string? fieldName = tableExports[i].Key;
                    if (fieldName == "posted_date" || fieldName == "refdate")
                    {
                        var value = payment.GetType().GetProperty(fieldName).GetValue(payment);
                        DateTime? date = value as DateTime?;
                        BindValue<DateTime?>(workSheet, row, i + 2, date, false);
                    }
                    else if (fieldName == "total_amount")
                    {
                        var value = payment.GetType().GetProperty(fieldName).GetValue(payment);
                        workSheet.Cells[row, i + 2].Style.Numberformat.Format = "#,##0.0";
                        workSheet.Cells[row, i + 2].Value = value;
                    }
                    else if (fieldName == "document_included")
                    {
                        var value = payment.GetType().GetProperty(fieldName).GetValue(payment);
                        BindValue<int?>(workSheet, row, i + 2, (int?)value, false);
                    }
                    else
                    {
                        var value = payment.GetType().GetProperty(fieldName).GetValue(payment);
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
