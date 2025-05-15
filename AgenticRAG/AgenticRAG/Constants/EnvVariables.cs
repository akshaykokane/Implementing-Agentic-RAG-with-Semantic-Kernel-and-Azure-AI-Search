using System;
namespace AgenticRAG.Constants
{
	public static class EnvVariables
	{
		public const string AzureOpenAIUrlEnvVar = "AZURE_OPENAI_URL";

        public const string AzureOpenAIKeyEnvVar = "AZURE_OPENAI_KEY";

        public const string AzureAISearchUrlEnvVar = "AZURE_AI_SEARCH_URL";

        public const string AzureAISearchKeyEnvVar = "AZURE_AI_SEARCH_KEY";

        public static string EmbeddingDeploymentName = "EMBEDDING_DEPLOYMENT_NAME";

        public static string IndexName = "INDEX_NAME";

        public static string GPTModelDeploymentname = "GPT_DEPLOYMENT_NAME";
    }
}

