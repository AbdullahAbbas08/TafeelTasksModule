namespace Models.ProjectionModels
{
    public class TreeDetailsDTO
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string text { get; set; }
        public string orgName { get; set; }
        public bool? isCategory { get; set; }
        //public bool? isFavourite { get; set; }
        // public int? childCount { get; set; }
        // public bool? isLoading { get; set; }
        //public bool? isLoadingMore { get; set; }

        //public List<TreeDetailsDTO> orgTreeDetailsLST { get; set; }
        //public string Color { get; set; }
        //public bool HasChilds { get; set; }


        //      id: number;
        //parentId: number;
        //name: string;
        //isCategory?: boolean;
        //isFavourite?: boolean;
        //childCount?: number;
        //children?: TreeNode[];
        //isLoading?: boolean;
        //isLoadingMore?: boolean;
    }
}
