using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Core.Interfaces.Base;

namespace MISA.WebApi.Controllers
{
    public class LayoutsController : MISABaseController<Layout>
    {
        ILayoutRepository _layoutRepository;
        ILayoutService _layoutService;
        public LayoutsController(ILayoutRepository layoutRepository, ILayoutService layoutService) : base(layoutRepository, layoutService)
        {
            _layoutRepository = layoutRepository;
            _layoutService = layoutService;
        }

        [HttpGet("filter")]
        public IActionResult GetLayout(bool is_default, string layout_code)
        {
            try
            {
                var res = _layoutRepository.GetLayout(is_default, layout_code);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] Layout layout)
        {
            try
            {
                var res = _layoutRepository.UpdateLayout(layout);
                if (res > 0)
                    return Ok(res);
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
