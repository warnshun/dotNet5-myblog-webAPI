using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace MyBlog.Model
{
    public class BlogNews:BaseId
    {
        // nvarchar 帶中文較好
        [SugarColumn(ColumnDataType = "nvarchar(30)")]
        public string Title { get; set; }
        [SugarColumn(ColumnDataType = "text")]
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public int BrowseCount { get; set; }
        public int LikeCount { get; set; }

        public int TypeId { get; set; }
        public int AuthorId { get; set; }

        // 類別，不映射到資料庫
        [SugarColumn(IsIgnore = true)]
        public TypeInfo TypeInfo { get; set; }
        [SugarColumn(IsIgnore = true)]
        public AuthorInfo AuthorInfo { get; set; }
    }
}
