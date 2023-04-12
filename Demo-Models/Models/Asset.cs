namespace Demo_Models
{
    public class Asset
    {
        public Asset(int id, int? parentId, string status=null)
        {
            Id = id;
            ParentId = parentId;
            Status = status;
        }
        public int Id { get; set; }
        public bool? IsStartable { get; set; }
        public string Location { get; set; }
        public string Owner { get; set; }
        public string CreatedBy { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public List<string> Tags { get; set; }
        public int Cpu { get; set; }
        public long Ram { get; set; }
        public string CreatedAt { get; set; }
        public int? ParentId { get; set; }
        public int ParentTargetAssetCount { get; set; }
    }

}