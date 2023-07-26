using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class MigratedTransactionMapDTO
    {
        public IEnumerable<MigratedTransactionActionMapDTO> MigratedTransactionAction { get; set; }

    }
}
