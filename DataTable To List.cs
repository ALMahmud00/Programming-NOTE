using System.Data;
using System.Text;

namespace Helper
{
    public class CommonHelper
    {
        public static async Task<GetDynamicDataTableDTO> GetDynamicDataTable(DataTable dt)
        {
            var headerNameList = new List<string>();
            var dynamicTableRow = new List<DynamicTableRowDTO>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                var rowDataList = new List<string>();
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    rowDataList.Add(dt.Rows[i][j].ToString());
                }
                dynamicTableRow.Add(new DynamicTableRowDTO()
                {
                    TableData = rowDataList
                });
            }

            foreach (DataColumn column in dt.Columns)
            {
                headerNameList.Add(column.ColumnName);
            }

            return new GetDynamicDataTableDTO()
            {
                HeadingNames = headerNameList,
                TableRow = dynamicTableRow
            };
        }
    }

    public class GetDynamicDataTableDTO
    {
        public List<string> HeadingNames { get; set; }
        public List<DynamicTableRowDTO> TableRow { get; set; }
    }
    public class DynamicTableRowDTO
    {
        public List<string> TableData { get; set; }
    }
}
