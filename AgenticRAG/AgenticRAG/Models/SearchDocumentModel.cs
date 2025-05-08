namespace AgenticRAG.Models
{
    using Azure.Search.Documents.Indexes;
    using Azure.Search.Documents.Indexes.Models;
    using System.Collections.Generic;

    public class SearchDocumentModel
    {
        public string chunk_id { get; set; }

        public string chunk { get; set; }

        public string score { get; set; }
    }

}

