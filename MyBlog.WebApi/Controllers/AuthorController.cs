using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.DTO;
using MyBlog.WebApi.Utility._MD5;
using MyBlog.WebApi.Utility.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorInfoService _iAuthorInfoService;

        public AuthorController(IAuthorInfoService iAuthorInfoService)
        {
            this._iAuthorInfoService = iAuthorInfoService;
        }

        [HttpPost("Create")]
        public async Task<ApiResult> Create (string name, string userName, string userPassword)
        {
            // 資料驗證
            //
            //

            AuthorInfo author = new AuthorInfo
            {
                Name = name,
                UserName = userName,
                // MD5 加密密碼
                UserPassword = MD5Helper.MD5Encrypt32(userPassword)
            };

            // 判斷資料庫中，是否已存在要新增的 userName
            var oldAuthor = await _iAuthorInfoService.FindAsync(c => c.UserName == userName);
            if (oldAuthor != null) return ApiResultHelper.Error("userName 已存在");

            bool b = await _iAuthorInfoService.CreateAsync(author);

            if (!b) return ApiResultHelper.Error("新增失敗");
            return ApiResultHelper.Succese(author);
        }

        [HttpPut("Edit")]
        public async Task<ApiResult> Edit(string name)
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);

            var author = await _iAuthorInfoService.FindAsync(id);

            if (author == null) return ApiResultHelper.Error("無法找到該用戶");

            author.Name = name;

            bool b = await _iAuthorInfoService.EditAsync(author);

            if (!b) return ApiResultHelper.Error("修改失敗");
            return ApiResultHelper.Succese("修改成功");
        }

        [AllowAnonymous]
        [HttpGet("FindAthor")]
        public async Task<ApiResult> FindAthor ([FromServices]IMapper iMapper, int id)
        {
            var author = await _iAuthorInfoService.FindAsync(id);
            var authorDTO = iMapper.Map<AuthorDTO>(author);
            return ApiResultHelper.Succese(authorDTO);
        }
    }
}
