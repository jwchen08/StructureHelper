using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper
{
    public class RecommendItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Remark { get; set; }
        public string Id { get; set; }
    }

    public class RecommendList : ObservableCollection<RecommendItem>
    {
        private RecommendList() { }
        public static RecommendList GetRecommendList()
        {
            var list = new RecommendList();
            list.Add(new RecommendItem
            {
                Name = "10句话",
                Icon = "/Recommend/10juhua.png",
                Remark = "每天十句话，句句都精彩！",
                Id = "d5b2e79d-c281-4545-a71f-9ee941b8a2a5"
            });
            list.Add(new RecommendItem
            {
                Name = "10幅图",
                Icon = "/Recommend/10futu.png",
                Remark = "每天十幅图，gif转不停！",
                Id = "9f48bf62-0b12-48cb-bea5-9f0c8102f700"
            });
            list.Add(new RecommendItem
            {
                Name = "枕边听书-明朝那些事",
                Icon = "/Recommend/AudioBook.png",
                Remark = "离线也能听书，闭目也能阅读！",
                Id = "76f30074-a7c5-4db2-bc64-e95cccf6f0d1"
            });
            list.Add(new RecommendItem
            {
                Name = "XNote",
                Icon = "/Recommend/xnote.png",
                Remark = "一款酷炫十足的记事本应用，界面简洁，功能强大!",
                Id = "13eaa951-787b-4795-a037-45f5eb3087d0"
            });
            list.Add(new RecommendItem
            {
                Name = "语音备忘录",
                Icon = "/Recommend/yuyin.png",
                Remark = "非常实用的语音记事本，直接说话就能保存，方便快捷，随时查看！",
                Id = "bc08a447-6f76-45ab-8db8-6e957b3150f4"
            });
            list.Add(new RecommendItem
            {
                Name = "二维码Maker",
                Icon = "/Recommend/erweimaMaker.png",
                Remark = "离线快速生成各种信息的二维码，同时支持将二维码保存到图片库！",
                Id = "0a4230e3-971c-4e7e-b4ba-416eb4b5cb1b"
            });

            return list;
        }

    }

}
