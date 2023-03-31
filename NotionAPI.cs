using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Notion;
using Notion.Client;

namespace NotionChartProgram
{
    public partial class NotionAPI : Form
    {
        string key = "";
        string databaseid = "";

        NotionClient client;

        public NotionAPI()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clientSetting();
            DataBaseQuery();
        }

        private void clientSetting()
        {
            client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = key
            });
        }

        async private void DataBaseQuery()
        {
            string textvalue = "";
            string textvalue2 = "";
            var dateFilter = new DateFilter("날짜", onOrAfter: Convert.ToDateTime(DateTime.Now.ToString("yyyy년 MM월 dd일")));

            var queryParmas = new DatabasesQueryParameters { Filter = dateFilter };
            var pages = await client.Databases.QueryAsync(databaseid, queryParmas);


            Database pages2 = await client.Databases.RetrieveAsync(databaseid);

            pages2.Properties.TryGetValue("태그", out var ve);
            string json2 = JsonConvert.SerializeObject(ve);
            var obj2 = JsonConvert.DeserializeObject<TagObject>(json2);
            foreach(var option in obj2.Multi_Select.Options)
            {
                textvalue2 += $"태그번호는 : {option.Name}, 색상은 : {option.Color} 입니다.  {Environment.NewLine}";
            }

            foreach (var page in pages.Results)
            {
                page.Properties.TryGetValue("날짜", out var v);

                string json = JsonConvert.SerializeObject(v);
                var obj = JsonConvert.DeserializeObject<DateObject>(json);

                string start = obj.Date.Start.ToString("yyyy-MM-ddTHH:mm:ss");

                textvalue += $"시간은 {Convert.ToDateTime(start).ToString("yyyy년 MM월 dd일")} 입니다. { Environment.NewLine}";
            }

            textBox1.Text = textvalue2 + textvalue;


            // 동적으로 만들기
            //string json3 = JsonConvert.SerializeObject(pages2.Properties.Values);
            //dynamic data = JsonConvert.DeserializeObject(json3);

            //string type = data.Type;
            //Console.WriteLine($"Type: {type}");

            //dynamic properties = data.properties;
            //foreach (dynamic value in properties)
            //{
            //    string name = value.name;
            //    string id = value.id;
            //    string color = value.color;

            //    Console.WriteLine($"Option: Name={name}, Id={id}, Color={color}");
            //}

            //string idValue = data.id;
            //Console.WriteLine($"Id: {idValue}");

            //string nameValue = data.name;
            //Console.WriteLine($"Name: {nameValue}");
        }

        public class TagObject
        {
            public string Type { get; set; }
            public MultiSelectData Multi_Select { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
        }

        // MultiSelectData 클래스 정의
        public class MultiSelectData
        {
            public List<OptionData> Options { get; set; }
        }

        // OptionData 클래스 정의
        public class OptionData
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public string Color { get; set; }
        }




        public class DateObject
        {
            public string Type { get; set; }
            public DateData Date { get; set; }
            public string Id { get; set; }
        }

        // DateData 클래스 정의
        public class DateData
        {
            public DateTime Start { get; set; }
            public DateTime? End { get; set; }
            public string TimeZone { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
