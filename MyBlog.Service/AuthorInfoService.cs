using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service
{
    public class AuthorInfoService:BaseService<AuthorInfo>, IAuthorInfoService
    {
        private readonly IAuthorInfoRepository _iAuthorInfoRepository;
        public AuthorInfoService(IAuthorInfoRepository iAuthorInfoRepository)
        {
            base._iBaseRepository = iAuthorInfoRepository;
            _iAuthorInfoRepository = iAuthorInfoRepository;
        }
    }
}
