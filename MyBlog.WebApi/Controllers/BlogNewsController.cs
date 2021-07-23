using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.DTO;
using MyBlog.WebApi.Utility.ApiResult;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogNewsController : ControllerBase
    {
        private readonly IBlogNewsService _iBlogNewsService;

        public BlogNewsController(IBlogNewsService iBlogNewsService)
        {
            this._iBlogNewsService = iBlogNewsService;
        }

        [HttpGet("BlogNews")]
        public async Task<ActionResult<ApiResult>> GetBlogNews()
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var data = await _iBlogNewsService.QueryAsync(c=>c.AuthorId == id);

            if (data.Count == 0) return ApiResultHelper.Error("沒有更多的文章");
            return ApiResultHelper.Succese(data);
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Content"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ApiResult>> Create(string title, string Content, int typeId)
        {
            // 資料驗證
            // 
            //


            BlogNews blogNews = new BlogNews
            {
                BrowseCount = 0,
                Content = Content,
                LikeCount = 0,
                Time = DateTime.Now,
                Title = title,
                TypeId = typeId,
                AuthorId = Convert.ToInt32(this.User.FindFirst("Id").Value)
        };

            bool b = await _iBlogNewsService.CreateAsync(blogNews);

            if (!b) return ApiResultHelper.Error("新增失敗，伺服器發生錯誤");
            return ApiResultHelper.Succese(blogNews);
        }

        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<ActionResult<ApiResult>> Delete(int id)
        {
            bool b = await _iBlogNewsService.Deletesync(id);

            if (!b) return ApiResultHelper.Error("刪除失敗");
            return ApiResultHelper.Succese(b);
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpPut("Edit")]
        public async Task<ActionResult<ApiResult>> Edit(int id, string title, string content, int typeId)
        {
            var blogNews = await _iBlogNewsService.FindAsync(id);

            if (blogNews == null) return ApiResultHelper.Error("無法找到該文章");

            blogNews.Title = title;
            blogNews.Content = content;
            blogNews.TypeId = typeId;

            bool b = await _iBlogNewsService.EditAsync(blogNews);

            if (!b) return ApiResultHelper.Error("修改失敗");
            return ApiResultHelper.Succese(blogNews);
        }

        [HttpGet("BlogNewsPage")]
        public async Task<ApiResult> GetBlogNewsPage([FromServices]IMapper iMapper, int page, int size)
        {
            RefAsync<int> total = 0;
            var blogNews = await _iBlogNewsService.QueryAsync(page, size, total);

            try
            {
                var blogNewsDTO = iMapper.Map<List<BlogNewsDTO>>(blogNews);
                return ApiResultHelper.Succese(blogNewsDTO, total);
            }
            catch (Exception)
            {

                return ApiResultHelper.Error("AutoMapper 映射錯誤");
            }
        }
    }
}
