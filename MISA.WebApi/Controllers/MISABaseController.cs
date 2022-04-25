using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces.Base;

namespace MISA.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MISABaseController<MISAEntity> : ControllerBase where MISAEntity : class
    {
        IBaseRepository<MISAEntity> _baseRepository;
        IBaseService<MISAEntity> _baseService;

        public MISABaseController(IBaseRepository<MISAEntity> baseRepository, IBaseService<MISAEntity> baseService)
        {
            _baseRepository = baseRepository;
            _baseService = baseService;
        }

        /// <summary>
        /// Thực hiện lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns>
        /// 200 - Danh sách dữ liệu,
        /// 204 - Không có dữ liệu,
        /// 500 - Lỗi server
        /// </returns>
        /// CreatedBy: NVLINH (11/03/2022)
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var entities = _baseRepository.Get();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Thực hiện lấy dữ liệu theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (12/03/2022)
        [HttpGet("{entityId}")]
        public IActionResult GetById(Guid entityId)
        {
            try
            {
                var entity = _baseRepository.GetById(entityId);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// 200 - Chưa thêm được vào database,
        /// 201 - Thành công,
        /// 400 - Dữ liệu đầu vào không hợp lệ,
        /// 500 - Lỗi server
        /// </returns>
        /// CreatedBy: NVLINH (11/03/2022)
        [HttpPost]
        public IActionResult Post(MISAEntity entity)
        {
            try
            {
                var res = _baseService.InsertService(entity);
                if (res > 0)
                {
                    var notify = new NotifyService();
                    return StatusCode(201,
                        notify.Success
                        (
                            devMsg: MISA.Core.Resources.ResourceVN.Success_Created,
                            userMsg: MISA.Core.Resources.ResourceVN.Success_Created,
                            data: entity,
                            statusCode: 201
                        ));
                }
                return Ok();
            }
            catch (ValidateException ex)
            {
                return HandleValidateException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns>
        /// 200 - Thành công,
        /// 400 - Dữ liệu đầu vào không hợp lệ,
        /// 500 - Lỗi server
        /// </returns>
        /// CreatedBy: NVLINH (09/03/2022)
        [HttpPut]
        public IActionResult Put(MISAEntity entity)
        {
            try
            {
                var res = _baseService.UpdateService(entity);
                if (res > 0)
                {
                    var notify = new NotifyService();
                    return Ok(notify.Success(
                            devMsg: MISA.Core.Resources.ResourceVN.Success_Updated,
                            userMsg: MISA.Core.Resources.ResourceVN.Success_Updated,
                            data: entity,
                            statusCode: 200
                        )
                    );
                }
                return Ok();

            }
            catch (ValidateException ex)
            {
                return HandleValidateException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// 200 - Thành công,
        /// 500 - Lỗi server
        /// </returns>
        /// CreatedBy: NVLINH (11/03/2022)
        [HttpDelete("{entityId}")]
        public IActionResult Delete(Guid entityId)
        {
            try
            {
                var res = _baseRepository.Delete(entityId);
                if (res > 0)
                {
                    var notify = new NotifyService();
                    return Ok(notify.Success(
                            devMsg: MISA.Core.Resources.ResourceVN.Success_Deleted,
                            userMsg: MISA.Core.Resources.ResourceVN.Success_Deleted,
                            statusCode: 200
                        )
                    );
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Xóa nhiều dữ liệu
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns>
        /// 200 - Thành công,
        /// 500 - Lỗi server
        /// </returns>
        /// CreatedBy: NVLINH (12/03/2022)
        [HttpDelete]
        public IActionResult Delete([FromBody] Guid[] entityIds)
        {
            try
            {
                var res = _baseRepository.Delete(entityIds);
                if (res > 0)
                {
                    var notify = new NotifyService();
                    return Ok(notify.Success(
                            devMsg: MISA.Core.Resources.ResourceVN.Success_Deleted,
                            userMsg: MISA.Core.Resources.ResourceVN.Success_Deleted,
                            statusCode: 200
                        )
                    );
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Trả về lỗi Validate
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        protected IActionResult HandleValidateException(ValidateException ex)
        {
            var notify = new NotifyService();
            notify.DevMsg = ex.Message;
            notify.UserMsg = MISA.Core.Resources.ResourceVN.Error_Exception;
            notify.Data = ex.Data;
            notify.StatusCode = 400;
            return Ok(notify);
        }

        /// <summary>
        /// Trả về lỗi Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>
        /// 500 - Lỗi server,
        /// error - Nội dung lỗi
        /// </returns>
        /// CreatedBy: NVLINH (07/03/2022)
        protected IActionResult HandleException(Exception ex)
        {
            var notify = new NotifyService();
            notify.DevMsg = ex.Message;
            notify.UserMsg = MISA.Core.Resources.ResourceVN.Error_Exception;
            notify.Data = ex.Data;
            notify.StatusCode = 500;
            return StatusCode(500, notify);
        }
    }
}
