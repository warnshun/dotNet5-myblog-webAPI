using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.WebApi.Utility.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
//    [Authorize]
    public class TypeController : ControllerBase
    {
        private readonly ITypeInfoService _iTypeInfoService;

        public TypeController(ITypeInfoService iTypeInfoService)
        {
            this._iTypeInfoService = iTypeInfoService;
        }

        [HttpGet("Types")]
        public async Task<ApiResult> Types()
        {
            var types = await _iTypeInfoService.QueryAsync();

            if (types.Count == 0) return ApiResultHelper.Error("沒有更多的文章類別");
            return ApiResultHelper.Succese(types);
        }

        [HttpPost("Create")]
        public async Task<ApiResult> Create(string name)
        {
            #region 資料驗證
            if (String.IsNullOrWhiteSpace(name)) return ApiResultHelper.Error("文章類別名不能為空");
            #endregion

            TypeInfo type = new TypeInfo
            {
                Name = name
            };

            bool b = await _iTypeInfoService.CreateAsync(type);

            if (!b) return ApiResultHelper.Error("新增失敗");
            return ApiResultHelper.Succese(b);
        }

        [HttpDelete("Delete")]
        public async Task<ApiResult> Delete(int id)
        {
            bool b = await _iTypeInfoService.Deletesync(id);

            if (!b) return ApiResultHelper.Error("刪除失敗");
            return ApiResultHelper.Succese(b);
        }

        [HttpPut("Edit")]
        public async Task<ApiResult> Edit(int id, string name)
        {
            var type = await _iTypeInfoService.FindAsync(id);

            if (type == null) return ApiResultHelper.Error("無法找到該文章類別");

            type.Name = name;

            bool b = await _iTypeInfoService.EditAsync(type);

            if (!b) return ApiResultHelper.Error("修改失敗");
            return ApiResultHelper.Succese(type);
        }
    }
}
