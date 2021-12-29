using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWebApi.Models
{
    internal class TestData
    {
        [Column(name: "test_data_id")]
        public int Id { get; set; }
        public string Value { get; set; } = null!;
    }
}
