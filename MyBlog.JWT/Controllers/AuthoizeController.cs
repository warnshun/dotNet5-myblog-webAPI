using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.IService;
using MyBlog.JWT.Utility._MD5;
using MyBlog.JWT.Utility.ApiResult;
using MyBlog.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoizeController : ControllerBase
    {
        private readonly IAuthorInfoService _iAuthorInfoService;

        public AuthoizeController(IAuthorInfoService iAuthorInfoService)
        {
            this._iAuthorInfoService = iAuthorInfoService;
        }

        [HttpPost("Login")]
        public async Task<ApiResult> Login(string userName, string userPassword)
        {
            // 加密後的密碼
            string password = MD5Helper.MD5Encrypt32(userPassword);

            // 資料認證
            var author = await _iAuthorInfoService.FindAsync(c => c.UserName == userName && c.UserPassword == password);

            if (author != null)
            {
                // 登入成功
                var claims = new Claim[]
                    {
                        // 不能放敏感訊息
                        new Claim(ClaimTypes.Name, author.Name),
                        new Claim("Id", author.Id.ToString()),
                        new Claim("UserName", author.UserName)
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF"));
                // issuer 為分發 Token 的 Web 應用程式， audience 是 Token 的接收者
                var token = new JwtSecurityToken(
                    issuer: "http://localhost:6060",
                    audience: "http://localhost:5000",
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1), // 多久過期
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return ApiResultHelper.Succese(jwtToken);
            }
            else
            {
                // 登入失敗
                return ApiResultHelper.Error("帳號或密碼錯誤");
            }
        }
    }
}
