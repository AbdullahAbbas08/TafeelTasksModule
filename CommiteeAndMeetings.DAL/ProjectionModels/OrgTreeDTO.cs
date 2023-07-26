namespace DbContexts.MasarContext.ProjectionModels
{
    public class OrgTreeDTO
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Color { get; set; }
        public bool HasChilds { get; set; }
        public string OrganizationCode { get; set; }


        //id: number;
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
